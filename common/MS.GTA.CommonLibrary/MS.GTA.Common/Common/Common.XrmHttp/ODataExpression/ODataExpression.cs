//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Xml;

    public struct ODataExpression
    {
        private string str;

        private ODataExpression(string str)
        {
            this.str = str;
        }

        /// <inheritdoc/>
        public override string ToString() => this.str;

        public override bool Equals(object obj) => obj is ODataExpression expression && this.str == expression.str;
        public override int GetHashCode() => EqualityComparer<string>.Default.GetHashCode(this.str);
        public static bool operator ==(ODataExpression a, ODataExpression b) => a.Equals(b);
        public static bool operator !=(ODataExpression a, ODataExpression b) => !a.Equals(b);

        /// <summary>
        /// Create a filter ODataExpression from a Linq expression.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="expression">The Linq expression.</param>
        /// <returns>The OData expression.</returns>
        /// <remarks>This is probably what you want.</remarks>
        public static ODataExpression Filter<T>(Expression<Func<T, bool>> expression) => Filter(expression?.Body, new Dictionary<Expression, bool>());

        #region Logical ops - note: prefer Filter above
        public static ODataExpression And(params ODataExpression[] values) => And(values as IEnumerable<ODataExpression>);
        public static ODataExpression And(IEnumerable<ODataExpression> values) => new ODataExpression($"({string.Join(") and (", values.Select(v => v.ToString()))})");
        public static ODataExpression Or(params ODataExpression[] values) => Or(values as IEnumerable<ODataExpression>);
        public static ODataExpression Or(IEnumerable<ODataExpression> values) => new ODataExpression($"({string.Join(") or (", values.Select(v => v.ToString()))})");
        public static ODataExpression Not(ODataExpression value) => new ODataExpression($"not ({value})");
        public ODataExpression And(ODataExpression value) => And(this, value);
        public ODataExpression Or(ODataExpression value) => Or(this, value);
        public ODataExpression Not() => Not(this);
        #endregion

        #region Comparison - note: prefer Filter above
        public static ODataExpression Eq(ODataExpression left, ODataExpression right) => new ODataExpression($"{left} eq {right}");
        public static ODataExpression Ne(ODataExpression left, ODataExpression right) => new ODataExpression($"{left} ne {right}");
        public static ODataExpression Gt(ODataExpression left, ODataExpression right) => new ODataExpression($"{left} gt {right}");
        public static ODataExpression Ge(ODataExpression left, ODataExpression right) => new ODataExpression($"{left} ge {right}");
        public static ODataExpression Lt(ODataExpression left, ODataExpression right) => new ODataExpression($"{left} lt {right}");
        public static ODataExpression Le(ODataExpression left, ODataExpression right) => new ODataExpression($"{left} le {right}");
        public static ODataExpression EqAny<TValue>(ODataExpression expression, IEnumerable<TValue> values)
        {
            if (!values.Any())
            {
                throw new ArgumentException("Contains list must have at least one value");
            }
            return Or(values.Select(v => Eq(expression, AnyValue(v))).ToArray());
        }

        public ODataExpression Eq(ODataExpression right) => Eq(this, right);
        public ODataExpression Ne(ODataExpression right) => Ne(this, right);
        public ODataExpression Gt(ODataExpression right) => Gt(this, right);
        public ODataExpression Ge(ODataExpression right) => Ge(this, right);
        public ODataExpression Lt(ODataExpression right) => Lt(this, right);
        public ODataExpression Le(ODataExpression right) => Le(this, right);
        public ODataExpression EqAny<TValue>(IEnumerable<TValue> values) => EqAny(this, values);
        #endregion

        #region Convert primitive values to expression - note: prefer Filter above
        public static implicit operator ODataExpression(ODataField value) => new ODataExpression(value.ToString());
        public static implicit operator ODataExpression(bool value) => new ODataExpression(value ? "true" : "false");
        public static implicit operator ODataExpression(int value) => new ODataExpression(value.ToString(CultureInfo.InvariantCulture));
        public static implicit operator ODataExpression(long value) => new ODataExpression(value.ToString(CultureInfo.InvariantCulture));
        public static implicit operator ODataExpression(DateTime value) => new ODataExpression(value.ToString("yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture));
        public static implicit operator ODataExpression(DateTimeOffset value) => new ODataExpression(value.ToString("yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture));
        public static implicit operator ODataExpression(TimeSpan value) => new ODataExpression($"duration'{XmlConvert.ToString(value)}'");
        public static implicit operator ODataExpression(Guid value) => new ODataExpression(value.ToString("D"));
        public static implicit operator ODataExpression(float value) => new ODataExpression(float.IsPositiveInfinity(value) ? "INF" : float.IsNegativeInfinity(value) ? "-INF" : float.IsNaN(value) ? "NaN" : value.ToString(CultureInfo.InvariantCulture));
        public static implicit operator ODataExpression(double value) => new ODataExpression(double.IsPositiveInfinity(value) ? "INF" : double.IsNegativeInfinity(value) ? "-INF" : double.IsNaN(value) ? "NaN" : value.ToString(CultureInfo.InvariantCulture));
        public static implicit operator ODataExpression(string value) => new ODataExpression(value == null ? "null" : $"'{value.Replace("'", "''")}'");

        public static ODataExpression Value(bool value) => new ODataExpression(value ? "true" : "false");
        public static ODataExpression Value(int value) => new ODataExpression(value.ToString(CultureInfo.InvariantCulture));
        public static ODataExpression Value(long value) => new ODataExpression(value.ToString(CultureInfo.InvariantCulture));
        public static ODataExpression Value(DateTime value) => new ODataExpression(value.ToString("yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture));
        public static ODataExpression Value(DateTimeOffset value) => new ODataExpression(value.ToString("yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture));
        public static ODataExpression Value(TimeSpan value) => new ODataExpression($"duration'{XmlConvert.ToString(value)}'");
        public static ODataExpression Value(Guid value) => new ODataExpression(value.ToString("D"));
        public static ODataExpression Value(float value) => new ODataExpression(float.IsPositiveInfinity(value) ? "INF" : float.IsNegativeInfinity(value) ? "-INF" : float.IsNaN(value) ? "NaN" : value.ToString(CultureInfo.InvariantCulture));
        public static ODataExpression Value(double value) => new ODataExpression(double.IsPositiveInfinity(value) ? "INF" : double.IsNegativeInfinity(value) ? "-INF" : double.IsNaN(value) ? "NaN" : value.ToString(CultureInfo.InvariantCulture));
        public static ODataExpression Value(string value, bool needsUriEscaping = false)
        {
            if (value == null)
            {
                return new ODataExpression("null");
            }

            value = value.Replace("'", "''");
            if (needsUriEscaping)
            {
                value = Uri.EscapeDataString(value);
            }

            return new ODataExpression($"'{value}'");
        }

        public static ODataExpression AnyValue(object value, bool needsUriEscaping = false) =>
            value == null ? new ODataExpression("null")
            : value is bool b ? Value(b)
            : value is int i ? Value(i)
            : value is long l ? Value(l)
            : value is DateTime dt ? Value(dt)
            : value is DateTimeOffset dto ? Value(dto)
            : value is TimeSpan t ? Value(t)
            : value is Guid g ? Value(g)
            : value is float f ? Value(f)
            : value is double d ? Value(d)
            : value is string s ? Value(s, needsUriEscaping)
            : value is Enum e ? Value(Convert.ToInt32(e))
            : throw new NotImplementedException($"ODataExpression.AnyValue not implemented for {value} of type {value.GetType()}");
        #endregion

        #region Misc
        public static ODataExpression Contains(ODataExpression left, ODataExpression right) => new ODataExpression($"contains({left}, {right})");
        public static ODataExpression EndsWith(ODataExpression left, ODataExpression right) => new ODataExpression($"endswith({left}, {right})");
        public static ODataExpression StartsWith(ODataExpression left, ODataExpression right) => new ODataExpression($"startswith({left}, {right})");
        public ODataExpression Contains(ODataExpression right) => Contains(this, right);
        public ODataExpression EndsWith(ODataExpression right) => EndsWith(this, right);
        public ODataExpression StartsWith(ODataExpression right) => StartsWith(this, right);
        #endregion

        #region Private
        private static ODataExpression Filter(Expression expression, Dictionary<Expression, bool> parmExprs)
        {
            if (expression == null)
            {
                return default(ODataExpression);
            }

            var binaryExpression = expression as BinaryExpression;

            switch (expression.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return And(Filter(binaryExpression.Left, parmExprs), Filter(binaryExpression.Right, parmExprs));

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return Or(Filter(binaryExpression.Left, parmExprs), Filter(binaryExpression.Right, parmExprs));

                case ExpressionType.Not:
                    return Not(Filter(((UnaryExpression)expression).Operand, parmExprs));

                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    string op = null;
                    switch (expression.NodeType)
                    {
                        case ExpressionType.Equal: op = "eq"; break;
                        case ExpressionType.NotEqual: op = "ne"; break;
                        case ExpressionType.GreaterThan: op = "gt"; break;
                        case ExpressionType.GreaterThanOrEqual: op = "ge"; break;
                        case ExpressionType.LessThan: op = "lt"; break;
                        case ExpressionType.LessThanOrEqual: op = "le"; break;
                    }
                    return new ODataExpression($"{Value(binaryExpression.Left, parmExprs)} {op} {Value(binaryExpression.Right, parmExprs)}");

                case ExpressionType.Call:
                    var callExpression = (MethodCallExpression)expression;
                    if (callExpression.Object != null && callExpression.Arguments.Count == 1)
                    {
                        if (typeof(string).IsAssignableFrom(callExpression.Object.Type))
                        {
                            // entity.Field.Contains("value") -> Contains(field, 'value')
                            var objectExpression = Value(callExpression.Object, parmExprs);
                            var argumentExpression = Value(callExpression.Arguments[0], parmExprs);
                            switch (callExpression.Method.Name)
                            {
                                case nameof(string.Contains): return Contains(objectExpression, argumentExpression);
                                case nameof(string.EndsWith): return EndsWith(objectExpression, argumentExpression);
                                case nameof(string.StartsWith): return StartsWith(objectExpression, argumentExpression);
                            }
                        }
                        else if (callExpression.Object.Type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                            && !ExpressionContainsParameter(callExpression.Object, parmExprs))
                        {
                            switch (callExpression.Method.Name)
                            {
                                case nameof(Enumerable.Contains):
                                    // list.Contains(entity.Field) -> (field eq list[0]) or (field eq list[1]) or ...
                                    return EqAny(Value(callExpression.Arguments[0], parmExprs), ((IEnumerable)EvaluateExpression(callExpression.Object)).OfType<object>());
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
                                    // list.Contains(entity.Field) -> (field eq list[0]) or (field eq list[1]) or ...
                                    return EqAny(Value(callExpression.Arguments[1], parmExprs), ((IEnumerable)EvaluateExpression(callExpression.Arguments[0])).OfType<object>());
                            }
                        }
                    }
                    break;
            }

            return Value(expression, parmExprs);
        }

        private static ODataExpression Value(Expression expression, Dictionary<Expression, bool> parmExprs)
        {
            if (expression is ConstantExpression constantExpression)
            {
                return AnyValue(constantExpression.Value);
            }

            if (!ExpressionContainsParameter(expression, parmExprs))
            {
                return AnyValue(EvaluateExpression(expression));
            }

            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return Value(((UnaryExpression)expression).Operand, parmExprs);

                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)expression;
                    if (memberExpression.Expression.NodeType == ExpressionType.Parameter)
                    {
                        // entity.Field -> field
                        return new ODataExpression(ODataEntityContractInfo.GetPropertyNameFromMemberInfo(memberExpression.Member));
                    }

                    // Convert the base expression first.
                    var containingObjectExpression = Value(memberExpression.Expression, parmExprs);

                    if (memberExpression.Member.DeclaringType.IsGenericType
                        && memberExpression.Member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>)
                        && memberExpression.Member.Name == "Value")
                    {
                        // foo.Value -> foo
                        return containingObjectExpression;
                    }

                    // ...Base.Field -> ...base/field
                    return new ODataExpression(containingObjectExpression + "/" + ODataEntityContractInfo.GetPropertyNameFromMemberInfo(memberExpression.Member));

                case ExpressionType.Call:
                    var callExpression = (MethodCallExpression)expression;
                    if (callExpression.Object != null && callExpression.Arguments.Count == 0 && typeof(object).IsAssignableFrom(callExpression.Object.Type))
                    {
                        // entity.Field.ToString() -> field
                        var objectExpression = Value(callExpression.Object, parmExprs);
                        switch (callExpression.Method.Name)
                        {
                            case nameof(object.ToString): return objectExpression;
                        }
                    }
                    break;
            }

            throw new NotImplementedException($"ODataExpression.Value: cannot handle expression of type {expression.NodeType}: {expression}");
        }

        internal static object EvaluateExpression(Expression expression)
        {
            var func = Expression
                .Lambda<Func<object>>(
                    expression.Type.IsValueType
                        ? Expression.Convert(expression, typeof(object))
                        : expression)
                .Compile();
            return func();
        }

        internal static bool ExpressionContainsParameter(Expression expression, Dictionary<Expression, bool> parmExprs)
        {
            if (expression is ParameterExpression parameterExpression)
            {
                return true;
            }

            if (expression == null || expression is ConstantExpression || expression is NewExpression)
            {
                return false;
            }

            if (parmExprs.TryGetValue(expression, out var ret))
            {
                return ret;
            }

            if (expression is MemberExpression memberExpression)
            {
                return parmExprs[expression] = ExpressionContainsParameter(memberExpression.Expression, parmExprs);
            }

            if (expression is UnaryExpression unaryExpression)
            {
                return parmExprs[expression] = ExpressionContainsParameter(unaryExpression.Operand, parmExprs);
            }

            if (expression is BinaryExpression binaryExpression)
            {
                return parmExprs[expression] =
                    ExpressionContainsParameter(binaryExpression.Left, parmExprs)
                    || ExpressionContainsParameter(binaryExpression.Right, parmExprs);
            }

            if (expression is MethodCallExpression callExpression)
            {
                return parmExprs[expression] =
                    ExpressionContainsParameter(callExpression.Object, parmExprs)
                    || callExpression.Arguments.Any(a => ExpressionContainsParameter(a, parmExprs));
            }

            throw new NotImplementedException($"ODataExpression.ExpressionContainsParameter: cannot handle expression of type {expression.NodeType}: {expression}");
        }
        #endregion
    }
}
