//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.DocumentDB.V2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Configuration;

    /// <summary>
    /// The document database repository interface.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    public interface IDocumentDBProvider<T> where T : class
    {
        /// <summary>
        /// Initializes configuration
        /// </summary>
        /// <param name="documentDbConfigurations">Document DB configurations</param>
        /// <param name="collectionId">Collection Id</param>
        void Build(DocumentDBClientConfiguration documentDbConfigurations, string collectionId = null);

        /// <summary>
        /// Initializes configuration
        /// </summary>
        /// <param name="documentDbStorageConfiguration">Document DB configurations</param>
        /// <param name="collectionId">Collection Id</param>
        void Build(DocumentDBStorageConfiguration documentDbStorageConfiguration, string collectionId = null);

        /// <summary>
        /// Gets an interface to query
        /// </summary>
        /// <param name="client">Document Client</param>
        /// /// <param name="feedOptions">Feed options</param>
        /// <returns>The query able interface.</returns>
        IOrderedQueryable<T> GetQueryable(DocumentClient client, FeedOptions feedOptions = null);

        /// <summary>
        /// Gets the document client
        /// </summary>
        /// <param name="documentCollection">The document collection</param>
        /// <param name="partitionId">The partition id</param>
        /// <returns>The document client.</returns>
        Task<DocumentClient> GetClient(DocumentCollection documentCollection = null, string partitionId = null);

        /// <summary>
        /// Gets one item.
        /// </summary>
        /// <param name="id">The id of the document that represents the item.</param>
        /// <returns>The item.</returns>
        Task<T> GetItemAsync(string id);

        /// <summary>Retrieves an item from the document database.</summary>
        /// <param name="id">The id of the item to be retrieved</param>
        /// <param name="partitionKeyValue">The partition Key Value.</param>
        /// <returns>The item being retrieved.</returns>
        Task<T> GetPartitionedItemAsync(string id, string partitionKeyValue = "");

        /// <summary>The get partitioned items async.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="take">The take.</param>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="orderDirection">The order direction.</param>
        /// <typeparam name="TKey">The key</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<IEnumerable<T>> GetPartitionedItemsAsync<TKey>(
            Expression<Func<T, bool>> expression,
            int take = 0,
            string partitionKey = "",
            Expression<Func<T, TKey>> orderBy = null,
            OrderDirection orderDirection = OrderDirection.Ascending);

        /// <summary>
        /// Creates a new item in the document database.
        /// </summary>
        /// <param name="item">The item to be added to the database.</param>
        /// <returns>The document that represents the item.</returns>
        Task<T> CreateItemAsync(T item);

        /// <summary>
        /// Deletes an item in the document database.
        /// </summary>
        /// <param name="id">The id of the document to be deleted.</param>
        /// <returns>True if the item has been deleted.</returns>
        Task<bool> DeleteItemAsync(string id);

        /// <summary>Deletes an item in the document database.</summary>
        /// <param name="id">The id of the document to be deleted.</param>
        /// <param name="requestOptions">The request Options.</param>
        /// <returns>True if the item has been deleted.</returns>
        Task<bool> DeleteItemAsync(string id, RequestOptions requestOptions);

        /// <summary>
        /// Updates a document with the item. 
        /// </summary>
        /// <param name="id">The id of the document to be updated.</param>
        /// <param name="item">The item with updates that will replace the existing document.</param>
        /// <returns>The updated item.</returns>
        Task<T> UpdateItemAsync(string id, T item);

        /// <summary>
        /// Retrieves a list of items from the document database.
        /// </summary>
        /// <param name="predicates">The conditions to retrieve the list of items from the document database.</param>
        /// <param name="take">Number of items to take</param>
        /// <param name="orderBy">The condition to order the results.</param>
        /// <param name="orderDirection">Order direction</param>
        /// <typeparam name="TKey">Type of the order</typeparam>
        /// <returns>A list of items.</returns>
        Task<IEnumerable<T>> GetItemsAsync<TKey>(List<Expression<Func<T, bool>>> predicates = null, int take = 0, Expression<Func<T, TKey>> orderBy = null, OrderDirection orderDirection = OrderDirection.Ascending);
    }
}
