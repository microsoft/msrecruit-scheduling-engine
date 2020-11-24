//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="HcmDocumentClient.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.DocumentDB
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Base.Utilities;
    using CommonDataService.Common.Internal;
    using Contracts;
    using Exceptions;
    using MS.GTA.Common.Contracts;
    using MS.GTA.Common.DocumentDB.Attributes;
    using MS.GTA.Common.DocumentDB.Expressions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using ServicePlatform.Context;
    using ServicePlatform.Exceptions;
    using ServicePlatform.Tracing;

    /// <summary>The HCM document client.</summary>
    public class HcmDocumentClient : IHcmDocumentClient
    {
        /// <summary>The default offer throughput.</summary>
        private const int DefaultOfferThroughput = 400;

        /// <summary>The maximum offer throughput.</summary>
        private const int MaximumOfferThroughput = 4000;

        /// <summary>The throughput factor.</summary>
        private const int ThroughputFactor = 2;

        /// <summary>The indexing policy template.</summary>
        private static readonly IndexingPolicy IndexingPolicyTemplate = new IndexingPolicy()
        {
            IncludedPaths = new Collection<IncludedPath>
                    {
                        new IncludedPath
                            {
                                Path = "/",
                                Indexes = new Collection<Index>
                                    {
                                        new RangeIndex(DataType.Number) { Precision = -1 },
                                        new RangeIndex(DataType.String) { Precision = -1 }
                                    }
                            }
                    }
        };

        /// <summary>The document DB bad request.</summary>
        private static string documentDbBadRequest = "BadRequest";

        /// <summary>The document DB not found.</summary>
        private static string documentDbNotFound = "NotFound";

        /// <summary>The request options.</summary>
        private readonly RequestOptions requestOptions;

        /// <summary>The collection template.</summary>
        private readonly DocumentCollection collectionTemplate;

        /// <summary>The trigger name.</summary>
        private readonly string triggerName;

        /// <summary>The partition key field.</summary>
        private readonly string partitionKeyField;

        /// <summary>Initializes a new instance of the <see cref="HcmDocumentClient"/> class.</summary>
        /// <param name="documentClient">The document client.</param>
        /// <param name="databaseId">The database id.</param>
        /// <param name="collectionId">The collection id.</param>
        /// <param name="partitionKey">The partition Key.</param>
        /// <param name="triggerName">The trigger Name.</param>
        public HcmDocumentClient(IDocumentClient documentClient, string databaseId, string collectionId, string partitionKey = null, string triggerName = null)
        {
            Contract.CheckValue(documentClient, nameof(documentClient));
            Contract.CheckNonEmpty(databaseId, nameof(databaseId));
            Contract.CheckNonEmpty(collectionId, nameof(collectionId));

            this.CollectionId = collectionId;
            this.DatabaseId = databaseId;
            this.DocumentClient = documentClient;

            // To support legacy code that does not specify a trigger we'll default to just the autoNumbers trigger
            if (triggerName == null)
            {
                triggerName = Constants.AutoNumbersTrigger;
            }

            this.triggerName = triggerName;

            this.collectionTemplate = new DocumentCollection
            {
                Id = this.CollectionId,
                IndexingPolicy = IndexingPolicyTemplate
            };

            this.requestOptions = new RequestOptions();

            if (!string.IsNullOrEmpty(partitionKey))
            {
                this.collectionTemplate.PartitionKey = new PartitionKeyDefinition { Paths = { partitionKey } };
                this.partitionKeyField = partitionKey;
            }
        }

        public HcmDocumentClient(IDocumentClient documentClient, string databaseId, string collectionId, int secondsToLive, string partitionKey = null, string triggerName = null)
        {
            Contract.CheckValue(documentClient, nameof(documentClient));
            Contract.CheckNonEmpty(databaseId, nameof(databaseId));
            Contract.CheckNonEmpty(collectionId, nameof(collectionId));

            if (secondsToLive < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(secondsToLive), "Time to live should be between -1 and Int.max");
            }

            this.CollectionId = collectionId;
            this.DatabaseId = databaseId;
            this.DocumentClient = documentClient;

            // To support legacy code that does not specify a trigger we'll default to just the autoNumbers trigger
            if (triggerName == null)
            {
                triggerName = Constants.AutoNumbersTrigger;
            }

            this.triggerName = triggerName;

            this.collectionTemplate = new DocumentCollection
            {
                Id = this.CollectionId,
                IndexingPolicy = IndexingPolicyTemplate,
                DefaultTimeToLive = secondsToLive,
            };

            this.requestOptions = new RequestOptions();

            if (!string.IsNullOrEmpty(partitionKey))
            {
                this.collectionTemplate.PartitionKey = new PartitionKeyDefinition { Paths = { partitionKey } };
                this.partitionKeyField = partitionKey;
            }
        }

        /// <summary>Gets the collection id.</summary>
        public string CollectionId { get; }

        /// <summary>Gets the database id.</summary>
        public string DatabaseId { get; private set; }

        /// <summary>Gets the document client.</summary>
        public IDocumentClient DocumentClient { get; }

        /// <summary>Gets the database.</summary>
        public Database Database { get; private set; }

        /// <summary>Gets the collection.</summary>
        public DocumentCollection Collection { get; private set; }

        /// <summary>The trace.</summary>
        private static ITraceSource Trace => HcmDocDBTrace.Instance;

        /// <summary>The logger.</summary>
        private static ILogger Logger => CommonLogger.Logger;

        /// <summary>The create.</summary>
        /// <param name="objectToSave">The object to save.</param>
        /// <param name="requestOptions">The request Options.</param>
        /// <typeparam name="T">The type of document we are saving.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<T> Create<T>(T objectToSave, RequestOptions requestOptions = null)
            where T : class
        {
            Contract.CheckValue(objectToSave, nameof(T));

            return await Logger.ExecuteAsync(
                "HcmDDBCreate",
                async () =>
                {
                    Trace.TraceInformation($"Saving document {typeof(T)}");

                    var resourceResponse = await this.CreatePrivate(objectToSave, requestOptions ?? this.GetRequestOptions<T>());

                    Trace.TraceInformation($"Finished saving document {objectToSave.GetType()} with id {resourceResponse.Resource.Id}");

                    return this.ProcessResponse<T>(resourceResponse);
                });
        }

        /// <summary>The save all.</summary>
        /// <param name="entitiesToSave">The entities to save.</param>
        /// <param name="requestOptions">The request Options.</param>
        /// <typeparam name="T">The type to save.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<T>> Create<T>(List<T> entitiesToSave, RequestOptions requestOptions = null)
            where T : class
        {
            Contract.CheckValue(entitiesToSave, nameof(T));

            return await Logger.ExecuteAsync(
                "HcmDDBCreateAll",
                async () =>
                {
                    Trace.TraceInformation($"Saving all {typeof(T).Name}");
                    var response = await Task.WhenAll(entitiesToSave.Select(
                        async entityToSave => await this.Create(entityToSave, requestOptions ?? this.GetRequestOptions<T>())));

                    return response;
                });
        }

        /// <summary>The save.</summary>
        /// <param name="objectToSave">The object to save.</param>
        /// <param name="triggerName">The trigger Name.</param>
        /// <param name="requestOptions">The request Options.</param>
        /// <typeparam name="T">The type of document we are saving.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<T> CreateWithTrigger<T>(T objectToSave, string triggerName = null, RequestOptions requestOptions = null)
            where T : class
        {
            Contract.CheckValue(objectToSave, nameof(T));

            return await Logger.ExecuteAsync(
                "HcmDDBCreateWithTrigger",
                async () =>
                {
                    if (triggerName == null)
                    {
                        triggerName = this.triggerName;
                    }

                    Trace.TraceInformation($"Saving {objectToSave.GetType().Name} with trigger {triggerName}");

                    if (requestOptions == null)
                    {
                        requestOptions = new RequestOptions { PartitionKey = this.GetRequestOptions<T>().PartitionKey };
                    }

                    if (requestOptions.PreTriggerInclude == null)
                    {
                        requestOptions.PreTriggerInclude = new List<string>();
                    }

                    if (!requestOptions.PreTriggerInclude.Contains(triggerName))
                    {
                        requestOptions.PreTriggerInclude.Add(triggerName);
                    }

                    return await this.Create(objectToSave, requestOptions);
                });
        }

        /// <summary>The create document with attachment.</summary>
        /// <param name="fileStream">The file stream.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<DocDbArtifact> CreateDocumentWithAttachment(Stream fileStream)
        {
            return await Logger.Execute(
                "HcmDDBCreateDocumentWithAttachment",
                async () =>
                {
                    Trace.TraceInformation($"Creating document and attachment");

                    var docDbArtifact = await this.Create(new DocDbArtifact());

                    Trace.TraceInformation($"Finished creating artifact {docDbArtifact.Id}");

                    var attachment = await this.CreateAttachmentOnDocument(docDbArtifact, fileStream);

                    Trace.TraceInformation($"Finished attachment {attachment?.Resource?.Id} for artifact {docDbArtifact.Id}");

                    return docDbArtifact;
                });
        }

        /// <summary>The create attachment on document.</summary>
        /// <param name="document">The document.</param>
        /// <param name="fileStream">The file stream.</param>
        /// <typeparam name="T">The type of document to save the file onto.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<ResourceResponse<Attachment>> CreateAttachmentOnDocument<T>(T document, Stream fileStream)
            where T : DocDbEntity
        {
            Contract.CheckValue(document, nameof(document));
            Contract.CheckValue(fileStream, nameof(fileStream));

            return await Logger.Execute(
                "HcmDDBCreateAttachmentOnDocument",
                async () =>
                {
                    Trace.TraceInformation($"Creating attachment on document {document.Id}");

                    var mediaOptions = new MediaOptions() { Slug = Guid.NewGuid().ToString() };

                    // I believe that the document client will just "hang" if we try to upload an empty document.
                    if (fileStream.Length <= 0)
                    {
                        throw new FileStreamEmptyException($"The file stream that is being saved for document {document.Id} is empty").EnsureTraced();
                    }

                    var attachment = await this.DocumentClient.CreateAttachmentAsync(
                                         this.GetDocumentUri(document.Id),
                                         fileStream,
                                         mediaOptions,
                                         this.GetRequestOptions<T>());

                    Trace.TraceInformation($"Finished creating attachment on document {document.Id}");

                    return attachment;
                });
        }

        /// <summary>The get.</summary>
        /// <param name="id">The ID.</param>
        /// <param name="requestOptions">The request options.</param>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<T> Get<T>(string id, RequestOptions requestOptions = null)
            where T : class
        {
            Contract.CheckValue(id, nameof(id));

            return await Logger.ExecuteAsync(
                "HcmDDBGet",
                async () =>
                {
                    Trace.TraceInformation($"Reading document {typeof(T)} id {id}");

                    var resourceResponse = await this.ReadPrivate<T, T>(id, requestOptions ?? this.GetRequestOptions<T>());

                    Trace.TraceInformation($"Finished reading document {typeof(T)} with id {resourceResponse?.Resource?.Id}");

                    return this.ProcessResponse<T>(resourceResponse);
                });
        }

        /// <summary>The get all.</summary>
        /// <param name="feedOptions">The feed Options.</param>
        /// <typeparam name="T">The type to get all for.</typeparam>
        /// <returns>The <see cref="IDocumentQuery"/>.</returns>
        public IDocumentQuery<T> GetAllDocDbEntities<T>(FeedOptions feedOptions = null)
            where T : DocDbEntity
        {
            return Logger.Execute(
                "HcmDDBGetAll",
                () => this.QueryDocDbEntities<T>(feedOptions).AsDocumentQuery());
        }

        /// <summary>The query.</summary>
        /// <param name="feedOptions">The feed Options.</param>
        /// <typeparam name="T">The type to query against.</typeparam>
        /// <returns>The <see cref="IQueryable"/>.</returns>
        public IQueryable<T> QueryDocDbEntities<T>(FeedOptions feedOptions = null)
            where T : DocDbEntity
        {
            return Logger.Execute(
                "HcmDDBQuery",
                () =>
                {
                    var documentType = typeof(T).Name;

                    Trace.TraceInformation($"Getting query for all {documentType}");

                    return this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                        .Where(t => t.Type == documentType);
                });
        }

        /// <summary>The get first or default.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<T> GetFirstOrDefault<T>(Expression<Func<T, bool>> expression, FeedOptions feedOptions = null)
            where T : class
        {
            var result = await this.Get(expression, feedOptions);
            return result?.FirstOrDefault();
        }

        /// <summary>The get first or default with projections.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="projectExpression">The project Expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<T> GetFirstOrDefaultWithProjection<T>(Expression<Func<T, bool>> expression, Expression<Func<T, T>> projectExpression, FeedOptions feedOptions = null)
            where T : class
        {
            var result = await this.GetWithProjections<T>(expression, projectExpression, feedOptions);
            return result?.FirstOrDefault();
        }

        /// <summary>The get.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> expression, FeedOptions feedOptions = null)
            where T : class
        {
            return await Logger.ExecuteAsync(
                "HcmDDBGetWithQuery",
                async () =>
                {
                    Trace.TraceInformation($"Getting {typeof(T).Name} with passed in expression");

                    var query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                            .Where(expression)
                            .AsDocumentQuery();

                    return await Logger.ExecuteAsync(
                        "HcmDDBGetWithQueryExec",
                        async () => await this.ReadPrivate<T, T>(query));
                });
        }

        /// <summary>
        /// Gets the with pagination.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        public async Task<SearchMetadataResponse> GetWithPagination<T>(Expression<Func<T, bool>> expression, FeedOptions feedOptions = null, int skip = 0, int take = 0)
            where T : class
        {
            IDocumentQuery<T> query;
            var result =  await Logger.ExecuteAsync(
                "HcmDDBGetWithQuery",
                async () =>
                {
                    Trace.TraceInformation($"Getting {typeof(T).Name} with passed in expression");
                    if (skip == 0 && take == 0)
                    {
                        query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                            .Where(expression)
                            .AsDocumentQuery();
                    }
                    else
                    {
                        query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                            .Where(expression)
                            .Skip(skip)
                            .Take(take)
                            .AsDocumentQuery();
                    }
                    

                    return await Logger.ExecuteAsync(
                        "HcmDDBGetWithQueryExec",
                        async () => await this.ReadPrivate<T, T>(query));
                });

            int total = await this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                            .Where(expression)
                            .CountAsync();

            return new SearchMetadataResponse
            {
                Result = result,
                Total = total
            };
        }

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
        public async Task<SearchMetadataResponse> GetWithPaginationAndSort<T>(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, object>> sortExpression,
            string sortDirection = "desc",
            FeedOptions feedOptions = null, 
            int skip = 0, 
            int take = 0)
            where T : DocDbEntity
        {
            IDocumentQuery<T> query;
            
            var result = await Logger.ExecuteAsync(
                "HcmDDBGetWithQuery",
                async () =>
                {
                    Trace.TraceInformation($"Getting {typeof(T).Name} with passed in expression");

                    if (skip == 0 && take == 0)
                    {
                        if (sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase))
                        {
                            query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                                .Where(expression)
                                .OrderBy(sortExpression)
                                .AsDocumentQuery();
                        }
                        else
                        {
                            query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                                .Where(expression)
                                .OrderByDescending(sortExpression)
                                .AsDocumentQuery();
                        }
                    }
                    else
                    {
                        if (sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase))
                        {
                            query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(),
                                    feedOptions ?? this.GetFeedOptions<T>())
                                .Where(expression)
                                .OrderBy(sortExpression)
                                .Skip(skip)
                                .Take(take)
                                .AsDocumentQuery();
                        }
                        else
                        {
                            query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(),
                                    feedOptions ?? this.GetFeedOptions<T>())
                                .Where(expression)
                                .OrderByDescending(sortExpression)
                                .Skip(skip)
                                .Take(take)
                                .AsDocumentQuery();
                        }

                    }
                    
                    return await Logger.ExecuteAsync(
                        "HcmDDBGetWithQueryExec",
                        async () => await this.ReadPrivate<T, T>(query).ConfigureAwait(false));
                });

            int total = await this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                .Where(expression)
                .CountAsync();

            return new SearchMetadataResponse
            {
                Result = result,
                Total = total,
            };
        }

        /// <summary>The get.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <param name="continuationToken">continuation token</param>
        /// <param name="take">take count</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<FeedResponse<T>> GetWithContinuationToken<T>(Expression<Func<T, bool>> expression, FeedOptions feedOptions = null, string continuationToken = null, int take = 100)
            where T : class
        {
            Contract.CheckValue(expression, nameof(expression));

            return await Logger.ExecuteAsync(
                "HcmDDBGetWithContToken",
                async () =>
                {
                    Trace.TraceInformation($"Getting {typeof(T).Name} with passed in expression and continuation token");
                    feedOptions = feedOptions ?? this.GetFeedOptions<T>();
                    feedOptions.MaxItemCount = take;
                    if (!string.IsNullOrWhiteSpace(continuationToken))
                    {
                        Trace.TraceInformation($"Processing request with continuation Token");
                        feedOptions.RequestContinuation = continuationToken;
                    }

                    var query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions)
                            .Where(expression)
                            .AsDocumentQuery();

                    return await Logger.ExecuteAsync(
                        "HcmDDBGetWithContTokenExec",
                        async () => await this.ReadPrivateWithToken<T, T>(query));
                });
        }

        public async Task<FeedResponse<TResult>> GetProjectionWithContinuationToken<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> projectionExpression, FeedOptions feedOptions = null, string continuationToken = null, int take = 100)
            where T : class
            where TResult : class
        {
            Contract.CheckValue(expression, nameof(expression));

            return await Logger.ExecuteAsync(
                "HcmDDBGetProjectionWithContToken",
                async () =>
                {
                    Trace.TraceInformation($"Getting {typeof(T).Name} with passed in expression and continuation token");
                    feedOptions = feedOptions ?? this.GetFeedOptions<T>();
                    feedOptions.MaxItemCount = take;
                    if (!string.IsNullOrWhiteSpace(continuationToken))
                    {
                        Trace.TraceInformation($"Processing request with continuation Token");
                        feedOptions.RequestContinuation = continuationToken;
                    }

                    var query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions)
                        .Where(expression)
                        .Select(projectionExpression)
                        .AsDocumentQuery();

                    return await Logger.ExecuteAsync(
                        "HcmDDBGetWithContTokenExec",
                        async () => await this.ReadPrivateWithToken<TResult, T>(query));
                });
        }

        public async Task<FeedResponse<T>> Search<T>(Expression<Func<T, bool>> expression, FeedOptions feedOptions = null, string continuationToken = null, int take = 100) where T : class
        {
            return await Logger.ExecuteAsync(
                "HcmDDBSearch",
                async () => await this.GetWithContinuationToken(expression, feedOptions, continuationToken, take));
        }

        public async Task<SearchMetadataResponse> SearchFromSearchRequest<T>(SearchMetadataRequest searchRequest, FeedOptions feedOptions = null) where T : class
        {
            Expression<Func<T, bool>> expression = GenerateSearchExpression<T>(searchRequest);

            FeedResponse<T> result = await Logger.ExecuteAsync(
                    "HcmDDBSearchFromSearchRequest",
                    async () => await this.GetWithContinuationToken(expression, feedOptions, searchRequest.ContinuationToken, searchRequest.Take));

            if (result?.Any() != null)
            {
                return new SearchMetadataResponse
                {
                    Result = result,
                    ContinuationToken = result.ResponseContinuation,
                    Total = result.Count
                };
            }

            return null;
        }

        public async Task<FeedResponse<TResult>> SearchWithProjections<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> projectionExpression, FeedOptions feedOptions = null, string continuationToken = null, int take = 100)
            where T : class
            where TResult : class
        {
            return await Logger.ExecuteAsync(
                "HcmDDBSearchWithProjections",
                async () => await this.GetProjectionWithContinuationToken(expression, projectionExpression, feedOptions, continuationToken, take));
        }

        public async Task<SearchMetadataResponse> SearchFromSearchRequestWithProjections<T, TResult>(SearchMetadataRequest searchRequest, Expression<Func<T, TResult>> projectionExpression, FeedOptions feedOptions = null)
                    where T : class
                    where TResult : class
        {
            Expression<Func<T, bool>> expression = GenerateSearchExpression<T>(searchRequest);

            FeedResponse<TResult> result = await Logger.ExecuteAsync(
                    "HcmDDBSearchFromSearchRequestWithProjections",
                    async () => await this.GetProjectionWithContinuationToken(expression, projectionExpression, feedOptions, searchRequest.ContinuationToken, searchRequest.Take));

            if (result?.Any() != null)
            {
                return new SearchMetadataResponse
                {
                    Result = result,
                    ContinuationToken = result.ResponseContinuation,
                    Total = result.Count
                };
            }

            return null;
        }

        /// <summary>Method to check if any records of the type T exists.</summary>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<bool> Any<T>(FeedOptions feedOptions = null)
            where T : DocDbEntity
        {
            return await Logger.ExecuteAsync(
                "HcmDDBGetAny",
                async () =>
                {
                    Trace.TraceInformation($"Getting single record of {typeof(T).Name}");
                    var documentType = typeof(T).Name;
                    var query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                            .Where(t => t.Type == documentType && t.Id != "autoNumbers")
                            .Select(t => t.Id)
                            .Take(1)
                            .AsDocumentQuery();

                    var result = await Logger.ExecuteAsync(
                        "HcmDDBGetAnyExec",
                        async () => await this.ReadPrivate<string, T>(query));
                    return result.Any();
                });
        }

        /// <summary>The get.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="projectExpression">The project Expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<T>> GetWithProjections<T>(Expression<Func<T, bool>> expression, Expression<Func<T, T>> projectExpression, FeedOptions feedOptions = null)
            where T : class
        {
            return await Logger.ExecuteAsync(
                "HcmDDBGetWithProjectionsQuery",
                async () =>
                {
                    Trace.TraceInformation($"Getting {typeof(T).Name} with passed in expression and projection expression");

                    var query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                            .Where(expression)
                            .Select(projectExpression)
                            .AsDocumentQuery();

                    return await Logger.ExecuteAsync(
                        "HcmDDBGetWithProjectionsQueryExec",
                        async () => await this.ReadPrivate<T, T>(query));
                });
        }

        /// <summary>The get with projects.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="projectExpression">The project Expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <typeparam name="RT">The secondary generic type</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<RT>> GetWithProjections<T, RT>(Expression<Func<T, bool>> expression, Expression<Func<T, RT>> projectExpression, FeedOptions feedOptions = null)
            where T : class
            where RT : class
        {
            return await Logger.ExecuteAsync(
                "HcmDDBGetWithProjectionsQuery",
                async () =>
                {
                    Trace.TraceInformation($"Getting {typeof(T).Name} with passed in expression and projection expression");

                    var query = this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                            .Where(expression)
                            .Select(projectExpression)
                            .AsDocumentQuery();

                    return await Logger.ExecuteAsync(
                        "HcmDDBGetWithProjectionsQueryExec",
                        async () => await this.ReadPrivate<RT, T>(query));
                });
        }

        /// <summary>The get.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<T>> GetWithJoinQuery<T>(Expression<Func<T, IEnumerable<T>>> expression, FeedOptions feedOptions = null)
            where T : class
        {
            return await this.GetWithJoinQuery<T, T>(expression, feedOptions);
        }

        /// <summary>The get.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <typeparam name="RT">The return generic type RT</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<RT>> GetWithJoinQuery<T, RT>(Expression<Func<T, IEnumerable<RT>>> expression, FeedOptions feedOptions = null)
            where T : class
            where RT : class
        {
            return await Logger.ExecuteAsync(
                "HcmDDBGetWithJQuery",
                async () =>
                {
                    Trace.TraceInformation($"Getting {typeof(T).Name} with passed in expression that contains joins or select many LINQ calls");

                    var query =
                        this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                            .SelectMany(expression)
                            .AsDocumentQuery();

                    var results = await Logger.ExecuteAsync(
                        "HcmDDBGetWithJQueryExec",
                        async () => await this.ReadPrivate<RT, T>(query));

                    Trace.TraceInformation($"Finished getting {typeof(T).Name}, found {results?.Count} documents");

                    return results;
                });
        }


        /// <summary>The get.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="whereExpression">The where Expression.</param>
        /// <param name="feedOptions">The feed options.</param>
        /// <typeparam name="T">The generic type T</typeparam>
        /// <typeparam name="RT">The return generic type RT</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<RT>> GetWithJoinQueryAndProjection<T, RT>(Expression<Func<T, IEnumerable<RT>>> expression, Expression<Func<T, bool>> whereExpression, FeedOptions feedOptions = null)
            where T : class
            where RT : class
        {
            return await Logger.ExecuteAsync(
                "HcmDDBGetWithJQuery",
                async () =>
                {
                    Trace.TraceInformation($"Getting {typeof(T).Name} with passed in expression that contains joins or select many LINQ calls");

                    var query =
                        this.DocumentClient.CreateDocumentQuery<T>(this.GetCollectionUri(), feedOptions ?? this.GetFeedOptions<T>())
                            .Where(whereExpression)
                            .SelectMany(expression)
                            .AsDocumentQuery();

                    var results = await Logger.ExecuteAsync(
                        "HcmDDBGetWithJQueryExec",
                        async () => await this.ReadPrivate<RT, T>(query));

                    Trace.TraceInformation($"Finished getting {typeof(T).Name}, found {results?.Count} documents");

                    return results;
                });
        }

        /// <summary>The get document file stream.</summary>
        /// <param name="documentId">The document id.</param>
        /// <typeparam name="T">The type of document the attachment should be under.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<Stream> GetDocumentFileStream<T>(string documentId)
            where T : class
        {
            Contract.CheckValue(documentId, nameof(documentId));

            return await Logger.ExecuteAsync(
                "HcmDDBGetDocumentFileStream",
                async () =>
                {
                    Trace.TraceInformation($"Fetching first file for document {documentId}");

                    var feedOptions = GetFeedOptions<T>();

                    var feed = await this.DocumentClient.ReadAttachmentFeedAsync(
                                   this.GetDocumentUri(documentId) + "/attachments",
                                   feedOptions);

                    Trace.TraceInformation($"Finished getting document attachment feed {typeof(T).Name} for {documentId} has {feed?.Count} documents - took {feed?.RequestCharge}");

                    if (feed != null && feed.Any())
                    {
                        Trace.TraceInformation($"Getting first attachment on document {documentId}");

                        var media = await this.DocumentClient.ReadMediaAsync(feed.FirstOrDefault()?.MediaLink);

                        Trace.TraceInformation($"Finished getting first attachment on document {documentId}");

                        return media.Media;
                    }

                    Trace.TraceInformation($"Found no attachments on document {documentId}");

                    return null;
                });
        }

        /// <summary>The execute query.</summary>
        /// <param name="query">The query.</param>
        /// <typeparam name="T">The generic type</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<T>> ExecuteQuery<T>(IDocumentQuery<T> query)
            where T : class
        {
            return await Logger.ExecuteAsync(
                "HcmDBQueryExec",
                async () =>
                {
                    Trace.TraceInformation($"Executing query for documents {typeof(T).Name}");

                    var results = await this.ReadPrivate<T, T>(query);

                    Trace.TraceInformation($"Finished executing query for documents {typeof(T).Name} found {results?.Count} documents");

                    return results;
                });
        }

        /// <summary>The update.</summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        /// <param name="requestOptions">The request options.</param>
        /// <typeparam name="T">The type to update.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<T> Update<T>(T entityToUpdate, RequestOptions requestOptions = null)
            where T : class
        {
            Contract.CheckValue(entityToUpdate, nameof(T));

            return await Logger.ExecuteAsync(
                       "HcmDDBUpdate",
                       async () =>
                       {
                           Trace.TraceInformation($"Updating document {typeof(T).Name}");

                           var resourcesResponse = await this.UpdatePrivate<T>(entityToUpdate, requestOptions ?? this.GetRequestOptions<T>());

                           Trace.TraceInformation($"Finished updating document {typeof(T).Name} with id {resourcesResponse?.Resource?.Id}");

                           return this.ProcessResponse<T>(resourcesResponse);
                       }, new[] { typeof(DocumentClientException) });
        }

        /// <summary>The delete.</summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="requestOptions">The request options.</param>
        /// <typeparam name="T">The type to delete.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Delete<T>(string entityId, RequestOptions requestOptions = null)
            where T : class
        {
            Contract.CheckValue(entityId, nameof(entityId));

            Trace.TraceInformation($"{entityId} about to be deleted");
            await this.DocumentClient.DeleteDocumentAsync(this.GetDocumentUri(entityId), requestOptions ?? this.GetRequestOptions<T>());
        }

        /// <summary>The ensure database and collection.</summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task EnsureDatabaseAndCollection(Type type = null)
        {
            Trace.TraceInformation($"Checking to make sure database {this.DatabaseId} and collection {this.CollectionId} exist");

            this.Database = (await this.CreateDatabasePrivate(type, this.DatabaseId))?.Resource;
            this.Collection = (await this.CreateCollectionPrivate(type))?.Resource;

            Trace.TraceInformation($"Finished checking to make sure database {this.DatabaseId} and collection {this.CollectionId} exist");
        }

        /// <summary>The ensure trigger.</summary>
        /// <param name="triggerName">The trigger name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task EnsureTrigger(string triggerName)
        {
            try
            {
                await this.DocumentClient.ReadTriggerAsync(UriFactory.CreateTriggerUri(this.DatabaseId, this.CollectionId, triggerName));
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == "NotFound")
                {
                    Trace.TraceInformation($"{triggerName} not found, creating");

                    var assembly = Assembly.GetExecutingAssembly();
                    var resources = assembly.GetManifestResourceNames();

                    var resourceName = $"TriggerScripts.{triggerName}.js";

                    var fullResourceName = resources.FirstOrDefault(r => r.Contains(resourceName));

                    using (Stream stream = assembly.GetManifestResourceStream(fullResourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string triggerBody = reader.ReadToEnd();

                        var trigger = new Trigger()
                        {
                            Id = triggerName,
                            Body = triggerBody,
                            TriggerType = TriggerType.Pre,
                            TriggerOperation = TriggerOperation.Create
                        };

                        await this.DocumentClient.CreateTriggerAsync(this.GetCollectionUri(), trigger);
                    }
                }
            }
        }

        /// <summary>The get document uri.</summary>
        /// <param name="documentId">The document id.</param>
        /// <returns>The <see cref="Uri"/>.</returns>
        public Uri GetDocumentUri(string documentId)
        {
            return UriFactory.CreateDocumentUri(this.DatabaseId, this.CollectionId, documentId);
        }

        /// <summary>The get collection uri.</summary>
        /// <returns>The <see cref="Uri"/>.</returns>
        public Uri GetCollectionUri()
        {
            return UriFactory.CreateDocumentCollectionUri(this.DatabaseId, this.CollectionId);
        }

        private Expression<Func<T, bool>> GenerateSearchExpression<T>(SearchMetadataRequest searchRequest)
        {
            var searchText = searchRequest.SearchText;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.ToLower();
            }
            else
            {
                searchText = string.Empty;
            }

            if (searchRequest.Take <= 0)
            {
                searchRequest.Take = Constants.DefaultTake;
            }

            Expression<Func<T, bool>> expression = p => false;
            foreach (var searchField in searchRequest.SearchFields)
            {
                if (typeof(T).GetProperty(searchField) == null || !typeof(T).GetProperty(searchField).CanRead)
                {
                    HcmDocDBTrace.Instance.TraceWarning($"searchfield: {searchField} does not exist on type {typeof(T).ToString()}");
                    continue;
                }

                Expression<Func<T, bool>> andExpression = p => true;

                var param = Expression.Parameter(typeof(T), "p");
                var property = Expression.Property(param, searchField);
                andExpression = andExpression.AndAlso(Expression.Lambda<Func<T, bool>>(
                    Expression.NotEqual(property, Expression.Constant(null, property.Type)), param));

                var searchExpression = Expression.Constant(searchText, typeof(string));
                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var propertyToLower = Expression.Call(property, "ToLower", null);
                var containsMethodExp = Expression.Call(propertyToLower, method, searchExpression);
                andExpression = andExpression.AndAlso(Expression.Lambda<Func<T, bool>>(containsMethodExp, param));

                expression = expression.OrElse(andExpression);
            }

            return expression;
        }

        /// <summary>The create collection private.</summary>
        /// <param name="type">The type.</param>
        /// <param name="retry">The retry.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task<ResourceResponse<DocumentCollection>> CreateCollectionPrivate(Type type, bool retry = false)
        {
            try
            {
                return await this.DocumentClient.ReadDocumentCollectionAsync(this.GetCollectionUri());
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == documentDbNotFound)
                {
                    Trace.TraceInformation($"Collection {this.CollectionId} not found, creating");

                    if (string.IsNullOrEmpty(this.partitionKeyField))
                    {
                        var partitionKey = type.GetCustomAttribute<PartitionKeyPathAttribute>()?.Path;
                        if (!string.IsNullOrEmpty(partitionKey))
                        {
                            this.collectionTemplate.PartitionKey =
                                new PartitionKeyDefinition { Paths = { partitionKey } };
                        }
                    }

                    // We will default to backfilling
                    var toBackfill = true;

                    if (type != null)
                    {
                        var backfillAttribute = type.GetCustomAttribute<BackfillDBAndCollectionAttribute>();

                        if (backfillAttribute != null)
                        {
                            HcmDocDBTrace.Instance.TraceInformation($"Setting backfill collection to be {backfillAttribute.Backfill}");

                            toBackfill = backfillAttribute.Backfill;
                        }
                    }

                    if (toBackfill)
                    {
                        HcmDocDBTrace.Instance.TraceInformation($"Backfilling collection {this.collectionTemplate.Id} for database {this.DatabaseId}");

                        try
                        {
                            return await this.DocumentClient.CreateDocumentCollectionAsync(
                                       UriFactory.CreateDatabaseUri(this.DatabaseId),
                                       this.collectionTemplate,
                                       new RequestOptions { OfferThroughput = DefaultOfferThroughput });
                        }
                        catch (DocumentClientException documentClientException)
                        {
                            if (documentClientException.StatusCode == HttpStatusCode.Conflict)
                            {
                                if (!retry)
                                {
                                    HcmDocDBTrace.Instance.TraceWarning($"Got conflict while trying to save new document collection, will retry.");
                                    return await this.CreateCollectionPrivate(type, true);
                                }

                                HcmDocDBTrace.Instance.TraceWarning($"Tried to backfill collection again but got conflict again.");
                            }

                            throw;
                        }

                    }

                    HcmDocDBTrace.Instance.TraceInformation($"Skipping backfill of collection {this.collectionTemplate.Id} for database {this.DatabaseId}");
                }
            }

            return null;
        }

        /// <summary>The create database.</summary>
        /// <param name="type">The type.</param>
        /// <param name="databaseId">The database id.</param>
        /// <param name="retry">The retry.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task<ResourceResponse<Database>> CreateDatabasePrivate(Type type, string databaseId, bool retry = false)
        {
            try
            {
                Trace.TraceInformation($"Checking if {databaseId} exists");
                return await this.DocumentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == documentDbNotFound)
                {
                    // We will default to backfilling
                    var toBackfill = true;

                    if (type != null)
                    {
                        HcmDocDBTrace.Instance.TraceInformation($"Checking typeof {type?.FullName} for backfill attribute");

                        var backfillAttribute = type.GetCustomAttribute<BackfillDBAndCollectionAttribute>();

                        if (backfillAttribute != null)
                        {
                            HcmDocDBTrace.Instance.TraceInformation($"Setting backfill database to be {backfillAttribute.Backfill}");

                            toBackfill = backfillAttribute.Backfill;
                        }
                    }

                    if (toBackfill)
                    {
                        HcmDocDBTrace.Instance.TraceInformation($"Backfilling database {this.DatabaseId}");

                        Trace.TraceInformation($"{databaseId} not found, creating");
                        var databaseTemplate = new Database { Id = this.DatabaseId };

                        try
                        {
                            return await this.DocumentClient.CreateDatabaseAsync(databaseTemplate);
                        }
                        catch (DocumentClientException documentClientException)
                        {
                            if (documentClientException.StatusCode == HttpStatusCode.Conflict)
                            {
                                if (!retry)
                                {
                                    HcmDocDBTrace.Instance.TraceWarning($"Got conflict while trying to save new database, will retry.");
                                    return await this.CreateDatabasePrivate(type, databaseId, true);
                                }

                                HcmDocDBTrace.Instance.TraceWarning($"Tried to backfill database again but got conflict again.");
                            }

                            throw;
                        }
                    }

                    HcmDocDBTrace.Instance.TraceInformation($"Skipping backfill of database {this.DatabaseId}");
                }
            }

            return null;
        }

        /// <summary>The get feed options.</summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <returns>The <see cref="FeedOptions"/>.</returns>
        private FeedOptions GetFeedOptions<T>()
            where T : class
        {
            var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true, MaxDegreeOfParallelism = -1 };

            if (typeof(T).IsSubclassOf(typeof(DocDbEntity)))
            {
                feedOptions.PartitionKey = new PartitionKey(typeof(T).Name);
            }

            return feedOptions;
        }

        /// <summary>
        /// The get request options.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <returns>The <see cref="RequestOptions"/>.</returns>
        private RequestOptions GetRequestOptions<T>() where T : class
        {
            if (typeof(T).IsSubclassOf(typeof(DocDbEntity)))
            {
                this.requestOptions.PartitionKey = new PartitionKey(typeof(T).Name);
            }

            return this.requestOptions;
        }

        /// <summary>The create private.</summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="document">The document.</param>
        /// <param name="requestOptions">The request options.</param>
        /// <param name="retry">The retry.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task<ResourceResponse<Document>> CreatePrivate<T>(T document, RequestOptions requestOptions, bool retry = false)
        {
            try
            {
                return await this.DocumentClient.CreateDocumentAsync(this.GetCollectionUri(), document, requestOptions);
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == documentDbNotFound && !retry)
                {
                    await this.EnsureDatabaseAndCollection(typeof(T));

                    if (requestOptions?.PreTriggerInclude?.Count >= 1)
                    {
                        await this.EnsureTrigger(requestOptions.PreTriggerInclude.FirstOrDefault());
                    }

                    return await this.CreatePrivate(document, requestOptions, true);
                }

                if (e.Error.Code == documentDbBadRequest && e.Error.Message.Contains("Ensure to pass a valid trigger") && !retry)
                {
                    await this.EnsureTrigger(requestOptions.PreTriggerInclude.FirstOrDefault());

                    return await this.CreatePrivate(document, requestOptions, true);
                }

                // Retry with (http://getstatuscode.com/449)
                // There is no enum for 449
                if (e.StatusCode != null && (int)e.StatusCode == 449)
                {
                    await Task.Delay(10);
                    return await this.CreatePrivate(document, requestOptions, true);
                }

                return await this.ScaleThroughputAndRetry(e, () => this.CreatePrivate(document, requestOptions, retry));
            }
        }

        /// <summary>The read private.</summary>
        /// <param name="documentQuery">The document query.</param>
        /// <param name="retry">The retry.</param>
        /// <typeparam name="T">The type of document to read.</typeparam>
        /// <typeparam name="BackfillCheckType">
        /// The backfill check type. In scenarios where the return type is actually diffrent than the read type.
        /// i.e you use OnboardingGuide as your querytype but when passed to Read the return type is string this can mess with the
        /// type we check for the backfill attribute and cause it to backfill unexpectedly since we default to doing the backfill.
        /// </typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task<IList<T>> ReadPrivate<T, BackfillCheckType>(IDocumentQuery<T> documentQuery, bool retry = false)
            where T : class
            where BackfillCheckType : class
        {
            Contract.CheckValue(documentQuery, nameof(documentQuery));

            try
            {
                var results = new List<T>();

                while (documentQuery.HasMoreResults)
                {
                    var response = await documentQuery.ExecuteNextAsync<T>();
                    this.ProcessFeedResponse(response);
                    if (response != null && response.Any())
                    {
                        Trace.TraceInformation($"Fetching results for {typeof(T)}");
                        results.AddRange(response);
                    }
                }

                return results;
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == documentDbNotFound)
                {
                    if (!retry)
                    {
                        await this.EnsureDatabaseAndCollection(typeof(BackfillCheckType));
                        return await this.ReadPrivate<T, BackfillCheckType>(documentQuery, true);
                    }

                    return null;
                }

                return await this.ScaleThroughputAndRetry(e, () => this.ReadPrivate<T, BackfillCheckType>(documentQuery));
            }
        }

        /// <summary>The read private.</summary>
        /// <param name="documentQuery">The document query.</param>
        /// <param name="retry">The retry.</param>
        /// <typeparam name="T">The type of document to read.</typeparam>
        /// <typeparam name="BackfillCheckType">
        /// The backfill check type. In scenarios where the return type is actually diffrent than the read type.
        /// i.e you use OnboardingGuide as your querytype but when passed to Read the return type is string this can mess with the
        /// type we check for the backfill attribute and cause it to backfill unexpectedly since we default to doing the backfill.
        /// </typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task<FeedResponse<T>> ReadPrivateWithToken<T, BackfillCheckType>(IDocumentQuery<T> documentQuery, bool retry = false)
            where T : class
            where BackfillCheckType : class
        {
            Contract.CheckValue(documentQuery, nameof(documentQuery));

            try
            {
                var results = new List<T>();

                var response = await documentQuery.ExecuteNextAsync<T>();
                this.ProcessFeedResponse(response);
                if (response != null && response.Any())
                {
                    return response;
                }

                return null;
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == documentDbNotFound)
                {
                    if (!retry)
                    {
                        await this.EnsureDatabaseAndCollection(typeof(BackfillCheckType));
                        return await this.ReadPrivateWithToken<T, BackfillCheckType>(documentQuery, true);
                    }

                    return null;
                }

                return await this.ScaleThroughputAndRetry(e, () => this.ReadPrivateWithToken<T, BackfillCheckType>(documentQuery));
            }
        }

        /// <summary>The read private.</summary>

        /// <param name="documentId">The document id.</param>
        /// <param name="requestOptions">The request options.</param>
        /// <param name="retry">The retry.</param>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <typeparam name="BackfillCheckType">
        /// The backfill check type. In scenarios where the return type is actually diffrent than the read type.
        /// i.e you use OnboardingGuide as your querytype but when passed to Read the return type is string this can mess with the
        /// type we check for the backfill attribute and cause it to backfill unexpectedly since we default to doing the backfill.
        /// </typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task<ResourceResponse<Document>> ReadPrivate<T, BackfillCheckType>(string documentId, RequestOptions requestOptions, bool retry = false)
        {
            try
            {
                return await this.DocumentClient.ReadDocumentAsync(this.GetDocumentUri(documentId), requestOptions);
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == documentDbNotFound)
                {
                    if (!retry)
                    {
                        await this.EnsureDatabaseAndCollection(typeof(BackfillCheckType));
                        return await this.ReadPrivate<T, BackfillCheckType>(documentId, requestOptions, true);
                    }

                    return null;
                }

                return await this.ScaleThroughputAndRetry(e, () => this.ReadPrivate<T, BackfillCheckType>(documentId, requestOptions));
            }
        }

        /// <summary>The update private.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="document">The document.</param>
        /// <param name="requestOptions">The request options.</param>
        /// <param name="retry">The retry.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task<ResourceResponse<Document>> UpdatePrivate<T>(T document, RequestOptions requestOptions, bool retry = false)
        {
            try
            {
                return await this.DocumentClient.UpsertDocumentAsync(this.GetCollectionUri(), document, requestOptions);
            }
            catch (DocumentClientException e)
            {
                if (e.Error.Code == documentDbNotFound)
                {
                    if (!retry)
                    {
                        await this.EnsureDatabaseAndCollection(typeof(T));
                        return await this.UpdatePrivate(document, requestOptions, true);
                    }

                    return null;
                }

                return await this.ScaleThroughputAndRetry(e, () => this.UpdatePrivate(document, requestOptions));
            }
        }

        /// <summary>The scale throughput and retry.</summary>
        /// <param name="documentClientException">The document client exception.</param>
        /// <param name="func">The lambda.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="DocumentClientException">Document client exception. </exception>
        private async Task<ResourceResponse<Document>> ScaleThroughputAndRetry(DocumentClientException documentClientException, Func<Task<ResourceResponse<Document>>> func)
        {
            if (await this.ShouldRetry(documentClientException))
            {
                return await func.Invoke();
            }

            throw documentClientException;
        }

        /// <summary>The scale throughput and retry.</summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="documentClientException">The document client exception.</param>
        /// <param name="func">The lambda.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="DocumentClientException">Document client exception. </exception>
        private async Task<IList<T>> ScaleThroughputAndRetry<T>(DocumentClientException documentClientException, Func<Task<IList<T>>> func)
        {
            if (await this.ShouldRetry(documentClientException))
            {
                return await func.Invoke();
            }

            throw documentClientException;
        }

        /// <summary>The scale throughput and retry.</summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="documentClientException">The document client exception.</param>
        /// <param name="func">The lambda.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="DocumentClientException">Document client exception. </exception>
        private async Task<FeedResponse<T>> ScaleThroughputAndRetry<T>(DocumentClientException documentClientException, Func<Task<FeedResponse<T>>> func)
        {
            if (await this.ShouldRetry(documentClientException))
            {
                return await func.Invoke();
            }

            throw documentClientException;
        }

        /// <summary>The is scaling up successful.</summary>
        /// <param name="documentClientException">The document client exception.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task<bool> ShouldRetry(DocumentClientException documentClientException)
        {
            if (!documentClientException.Error.Message.Contains("Request rate is large"))
            {
                Logger.LogInformation("Did not detect request rate to large, will not scale resource.");

                return false;
            }

            return await Logger.ExecuteAsync(
                "ScaleResource",
                async () =>
                {
                    Logger.LogInformation("Detected request rate to large. Attempting to scale resource.");

                    if (this.Collection == null)
                    {
                        this.Collection = await this.DocumentClient.ReadDocumentCollectionAsync(this.GetCollectionUri());
                    }

                    var offer = (OfferV2)this.DocumentClient.CreateOfferQuery()
                        .Where(r => r.ResourceLink == this.Collection.SelfLink)
                        .AsEnumerable()
                        .SingleOrDefault();

                    Logger.LogInformation($"OfferDocument: {(offer != null ? JsonConvert.SerializeObject(offer) : string.Empty)}");

                    if (offer?.Timestamp.ToLocalTime() > DateTime.Now.AddMinutes(-5))
                    {
                        Logger.LogInformation($"Offer document was just updated, will not retry.");

                        // if last update was within 5 minutes, just retry
                        return true;
                    }

                    if (offer?.Content?.OfferThroughput * ThroughputFactor < MaximumOfferThroughput)
                    {
                        var oldThroughput = offer.Content.OfferThroughput;
                        var newThroughput = oldThroughput * ThroughputFactor;
                        offer = new OfferV2(offer, newThroughput);
                        await this.DocumentClient.ReplaceOfferAsync(offer);

                        Logger.LogInformation($"Successfully scaled throughput from {oldThroughput} to {newThroughput}");

                        return true;
                    }

                    return false;
                });
        }

        /// <summary>The process response.</summary>
        /// <param name="resourceResponse">The resource response.</param>
        /// <typeparam name="T">The type of object to cast the response back to.</typeparam>
        /// <returns>The instance of <typeparamref name="T"/>.</returns>
        private T ProcessResponse<T>(ResourceResponse<Document> resourceResponse)
        {
            this.ProcessResponseWithoutResult(resourceResponse);

            return (T)(dynamic)resourceResponse?.Resource;
        }

        /// <summary>The process response without result.</summary>
        /// <param name="resourceResponse">The resource response.</param>
        private void ProcessResponseWithoutResult(ResourceResponse<Document> resourceResponse)
        {
            try
            {
                Trace.TraceInformation($"ActivityId: {resourceResponse?.ActivityId}, StatusCode: {(resourceResponse?.StatusCode)}, Charge: {resourceResponse?.RequestCharge}");
            }
            catch (NullReferenceException e)
            {
                //// This should only occur when mocking
                Trace.TraceWarning($"Unable to log resource response information {e}");
            }
        }

        /// <summary>The process feed response.</summary>
        /// <param name="feedResponse">The feed response.</param>
        /// <typeparam name="T">The generic type.</typeparam>
        private void ProcessFeedResponse<T>(FeedResponse<T> feedResponse)
        {
            try
            {
                Trace.TraceInformation($"Read a page of {feedResponse?.Count} elements, ActivityId: {feedResponse?.ActivityId}, Charge: {feedResponse?.RequestCharge}");
            }
            catch (NullReferenceException e)
            {
                //// This should only occur when mocking
                Trace.TraceWarning($"Unable to log feed response information {e}");
            }
        }
    }

    /// <summary>The document DB wrapper trace.</summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    internal sealed class HcmDocDBTrace : TraceSourceBase<HcmDocDBTrace>
    {
        /// <summary>Gets the name.</summary>
        public override string Name => "HcmDocDBTrace";

        /// <summary>Gets the verbosity.</summary>
        public override TraceVerbosity Verbosity => TraceVerbosity.Info;
    }
}