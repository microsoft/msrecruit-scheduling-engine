// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using MS.GTA.Common.XrmHttp;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IXrmHttpClient
    {
        /// <summary>
        /// Refresh the security token for sending requests to the XRM environment.
        /// </summary>
        /// <returns>The updated <c>XRMHttpClient</c>.</returns>
        Task<IXrmHttpClient> RefreshToken();

        /// <summary>
        /// Constructs a new batch.
        /// </summary>
        /// <returns>A new instance of a class implementing the <see cref="IXrmHttpClientBatch"/> interface.</returns>
        IXrmHttpClientBatch NewBatch();

        /// <summary>
        /// Constructs a new batch that automatically executes chuncks of actions if there are too many actions in the batch.
        /// </summary>
        /// <param name="maximumActionsPerBatch">The maximum number of actions per batch; optional.</param>
        /// <returns>A new instance of a class implementing the <see cref="IXrmHttpClientBatch"/> interface.</returns>
        IXrmHttpClientBatch NewAutoflushingBatch(int maximumActionsPerBatch = 50);

        #region Get

        /// <summary>
        /// Get an entity by id.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="selectFields">The select list.</param>
        /// <param name="expandFields">The expand list; optional - use `.Expand` instead to set the select list.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> Get<T>(Guid id, IEnumerable<ODataField> selectFields, IEnumerable<ODataField> expandFields)
            where T : ODataEntity;

        /// <summary>
        /// Get an entity by id.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="select">The select list.</param>
        /// <param name="expand">The expand list; optional - use `.Expand` instead to set the select list.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> Get<T>(Guid id, Expression<Func<T, object>> select = null, Expression<Func<T, object>> expand = null)
            where T : ODataEntity;

        /// <summary>
        /// Get an entity by key.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="keyValues">The key field names and values.</param>
        /// <param name="selectFields">The select list.</param>
        /// <param name="expandFields">The expand list; optional - use `.Expand` instead to set the select list.</param>
        /// <returns>The query object.</returns>
        /// <example>
        /// Get(new[] { Tuple(ODataField.Field(entity => entity.Key1), 'value') })
        /// OR Get(ODataField.KeyFieldValues(entity => entity.Key1 == 'value1')).
        /// </example>
        IXrmHttpClientQuery<T> Get<T>(IEnumerable<Tuple<ODataField, object>> keyValues, IEnumerable<ODataField> selectFields, IEnumerable<ODataField> expandFields)
            where T : ODataEntity;

        /// <summary>
        /// Get an entity by key.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="select">The select list.</param>
        /// <param name="expand">The expand list; optional - use `.Expand` instead to set the select list.</param>
        /// <returns>The query object.</returns>
        /// <example>
        /// Get(entity => entity.Key1 == 'value1' &amp;&amp; entity.Key2 == 'value2')
        /// </example>
        IXrmHttpClientQuery<T> Get<T>(Expression<Func<T, bool>> keyExpression, Expression<Func<T, object>> select = null, Expression<Func<T, object>> expand = null)
            where T : ODataEntity;

        /// <summary>
        /// Get an entity at an OData path.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The OData path to the entity.</param>
        /// <param name="selectFields">The select list.</param>
        /// <param name="expandFields">The expand list; optional - use `.Expand` instead to set the select list.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> GetAtPath<T>(ODataPath<T> path, IEnumerable<ODataField> selectFields, IEnumerable<ODataField> expandFields)
            where T : ODataEntity;

        /// <summary>
        /// Get an entity at an OData path.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The OData path to the entity.</param>
        /// <param name="select">The select list.</param>
        /// <param name="expand">The expand list; optional - use `.Expand` instead to set the select list.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> GetAtPath<T>(ODataPath<T> path, Expression<Func<T, object>> select = null, Expression<Func<T, object>> expand = null)
            where T : ODataEntity;

        #endregion

        #region Get all

        /// <summary>
        /// Get a list of entity records.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="filterExpression">The filter expression.</param>
        /// <param name="selectFields">The select list.</param>
        /// <param name="expandFields">The expand list; optional - use `.Expand` instead to set the select list.</param>
        /// <param name="orderBy">The order by fields; optional - use `.OrderByAscending` for simpler writing.</param>
        /// <param name="maxpagesize">The number of records to return in a page; optional.</param>
        /// <param name="count">Whether to return the total number of records; default no.</param>
        /// <param name="top">The number of records to return; optional.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<ODataResponseList<T>> GetAll<T>(
            ODataExpression filterExpression = default(ODataExpression),
            IEnumerable<ODataField> selectFields = null,
            IEnumerable<ODataField> expandFields = null,
            IEnumerable<Tuple<string, bool>> orderBy = null,
            int maxpagesize = XrmHttpClient.DefaultPageSize,
            bool count = false,
            int top = 0)
            where T : ODataEntity;

        /// <summary>
        /// Get a list of entity records.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="filter">The filter expression.</param>
        /// <param name="select">The select list.</param>
        /// <param name="expand">The expand list; optional - use `.Expand` instead to set the select list.</param>
        /// <param name="maxpagesize">The number of records to return in a page; optional.</param>
        /// <param name="count">Whether to return the total number of records; default no.</param>
        /// <param name="top">The number of records to return; optional.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<ODataResponseList<T>> GetAll<T>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, object>> select = null,
            Expression<Func<T, object>> expand = null,
            int maxpagesize = XrmHttpClient.DefaultPageSize,
            bool count = false,
            int top = 0)
            where T : ODataEntity;

        /// <summary>
        /// Get a list of entity records at the path specified.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The OData path to the list.</param>
        /// <param name="filterExpression">The filter expression.</param>
        /// <param name="selectFields">The select list.</param>
        /// <param name="expandFields">The expand list; optional - use `.Expand` instead to set the select list.</param>
        /// <param name="orderBy">The order by fields; optional - use `.OrderByAscending` for simpler writing.</param>
        /// <param name="maxpagesize">The number of records to return in a page; optional.</param>
        /// <param name="count">Whether to return the total number of records; default no.</param>
        /// <param name="top">The number of records to return; optional.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<ODataResponseList<T>> GetAllAtPath<T>(
            ODataPath<IEnumerable<T>> path,
            ODataExpression filterExpression = default(ODataExpression),
            IEnumerable<ODataField> selectFields = null,
            IEnumerable<ODataField> expandFields = null,
            IEnumerable<Tuple<string, bool>> orderBy = null,
            int maxpagesize = XrmHttpClient.DefaultPageSize,
            bool count = false,
            int top = 0)
            where T : ODataEntity;

        /// <summary>
        /// Get a list of entity records at the path specified.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The OData path to the list.</param>
        /// <param name="filter">The filter expression.</param>
        /// <param name="select">The select list.</param>
        /// <param name="expand">The expand list; optional - use `.Expand` instead to set the select list.</param>
        /// <param name="maxpagesize">The number of records to return in a page; optional.</param>
        /// <param name="count">Whether to return the total number of records; default no.</param>
        /// <param name="top">The number of records to return; optional.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<ODataResponseList<T>> GetAllAtPath<T>(
            ODataPath<IEnumerable<T>> path,
            Expression<Func<T, bool>> filter,
            Expression<Func<T, object>> select = null,
            Expression<Func<T, object>> expand = null,
            int maxpagesize = XrmHttpClient.DefaultPageSize,
            bool count = false,
            int top = 0)
            where T : ODataEntity;

        #endregion

        #region Get all with Xml filter

        /// <summary>
        /// Get all records matching the fetchXml query.
        /// </summary>
        /// <typeparam name="TQuery">The entity type to query against.</typeparam>
        /// <typeparam name="TResponse">The response contract type.</typeparam>
        /// <param name="fetchXmlQuery">The fetchXml query.</param>
        /// <returns>The query object.</returns>
        /// <note>TResponse can differ from TQuery if aggregation or joins are used.</note>
        IXrmHttpClientQuery<ODataResponseList<TResponse>> GetAllWithFetchXml<TQuery, TResponse>(FetchXmlQuery<TQuery> fetchXmlQuery);

        /// <summary>
        /// Get all records matching the fetchXml filter.
        /// </summary>
        /// <typeparam name="TQuery">The entity type to query against.</typeparam>
        /// <typeparam name="TResponse">The response contract type.</typeparam>
        /// <param name="fetchXml">The fetchXml filter.</param>
        /// <returns>The query object.</returns>
        /// <note>TResponse can differ from TQuery if aggregation or joins are used.</note>
        IXrmHttpClientQuery<ODataResponseList<TResponse>> GetAllWithFetchXml<TQuery, TResponse>(string fetchXml);

        #endregion

        #region Get next page

        /// <summary>
        /// Get the next page of a list of entity records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="previousPage">The previously retrieved page.</param>
        /// <param name="maxpagesize">The maximum page size.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<ODataResponseList<T>> GetNextPage<T>(
                ODataResponseList<T> previousPage,
                int maxpagesize = XrmHttpClient.DefaultPageSize)
            where T : ODataEntity;

        #endregion

        #region Create

        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity object.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction Create<T>(T entity)
            where T : ODataEntity;

        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity list.</param>
        /// <param name="entity">The entity object.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction CreateAtPath<T>(ODataPath<IEnumerable<T>> path, T entity)
            where T : ODataEntity;

        #endregion

        #region CreateAndReturn

        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity object.</param>
        /// <param name="selectFields">The fields to return.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> CreateAndReturn<T>(T entity, IEnumerable<ODataField> selectFields = null)
            where T : ODataEntity;

        /// <summary>
        /// Get an entity by id.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity object.</param>
        /// <param name="select">The select list.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> CreateAndReturn<T>(T entity, Expression<Func<T, object>> select)
            where T : ODataEntity;

        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity list.</param>
        /// <param name="entity">The entity object.</param>
        /// <param name="selectFields">The fields to return.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> CreateAndReturnAtPath<T>(ODataPath<IEnumerable<T>> path, T entity, IEnumerable<ODataField> selectFields = null)
            where T : ODataEntity;

        /// <summary>
        /// Get an entity by id.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity list.</param>
        /// <param name="entity">The entity object.</param>
        /// <param name="select">The select list.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> CreateAndReturnAtPath<T>(ODataPath<IEnumerable<T>> path, T entity, Expression<Func<T, object>> select)
            where T : ODataEntity;

        #endregion

        #region Update

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fields">The field list.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction Update<T>(T entity, IEnumerable<ODataField> fields, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fieldsExpression">The field list.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction Update<T>(T entity, Expression<Func<T, object>> fieldsExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fields">The field list.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction Update<T>(Guid id, T entity, IEnumerable<ODataField> fields, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fieldsExpression">The field list.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction Update<T>(Guid id, T entity, Expression<Func<T, object>> fieldsExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="entityCreateExpression">An expression of the form `() => new T `</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction Update<T>(Guid id, Expression<Func<T>> entityCreateExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity by key.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="keyExpression">The key expression.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fieldsExpression">The field list.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction Update<T>(Expression<Func<T, bool>> keyExpression, T entity, Expression<Func<T, object>> fieldsExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity by key.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="keyExpression">The key expression.</param>
        /// <param name="entityCreateExpression">An expression of the form `() => new T `</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction Update<T>(Expression<Func<T, bool>> keyExpression, Expression<Func<T>> entityCreateExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity by path.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity list.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fields">The field list.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction UpdateAtPath<T>(ODataPath<T> path, T entity, IEnumerable<ODataField> fields, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity by path.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity list.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fieldsExpression">The field list.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction UpdateAtPath<T>(ODataPath<T> path, T entity, Expression<Func<T, object>> fieldsExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity by path.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity list.</param>
        /// <param name="entityCreateExpression">An expression of the form `() => new T `</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientAction UpdateAtPath<T>(ODataPath<T> path, Expression<Func<T>> entityCreateExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        #endregion

        #region UpdateAndReturn

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fields">The field list.</param>
        /// <param name="selectFields">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturn<T>(T entity, IEnumerable<ODataField> fields, IEnumerable<ODataField> selectFields = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fieldsExpression">The field list.</param>
        /// <param name="select">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturn<T>(T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fields">The field list.</param>
        /// <param name="selectFields">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturn<T>(Guid id, T entity, IEnumerable<ODataField> fields, IEnumerable<ODataField> selectFields = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fieldsExpression">The field list.</param>
        /// <param name="select">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturn<T>(Guid id, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity along with all its custom fields.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fieldsExpression">The field list.</param>
        /// <param name="select">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturnWithCustomFields<T>(Guid id, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="entityCreateExpression">An expression of the form `() => new T `</param>
        /// <param name="select">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturn<T>(Guid id, Expression<Func<T>> entityCreateExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity by key.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="keyExpression">The key expression.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fieldsExpression">The field list.</param>
        /// <param name="select">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturn<T>(Expression<Func<T, bool>> keyExpression, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity by key.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="keyExpression">The key expression.</param>
        /// <param name="entityCreateExpression">An expression of the form `() => new T `</param>
        /// <param name="select">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturn<T>(Expression<Func<T, bool>> keyExpression, Expression<Func<T>> entityCreateExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity by path.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity list.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fields">The field list.</param>
        /// <param name="selectFields">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturnAtPath<T>(ODataPath<T> path, T entity, IEnumerable<ODataField> fields, IEnumerable<ODataField> selectFields = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity by path.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity list.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fieldsExpression">The field list.</param>
        /// <param name="select">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturnAtPath<T>(ODataPath<T> path, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity along with all its custom fields.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity list.</param>
        /// <param name="entity">The entity object containing the new values.</param>
        /// <param name="fieldsExpression">The field list.</param>
        /// <param name="select">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturnAtPathWithCustomFields<T>(ODataPath<T> path, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        /// <summary>
        /// Update an entity by path.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity list.</param>
        /// <param name="entityCreateExpression">An expression of the form `() => new T `</param>
        /// <param name="select">The select list; optional.</param>
        /// <param name="upsertBehavior">The upsert behavior; default is update only.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<T> UpdateAndReturnAtPath<T>(ODataPath<T> path, Expression<Func<T>> entityCreateExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity;

        #endregion

        #region Delete

        /// <summary>
        /// Delete an entity by id.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction Delete<T>(Guid id)
            where T : ODataEntity;

        /// <summary>
        /// Delete an entity by key.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="keyExpression">The key expression.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction Delete<T>(Expression<Func<T, bool>> keyExpression)
            where T : ODataEntity;

        /// <summary>
        /// Delete an entity by path.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="path">The path to the entity.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction DeleteAtPath<T>(ODataPath<T> path)
            where T : ODataEntity;
        #endregion

        #region Associate

        /// <summary>
        /// Associate two entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TOther">The other entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="other">The other entity.</param>
        /// <param name="relationship">The relationship on the entity to the list of other entity records.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction Associate<T, TOther>(T entity, TOther other, Expression<Func<T, IEnumerable<TOther>>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity;

        /// <summary>
        /// Associate two entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TOther">The other entity type.</typeparam>
        /// <param name="entityRecId">The entity record id.</param>
        /// <param name="otherRecId">The other entity id.</param>
        /// <param name="relationship">The relationship on the entity to the list of other entity records.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction Associate<T, TOther>(Guid entityRecId, Guid otherRecId, Expression<Func<T, IEnumerable<TOther>>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity;

        /// <summary>
        /// Associate two entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="relationshipPath">The path to a relationship list of entity records.</param>
        /// <param name="entityPath">The path to the entity to add to the list.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction AssociateAtPath<T>(ODataPath<IEnumerable<T>> relationshipPath, ODataPath<T> entityPath)
            where T : ODataEntity;

        /// <summary>
        /// Dissociate two entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TOther">The other entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="other">The other entity.</param>
        /// <param name="relationship">The relationship on the entity to the list of other entity records.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction Dissociate<T, TOther>(T entity, TOther other, Expression<Func<T, IEnumerable<TOther>>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity;

        /// <summary>
        /// Dissociate two entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TOther">The other entity type.</typeparam>
        /// <param name="entityRecId">The entity record id.</param>
        /// <param name="otherRecId">The other entity id.</param>
        /// <param name="relationship">The relationship on the entity to the list of other entity records.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction Dissociate<T, TOther>(Guid entityRecId, Guid otherRecId, Expression<Func<T, IEnumerable<TOther>>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity;

        /// <summary>
        /// Dissociate two entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TOther">The other entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="relationship">The relationship on the entity to the other entity record.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction Dissociate<T, TOther>(T entity, Expression<Func<T, TOther>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity;

        /// <summary>
        /// Dissociate two entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TOther">The other entity type.</typeparam>
        /// <param name="entityRecId">The entity record id.</param>
        /// <param name="relationship">The relationship on the entity to the list of other entity records.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction Dissociate<T, TOther>(Guid entityRecId, Expression<Func<T, TOther>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity;

        /// <summary>
        /// Dissociate two entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="relationshipPath">The path to a member of a relationship list of entity records.</param>
        /// <returns>The action object.</returns>
        IXrmHttpClientAction DissociateAtPath<T>(ODataPath<T> relationshipPath)
            where T : ODataEntity;

        #endregion

        #region Actions

        /// <summary>
        /// Run an action.
        /// </summary>
        /// <typeparam name="TRequest">The request contract type.</typeparam>
        /// <typeparam name="TResponse">The response contract type.</typeparam>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="request">The request object.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<TResponse> Action<TRequest, TResponse>(string actionName, TRequest request);

        /// <summary>
        /// Run an action.
        /// </summary>
        /// <typeparam name="TEntity">The bound entity type.</typeparam>
        /// <typeparam name="TRequest">The request contract type.</typeparam>
        /// <typeparam name="TResponse">The response contract type.</typeparam>
        /// <param name="id">The bound entity id.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="request">The request object.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<TResponse> Action<TEntity, TRequest, TResponse>(Guid id, string actionName, TRequest request)
            where TEntity : ODataEntity;

        /// <summary>
        /// Run an action.
        /// </summary>
        /// <typeparam name="TEntity">The bound entity type.</typeparam>
        /// <typeparam name="TRequest">The request contract type.</typeparam>
        /// <typeparam name="TResponse">The response contract type.</typeparam>
        /// <param name="keyExpression">The key expression.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="request">The request object.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<TResponse> Action<TEntity, TRequest, TResponse>(Expression<Func<TEntity, bool>> keyExpression, string actionName, TRequest request)
            where TEntity : ODataEntity;

        /// <summary>
        /// Run an action.
        /// </summary>
        /// <typeparam name="TEntity">The bound entity type.</typeparam>
        /// <typeparam name="TRequest">The request contract type.</typeparam>
        /// <typeparam name="TResponse">The response contract type.</typeparam>
        /// <param name="path">The bound path.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="request">The request object.</param>
        /// <returns>The query object.</returns>
        IXrmHttpClientQuery<TResponse> ActionAtPath<TEntity, TRequest, TResponse>(ODataPath<TEntity> path, string actionName, TRequest request)
            where TEntity : ODataEntity;

        #endregion

        #region Misc

        /// <summary>
        /// The AAD object id for the user to impersonate.
        /// </summary>
        Guid? ImpersonatedUserObjectId { get; set; }

        /// <summary>
        /// The primary key value from the systemuser entity for the user to impersonate.
        /// </summary>
        Guid? ImpersonatedUser { get; set; }

        Uri BaseAddress { get; }

        ILogger Logger { get; }

        bool ShouldTracePotentialPII { get; }

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);

        /// <summary>
        /// Gets an XRM organization service instance.
        /// </summary>
        /// <returns>The organization service.</returns>
        Task<IOrganizationServiceAsyncDisposable> GetOrganizationService();
        #endregion
    }
}