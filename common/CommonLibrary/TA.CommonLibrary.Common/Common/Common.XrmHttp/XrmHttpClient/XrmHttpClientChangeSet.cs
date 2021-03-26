//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// An OData changeset containing actions.
    /// </summary>
    public class XrmHttpClientChangeSet : IXrmHttpClientChangeSet
    {
        private readonly XrmHttpClientBatch xrmHttpClientBatch;

        public XrmHttpClientChangeSet(XrmHttpClientBatch xrmHttpClientBatch)
        {
            this.xrmHttpClientBatch = xrmHttpClientBatch;
        }

        public IXrmHttpClient Client => this.xrmHttpClientBatch.Client;

        public List<IXrmHttpClientAction> Actions { get; } = new List<IXrmHttpClientAction>();

        /// <summary>
        /// The index in the batch of the last action added.
        /// </summary>
        /// <remarks>Reference in subsequent actions with ODataExpression.ContentId or ODataEntity.ODataBatchContentIdReference.</remarks>
        public int LastContentId => this.xrmHttpClientBatch.LastContentId;

        /// <summary>
        /// Add an action to the changeset.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="httpResponseCallback">A callback to be called with the full response data; optional.</param>
        /// <returns>This batch.</returns>
        public IXrmHttpClientChangeSet Add(IXrmHttpClientAction action, Func<HttpResponseMessage, Task> httpResponseCallback = null)
        {
            if (action != null)
            {
                action.ContentId = ++this.xrmHttpClientBatch.LastContentId;
                this.Actions.Add(action);

                httpResponseCallback = httpResponseCallback
                    ?? (async httpResponse =>
                        {
                            await action.HandleActionResponse(httpResponse);
                        });
                this.xrmHttpClientBatch.Callbacks.Add(action, httpResponseCallback);
            }

            return this;
        }

        /// <summary>
        /// Add an action to the change set.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="entityIdCallback">A callback to be called with the entity id; optional.</param>
        /// <returns>This batch.</returns>
        public IXrmHttpClientChangeSet Add(IXrmHttpClientAction action, Action<Guid> entityIdCallback)
        {
            return this.Add(action, httpResponseCallback: async httpResponse =>
            {
                var result = await action.HandleActionResponse(httpResponse);
                entityIdCallback(result.Value);
            });
        }

        /// <summary>
        /// Add a query action to the change set.
        /// </summary>
        /// <param name="query">The query action.</param>
        /// <param name="deserializedResponseCallback">A callback to be called with the deserialized response data; optional.</param>
        /// <returns>This batch.</returns>
        public IXrmHttpClientChangeSet Add<T>(IXrmHttpClientQuery<T> query, Action<T> deserializedResponseCallback)
        {
            return this.Add(query, httpResponseCallback: async httpResponse =>
            {
                var result = await query.HandleQueryResponse(httpResponse);
                deserializedResponseCallback(result);
            });
        }
    }
}
