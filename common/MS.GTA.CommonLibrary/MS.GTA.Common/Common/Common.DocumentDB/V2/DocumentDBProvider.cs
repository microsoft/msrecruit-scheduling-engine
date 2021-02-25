//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.DocumentDB.V2
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Base.Utilities;
    using CommonDataService.Common.Internal;
    using Configuration;
    using Exceptions;
    using ServicePlatform.Context;
    using ServicePlatform.Exceptions;
    using ServicePlatform.Tracing;

    /// <summary>
    /// Order direction
    /// </summary>
    public enum OrderDirection
    {
        /// <summary>
        /// Ascending order
        /// </summary>
        Ascending,

        /// <summary>
        /// Descending order
        /// </summary>
        Descending
    }

    /// <summary>
    /// An instance of this class is created for communicating with the document database.
    /// </summary>
    /// <typeparam name="T">Generic type.</typeparam>
    public class DocumentDBProvider<T> : IDocumentDBProvider<T> where T : class
    {
        /// <summary>
        /// Text index precision
        /// </summary>
        private const int StringIndexPrecision = -1;

        /// <summary>
        /// Number index precision
        /// </summary>
        private const int NumberIndexPrecision = -1;

        /// <summary>
        /// The path for default indexing in the document database collection
        /// </summary>
        private const string DefaultIndexingPath = "/";

        /// <summary>
        /// Trace source
        /// </summary>
        private readonly ITraceSource trace;

        /// <summary>
        /// Document DB provider
        /// </summary>
        private readonly IDocumentDB documentDB;

        /// <summary>
        /// The database id
        /// </summary>
        private string databaseId;

        /// <summary>
        /// The collection id
        /// </summary>
        private string collectionId;

        /// <summary> Collection uri </summary>
        private Uri collectionUri;

        /// <summary>
        /// The throughput of the document database collection in response units per second
        /// </summary>
        private int responseUnitsPerSecond;

        /// <summary>
        /// The number of documents to return from the document database
        /// </summary>
        private int numberOfDocumentsToReturn;

        /// <summary>
        /// document configurations
        /// </summary>
        private DocumentDBClientConfiguration documentDBConfigurations;

        /// <summary>
        /// document configurations
        /// </summary>
        private DocumentDBStorageConfiguration documentDBStorageConfiguration;

        /// <summary>
        /// Collection is initialized or not.
        /// </summary>
        private bool collectionIntialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDBProvider{T}" /> class.
        /// </summary>
        /// <param name="trace">Trace source</param>
        /// <param name="documentDB">Document DB provider</param>
        public DocumentDBProvider(ITraceSource trace, IDocumentDB documentDB)
        {
            Contract.CheckValue(trace, nameof(trace));
            Contract.CheckValue(documentDB, nameof(documentDB));

            this.trace = trace;
            this.documentDB = documentDB;
        }

        /// <summary>
        /// Initializes configuration
        /// </summary>
        /// <param name="documentDBConfigurations">Document DB configurations</param>
        /// <param name="collectionId">Collection Id</param>
        public void Build(DocumentDBClientConfiguration documentDBConfigurations, string collectionId = null)
        {
            Contract.CheckValue(documentDBConfigurations, nameof(documentDBConfigurations));

            this.documentDBConfigurations = documentDBConfigurations;
            this.Initialize(
                documentDBConfigurations.DatabaseId,
                collectionId ?? typeof(T).ToString(),
                documentDBConfigurations.ResponseUnitsPerSecond,
                documentDBConfigurations.NumberOfDocumentsReturned);
        }

        /// <summary>
        /// Initializes configuration
        /// </summary>
        /// <param name="documentDBStorageConfiguration">Document DB configurations</param>
        /// <param name="collectionId">Collection Id</param>
        public void Build(DocumentDBStorageConfiguration documentDBStorageConfiguration, string collectionId = null)
        {
            Contract.CheckValue(documentDBStorageConfiguration, nameof(documentDBStorageConfiguration));

            this.documentDBStorageConfiguration = documentDBStorageConfiguration;
            this.Initialize(
                this.documentDBStorageConfiguration.DatabaseId,
                collectionId ?? typeof(T).ToString(),
                this.documentDBStorageConfiguration.ResponseUnitsPerSecond,
                documentDBStorageConfiguration.NumberOfDocumentsReturned);
        }

        /// <summary>
        /// Gets document client
        /// </summary>
        /// <param name="documentCollection">The document collection</param>
        /// <param name="partitionId">The partition Id</param>
        /// <returns>Document Client</returns>
        public async Task<DocumentClient> GetClient(DocumentCollection documentCollection = null, string partitionId = null)
        {
            if (!this.collectionIntialized)
            {
                await this.GetOrCreateCollection(documentCollection, partitionId);
                this.collectionIntialized = true;
            }

            return await this.GetDocumentClient();
        }

        /// <summary>
        /// Retrieves an item from the document database.
        /// </summary>
        /// <param name="id">The id of the item to be retrieved</param>
        /// <returns>The item being retrieved.</returns>
        public async Task<T> GetItemAsync(string id)
        {
            Contract.CheckValue(id, nameof(id));

            try
            {
                using (var client = await this.GetClient())
                {
                    var document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(this.databaseId, this.collectionId, id));
                    this.trace.TraceInformation($"Document DB: Executed ReadDocumentAsync on {this.databaseId} for collection {this.collectionId}: ActivityID {document?.ActivityId} ; Request Units: {document?.RequestCharge} ");
                    return (T)(dynamic)document?.Resource;
                }
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    this.trace.TraceInformation($"Document DB: Document not found {id}.");
                    return null;
                }
                else
                {
                    throw new DocumentDBException($"Document DB: Get document id: {id} failed with error: {e.Message}").EnsureTraced(this.trace);
                }
            }
        }

        /// <summary>
        /// Gets an interface to execute any query
        /// </summary>
        /// <param name="client">Document Client</param>
        /// <param name="feedOptions">The instnce of <see cref="FeedOptions"/>.</param>
        /// <returns>A query collection</returns>
        public IOrderedQueryable<T> GetQueryable(DocumentClient client, FeedOptions feedOptions = null)
        {
            return feedOptions != null
                ? client.CreateDocumentQuery<T>(this.collectionUri, feedOptions)
                : client.CreateDocumentQuery<T>(this.collectionUri);
        }

        /// <summary>The get partitioned items async.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="take">The take.</param>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="orderDirection">The order direction.</param>
        /// <typeparam name="TKey">The key</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<IEnumerable<T>> GetPartitionedItemsAsync<TKey>(
            Expression<Func<T, bool>> expression,
            int take = 0,
            string partitionKey = "",
            Expression<Func<T, TKey>> orderBy = null,
            OrderDirection orderDirection = OrderDirection.Ascending)
        {
            using (var client = await this.GetClient())
            {
                var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true, MaxDegreeOfParallelism = -1 };
                if (!string.IsNullOrEmpty(partitionKey))
                {
                    feedOptions.PartitionKey = new PartitionKey(partitionKey);
                }

                var query = this.GetQueryable(client, feedOptions).Where(expression);
                if (orderBy != null)
                {
                    query = orderDirection == OrderDirection.Ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
                }

                if (take == 0)
                {
                    take = this.numberOfDocumentsToReturn;
                }

                var documentQuery = query.AsDocumentQuery();

                var results = new List<T>();
                while (documentQuery.HasMoreResults)
                {
                    var res = await documentQuery.ExecuteNextAsync<T>();
                    this.trace.TraceInformation($"Document DB: Executed DocumentQuery on {this.databaseId} for collection {this.collectionId}: ActivityID {res?.ActivityId} ; Request Units: {res?.RequestCharge} ");

                    results.AddRange(res);
                }

                return results;
            }

        }


        /// <summary>
        /// Retrieves a list of items from the document database.
        /// </summary>
        /// <param name="predicates">The conditions to retrieve the list of items from the document database.</param>
        /// <param name="take">Number of items to take</param>
        /// <param name="orderBy">The condition to order the results.</param>
        /// <param name="orderDirection">Order direction</param>
        /// <typeparam name="TKey">Type of the order</typeparam>
        /// <returns>A list of items.</returns>
        public async Task<IEnumerable<T>> GetItemsAsync<TKey>(List<Expression<Func<T, bool>>> predicates = null, int take = 0, Expression<Func<T, TKey>> orderBy = null, OrderDirection orderDirection = OrderDirection.Ascending)
        {
            using (var client = await this.GetClient())
            {
                var documentQuery = client.CreateDocumentQuery<T>(this.collectionUri)
                .AsQueryable();

                if (predicates != null)
                {
                    foreach (var predicate in predicates)
                    {
                        documentQuery = documentQuery.Where(predicate);
                    }
                }

                if (orderBy != null)
                {
                    documentQuery = orderDirection == OrderDirection.Ascending ? documentQuery.OrderBy(orderBy) : documentQuery.OrderByDescending(orderBy);
                }

                if (take == 0)
                {
                    take = this.numberOfDocumentsToReturn;
                }

                var query = documentQuery
                                    .Take(take)
                                    .AsDocumentQuery();

                List<T> results = new List<T>();
                while (query.HasMoreResults)
                {
                    var document = await query.ExecuteNextAsync<T>();
                    this.trace.TraceInformation($"Document DB: Executed DocumentQuery on {this.databaseId} for collection {this.collectionId}: ActivityID {document?.ActivityId} ; Request Units: {document?.RequestCharge} ");
                    if (document != null)
                    {
                        results.AddRange(document);
                    }
                }

                return results;
            }
        }

        /// <summary>
        /// Deletes a list of items from the document database.
        /// </summary>
        /// <param name="predicate">The condition to retrieve the list of items from the document database.</param>        
        /// <returns>A list of items.</returns>
        public async Task DeleteItemsAsync(Expression<Func<T, bool>> predicate)
        {
            Contract.CheckValue(predicate, nameof(predicate));
            using (var client = await this.GetClient())
            {
                var query = client.CreateDocumentQuery<T>(this.collectionUri)
                .Where(predicate)
                .AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    foreach (var document in await query.ExecuteNextAsync())
                    {
                        this.trace.TraceInformation($"Document DB: Executed DocumentQuery on {this.databaseId} for collection {this.collectionId}: ActivityID {document?.ActivityId} ; Request Units: {document?.RequestCharge} ");
                        var response = await await client.DeleteDocumentAsync(document.SelfLink);
                        this.trace.TraceInformation($"Document DB: Executed DeleteDocumentAsync on {this.databaseId} for collection {this.collectionId}: ActivityID {response?.ActivityId} ; Request Units: {response?.RequestCharge} ");
                    }
                }
            }
        }

        /// <summary>
        /// Inserts an item into the document database.
        /// </summary>
        /// <param name="item">The item to be stored.</param>
        /// <returns>The document that stores the new item.</returns>
        public async Task<T> CreateItemAsync(T item)
        {
            Contract.CheckValue(item, nameof(item));
            using (var client = await this.GetClient())
            {
                var document = await client.CreateDocumentAsync(this.collectionUri, item);
                this.trace.TraceInformation($"Document DB: Executed CreateDocumentAsync on {this.databaseId} for collection {this.collectionId}: ActivityID {document?.ActivityId} ; Request Units: {document?.RequestCharge} ");
                return (T)(dynamic)document?.Resource;
            }
        }

        /// <summary>
        /// Deletes an item in the document database.
        /// </summary>
        /// <param name="id">The id of the document to be deleted.</param>
        /// <returns>True if the item has been deleted.</returns>
        public async Task<bool> DeleteItemAsync(string id)
        {
            Contract.CheckValue(id, nameof(id));
            using (var client = await this.GetClient())
            {
                var deleted = await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(this.databaseId, this.collectionId, id));
                this.trace.TraceInformation($"Document DB: Executed DeleteDocumentAsync on {databaseId} for collection {collectionId}: ActivityID {deleted?.ActivityId} ; Request Units: {deleted?.RequestCharge}");
                return true;
            }
        }


        /// <summary>Deletes an item in the document database.</summary>
        /// <param name="id">The id of the document to be deleted.</param>
        /// <param name="requestOptions">The request Options.</param>
        /// <returns>True if the item has been deleted.</returns>
        public async Task<bool> DeleteItemAsync(string id, RequestOptions requestOptions)
        {
            Contract.CheckValue(id, nameof(id));
            using (var client = await this.GetClient())
            {
                var deleted = await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(this.databaseId, this.collectionId, id), requestOptions);
                this.trace.TraceInformation($"Document DB: Executed DeleteDocumentAsync on {this.databaseId} for collection {this.collectionId}: ActivityID {deleted?.ActivityId} ; Request Units: {deleted?.RequestCharge} ");
                return true;
            }
        }

        /// <summary>
        /// Updates a document in the document database.
        /// </summary>
        /// <param name="id">The id of the document to be updated.</param>
        /// <param name="item">The item with the updates to be replaced in the document.</param>
        /// <returns>The item with the updates.</returns>
        public async Task<T> UpdateItemAsync(string id, T item)
        {
            Contract.CheckValue(id, nameof(id));
            Contract.CheckValue(item, nameof(item));

            try
            {
                using (var client = await this.GetClient())
                {
                    var document = await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(this.databaseId, this.collectionId, id), item);
                    this.trace.TraceInformation($"Document DB: Executed ReplaceDocumentAsync on {this.databaseId} for collection {this.collectionId}: ActivityID {document?.ActivityId} ; Request Units: {document?.RequestCharge} ");

                    return (T)(dynamic)document?.Resource;
                }
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw new DocumentDBException($"Document DB: Update document id: {id} failed with error: {e.Message}.").EnsureTraced(this.trace);
                }
            }
        }

        /// <summary>
        /// Initializes document DB
        /// </summary>
        /// <param name="databaseId">Database id</param>
        /// <param name="collectionId">Collection id</param>
        /// <param name="responseUnitsPerSecond">Response count per second</param>
        /// <param name="numberOfDocumentsToReturn">Number of documents per request</param>
        private void Initialize(string databaseId, string collectionId, int responseUnitsPerSecond, int numberOfDocumentsToReturn)
        {
            this.databaseId = databaseId;
            this.collectionId = collectionId;
            this.responseUnitsPerSecond = responseUnitsPerSecond;
            this.collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            this.numberOfDocumentsToReturn = numberOfDocumentsToReturn;
        }

        /// <summary>
        /// Gets document client
        /// </summary>
        /// <returns>Document Client</returns>
        private async Task<DocumentClient> GetDocumentClient()
        {
            DocumentClient client = null;
            if (this.documentDBConfigurations != null)
            {
                client = await this.documentDB.GetDocumentClient(this.documentDBConfigurations);
            }
            else if (this.documentDBStorageConfiguration != null)
            {
                client = await this.documentDB.GetDocumentClient(this.documentDBStorageConfiguration);
            }
            else
            {
                throw new DocumentDBException("Document DB: no configuration found.").EnsureTraced(this.trace);
            }

            return client;
        }

        /// <summary>
        /// Creates a collection.
        /// </summary>
        /// <param name="documentCollection">The document collection</param>
        /// <param name="partitionId">The partition id</param>
        /// <returns>Nothing but style cop thinks it does.</returns>
        private async Task GetOrCreateCollection(DocumentCollection documentCollection = null, string partitionId = null)
        {
            using (var client = await this.GetDocumentClient())
            {
                await CommonLogger.Logger.ExecuteAsync(
                    "HcmDBReadCollect",
                    async () =>
                    {
                        try
                        {
                            var response = await client.ReadDocumentCollectionAsync(this.collectionUri);
                            this.trace.TraceInformation($"Document DB: Executed ReadDatabaseAsync on {this.databaseId} for collection {this.collectionId}: ActivityID {response?.ActivityId} ; Request Units: {response?.RequestCharge} ");
                        }
                        catch (DocumentClientException e)
                        {
                            if (e.StatusCode == HttpStatusCode.NotFound)
                            {
                                this.trace.TraceInformation($"Document DB: Create collection: {this.collectionId}.");

                                if (documentCollection == null)
                                {
                                    documentCollection = new DocumentCollection();
                                    documentCollection.Id = this.collectionId;
                                    if (!string.IsNullOrEmpty(partitionId))
                                    {
                                        documentCollection.PartitionKey?.Paths?.Add(partitionId);
                                    }

                                    documentCollection.IndexingPolicy.IncludedPaths.Add(new IncludedPath
                                    {
                                        Path = DefaultIndexingPath,
                                        Indexes = new Collection<Index>
                                        {
                                        new RangeIndex(DataType.Number) { Precision = NumberIndexPrecision },
                                        new RangeIndex(DataType.String) { Precision = StringIndexPrecision },
                                        }
                                    });
                                }

                                await CommonLogger.Logger.ExecuteAsync(
                                    "HcmDBCreCollect",
                                    async () =>
                                    {
                                        var document = await client.CreateDocumentCollectionAsync(
                                            UriFactory.CreateDatabaseUri(this.databaseId),
                                            documentCollection,
                                            new RequestOptions { OfferThroughput = this.responseUnitsPerSecond });
                                        this.trace.TraceInformation($"Document DB: Executed CreateDocumentCollectionAsync on {this.databaseId} for collection {this.collectionId}: ActivityID {document?.ActivityId} ; Request Units: {document?.RequestCharge} ");

                                    });
                            }
                            else
                            {
                                throw new DocumentDBException($"Document DB: Read collection id: {this.collectionId} failed with error: {e.Message}.").EnsureTraced(this.trace);
                            }
                        }
                    });
            }
        }

        public Task<T> GetPartitionedItemAsync(string id, string partitionKeyValue = "")
        {
            throw new NotImplementedException();
        }
    }
}
