//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;

    public struct ODataPath<T>
    {
        private readonly bool includeBaseAddress;
        private readonly string str;

        private ODataPath(string str, bool includeBaseAddress = true)
        {
            this.str = str;
            this.includeBaseAddress = includeBaseAddress;
        }

        public string ToUri(Uri baseAddress)
        {
            return this.ToUri(baseAddress.PathAndQuery);
        }

        public string ToUri(string baseAddress)
        {
            return includeBaseAddress ? baseAddress + this.str : this.str;
        }

        public string ToFullUri(Uri baseAddress)
        {
            return this.ToUri(baseAddress.ToString());
        }

        public override string ToString()
        {
            return this.ToUri("/");
        }

        /// <summary>
        /// Build an OData path like: `/base/JobOpenings'
        /// </summary>
        /// <returns>The OData path object.</returns>
        public static ODataPath<IEnumerable<T>> FromEntity()
        {
            var typeName = ODataEntityContractInfo.GetEntityPluralName(typeof(T));
            return new ODataPath<IEnumerable<T>>(typeName, includeBaseAddress: true);
        }

        /// <summary>
        /// Build an OData path like: `/base/JobOpenings'
        /// </summary>
        /// <returns>The OData path object.</returns>
        public static ODataPath<IEnumerable<T>> FromEntityName(string pluralName)
        {
            if (!XrmHttpClientUriSanitizer.IsValidODataIdentifier(pluralName))
            {
                throw new ArgumentException("entity name is not valid odata identifier", nameof(pluralName));
            }
            return new ODataPath<IEnumerable<T>>(pluralName, includeBaseAddress: true);
        }

        /// <summary>
        /// Build an OData path like: `/base/JobOpenings(75c65c10-8aa8-4ac3-9c70-e6ffeef17491)`
        /// </summary>
        /// <returns>The OData path object.</returns>
        public static ODataPath<T> FromId(Guid id)
        {
            var typeName = ODataEntityContractInfo.GetEntityPluralName(typeof(T));
            return new ODataPath<T>($"{typeName}({id})", includeBaseAddress: true);
        }

        /// <summary>
        /// Build an OData path like: `/base/JobOpenings(field1='value1', field2='value2')`
        /// </summary>
        /// <returns>The OData path object.</returns>
        public static ODataPath<T> FromKeyExpression(Expression<Func<T, bool>> expression)
        {
            return FromKeyFieldValues(ODataField.KeyFieldValues(expression));
        }

        /// <summary>
        /// Build an OData path like: `/base/JobOpenings(field1='value1', field2='value2')`
        /// </summary>
        /// <returns>The OData path object.</returns>
        public static ODataPath<T> FromKeyFieldValues(IEnumerable<Tuple<ODataField, object>> keyValues)
        {
            var typeName = ODataEntityContractInfo.GetEntityPluralName(typeof(T));
            return new ODataPath<T>($"{typeName}({BuildKeyString(keyValues)})", includeBaseAddress: true);
        }

        /// <summary>
        /// Build an OData path like: `$1` (for use in batches)
        /// </summary>
        /// <returns>The OData path object.</returns>
        public static ODataPath<T> FromBatchContentId(int id)
        {
            return new ODataPath<T>($"${id}", includeBaseAddress: false);
        }

        /// <summary>
        /// Build an OData path like: `[...]/Owner` (e.g. `$1/Owner` or `/base/JobOpenings(75c65c10-8aa8-4ac3-9c70-e6ffeef17491)/Owner`)
        /// </summary>
        /// <returns>The OData path object.</returns>
        public ODataPath<TChild> Child<TChild>(ODataField field)
        {
            return new ODataPath<TChild>($"{this.str}/{field}", includeBaseAddress: this.includeBaseAddress);
        }

        /// <summary>
        /// Build an OData path like: `[...]/Owner` (e.g. `$1/Owner` or `/base/JobOpenings(75c65c10-8aa8-4ac3-9c70-e6ffeef17491)/Owner`)
        /// </summary>
        /// <returns>The OData path object.</returns>
        public ODataPath<TChild> Child<TChild>(Expression<Func<T, TChild>> fieldExpression)
        {
            return this.Child<TChild>(ODataField.Field(fieldExpression));
        }

        /// <summary>
        /// Build an OData path like: `[...]/Participants` (e.g. `$1/Participants` or `/base/JobOpenings(75c65c10-8aa8-4ac3-9c70-e6ffeef17491)/Participants`)
        /// </summary>
        /// <returns>The OData path object.</returns>
        public ODataPath<IEnumerable<TChild>> Children<TChild>(Expression<Func<T, IEnumerable<TChild>>> fieldExpression)
        {
            return this.Child<IEnumerable<TChild>>(ODataField.Field(fieldExpression));
        }

        /// <summary>
        /// Build an OData path like: `[...]/Participants(ed123893-fc47-45cc-9450-b36e14890795)` (e.g. `/base/JobOpenings(75c65c10-8aa8-4ac3-9c70-e6ffeef17491)/Participants(ed123893-fc47-45cc-9450-b36e14890795)`)
        /// </summary>
        /// <returns>The OData path object.</returns>
        public ODataPath<TChild> ChildById<TChild>(Expression<Func<T, IEnumerable<TChild>>> fieldExpression, Guid id)
        {
            var childrenPath = this.Children(fieldExpression);
            return new ODataPath<TChild>($"{childrenPath.str}({id})", childrenPath.includeBaseAddress);
        }

        /// <summary>
        /// Build an OData path like: `[...]/Participants(field1='value1', field2='value2')` (e.g. `/base/JobOpenings(75c65c10-8aa8-4ac3-9c70-e6ffeef17491)/Participants(field1='value1', field2='value2')`)
        /// </summary>
        /// <returns>The OData path object.</returns>
        public ODataPath<TChild> ChildByKeyExpression<TChild>(Expression<Func<T, IEnumerable<TChild>>> fieldExpression, Expression<Func<TChild, bool>> expression)
        {
            return this.ChildByKeyFieldValues(fieldExpression, ODataField.KeyFieldValues(expression));
        }

        /// <summary>
        /// Build an OData path like: `[...]/Participants(field1='value1', field2='value2')` (e.g. `/base/JobOpenings(75c65c10-8aa8-4ac3-9c70-e6ffeef17491)/Participants(field1='value1', field2='value2')`)
        /// </summary>
        /// <returns>The OData path object.</returns>
        public ODataPath<TChild> ChildByKeyFieldValues<TChild>(Expression<Func<T, IEnumerable<TChild>>> fieldExpression, IEnumerable<Tuple<ODataField, object>> keyValues)
        {
            var childrenPath = this.Children(fieldExpression);
            return new ODataPath<TChild>($"{childrenPath.str}({BuildKeyString(keyValues)})", childrenPath.includeBaseAddress);
        }

        /// <summary>
        /// Build an OData path like: `[...]/Attributes(LogicalName='foo')/Microsoft.Dynamics.CRM.PicklistAttributeMetadata`
        /// </summary>
        /// <returns>The OData path object.</returns>
        public ODataPath<TCast> CastOne<TCast>()
        {
            var typeName = ODataEntityContractInfo.GetEntitySingularNameWithNamespace(typeof(TCast));
            return new ODataPath<TCast>($"{this.str}/{typeName}", includeBaseAddress: this.includeBaseAddress);
        }

        /// <summary>
        /// Build an OData path like: `[...]/Attributes/Microsoft.Dynamics.CRM.PicklistAttributeMetadata`
        /// </summary>
        /// <returns>The OData path object.</returns>
        public ODataPath<IEnumerable<TCast>> CastList<TCast>()
        {
            var typeName = ODataEntityContractInfo.GetEntitySingularNameWithNamespace(typeof(TCast));
            return new ODataPath<IEnumerable<TCast>>($"{this.str}/{typeName}", includeBaseAddress: this.includeBaseAddress);
        }

        private static string BuildKeyString(IEnumerable<Tuple<ODataField, object>> keyValues)
        {
            return string.Join(
                ",",
                keyValues.Select(kv =>
                    $"{kv.Item1}={ODataExpression.AnyValue(kv.Item2, needsUriEscaping: true)}"));
        }
    }
}
