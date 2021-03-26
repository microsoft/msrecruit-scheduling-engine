//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// An OData query action that can be put in a batch or executed directly.
    /// </summary>
    public class XrmHttpClientQuery<T> : XrmHttpClientAction, IXrmHttpClientQuery<T>
    {
        public XrmHttpClientQuery(ILogger logger, IXrmHttpClient xrmHttpClient, HttpMethod method, string requestUri, string contentString = null, bool is404Ok = false, params Tuple<string, string>[] headers)
            : base(logger, xrmHttpClient, method, requestUri, contentString, is404Ok, headers)
        {
        }

        /// <summary>
        /// Execute the query and gets the result.
        /// </summary>
        /// <returns>The result.</returns>
        public async Task<T> ExecuteAndGetAsync()
        {
            using (var response = await this.xrmHttpClient.SendAsyncWithRetry(() => this.ToHttpRequestMessage()))
            {
                return await this.HandleQueryResponse(response);
            }
        }

        public async Task<T> HandleQueryResponse(HttpResponseMessage response)
        {
            var contentString = response.Content == null ? null : await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return await this.HandleNonSuccessResponse(response, contentString);
            }

            var result = string.IsNullOrEmpty(contentString) ? default(T) : JsonConvert.DeserializeObject<T>(contentString, XrmHttpClient.DefaultJsonSerializerSettings);
            this.LogDeserializedResult(response, contentString, result);

            return result;
        }

        private async Task<T> HandleNonSuccessResponse(HttpResponseMessage response, string contentString)
        {
            if (this.is404Ok && response.StatusCode == HttpStatusCode.NotFound)
            {
                this.logger.LogInformation($"XrmHttpClientQuery.HandleQueryResponse({this.ContentId}): Got NotFound ({response.ReasonPhrase}), returning null: {contentString}");
                return default(T);
            }
            else
            {
                var exception = XrmHttpClientExceptionFactory.CreateExceptionFromResponse(response, contentString);

                if (exception is XrmHttpClientMissingAKException
                    && this.Method == HttpMethod.Get
                    && typeof(ODataEntity).IsAssignableFrom(typeof(T)))
                {
                    // TODO: this is all super hacky!
                    this.logger.LogWarning($"XrmHttpClientQuery.HandleQueryResponse({this.ContentId}): Got AK missing error from XRM, trying workaround");

                    var retryResult = await RetryFailedQueryDueToMissingAK(response);
                    if (retryResult != null)
                    {
                        return retryResult.Item1;
                    }
                }

                if (exception is XrmHttpClientRelevanceSearchNotEnabledException
                    && this.Method == HttpMethod.Get)
                {
                    this.logger.LogError($"XrmHttpClientQuery.HandleQueryResponse({this.ContentId}): Relevance search is not enabled for current tenant");
                }

                throw exception;
            }
        }

        private async Task<Tuple<T>> RetryFailedQueryDueToMissingAK(HttpResponseMessage response)
        {
            // Check whether this is a Get by AK query, and we can workaround the failure.
            var keyProperty = ODataEntityContractInfo.GetKeyProperty(typeof(T));
            (var alternateKeyValues, var query) = ParseGetByAKQuery(this.RequestUri, this.Client.BaseAddress);

            if (keyProperty == null || alternateKeyValues == null)
            {
                this.logger.LogWarning($"RetryFailedQueryDueToMissingAK: Got AK missing error from XRM, but cannot retry: (keyProperty={keyProperty?.Name}, alternateKeyValues={alternateKeyValues?.Count})");
                return null;
            }

            // Use a GetAll query to find the record id.
            var record = await GetByAKUsingFilterQuery(alternateKeyValues, query == null ? null : $"$select={ODataField.Field(keyProperty)}");
            if (record == null)
            {
                this.logger.LogWarning($"RetryFailedQueryDueToMissingAK: Record not found using filter, returning null");
                return Tuple.Create(default(T));
            }

            if (query == null)
            {
                this.logger.LogInformation($"RetryFailedQueryDueToMissingAK: Record found using filter and query is null, returning record");
                return Tuple.Create(record);
            }

            this.logger.LogInformation($"RetryFailedQueryDueToMissingAK: Record found using filter, rerunning to handle expand list");

            // Query by the recid we found in order to handle 1:N expands.
            // e.g. https://example.com/base/v1/entityName(GUID)
            var entityByIdPath = ODataPath<T>.FromId(ODataEntityContractInfo.GetKeyPropertyValue(record).Value).ToUri(this.Client.BaseAddress);

            return Tuple.Create(
                await new XrmHttpClientQuery<T>(
                    this.logger,
                    this.Client,
                    HttpMethod.Get,
                    $"{entityByIdPath}?{query}",
                    headers: this.Headers.ToArray())
                .ExecuteAndGetAsync());
        }

        /// <summary>
        /// Parse a query that looks like https://example.com/base/v1/entityName(field1='value1',field2=234)?$select=field3,field4&$expand=...
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="baseAddress">The client base address.</param>
        /// <returns>
        /// The alternateKeyValues: [("field1", "'value1'"), ("field2", "234")] and query: "?$select=field3,field4&$expand=..."
        /// </returns>
        private static ValueTuple<IList<Tuple<string, string>>, string> ParseGetByAKQuery(string requestUri, Uri baseAddress)
        {
            // Strip off "https://example.com/base/v1/entityName" from the start of the url
            var entityPath = ODataPath<T>.FromEntity().ToUri(baseAddress);
            if (requestUri.StartsWith(entityPath))
            {
                var pathAndQuery = requestUri.Substring(entityPath.Length).Split(new[] { '?' }, 2);

                // Set path = "(field1='value1',field2=234)" and query = "?$select=field3,field4&$expand=..."
                var path = pathAndQuery[0];
                var query = pathAndQuery.Length == 2
                    ? pathAndQuery[1]
                    : null;

                if (path.StartsWith("(") && path.EndsWith(")"))
                {
                    var commaSeparated = path.Substring(1, path.Length - 2).Split(',');
                    if (commaSeparated.Length > 0)
                    {
                        var alternateKeyValues = new List<Tuple<string, string>>();
                        foreach (var commaPair in commaSeparated)
                        {
                            var keyValue = commaPair.Split(new[] { '=' }, 2);
                            if (keyValue.Length != 2)
                            {
                                // No equals sign!
                                // e.g. https://example.com/base/v1/entityName(GUID)
                                // This is not an AK query, so return null.
                                return (null, null);
                            }

                            // Add ("field1", "'value1'") to the list
                            alternateKeyValues.Add(Tuple.Create(keyValue[0], keyValue[1]));
                        }

                        return (alternateKeyValues, query);
                    }
                }
            }

            return (null, null);
        }

        private async Task<T> GetByAKUsingFilterQuery(IList<Tuple<string, string>> alternateKeyValues, string query = null)
        {
            // Build a filter that looks like "field1 eq 'value1' and field2 eq 234"
            var filter = string.Join(" and ", alternateKeyValues.Select(kv => $"{kv.Item1} eq {kv.Item2}"));

            var requestUri = ODataPath<T>.FromEntity().ToUri(this.Client.BaseAddress)
                + $"?$filter={filter}"
                + (query == null ? string.Empty : $"&{query}");

            var listResult = await new XrmHttpClientQuery<ODataResponseList<T>>(
                    this.logger,
                    this.Client,
                    HttpMethod.Get,
                    requestUri,
                    headers: new[] { Tuple.Create("Prefer", $"odata.maxpagesize=1") })
                .ExecuteAndGetAsync();

            return listResult.Result.FirstOrDefault();
        }

        private void LogDeserializedResult(HttpResponseMessage response, string contentString, object result)
        {
            if (result == null)
            {
                this.logger.LogInformation($"XrmHttpClientQuery.HandleQueryResponse({this.ContentId}): Deserialized query response as {typeof(T).Name} (null)");
            }
            else if (result is ODataEntity entity)
            {
                this.logger.LogInformation($"XrmHttpClientQuery.HandleQueryResponse({this.ContentId}): Deserialized query response as {typeof(T).Name} (etag={entity.ODataEtag}, context={entity.ODataContext})");
            }
            else if (result is IODataResponseList responseList)
            {
                if (responseList.Result == null)
                {
                    // Queries can fail, yet still result in OK status codes.
                    // In that case, the response will be something like: {"_error":{...}}
                    // Service code is written to expect that the .Result property is always filled in, which it won't be in this scenario.
                    // Thus, we throw an exception.
                    throw XrmHttpClientExceptionFactory.CreateExceptionFromResponse(response, contentString);
                }
                else
                {
                    this.logger.LogInformation($"XrmHttpClientQuery.HandleQueryResponse({this.ContentId}): Deserialized query response as list of {typeof(T).GetGenericArguments().FirstOrDefault()?.Name} (count={responseList.Result.Count()}, totalcount={responseList.ODataCount}, hasnextpage={!string.IsNullOrEmpty(responseList.ODataNextLink)}, context={responseList.ODataContext})");
                }
            }
            else
            {
                this.logger.LogInformation($"XrmHttpClientQuery.HandleQueryResponse({this.ContentId}): Deserialized query response as {typeof(T).Name}");
            }
        }
    }
}
