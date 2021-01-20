// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public struct ODataField
    {
        private string str;

        private ODataField(string str)
        {
            this.str = str;
        }

        public override string ToString() => this.str;

        public string ToFetchXmlFieldName()
        {
            // Foreign key field names differ between OData and FetchXml
            // OData: _Foo_value
            // FetchXml: Foo
            // Thus, to support using the OData contracts to build FetchXml queries,
            // remove the _ prefix and _value suffix.

            string fieldName = this.str;
            if (fieldName.StartsWith("_") && fieldName.EndsWith("_value"))
            {
                fieldName = fieldName.Substring("_".Length, fieldName.Length - "_".Length - "_value".Length);
            }

            return fieldName.ToLowerInvariant();
        }

        public static ODataField KeyField<T>()
        {
            return ODataField.Field(ODataEntityContractInfo.GetKeyProperty(typeof(T)));
        }

        public static ODataField Field<T>(Expression<Func<T, object>> expression)
        {
            return expression == null ? default(ODataField) : Field(expression.Body);
        }

        public static ODataField Field<T, TField>(Expression<Func<T, TField>> expression)
        {
            return expression == null ? default(ODataField) : Field(expression.Body);
        }

        public static ODataField Field(MemberInfo memberInfo)
        {
            return memberInfo == null
                ? default(ODataField)
                : new ODataField(ODataEntityContractInfo.GetPropertyNameFromMemberInfo(memberInfo));
        }

        public static ODataField FieldByName(string fieldName)
        {
            if (!XrmHttpClientUriSanitizer.IsValidODataIdentifier(fieldName))
            {
                throw new ArgumentException("field name is not valid odata identifier", nameof(fieldName));
            }
            return new ODataField(fieldName);
        }

        public ODataExpression And(ODataExpression value) => ODataExpression.And(this, value);
        public ODataExpression Or(ODataExpression value) => ODataExpression.Or(this, value);
        public ODataExpression Not() => ODataExpression.Not(this);
        public ODataExpression Eq(ODataExpression right) => ODataExpression.Eq(this, right);
        public ODataExpression Ne(ODataExpression right) => ODataExpression.Ne(this, right);
        public ODataExpression Gt(ODataExpression right) => ODataExpression.Gt(this, right);
        public ODataExpression Ge(ODataExpression right) => ODataExpression.Ge(this, right);
        public ODataExpression Lt(ODataExpression right) => ODataExpression.Lt(this, right);
        public ODataExpression Le(ODataExpression right) => ODataExpression.Le(this, right);
        public ODataExpression EqAny<TValue>(IEnumerable<TValue> values) => ODataExpression.EqAny(this, values);
        public ODataExpression Contains(ODataExpression right) => ODataExpression.Contains(this, right);
        public ODataExpression EndsWith(ODataExpression right) => ODataExpression.EndsWith(this, right);
        public ODataExpression StartsWith(ODataExpression right) => ODataExpression.StartsWith(this, right);

        public static IEnumerable<ODataField> Fields<T>(Expression<Func<T, object>> expression)
        {
            return expression == null ? null : Fields(expression.Body);
        }

        public static IEnumerable<ODataField> InitializationFields<T>(Expression<Func<T>> expression)
            where T : class
        {
            // TODO: validate all the stuff
            switch (expression.Body.NodeType)
            {
                case ExpressionType.MemberInit:
                    return ((MemberInitExpression)expression.Body).Bindings.Select(b => Field(b.Member)).ToArray();
            }

            throw new NotImplementedException($"InitializationFields: cannot handle expression of type {expression.NodeType}: {expression}");
        }

        public static IEnumerable<Tuple<ODataField, object>> KeyFieldValues<T>(Expression<Func<T, bool>> keyExpression)
        {
            return KeyFieldValues(keyExpression.Body);
        }

        private static IEnumerable<ODataField> Fields(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }

            // TODO: validate all the stuff
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return Fields(((UnaryExpression)expression).Operand);

                case ExpressionType.NewArrayInit:
                    return ((NewArrayExpression)expression).Expressions.Select(e => Field(e)).ToArray();

                case ExpressionType.New:
                    return ((NewExpression)expression).Arguments.Select(e => Field(e)).ToArray();

                case ExpressionType.MemberAccess:
                    return new[] { new ODataField(ODataEntityContractInfo.GetPropertyNameFromMemberInfo(((MemberExpression)expression).Member)) };
            }

            throw new NotImplementedException($"GetFieldNames: cannot handle expression of type {expression.NodeType}: {expression}");
        }

        internal static ODataField Field(Expression expression)
        {
            if (expression == null)
            {
                return default(ODataField);
            }

            expression = StripConversions(expression);
            if (expression is MemberExpression memberExpression)
            {
                return Field(memberExpression.Member);
            }

            throw new NotImplementedException($"Field: cannot handle expression of type {expression.NodeType}: {expression}");
        }

        private static IEnumerable<Tuple<ODataField, object>> KeyFieldValues(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }

            if (expression is BinaryExpression binaryExpression)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                        return KeyFieldValues(binaryExpression.Left).Concat(KeyFieldValues(binaryExpression.Right));

                    case ExpressionType.Equal:
                        var left = StripConversions(binaryExpression.Left);
                        if (left is MemberExpression leftMember)
                        {
                            var field = Field(leftMember.Member);
                            var value = ODataExpression.EvaluateExpression(binaryExpression.Right);
                            return new[] { Tuple.Create(field, value) };
                        }

                        var right = StripConversions(binaryExpression.Right);
                        if (right is MemberExpression rightMember)
                        {
                            var field = Field(rightMember.Member);
                            var value = ODataExpression.EvaluateExpression(binaryExpression.Left);
                            return new[] { Tuple.Create(field, value) };
                        }

                        break;
                }
            }

            throw new NotImplementedException($"KeyFieldValues: cannot handle expression of type {expression.NodeType}: {expression}");
        }

        private static Expression StripConversions(Expression expression)
        {
            while (true)
            {
                if (expression.NodeType == ExpressionType.Convert)
                {
                    expression = ((UnaryExpression)expression).Operand;
                }
                else if (expression.NodeType == ExpressionType.MemberAccess
                    && expression is MemberExpression memberExpression
                    && Nullable.GetUnderlyingType(memberExpression.Expression.Type) != null
                    && memberExpression.Member.Name == "Value")
                {
                    // Strip the .Value from foo.Value
                    expression = memberExpression.Expression;
                }
                else
                {
                    break;
                }
            }

            return expression;
        }
    }
}
