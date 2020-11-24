// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Xml.Linq;

    public class FetchXmlQuery<T>
    {
        public FetchXmlFilter Filter { get; set; }

        public int? Count { get; set; }

        public bool Aggregate { get; set; }

        public bool SelectAllFields { get; set; }

        public bool Distinct { get; set; }

        public ICollection<FetchXmlAttribute> SelectFields { get; } = new List<FetchXmlAttribute>();

        public ICollection<Tuple<ODataField, FetchXmlOrderBy>> OrderByFields { get; } = new List<Tuple<ODataField, FetchXmlOrderBy>>();

        public ICollection<Tuple<ODataField, FetchXmlOrderBy>> AggregateOrderByFields { get; } = new List<Tuple<ODataField, FetchXmlOrderBy>>();

        public ICollection<IFetchXmlLinkEntity> LinkEntities { get; } = new List<IFetchXmlLinkEntity>();

        public FetchXmlQuery(
            FetchXmlFilter filter = default(FetchXmlFilter),
            int? count = null,
            bool aggregate = false,
            bool selectAllFields = false,
            bool distinct = false)
        {
            this.Filter = filter;
            this.Count = count;
            this.Aggregate = aggregate;
            this.SelectAllFields = selectAllFields;
            this.Distinct = distinct;
        }

        public FetchXmlQuery(
            Expression<Func<T, bool>> filter,
            int? count = null,
            bool aggregate = false,
            bool selectAllFields = false,
            bool distinct = false)
        {
            this.Filter = FetchXmlFilter.Filter(filter);
            this.Count = count;
            this.Aggregate = aggregate;
            this.SelectAllFields = selectAllFields;
            this.Distinct = distinct;
        }

        public override string ToString() => this.ToXElement().ToString(SaveOptions.DisableFormatting);

        public XElement ToXElement()
        {
            var entityElement = new XElement("entity", new XAttribute("name", ODataEntityContractInfo.GetEntitySingularName(typeof(T))));

            if (this.Filter != default(FetchXmlFilter))
            {
                entityElement.Add(this.Filter.ToXElement());
            }

            if (this.SelectAllFields)
            {
                entityElement.Add(new XElement("all-attributes"));
            }
            else
            {
                entityElement.Add(this.SelectFields.Select(field => field.ToXElement()));
            }

            entityElement.Add(this.OrderByFields.Select(field =>
                field.Item2 == FetchXmlOrderBy.Ascending
                    ? new XElement("order", new XAttribute("attribute", field.Item1.ToFetchXmlFieldName()))
                    : new XElement("order", new XAttribute("attribute", field.Item1.ToFetchXmlFieldName()), new XAttribute("descending", true))));

            entityElement.Add(this.AggregateOrderByFields.Select(field =>
                field.Item2 == FetchXmlOrderBy.Ascending
                    ? new XElement("order", new XAttribute("alias", field.Item1.ToFetchXmlFieldName()))
                    : new XElement("order", new XAttribute("alias", field.Item1.ToFetchXmlFieldName()), new XAttribute("descending", true))));

            entityElement.Add(this.LinkEntities.Select(e => e.ToXElement()));

            var fetchElement = new XElement("fetch", entityElement);

            if (this.Count != null)
            {
                fetchElement.SetAttributeValue("count", this.Count.Value);
            }

            if (this.Aggregate)
            {
                fetchElement.SetAttributeValue("aggregate", "true");
            }

            if (this.Distinct)
            {
                fetchElement.SetAttributeValue("distinct", "true");
            }

            return fetchElement;
        }

        public FetchXmlQuery<T> AddSelectField(Expression<Func<T, object>> fieldExpression)
        {
            this.SelectFields.Add(new FetchXmlAttribute { Field = ODataField.Field(fieldExpression) });
            return this;
        }

        public FetchXmlQuery<T> AddAggregateField<TResult>(Expression<Func<T, object>> fieldExpression, Expression<Func<TResult, object>> aliasExpression, FetchXmlAggregateType aggregateType, bool distinct = false)
        {
            if (!this.Aggregate)
            {
                throw new InvalidOperationException("AddAggregateField can only be used with aggregate queries");
            }

            this.SelectFields.Add(new FetchXmlAttribute
            {
                Field = ODataField.Field(fieldExpression),
                Alias = ODataField.Field(aliasExpression).ToString(),
                AggregateType = aggregateType,
                Distinct = distinct,
            });
            return this;
        }

        public FetchXmlQuery<T> AddGroupByField<TResult>(Expression<Func<T, object>> fieldExpression, Expression<Func<TResult, object>> aliasExpression, FetchXmlDateGrouping? dateGrouping = null)
        {
            if (!this.Aggregate)
            {
                throw new InvalidOperationException("AddGroupByField can only be used with aggregate queries");
            }

            this.SelectFields.Add(new FetchXmlAttribute
            {
                Field = ODataField.Field(fieldExpression),
                Alias = ODataField.Field(aliasExpression).ToString(),
                GroupBy = true,
                DateGrouping = dateGrouping,
            });
            return this;
        }

        public FetchXmlQuery<T> AddOrderByAscending(Expression<Func<T, object>> fieldExpression)
        {
            if (this.Aggregate)
            {
                throw new InvalidOperationException("Use AddAggregateOrderByAscending for aggregate queries");
            }

            this.OrderByFields.Add(Tuple.Create(ODataField.Field(fieldExpression), FetchXmlOrderBy.Ascending));
            return this;
        }

        public FetchXmlQuery<T> AddOrderByDescending(Expression<Func<T, object>> fieldExpression)
        {
            if (this.Aggregate)
            {
                throw new InvalidOperationException("Use AddAggregateOrderByDescending for aggregate queries");
            }

            this.OrderByFields.Add(Tuple.Create(ODataField.Field(fieldExpression), FetchXmlOrderBy.Descending));
            return this;
        }

        public FetchXmlQuery<T> AddAggregateOrderByAscending<TResult>(Expression<Func<TResult, object>> fieldExpression)
        {
            if (!this.Aggregate)
            {
                throw new InvalidOperationException("Use AddOrderByAscending for non-aggregate queries");
            }

            this.AggregateOrderByFields.Add(Tuple.Create(ODataField.Field(fieldExpression), FetchXmlOrderBy.Ascending));
            return this;
        }

        public FetchXmlQuery<T> AddAggregateOrderByDescending<TResult>(Expression<Func<TResult, object>> fieldExpression)
        {
            if (!this.Aggregate)
            {
                throw new InvalidOperationException("Use AddOrderByDescending for non-aggregate queries");
            }

            this.AggregateOrderByFields.Add(Tuple.Create(ODataField.Field(fieldExpression), FetchXmlOrderBy.Descending));
            return this;
        }

        public FetchXmlLinkEntity<TAdded> AddInnerJoin<TAdded>(
                Expression<Func<T, Guid?>> field,
                Expression<Func<TAdded, Guid?>> fieldAdded,
                Expression<Func<TAdded, bool>> filter = null,
                string alias = null,
                bool selectAllFields = false,
                Expression<Func<TAdded, object>> select = null)
            where TAdded : ODataEntity
        {
            return this.AddJoin<TAdded>(ODataField.Field(fieldAdded), ODataField.Field(field), FetchXmlJoinOperator.Inner, FetchXmlFilter.Filter(filter), alias, selectAllFields, ODataField.Fields(select));
        }

        public FetchXmlLinkEntity<TAdded> AddInnerJoinManyToOne<TAdded>(
                Expression<Func<T, Guid?>> field,
                Expression<Func<TAdded, bool>> filter = null,
                string alias = null,
                bool selectAllFields = false,
                Expression<Func<TAdded, object>> select = null)
            where TAdded : ODataEntity
        {
            return this.AddJoinManyToOne(field, FetchXmlJoinOperator.Inner, filter, alias, selectAllFields, select);
        }

        public FetchXmlLinkEntity<TAdded> AddInnerJoinOneToMany<TAdded>(
                Expression<Func<TAdded, Guid?>> fieldAdded,
                Expression<Func<TAdded, bool>> filter = null,
                string alias = null,
                bool selectAllFields = false,
                Expression<Func<TAdded, object>> select = null)
            where TAdded : ODataEntity
        {
            return this.AddJoinOneToMany(fieldAdded, FetchXmlJoinOperator.Inner, filter, alias, selectAllFields, select);
        }

        public FetchXmlLinkEntity<TAdded> AddOuterJoin<TAdded>(
                Expression<Func<T, Guid?>> field,
                Expression<Func<TAdded, Guid?>> fieldAdded,
                Expression<Func<TAdded, bool>> filter = null,
                string alias = null,
                bool selectAllFields = false,
                Expression<Func<TAdded, object>> select = null)
            where TAdded : ODataEntity
        {
            return this.AddJoin<TAdded>(ODataField.Field(fieldAdded), ODataField.Field(field), FetchXmlJoinOperator.LeftOuter, FetchXmlFilter.Filter(filter), alias, selectAllFields, ODataField.Fields(select));
        }

        public FetchXmlLinkEntity<TAdded> AddOuterJoinManyToOne<TAdded>(
                Expression<Func<T, Guid?>> field,
                Expression<Func<TAdded, bool>> filter = null,
                string alias = null,
                bool selectAllFields = false,
                Expression<Func<TAdded, object>> select = null)
            where TAdded : ODataEntity
        {
            return this.AddJoinManyToOne(field, FetchXmlJoinOperator.LeftOuter, filter, alias, selectAllFields, select);
        }

        public FetchXmlLinkEntity<TAdded> AddOuterJoinOneToMany<TAdded>(
                Expression<Func<TAdded, Guid?>> fieldAdded,
                Expression<Func<TAdded, bool>> filter = null,
                string alias = null,
                bool selectAllFields = false,
                Expression<Func<TAdded, object>> select = null)
            where TAdded : ODataEntity
        {
            return this.AddJoinOneToMany(fieldAdded, FetchXmlJoinOperator.LeftOuter, filter, alias, selectAllFields, select);
        }

        public FetchXmlLinkEntity<TAdded> AddParentLinkEntity<TAdded>(
                Expression<Func<T, TAdded>> referenceToParent,
                FetchXmlJoinOperator joinOperator = FetchXmlJoinOperator.Inner,
                FetchXmlFilter filter = default(FetchXmlFilter),
                string alias = null,
                bool selectAllFields = false,
                Expression<Func<TAdded, object>> select = null)
            where TAdded : ODataEntity
        {
            var keyProperty = ODataEntityContractInfo.GetKeyProperty(typeof(TAdded))
                ?? throw new ArgumentException($"Type {typeof(TAdded).FullName} does not have a property marked with the Key attribute");
            return this.AddJoin<TAdded>(ODataField.Field(keyProperty), ODataField.Field(referenceToParent), joinOperator, filter, alias, selectAllFields, ODataField.Fields(select));
        }

        public FetchXmlLinkEntity<TAdded> AddChildrenLinkEntity<TAdded>(
                Expression<Func<TAdded, T>> referenceFromChild,
                FetchXmlJoinOperator joinOperator = FetchXmlJoinOperator.Inner,
                FetchXmlFilter filter = default(FetchXmlFilter),
                string alias = null,
                bool selectAllFields = false,
                Expression<Func<TAdded, object>> select = null)
            where TAdded : ODataEntity
        {
            var keyProperty = ODataEntityContractInfo.GetKeyProperty(typeof(T))
                ?? throw new ArgumentException($"Type {typeof(T).FullName} does not have a property marked with the Key attribute");
            return this.AddJoin<TAdded>(ODataField.Field(referenceFromChild), ODataField.Field(keyProperty), joinOperator, filter, alias, selectAllFields, ODataField.Fields(select));
        }

        private FetchXmlLinkEntity<TAdded> AddJoin<TAdded>(
                ODataField from,
                ODataField to,
                FetchXmlJoinOperator joinOperator,
                FetchXmlFilter filter,
                string alias,
                bool selectAllFields,
                IEnumerable<ODataField> select)
            where TAdded : ODataEntity
        {
            var linkEntity = new FetchXmlLinkEntity<TAdded>()
            {
                From = from,
                To = to,
                JoinOperator = joinOperator,
                Filter = filter,
                Alias = alias,
                SelectAllFields = selectAllFields,
            };
            if (select != null)
            {
                foreach (var field in select)
                {
                    linkEntity.SelectFields.Add(new FetchXmlAttribute { Field = field });
                }
            }
            this.LinkEntities.Add(linkEntity);
            return linkEntity;
        }

        private FetchXmlLinkEntity<TAdded> AddJoinOneToMany<TAdded>(
                Expression<Func<TAdded, Guid?>> fieldAdded,
                FetchXmlJoinOperator joinOperator,
                Expression<Func<TAdded, bool>> filter,
                string alias,
                bool selectAllFields,
                Expression<Func<TAdded, object>> select)
            where TAdded : ODataEntity
        {
            var keyField = ODataEntityContractInfo.GetKeyProperty(typeof(T))
                ?? throw new ArgumentException($"Type {typeof(T).FullName} does not have a property marked with the Key attribute");
            return this.AddJoin<TAdded>(ODataField.Field(fieldAdded), ODataField.Field(keyField), joinOperator, FetchXmlFilter.Filter(filter), alias, selectAllFields, ODataField.Fields(select));
        }

        private FetchXmlLinkEntity<TAdded> AddJoinManyToOne<TAdded>(
                Expression<Func<T, Guid?>> field,
                FetchXmlJoinOperator joinOperator,
                Expression<Func<TAdded, bool>> filter,
                string alias,
                bool selectAllFields,
                Expression<Func<TAdded, object>> select)
            where TAdded : ODataEntity
        {
            var keyField = ODataEntityContractInfo.GetKeyProperty(typeof(TAdded))
                ?? throw new ArgumentException($"Type {typeof(T).FullName} does not have a property marked with the Key attribute");
            return this.AddJoin<TAdded>(ODataField.Field(keyField), ODataField.Field(field), joinOperator, FetchXmlFilter.Filter(filter), alias, selectAllFields, ODataField.Fields(select));
        }
    }
}
