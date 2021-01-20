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
    using System.Xml.Linq;

    public interface IFetchXmlLinkEntity
    {
        string Alias { get; set; }

        string Name { get; }

        ODataField From { get; set; }

        ODataField To { get; set; }

        FetchXmlJoinOperator JoinOperator { get; set; }

        FetchXmlFilter Filter { get; set; }

        bool Aggregate { get; set; }

        bool SelectAllFields { get; set; }

        ICollection<FetchXmlAttribute> SelectFields { get; }

        ICollection<Tuple<ODataField, FetchXmlOrderBy>> OrderByFields { get; }

        ICollection<IFetchXmlLinkEntity> LinkEntities { get; }

        XElement ToXElement();
    }

    public class FetchXmlLinkEntity<T> : IFetchXmlLinkEntity
    {
        public string Alias { get; set; }

        public string Name { get; } = ODataEntityContractInfo.GetEntitySingularName(typeof(T));

        public ODataField From { get; set; }

        public ODataField To { get; set; }

        public FetchXmlJoinOperator JoinOperator { get; set; }

        public FetchXmlFilter Filter { get; set; }

        public bool Aggregate { get; set; }

        public bool SelectAllFields { get; set; }

        public ICollection<FetchXmlAttribute> SelectFields { get; } = new List<FetchXmlAttribute>();

        public ICollection<Tuple<ODataField, FetchXmlOrderBy>> OrderByFields { get; } = new List<Tuple<ODataField, FetchXmlOrderBy>>();

        public ICollection<IFetchXmlLinkEntity> LinkEntities { get; } = new List<IFetchXmlLinkEntity>();

        public override string ToString() => this.ToXElement().ToString(SaveOptions.DisableFormatting);

        public XElement ToXElement()
        {
            var element = new XElement("link-entity");

            element.SetAttributeValue("alias", this.Alias);
            element.SetAttributeValue("name", this.Name);
            element.SetAttributeValue("from", this.From.ToFetchXmlFieldName());
            element.SetAttributeValue("to", this.To.ToFetchXmlFieldName());

            switch (this.JoinOperator)
            {
                case FetchXmlJoinOperator.Inner:
                    element.SetAttributeValue("link-type", "inner");
                    break;
                case FetchXmlJoinOperator.LeftOuter:
                    element.SetAttributeValue("link-type", "outer");
                    break;
                case FetchXmlJoinOperator.Natural:
                    break;
            }

            if (this.Aggregate)
            {
                element.SetAttributeValue("aggregate", "true");
            }

            if (this.Filter != default(FetchXmlFilter))
            {
                element.Add(this.Filter.ToXElement());
            }

            if (this.SelectAllFields)
            {
                element.Add(new XElement("all-attributes"));
            }
            else
            {
                element.Add(this.SelectFields.Select(field => field.ToXElement()));
            }

            element.Add(this.OrderByFields.Select(field =>
                field.Item2 == FetchXmlOrderBy.Ascending
                    ? new XElement("order", new XAttribute("attribute", field.Item1.ToFetchXmlFieldName()))
                    : new XElement("order", new XAttribute("attribute", field.Item1.ToFetchXmlFieldName()), new XAttribute("descending", true))));

            element.Add(this.LinkEntities.Select(e => e.ToXElement()));

            return element;
        }

        public FetchXmlLinkEntity<T> AddSelectField(Expression<Func<T, object>> fieldExpression)
        {
            this.SelectFields.Add(new FetchXmlAttribute { Field = ODataField.Field(fieldExpression) });
            return this;
        }

        public FetchXmlLinkEntity<T> AddAggregateField<TResult>(Expression<Func<T, object>> fieldExpression, Expression<Func<TResult, object>> aliasExpression, FetchXmlAggregateType aggregateType, bool distinct = false)
        {
            this.SelectFields.Add(new FetchXmlAttribute
            {
                Field = ODataField.Field(fieldExpression),
                Alias = ODataField.Field(aliasExpression).ToString(),
                AggregateType = aggregateType,
                Distinct = distinct,
            });
            return this;
        }

        public FetchXmlLinkEntity<T> AddGroupByField<TResult>(Expression<Func<T, object>> fieldExpression, Expression<Func<TResult, object>> aliasExpression, FetchXmlDateGrouping? dateGrouping = null)
        {
            this.SelectFields.Add(new FetchXmlAttribute
            {
                Field = ODataField.Field(fieldExpression),
                Alias = ODataField.Field(aliasExpression).ToString(),
                GroupBy = true,
                DateGrouping = dateGrouping,
            });
            return this;
        }

        public FetchXmlLinkEntity<T> AddOrderByAscending(Expression<Func<T, object>> fieldExpression)
        {
            this.OrderByFields.Add(Tuple.Create(ODataField.Field(fieldExpression), FetchXmlOrderBy.Ascending));
            return this;
        }

        public FetchXmlLinkEntity<T> AddOrderByDescending(Expression<Func<T, object>> fieldExpression)
        {
            this.OrderByFields.Add(Tuple.Create(ODataField.Field(fieldExpression), FetchXmlOrderBy.Descending));
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
