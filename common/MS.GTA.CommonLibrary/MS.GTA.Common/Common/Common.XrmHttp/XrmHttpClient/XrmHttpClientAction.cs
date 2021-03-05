//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An OData action that can be put in a batch or executed directly.
    /// </summary>
    public class XrmHttpClientAction : IXrmHttpClientAction
    {
        protected readonly ILogger logger;
        protected readonly IXrmHttpClient xrmHttpClient;
        protected readonly bool is404Ok;

        public XrmHttpClientAction(ILogger logger, IXrmHttpClient xrmHttpClient, HttpMethod method, string requestUri, string contentString = null, bool is404Ok = false, params Tuple<string, string>[] headers)
        {
            this.logger = logger;
            this.xrmHttpClient = xrmHttpClient;
            this.is404Ok = is404Ok;
            this.Method = method;
            this.RequestUri = requestUri;
            this.Content = contentString;
            this.Headers = headers;
        }

        public HttpMethod Method { get; set; }

        public string RequestUri { get; set; }

        public string Content { get; set; }

        public IEnumerable<Tuple<string, string>> Headers { get; }

        public IXrmHttpClient Client
        {
            get
            {
                return xrmHttpClient;
            }
        }

        /// <summary>
        /// The index of the action in the batch.
        /// </summary>
        /// <remarks>Reference in subsequent actions with ODataExpression.ContentId or ODataEntity.ODataBatchContentIdReference.</remarks>
        public int ContentId { get; set; }

        /// <summary>
        /// Execute the action.
        /// </summary>
        /// <returns>The task to await.</returns>
        public async Task ExecuteAsync()
        {
            using (var response = await this.xrmHttpClient.SendAsyncWithRetry(() => this.ToHttpRequestMessage()))
            {
                await HandleActionResponse(response);
            }
        }

        public async Task<Guid?> HandleActionResponse(HttpResponseMessage response)
        {
            if (this.is404Ok && response.StatusCode == HttpStatusCode.NotFound)
            {
                var contentString = response.Content == null ? null : await response.Content.ReadAsStringAsync();
                this.logger.LogInformation($"XrmHttpClientAction.HandleActionResponse({this.ContentId}): Got NotFound ({response.ReasonPhrase}), returning null: {contentString}");
                return null;
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw await XrmHttpClientExceptionFactory.CreateExceptionFromResponse(response);
            }

            if (response.Headers.TryGetValues("Location", out var location))
            {
                return ODataUriParse.TryGetEntityIdFromUri(location.FirstOrDefault());
            }

            return null;
        }

        public HttpContent ToHttpContent()
        {
            var stringBuilder = new StringBuilder(2048);
            stringBuilder.Append(this.Method + " " + this.RequestUri + " HTTP/1.1\r\n");
            stringBuilder.Append("Host: " + this.xrmHttpClient.BaseAddress.Authority + "\r\n");
            stringBuilder.Append("Accept: application/json\r\n");

            foreach (var header in this.Headers)
            {
                stringBuilder.Append($"{header.Item1}: {header.Item2}\r\n");
            }

            if (this.Content != null)
            {
                stringBuilder.Append("Content-Type: application/json; charset=utf-8\r\n");
            }

            stringBuilder.Append("\r\n");

            if (this.Content != null)
            {
                stringBuilder.Append(Content);
            }

            var content = new StringContent(stringBuilder.ToString(), Encoding.UTF8, "application/http")
            {
                Headers =
                {
                    ContentType = { CharSet = null },
                },
            };
            content.Headers.Add("Content-Transfer-Encoding", "binary");
            content.Headers.Add("Content-ID", this.ContentId.ToString());

            return content;
        }

        public HttpRequestMessage ToHttpRequestMessage()
        {
            var request = new HttpRequestMessage(this.Method, this.xrmHttpClient.BaseAddress.GetLeftPart(UriPartial.Authority) + this.RequestUri)
            {
                Content = this.Content == null ? null : new StringContent(this.Content, Encoding.UTF8, "application/json"),
                Headers = { Accept = { new MediaTypeWithQualityHeaderValue("application/json") } },
            };

            foreach (var header in this.Headers)
            {
                request.Headers.Add(header.Item1, header.Item2);
            }

            return request;
        }
    }
}
