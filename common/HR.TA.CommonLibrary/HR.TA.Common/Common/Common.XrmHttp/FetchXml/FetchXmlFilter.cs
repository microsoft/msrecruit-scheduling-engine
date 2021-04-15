//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Xml.Linq;

    public struct FetchXmlFilter
    {
        public enum LogicalOperator
        {
            And = 1,
            Or = 2,
        }

        public enum ComparisonOperator
        {
            Eq,
            Ne,
            Gt,
            Ge,
            Lt,
            Le,
            Like,
            NotLike,
            In,
            NotIn,
            Null,
            NotNull,
            BeginsWith,
            NotBeginWith,
            EndsWith,
            NotEndWith,
        }

        private readonly IList<Tuple<string, ODataField, ComparisonOperator, object[]>> conditions;
        private readonly IList<FetchXmlFilter> children;
        private readonly LogicalOperator logicalOp;

        public FetchXmlFilter(LogicalOperator logicalOperator, params FetchXmlFilter[] children)
        {
            var childrenToMergeIn = children.Where(c => c.logicalOp == 0 || c.logicalOp == logicalOperator).ToArray();
            var childrenToKeepSeparate = children.Where(c => c.logicalOp != 0 && c.logicalOp != logicalOperator).ToArray();

            this.conditions = childrenToMergeIn
                .Where(c => c.conditions != null)
                .SelectMany(c => c.conditions)
                .ToArray();
            this.children = childrenToMergeIn
                .Where(c => c.children != null)
                .SelectMany(c => c.children)
                .Concat(childrenToKeepSeparate)
                .ToArray();
            this.logicalOp = this.conditions.Count + this.children.Count > 1 ? logicalOperator : 0;
        }

        public FetchXmlFilter(string entityName, ODataField field, ComparisonOperator comparisonOp, params object[] values)
        {
            this.conditions = new[] { Tuple.Create(entityName, field, comparisonOp, values) };
            this.children = null;
            this.logicalOp = 0;
        }

        public FetchXmlFilter(ODataField field, ComparisonOperator op, params object[] values)
        {
            this.conditions = new[] { Tuple.Create((string)null, field, op, values) };
            this.children = null;
            this.logicalOp = 0;
        }

        /// <summary>
        /// Create a filter with a Linq expression.
        /// </summary>
        /// <param name="expression">The Linq expression.</param>
        public static FetchXmlFilter Filter<T>(Expression<Func<T, bool>> expression)
            => Filter(null, expression);

        /// <summary>
        /// Create a filter with a Linq expression.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="expression">The Linq expression.</param>
        public static FetchXmlFilter Filter<T>(string entityName, Expression<Func<T, bool>> expression)
            => expression == null ? default(FetchXmlFilter) : Expression(entityName, expression.Body, false);

        /// <summary>
        /// Combine filters with And.
        /// </summary>
        /// <param name="other">The other filter.</param>
        public FetchXmlFilter And(FetchXmlFilter other) => new FetchXmlFilter(LogicalOperator.And, this, other);

        /// <summary>
        /// Combine filters with Or.
        /// </summary>
        /// <param name="other">The other filter.</param>
        public FetchXmlFilter Or(FetchXmlFilter other) => new FetchXmlFilter(LogicalOperator.Or, this, other);

        public override string ToString() => this.ToXElement().ToString(SaveOptions.DisableFormatting);

        public XElement ToXElement()
        {
            var filterElement = new XElement("filter");

            if (logicalOp == LogicalOperator.Or)
            {
                filterElement.SetAttributeValue("type", "or");
            }

            filterElement.Add(this.conditions.Select(c =>
            {
                string opName;
                var takesList = false;
                switch (c.Item3)
                {
                    case ComparisonOperator.Eq: opName = "eq"; break;
                    case ComparisonOperator.Ne: opName = "ne"; break;
                    case ComparisonOperator.Gt: opName = "gt"; break;
                    case ComparisonOperator.Ge: opName = "ge"; break;
                    case ComparisonOperator.Lt: opName = "lt"; break;
                    case ComparisonOperator.Le: opName = "le"; break;
                    case ComparisonOperator.Like: opName = "like"; break;
                    case ComparisonOperator.NotLike: opName = "not-like"; break;
                    case ComparisonOperator.In: opName = "in"; takesList = true; break;
                    case ComparisonOperator.NotIn: opName = "not-in"; takesList = true; break;
                    case ComparisonOperator.Null: opName = "null"; break;
                    case ComparisonOperator.NotNull: opName = "not-null"; break;
                    case ComparisonOperator.BeginsWith: opName = "begins-with"; break;
                    case ComparisonOperator.NotBeginWith: opName = "not-begin-with"; break;
                    case ComparisonOperator.EndsWith: opName = "ends-with"; break;
                    case ComparisonOperator.NotEndWith: opName = "not-end-with"; break;
                    default: throw new NotSupportedException($"Not supported comparison operator {c.Item3}");
                }

                var conditionElement = new XElement("condition");
                conditionElement.SetAttributeValue("entityname", c.Item1);
                conditionElement.SetAttributeValue("attribute", c.Item2.ToFetchXmlFieldName());
                conditionElement.SetAttributeValue("operator", opName);

                var values = c.Item4;
                if (takesList || values.Length > 1)
                {
                    conditionElement.Add(values.Select(value =>
                    {
                        value = value is bool b ? (b ? "1" : "0")
                              : value is Enum e ? Convert.ToInt32(e)
                              : value;
                        return new XElement("value", value);
                    }));
                }
                else if (values.Length == 1)
                {
                    var value = values[0];
                    value = value is bool b ? (b ? "1" : "0")
                          : value is Enum e ? Convert.ToInt32(e)
                          : value;
                    conditionElement.SetAttributeValue("value", value);
                }

                return conditionElement;
            }));

            filterElement.Add(this.children?.Select(c => c.ToXElement()));

            return filterElement;
        }

        public override bool Equals(object obj) =>
            obj is FetchXmlFilter expression
            && this.conditions == expression.conditions
            && this.children == expression.children
            && this.logicalOp == expression.logicalOp;
        public override int GetHashCode() => (int)this.logicalOp + (this.conditions?.GetHashCode() ?? 0) + (this.children?.GetHashCode() ?? 0);
        public static bool operator ==(FetchXmlFilter a, FetchXmlFilter b) => a.Equals(b);
        public static bool operator !=(FetchXmlFilter a, FetchXmlFilter b) => !a.Equals(b);

        private static FetchXmlFilter Expression(string entityName, Expression expression, bool negate)
        {
            if (expression == null)
            {
                return default(FetchXmlFilter);
            }

            var binaryExpression = expression as BinaryExpression;

            switch (expression.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return new FetchXmlFilter(negate ? LogicalOperator.Or : LogicalOperator.And, Expression(entityName, binaryExpression.Left, negate), Expression(entityName, binaryExpression.Right, negate));

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return new FetchXmlFilter(negate ? LogicalOperator.And : LogicalOperator.Or, Expression(entityName, binaryExpression.Left, negate), Expression(entityName, binaryExpression.Right, negate));

                case ExpressionType.Not:
                    return Expression(entityName, ((UnaryExpression)expression).Operand, !negate);

                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    ComparisonOperator op;
                    var value = ODataExpression.EvaluateExpression(binaryExpression.Right);
                    if (value == null)
                    {
                        switch (expression.NodeType)
                        {
                            case ExpressionType.Equal: op = negate ? ComparisonOperator.NotNull : ComparisonOperator.Null; break;
                            case ExpressionType.NotEqual: op = negate ? ComparisonOperator.Null : ComparisonOperator.NotNull; break;
                            default: throw new NotImplementedException($"FetchXmlFilter.Expression: cannot handle expression of type {expression.NodeType}: {expression}");
                        }
                        return new FetchXmlFilter(entityName, ODataField.Field(binaryExpression.Left), op);
                    }
                    else
                    {
                        switch (expression.NodeType)
                        {
                            case ExpressionType.Equal: op = negate ? ComparisonOperator.Ne : ComparisonOperator.Eq; break;
                            case ExpressionType.NotEqual: op = negate ? ComparisonOperator.Eq : ComparisonOperator.Ne; break;
                            case ExpressionType.GreaterThan: op = negate ? ComparisonOperator.Le : ComparisonOperator.Gt; break;
                            case ExpressionType.LessThanOrEqual: op = negate ? ComparisonOperator.Gt : ComparisonOperator.Le; break;
                            case ExpressionType.GreaterThanOrEqual: op = negate ? ComparisonOperator.Lt : ComparisonOperator.Ge; break;
                            case ExpressionType.LessThan: op = negate ? ComparisonOperator.Ge : ComparisonOperator.Lt; break;
                            default: throw new NotImplementedException($"FetchXmlFilter.Expression: cannot handle expression of type {expression.NodeType}: {expression}");
                        }
                        return new FetchXmlFilter(entityName, ODataField.Field(binaryExpression.Left), op, value);
                    }

                case ExpressionType.Call:
                    var callExpression = (MethodCallExpression)expression;
                    if (callExpression.Object != null && callExpression.Arguments.Count == 1)
                    {
                        if (typeof(string).IsAssignableFrom(callExpression.Object.Type))
                        {
                            // entity.Field.Contains("value") -> field like '%value%')
                            var field = ODataField.Field(callExpression.Object);
                            var argumentExpression = ODataExpression.EvaluateExpression(callExpression.Arguments[0]);
                            switch (callExpression.Method.Name)
                            {
                                case nameof(string.Contains): return new FetchXmlFilter(entityName, field, negate ? ComparisonOperator.NotLike : ComparisonOperator.Like, "%" + argumentExpression + "%");
                                case nameof(string.EndsWith): return new FetchXmlFilter(entityName, field, negate ? ComparisonOperator.NotEndWith : ComparisonOperator.EndsWith, argumentExpression);
                                case nameof(string.StartsWith): return new FetchXmlFilter(entityName, field, negate ? ComparisonOperator.NotBeginWith : ComparisonOperator.BeginsWith, argumentExpression);
                            }
                        }
                        else if (callExpression.Object.Type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                        {
                            switch (callExpression.Method.Name)
                            {
                                case nameof(Enumerable.Contains):
                                    // list.Contains(entity.Field) -> field in list
                                    return BuildFetchXmlFilterForContainsCall(entityName, negate, callExpression.Object, callExpression.Arguments[0]);
                            }
                        }
                    }
                    else if (callExpression.Object == null && callExpression.Arguments.Count == 2)
                    {
                        if (callExpression.Method.DeclaringType == typeof(Enumerable))
                        {
                            switch (callExpression.Method.Name)
                            {
                                case nameof(Enumerable.Contains):
                                    // list.Contains(entity.Field) -> field in list
                                    return BuildFetchXmlFilterForContainsCall(entityName, negate, callExpression.Arguments[0], callExpression.Arguments[1]);
                            }
                        }
                    }
                    break;
            }

            if (expression.Type == typeof(bool))
            {
                return new FetchXmlFilter(entityName, ODataField.Field(expression), ComparisonOperator.Eq, negate ? "0" : "1");
            }

            throw new NotImplementedException($"FetchXmlFilter.Expression: cannot handle expression of type {expression.NodeType}: {expression}");
        }

        private static FetchXmlFilter BuildFetchXmlFilterForContainsCall(string entityName, bool negate, Expression containsSource, Expression containsArgument)
        {
            var list = ODataExpression.EvaluateExpression(containsSource) as IEnumerable;
            var values = list?.Cast<object>()?.ToArray();
            if (values == null || values.Length == 0)
            {
                throw new ArgumentNullException("source", $"FetchXmlFilter.Expression: Contains must not be called on null or empty lists: {containsSource}.Contains({containsArgument})");
            }
            return new FetchXmlFilter(entityName, ODataField.Field(containsArgument), negate ? ComparisonOperator.NotIn : ComparisonOperator.In, values);
        }
    }
}
