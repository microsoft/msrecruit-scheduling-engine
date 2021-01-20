// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// An OData changeset containing actions.
    /// </summary>
    public interface IXrmHttpClientChangeSet
    {
        IXrmHttpClient Client { get; }

        List<IXrmHttpClientAction> Actions { get; }

        /// <summary>
        /// The index in the batch of the last action added.
        /// </summary>
        /// <remarks>Reference in subsequent actions with ODataExpression.ContentId or ODataEntity.ODataBatchContentIdReference.</remarks>
        int LastContentId { get; }

        /// <summary>
        /// Add an action to the changeset.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="httpResponseCallback">A callback to be called with the full response data; optional.</param>
        /// <returns>This batch.</returns>
        IXrmHttpClientChangeSet Add(IXrmHttpClientAction action, Func<HttpResponseMessage, Task> httpResponseCallback = null);

        /// <summary>
        /// Add an action to the change set.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="entityIdCallback">A callback to be called with the entity id; optional.</param>
        /// <returns>This batch.</returns>
        IXrmHttpClientChangeSet Add(IXrmHttpClientAction action, Action<Guid> entityIdCallback);

        /// <summary>
        /// Add a query action to the change set.
        /// </summary>
        /// <param name="query">The query action.</param>
        /// <param name="deserializedResponseCallback">A callback to be called with the deserialized response data; optional.</param>
        /// <returns>This batch.</returns>
        IXrmHttpClientChangeSet Add<T>(IXrmHttpClientQuery<T> query, Action<T> deserializedResponseCallback);
    }
}