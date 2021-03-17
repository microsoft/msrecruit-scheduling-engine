//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.DocumentDB
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Contracts;
    using CommonLibrary.Common.Contracts;

    /// <summary>The HCM DocumentClient interface.</summary>
    public interface IHcmDocumentClient
    {
        /// <summary>Gets the document client.</summary>
        IDocumentClient DocumentClient { get; }

        /// <summary>Gets the collection id.</summary>
        string CollectionId { get; }

        /// <summary>Gets the database id.</summary>
        string DatabaseId { get; }

        /// <summary>The create.</summary>
        /// <param name="objectToSave">The object to save.</param>
        /// <param name="requestOptions">The request Options.</param>
        /// <typeparam name="T">The type of document we are saving.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> Create<T>(T objectToSave, RequestOptions requestOptions = null)
            where T : class;

        /// <summary>The create with trigger.</summary>
        /// <param name="objectToSave">The object to save.</param>
        /// <param name="triggerName">The trigger Name.</param>
        /// <param name="requestOptions">The request Options.</param>
        /// <typeparam name="T">The type of document we are saving.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> CreateWithTrigger<T>(T objectToSave, string triggerName = null, RequestOptions requestOptions = null)
            where T : class;

        /// <summary>The create all.</summary>
        /// <param name="entitiesToSave">The entities to save.</param>
        /// <param name="requestOptions">The request Options.</param>
        /// <typeparam name="T">The type to save.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<T>> Create<T>(List<T> entitiesToSave, RequestOptions requestOptions = null)
            where T : class;

        /// <summary>The get.</summary>
        /// <param name="id">The id.</param>
        /// <param name="requestOptions">The request options</param>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> Get<T>(string id, RequestOptions requestOptions = null)
            where T : class;

        /// <summary>The get first or default.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed Options.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> GetFirstOrDefault<T>(Expression<Func<T, bool>> expression, FeedOptions feedOptions = null)
            where T : class;

        /// <summary>The get.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed Options.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> expression, FeedOptions feedOptions = null)
            where T : class;

        /// <summary>
        /// Gets the with pagination.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<SearchMetadataResponse> GetWithPagination<T>(Expression<Func<T, bool>> expression, FeedOptions feedOptions = null, int skip = 0, int take = 0)
            where T : class;

        /// <summary>
        /// Gets with pagination and sorting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>The <see cref="Task" />.</returns>
        Task<SearchMetadataResponse> GetWithPaginationAndSort<T>(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, object>> sortExpression,
            string sortDirection,
            FeedOptions feedOptions = null,
            int skip = 0,
            int take = 0)
            where T : DocDbEntity;


        /// <summary>The get.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <param name="continuationToken">continuation token</param>
        /// <param name="take">take count</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<FeedResponse<T>> GetWithContinuationToken<T>(Expression<Func<T, bool>> expression, FeedOptions feedOptions = null, string continuationToken = null, int take = 100)
            where T : class;

        /// <summary>The get with join query.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed Options.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <typeparam name="RT">The return generic type RT</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<RT>> GetWithJoinQuery<T, RT>(Expression<Func<T, IEnumerable<RT>>> expression, FeedOptions feedOptions = null)
            where T : class
            where RT : class;

        /// <summary>The get with join query.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed Options.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<T>> GetWithJoinQuery<T>(Expression<Func<T, IEnumerable<T>>> expression, FeedOptions feedOptions = null)
            where T : class;

        /// <summary>The get all.</summary>
        /// <param name="feedOptions">The feed Options.</param>
        /// <typeparam name="T">The type to get all for.</typeparam>
        /// <returns>The <see cref="IDocumentQuery"/>.</returns>
        IDocumentQuery<T> GetAllDocDbEntities<T>(FeedOptions feedOptions = null)
            where T : DocDbEntity;

        /// <summary>The execute query.</summary>
        /// <param name="query">The query.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<T>> ExecuteQuery<T>(IDocumentQuery<T> query)
            where T : class;

        /// <summary>The query.</summary>
        /// <param name="feedOptions">The feed Options.</param>
        /// <typeparam name="T">The type to query against.</typeparam>
        /// <returns>The <see cref="IQueryable"/>.</returns>
        IQueryable<T> QueryDocDbEntities<T>(FeedOptions feedOptions = null)
            where T : DocDbEntity;

        /// <summary>The delete.</summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="requestOptions">The request Options.</param>
        /// <typeparam name="T">The type of entity to delete (used for partitioning)</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Delete<T>(string entityId, RequestOptions requestOptions = null)
            where T : class;

        /// <summary>The create document with attachment.</summary>
        /// <param name="fileStream">The file stream.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<DocDbArtifact> CreateDocumentWithAttachment(Stream fileStream);

        /// <summary>The create attachment on document.</summary>
        /// <param name="document">The document.</param>
        /// <param name="fileStream">The file stream.</param>
        /// <typeparam name="T">The type of document to save the file onto.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<ResourceResponse<Attachment>> CreateAttachmentOnDocument<T>(T document, Stream fileStream)
            where T : DocDbEntity;

        /// <summary>The get document file stream.</summary>
        /// <param name="documentId">The document id.</param>
        /// <typeparam name="T">The type of document the attachment should be under.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<Stream> GetDocumentFileStream<T>(string documentId) where T : class;

        /// <summary>The update.</summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        /// <param name="requestOptions">The request Options.</param>
        /// <typeparam name="T">The type to update.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> Update<T>(T entityToUpdate, RequestOptions requestOptions = null)
            where T : class;

        /// <summary>The ensure database and collection.</summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task EnsureDatabaseAndCollection(Type type = null);

        /// <summary>The ensure trigger.</summary>
        /// <param name="triggerName">The trigger name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task EnsureTrigger(string triggerName);

        /// <summary>The get document uri.</summary>
        /// <param name="documentId">The document id.</param>
        /// <returns>The <see cref="Uri"/>.</returns>
        Uri GetDocumentUri(string documentId);

        /// <summary>The get collection uri.</summary>
        /// <returns>The <see cref="Uri"/>.</returns>
        Uri GetCollectionUri();

        /// <summary>The any.</summary>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<bool> Any<T>(FeedOptions feedOptions = null) where T : DocDbEntity;

        /// <summary>The get with projections.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="projectExpression">The project expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<T>> GetWithProjections<T>(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, T>> projectExpression,
            FeedOptions feedOptions = null) where T : class;

        /// <summary>The get with projections.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="projectExpression">The project expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <typeparam name="RT">The generic type RT</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<RT>> GetWithProjections<T, RT>(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, RT>> projectExpression,
            FeedOptions feedOptions = null) where T : class where RT : class;

        /// <summary>The get first or default with projection.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="projectExpression">The project expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> GetFirstOrDefaultWithProjection<T>(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, T>> projectExpression,
            FeedOptions feedOptions = null) where T : class;

        /// <summary>The get with join query and projection.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="whereExpression">The where expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <typeparam name="RT">The generic type RT</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<RT>> GetWithJoinQueryAndProjection<T, RT>(
            Expression<Func<T, IEnumerable<RT>>> expression,
            Expression<Func<T, bool>> whereExpression,
            FeedOptions feedOptions = null) where T : class where RT : class;

        Task<FeedResponse<T>> Search<T>(
            Expression<Func<T, bool>> expression,
            FeedOptions feedOptions = null,
            string continuationToken = null,
            int take = 100) where T : class;

        Task<SearchMetadataResponse> SearchFromSearchRequest<T>(
            SearchMetadataRequest searchRequest,
            FeedOptions feedOptions = null) where T : class;

        Task<FeedResponse<TResult>> SearchWithProjections<T, TResult>(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, TResult>> projectionExpression,
            FeedOptions feedOptions = null,
            string continuationToken = null,
            int take = 100) where T : class where TResult : class;

        Task<SearchMetadataResponse> SearchFromSearchRequestWithProjections<T, TResult>(
            SearchMetadataRequest searchRequest,
            Expression<Func<T, TResult>> projectionExpression,
            FeedOptions feedOptions = null) where T : class where TResult : class;

    }
}
