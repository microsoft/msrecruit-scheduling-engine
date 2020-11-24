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
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// An OData batch containing actions (possibly inside changesets).
    /// </summary>
    public interface IXrmHttpClientBatch
    {
        IXrmHttpClient Client { get; }

        List<IXrmHttpClientAction> Actions { get; }

        List<IXrmHttpClientChangeSet> ChangeSets { get; }

        bool ContinueOnError { get; set; }

        /// <summary>
        /// The index in the batch of the last action added.
        /// </summary>
        /// <remarks>Reference in subsequent actions with ODataExpression.ContentId or ODataEntity.ODataBatchContentIdReference.</remarks>
        int LastContentId { get; }

        /// <summary>
        /// The callbacks to execute with the results for each action in the batch.
        /// </summary>
        Dictionary<IXrmHttpClientAction, Func<HttpResponseMessage, Task>> Callbacks { get; }

        /// <summary>
        /// Add an action to the batch.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="httpResponseCallback">A callback to be called with the full response data; optional.</param>
        /// <returns>This batch.</returns>
        IXrmHttpClientBatch Add(IXrmHttpClientAction action, Func<HttpResponseMessage, Task> httpResponseCallback = null);

        /// <summary>
        /// Add an action to the batch.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="entityIdCallback">A callback to be called with the entity id; optional.</param>
        /// <returns>This batch.</returns>
        IXrmHttpClientBatch Add(IXrmHttpClientAction action, Action<Guid> entityIdCallback);

        /// <summary>
        /// Add a query action to the batch.
        /// </summary>
        /// <param name="query">The query action.</param>
        /// <param name="deserializedResponseCallback">A callback to be called with the deserialized response data; optional.</param>
        /// <returns>This batch.</returns>
        IXrmHttpClientBatch Add<T>(IXrmHttpClientQuery<T> query, Action<T> deserializedResponseCallback);

        /// <summary>
        /// Constructs a new changeset.
        /// </summary>
        /// <returns>A new instance of the <see cref="IXrmHttpClientChangeSet"/> class.</returns>
        IXrmHttpClientChangeSet AddNewChangeSet();

        /// <summary>
        /// Executes the batch.
        /// </summary>
        /// <returns>The task to await.</returns>
        /// <remarks>Executes all callbacks for returned data; users should use that to get access to query results.</remarks>
        Task ExecuteAsync();

        /// <summary>
        /// Split the batch into multiple batches with each batch having a maximum number of actions.
        /// </summary>
        /// <param name="maximumActionsPerBatch">Maximum actions per batch.</param>
        /// <returns>Enumerator for the batches.</returns>
        IEnumerable<IXrmHttpClientBatch> Split(int maximumActionsPerBatch = 50);
    }
}
