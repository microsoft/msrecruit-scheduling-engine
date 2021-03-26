//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// An OData batch containing actions (possibly inside changesets).
    /// </summary>
    public class XrmHttpClientBatch : IXrmHttpClientBatch
    {
        private readonly ILogger logger;

        public XrmHttpClientBatch(IXrmHttpClient xrmHttpClient)
        {
            this.logger = xrmHttpClient.Logger;
            this.Client = xrmHttpClient;
        }

        public IXrmHttpClient Client { get; }

        public List<IXrmHttpClientAction> Actions { get; } = new List<IXrmHttpClientAction>();

        public List<IXrmHttpClientChangeSet> ChangeSets { get; } = new List<IXrmHttpClientChangeSet>();

        public bool ContinueOnError { get; set; } = false;

        /// <summary>
        /// The index in the batch of the last action added.
        /// </summary>
        /// <remarks>Reference in subsequent actions with ODataExpression.ContentId or ODataEntity.ODataBatchContentIdReference.</remarks>
        public int LastContentId { get; internal set; } = 0;

        /// <summary>
        /// The callbacks to execute with the results for each action in the batch.
        /// </summary>
        public Dictionary<IXrmHttpClientAction, Func<HttpResponseMessage, Task>> Callbacks { get; } = new Dictionary<IXrmHttpClientAction, Func<HttpResponseMessage, Task>>();

        /// <summary>
        /// Add an action to the batch.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="httpResponseCallback">A callback to be called with the full response data; optional.</param>
        /// <returns>This batch.</returns>
        public IXrmHttpClientBatch Add(IXrmHttpClientAction action, Func<HttpResponseMessage, Task> httpResponseCallback = null)
        {
            if (action != null)
            {
                action.ContentId = ++this.LastContentId;
                this.Actions.Add(action);

                httpResponseCallback = httpResponseCallback
                    ?? (async httpResponse =>
                        {
                            await action.HandleActionResponse(httpResponse);
                        });
                this.Callbacks.Add(action, httpResponseCallback);
            }

            return this;
        }

        /// <summary>
        /// Add an action to the batch.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="entityIdCallback">A callback to be called with the entity id; optional.</param>
        /// <returns>This batch.</returns>
        public IXrmHttpClientBatch Add(IXrmHttpClientAction action, Action<Guid> entityIdCallback)
        {
            return this.Add(action, httpResponseCallback: async httpResponse =>
            {
                var result = await action.HandleActionResponse(httpResponse);
                entityIdCallback(result.Value);
            });
        }

        /// <summary>
        /// Add a query action to the batch.
        /// </summary>
        /// <param name="query">The query action.</param>
        /// <param name="deserializedResponseCallback">A callback to be called with the deserialized response data; optional.</param>
        /// <returns>This batch.</returns>
        public IXrmHttpClientBatch Add<T>(IXrmHttpClientQuery<T> query, Action<T> deserializedResponseCallback)
        {
            return this.Add(query, httpResponseCallback: async httpResponse =>
            {
                var result = await query.HandleQueryResponse(httpResponse);
                deserializedResponseCallback(result);
            });
        }

        /// <summary>
        /// Constructs a new changeset.
        /// </summary>
        /// <returns>A new instance of the <see cref="XrmHttpClientChangeSet"/> class.</returns>
        public IXrmHttpClientChangeSet AddNewChangeSet()
        {
            var transaction = new XrmHttpClientChangeSet(this);
            this.ChangeSets.Add(transaction);
            return transaction;
        }

        /// <summary>
        /// Executes the batch.
        /// </summary>
        /// <returns>The task to await.</returns>
        /// <remarks>Executes all callbacks for returned data; users should use that to get access to query results.</remarks>
        public async Task ExecuteAsync()
        {
            this.logger.LogDebug($"XrmHttpClientBatch.ExecuteAsync: Executing batch with {this.LastContentId} actions");

            this.ChangeSets.RemoveAll(c => !c.Actions.Any());

            if (!this.ChangeSets.Any() && !this.Actions.Any())
            {
                this.logger.LogDebug($"XrmHttpClientBatch.ExecuteAsync: Skipping empty batch execution");
                return;
            }

            using (var response = await this.Client.SendAsyncWithRetry(() => this.ToHttpRequestMessage()))
            {
                if (!response.Content.IsMimeMultipartContent())
                {
                    throw await XrmHttpClientExceptionFactory.CreateExceptionFromResponse(response);
                }

                var responseMultipart = await response.Content.ReadAsMultipartAsync();
                int changeSets = 0, actions = 0;
                foreach (var part in responseMultipart.Contents)
                {
                    if (part.IsMimeMultipartContent())
                    {
                        var changeSet = this.ChangeSets[changeSets++];

                        var partContents = await part.ReadAsMultipartAsync();
                        int changeSetActions = 0;
                        foreach (var subpart in partContents.Contents)
                        {
                            await this.ParseAndRunCallbackForActionResponse(changeSet.Actions[changeSetActions++], subpart);
                        }
                    }
                    else
                    {
                        await this.ParseAndRunCallbackForActionResponse(this.Actions[actions++], part);
                    }
                }

                this.logger.LogDebug($"XrmHttpClientBatch.ExecuteAsync: Deserialized and ran callbacks for batch");
            }
        }

        /// <summary>
        /// Split the batch into multiple batches with each batch having a maximum number of actions.
        /// </summary>
        /// <param name="maximumActionsPerBatch">Maximum actions per batch.</param>
        /// <returns>Enumerator for the batches.</returns>
        public IEnumerable<IXrmHttpClientBatch> Split(int maximumActionsPerBatch = 50)
        {
            var actionsQueued = 0;
            var realBatch = new XrmHttpClientBatch(this.Client) { ContinueOnError = this.ContinueOnError };

            this.ChangeSets.RemoveAll(c => !c.Actions.Any());

            foreach (var changeSet in this.ChangeSets)
            {
                if (actionsQueued > 0 && actionsQueued + changeSet.Actions.Count >= maximumActionsPerBatch)
                {
                    yield return realBatch;
                    realBatch = new XrmHttpClientBatch(this.Client) { ContinueOnError = this.ContinueOnError };
                    actionsQueued = 0;
                }

                var realChangeSet = realBatch.AddNewChangeSet();
                foreach (var action in changeSet.Actions)
                {
                    this.Callbacks.TryGetValue(action, out var callback);
                    realChangeSet.Add(action, callback);
                    actionsQueued++;
                }
            }

            foreach (var action in this.Actions)
            {
                if (actionsQueued > 0 && actionsQueued >= maximumActionsPerBatch)
                {
                    yield return realBatch;
                    realBatch = new XrmHttpClientBatch(this.Client) { ContinueOnError = this.ContinueOnError };
                    actionsQueued = 0;
                }

                this.Callbacks.TryGetValue(action, out var callback);
                realBatch.Add(action, callback);
                actionsQueued++;
            }

            yield return realBatch;
        }

        private HttpRequestMessage ToHttpRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, this.Client.BaseAddress + "$batch")
            {
                Content = this.BuildRequestContent(),
                Headers =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") },
                },
            };

            if (this.ContinueOnError)
            {
                request.Headers.Add("Prefer", "odata.continue-on-error");
            }

            return request;
        }

        private MultipartContent BuildRequestContent()
        {
            var multipartContent = new MultipartContent("mixed", "batch_" + Guid.NewGuid().ToString("N"));

            foreach (var changeSet in this.ChangeSets)
            {
                var changeSetContent = new MultipartContent("mixed", "changeset_" + Guid.NewGuid().ToString("N"));
                multipartContent.Add(changeSetContent);

                foreach (var action in changeSet.Actions)
                {
                    this.TraceAction(action);
                    changeSetContent.Add(action.ToHttpContent());
                }
            }

            foreach (var action in this.Actions)
            {
                this.TraceAction(action);
                multipartContent.Add(action.ToHttpContent());
            }

            return multipartContent;
        }

        private void TraceAction(IXrmHttpClientAction action)
        {
            var fullUri = new Uri(this.Client.BaseAddress.GetLeftPart(UriPartial.Authority) + action.RequestUri);
            var uri = this.Client.ShouldTracePotentialPII
                ? fullUri.ToString()
                : XrmHttpClientUriSanitizer.SanitizeUri(fullUri);
            var content = this.Client.ShouldTracePotentialPII || string.IsNullOrEmpty(action.Content)
                ? action.Content
                :  "...";
            this.logger.LogInformation($"XrmHttpClientBatch.ExecuteAsync: Adding {action.Method} action {action.ContentId} to request: {uri} ({string.Join(" ; ", action.Headers.Select(h => $"{h.Item1}: {h.Item2}"))}): {content}");
        }

        private async Task ParseAndRunCallbackForActionResponse(IXrmHttpClientAction action, HttpContent part)
        {
            part.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("msgtype", "response"));
            var content = await part.ReadAsHttpResponseMessageAsync();

            if (this.Callbacks.TryGetValue(action, out var callback))
            {
                await callback(content);
            }
        }
    }
}
