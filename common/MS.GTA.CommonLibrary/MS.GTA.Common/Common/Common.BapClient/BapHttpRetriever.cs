//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace MS.GTA.Common.BapClient
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using BapClient.Contracts;
    using Base.Configuration;
    using Base.Exceptions;
    using Base.Utilities;
    using CommonDataService.Common.Internal;
    using Configuration;
    using Constants;
    using Exceptions;
    using MS.GTA.ServicePlatform.Utils;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using ServicePlatform.Azure.AAD;
    using ServicePlatform.Communication.Http;
    using ServicePlatform.Communication.Http.Routers;
    using ServicePlatform.Configuration;
    using ServicePlatform.Context;

    /// <summary>
    /// Creates an HTTP helper to make BAP calls.
    /// </summary>
    public class BapHttpRetriever : IBapHttpRetriever
    {
        /// <summary>The retry count.</summary>
        private const int MaxRetryCount = 5;

        /// <summary>The user id header for BAP service call.</summary>
        protected const string BapUserIdHeader = "x-ms-override-object-id";

        /// <summary>The tenant id header for BAP service call.</summary>
        protected const string BapTenantIdHeader = "x-ms-override-tenant-id";

        /// <summary>The client id header for BAP/CDS service call.</summary>
        protected const string ClientRequestIdHeader = "x-ms-client-request-id";

        /// <summary>The session id header for BAP/CDS service call.</summary>
        protected const string ClientSessionIdHeader = "x-ms-client-session-id";

        /// <summary>The router.</summary>
        private readonly SingleUriRouter router;

        /// <summary>The http communication client factory.</summary>
        private readonly IHttpCommunicationClientFactory httpCommunicationClientFactory;

        /// <summary>
        /// The AAD client configuration.
        /// </summary>
        private readonly AADClientConfiguration aadClientConfiguration;

        /// <summary>
        /// The BAP client configuration.
        /// </summary>
        private readonly BapClientConfiguration bapClientConfiguration;

        private readonly ILogger<BapHttpRetriever> logger;

        /// <summary>
        /// The http client.
        /// </summary>
        private IHttpCommunicationClient httpClient;

        private readonly IAzureActiveDirectoryClient azureActiveDirectoryClient;

        /// <summary>Initializes a new instance of the <see cref="BapHttpRetriever"/> class.</summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="httpCommunicationClientFactory">The http Communication Client Factory.</param>
        /// <param name="logger">The instnce for <see cref="ILogger{BapHttpRetriever}"/>.</param>
        /// <param name="azureActiveDirectoryClient">The instnce for <see cref="IAzureActiveDirectoryClient"/>.</param>
        public BapHttpRetriever(
            IConfigurationManager configurationManager,
            IHttpCommunicationClientFactory httpCommunicationClientFactory,
            ILogger<BapHttpRetriever> logger,
            IAzureActiveDirectoryClient azureActiveDirectoryClient)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));
            Contract.CheckValue(httpCommunicationClientFactory, nameof(httpCommunicationClientFactory));

            this.aadClientConfiguration = configurationManager.Get<AADClientConfiguration>();
            var environmentName = configurationManager.Get<EnvironmentNameConfiguration>().EnvironmentName;
            this.bapClientConfiguration = BapEnvironmentSettings.GetBAPClientConfiguration(environmentName);

            this.httpCommunicationClientFactory = httpCommunicationClientFactory;
            this.logger = logger;
            this.router = new SingleUriRouter(new Uri(this.bapClientConfiguration.BapBaseUrl));
            this.azureActiveDirectoryClient = azureActiveDirectoryClient;
        }

        /// <summary>Send a request as the service async.</summary>
        /// <param name="url">The url.</param>
        /// <param name="tenantId">The tenant Id, if not specified the tenant id from the active token will be used.</param>
        /// <param name="userObjectId">The user object id</param>
        /// <param name="content">The post message content.</param>
        /// <param name="method">The method.</param>
        /// <param name="retryOnTransientErrors">Retry on transient errors.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<HttpResponseMessage> SendAsServiceAsync(string url, string tenantId, string userObjectId = null, HttpContent content = null, HttpMethod method = null, bool retryOnTransientErrors = false)
        {
            Contract.CheckNonEmpty(url, nameof(url));
            Contract.CheckNonEmpty(tenantId, nameof(tenantId));
            Contract.CheckValueOrNull(userObjectId, nameof(userObjectId));
            Contract.CheckValueOrNull(content, nameof(content));
            Contract.CheckValueOrNull(method, nameof(method));

            return await this.SendAsync(url, tenantId, userObjectId, content, method, retryOnTransientErrors);
        }

        /// <summary>Send a request as a user async.</summary>
        /// <param name="url">The url.</param>
        /// <param name="tenantId">The tenant Id.</param>
        /// <param name="userObjectId">The user Object Id.</param>
        /// <param name="content">The content.</param>
        /// <param name="method">The method.</param>
        /// <param name="retryOnTransientErrors">Retry on transient errors.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<HttpResponseMessage> SendAsUserAsync(string url, string tenantId, string userObjectId, HttpContent content = null, HttpMethod method = null, bool retryOnTransientErrors = false)
        {
            Contract.CheckNonEmpty(url, nameof(url));
            Contract.CheckNonEmpty(tenantId, nameof(tenantId));
            Contract.CheckNonEmpty(userObjectId, nameof(userObjectId));
            Contract.CheckValueOrNull(content, nameof(content));
            Contract.CheckValueOrNull(method, nameof(method));

            return await this.SendAsync(url, tenantId, userObjectId, content, method, retryOnTransientErrors);
        }

        /// <summary>Send a request with a token async.</summary>
        /// <param name="url">The url.</param>
        /// <param name="token">The token.</param>
        /// <param name="method">The method.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<HttpResponseMessage> SendWithTokenAsync(string url, string token, HttpMethod method = null)
        {
            Contract.CheckNonEmpty(url, nameof(url));
            Contract.CheckNonEmpty(token, nameof(token));
            Contract.CheckValueOrNull(method, nameof(method));

            if (this.httpClient == null)
            {
                this.httpClient = this.httpCommunicationClientFactory.CreateGTA(this.router, new HttpCommunicationClientOptions() { ThrowOnNonSuccessResponse = false });
            }

            var requestId = Guid.NewGuid();

            var message = new HttpRequestMessage(HttpMethod.Get, url);
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(Base.Constants.ApplicationJsonMediaType));
            message.Headers.Authorization = new AuthenticationHeaderValue($"{Base.Constants.BearerAuthenticationScheme}", token);
            message.Headers.Add(ClientRequestIdHeader, requestId.ToString());

            if (method != null)
            {
                message.Method = method;
            }

            this.logger.LogInformation($"Making a {message.Method} request to {url} with request id {requestId}");

            HttpResponseMessage response;
            try
            {
                response = await this.httpClient.SendAsync(message, CancellationToken.None);
            }
            catch (Exception e)
            {
                throw new BapHttpRetrieverException($"Received an exception when calling the BAP service {url} with request id {requestId}: {e}");
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                this.logger.LogInformation($"Received a not found response from the BAP service with request id {requestId}, returning null - response was {await response.Content.ReadAsStringAsync()}");
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new BapHttpRetrieverException($"Received a non success response from the BAP service with request id {requestId} code was {response.StatusCode}, response was {await response.Content.ReadAsStringAsync()}");
            }

            return response;
        }

        /// <summary>The attach API version.</summary>
        /// <param name="url">The url.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string AttachApiVersion(string url)
        {
            var endpoint = $"{url}?api-version={this.bapClientConfiguration.BapApiVersion}";
            this.logger.LogInformation($"Returning this endpoint for use {endpoint}");
            return endpoint;
        }

        /// <summary>The attach API version.</summary>
        /// <param name="url">The url.</param>
        /// <param name="apiVersion">The API version.</param>
        /// <param name="expand">The expand value string.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string AppendQueryParams(string url, string apiVersion = null, string expand = null)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                throw new BapHttpRetrieverException($"Invalid url {url} supplied!");
            }

            if (!ApiVersion.isValidApiVersion(apiVersion))
            {
                apiVersion = ApiVersion.DefaultApiVersion;
            }

            var apiVersionQueryParam = $"?api-version={apiVersion}";
            if (!Expand.isValidExpandString(expand))
            {
                expand = Expand.None;
            }

            var endpoint = $"{url}{apiVersionQueryParam}{expand}".Trim();
            this.logger.LogInformation($"Returning endpoint with query params for use: {endpoint}");
            return endpoint;
        }

        /// <summary>Attaches the XRM API version.</summary>
        /// <param name="url">The url.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string AttachXRMApiVersion(string url)
        {
            var endpoint = $"{url}?api-version={this.bapClientConfiguration.BapXRMApiVersion}";
            this.logger.LogInformation($"Returning this endpoint for use {endpoint}");
            return endpoint;
        }

        /// <summary>The create authorization header.</summary>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task<string> GetAuthorizationHeader()
        {
            return await this.logger.ExecuteAsync(
                "GetAuthorizationHeader",
                async () =>
                {
                    var token = await this.azureActiveDirectoryClient.GetAppOnlyAccessTokenAsync(
                        this.aadClientConfiguration.AADInstance.FormatWithInvariantCulture(this.bapClientConfiguration.BapAADTenantId),
                        this.bapClientConfiguration.BapResourceId);
                    return token?.AccessToken;
                });
        }

        /// <summary>Send a request async.</summary>
        /// <param name="url">The url.</param>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="userObjectId">The user object id.</param>
        /// <param name="content">The content.</param>
        /// <param name="method">The method.</param>
        /// <param name="retryOnTransientErrors">Retry on transient errors.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="BapHttpRetrieverException">The BAP HTTP request exception.</exception>
        private async Task<HttpResponseMessage> SendAsync(string url, string tenantId, string userObjectId, HttpContent content = null, HttpMethod method = null, bool retryOnTransientErrors = false)
        {
            Contract.CheckNonEmpty(url, nameof(url));
            Contract.CheckNonEmpty(tenantId, nameof(tenantId));
            Contract.CheckValueOrNull(userObjectId, nameof(userObjectId));
            Contract.CheckValueOrNull(content, nameof(content));
            Contract.CheckValueOrNull(method, nameof(method));

            return await this.logger.ExecuteAsync(
                "HcmCmnBapHttpRetriever",
                async () =>
                {
                    if (this.httpClient == null)
                    {
                        this.httpClient = this.httpCommunicationClientFactory.CreateGTA(this.router, new HttpCommunicationClientOptions() { ThrowOnNonSuccessResponse = false });
                    }

                    var retryCount = 1;
                    if (!retryOnTransientErrors)
                    {
                        // ensure that the retry count > MaxRetryCount
                        retryCount += MaxRetryCount;
                    }

                    var shouldRetry = false;
                    HttpResponseMessage response = null;
                    var exponentialBackoffDelay = new ExponentialBackoffDelay(TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(30000));
                    Guid requestId;

                    do
                    {
                        var message = await this.GetHttpRequestMessage(url, tenantId, userObjectId, method, content);
                        requestId = Guid.NewGuid();
                        message.Headers.Add(ClientRequestIdHeader, requestId.ToString());

                        this.logger.LogInformation($"Making a {message.Method} request to {url} with request id {requestId} for tenant {tenantId}");

                        try
                        {
                            response = await this.httpClient.SendAsync(message, CancellationToken.None);
                        }
                        catch (Exception e)
                        {
                            throw new BapHttpRetrieverException($"Received an exception when calling the BAP service {url} with request id {requestId} for tenant {tenantId} : {e}");
                        }

                        if (response?.IsSuccessStatusCode == true)
                        {
                            return response;
                        }

                        shouldRetry = this.ShouldRetry(message.Method, response.StatusCode, retryCount);
                        if (shouldRetry)
                        {
                            var headerDelay = response?.Headers?.RetryAfter?.Delta;
                            var delay = headerDelay != null ? headerDelay.Value : exponentialBackoffDelay.GetDelay(retryCount);
                            this.logger.LogInformation($"Retry Count: {retryCount}; will make new call after {delay.TotalSeconds} seconds");
                            await Task.Delay(delay);
                        }
                        retryCount++;
                    } while (shouldRetry);

                    return await this.HandleFailedRequest(response, requestId, tenantId);
                }, new [] { typeof(BapHttpRetrieverException) });
        }

        private bool ShouldRetry(HttpMethod method, HttpStatusCode statusCode, int retryCount)
        {
            if (method != HttpMethod.Get)
            {
                return false;
            }

            if (retryCount > MaxRetryCount)
            {
                return false;
            }

            if (statusCode == HttpStatusCode.Forbidden)
            {
                return false;
            }

            return statusCode == HttpStatusCode.ServiceUnavailable ||
                statusCode == HttpStatusCode.GatewayTimeout ||
                statusCode == HttpStatusCode.NotFound;
        }

        private async Task<HttpRequestMessage> GetHttpRequestMessage(string url, string tenantId, string userObjectId, HttpMethod method = null, HttpContent content = null)
        {
            var httpMethod = this.GetHttpMethod(content, method);
            var message = new HttpRequestMessage(httpMethod, url);
            if (content != null)
            {
                message.Content = content;
            }

            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(Base.Constants.ApplicationJsonMediaType));
            message.Headers.Authorization = new AuthenticationHeaderValue($"{Base.Constants.BearerAuthenticationScheme}", await this.GetAuthorizationHeader());

            if (userObjectId != null)
            {
                this.logger.LogInformation($"Adding user id to the request header");
                message.Headers.Add(BapUserIdHeader, userObjectId);
            }
            message.Headers.Add(BapTenantIdHeader, tenantId);

            return message;
        }

        private HttpMethod GetHttpMethod(HttpContent content = null, HttpMethod method = null)
        {
            var httpMethod = HttpMethod.Get;
            if (content != null)
            {
                httpMethod = HttpMethod.Post;
            }
            
            if (method != null)
            {
                httpMethod = method;
            }

            return httpMethod;
        }

        private async Task<HttpResponseMessage> HandleFailedRequest(HttpResponseMessage response, Guid requestId, string tenantId)
        {
            var responseString = await response?.Content?.ReadAsStringAsync();
            
            if (response?.StatusCode == HttpStatusCode.NotFound)
            {
                this.logger.LogInformation($"Received a not found response from the BAP service with request id {requestId} for tenant {tenantId}, returning null - response was {responseString}");
                return null;
            }

            var error = JsonConvert.DeserializeObject<BapException>(responseString);

            if (response?.StatusCode == HttpStatusCode.Forbidden && error?.Error?.Code == "GraphRequestFailed")
            {
                throw new ApplicationPrincipalNotProvisionedException($"Received a forbidden response from the BAP service with request id {requestId} because the application principal is not provisioned for tenant {tenantId}, response was {responseString}");
            }
            
            throw new BapHttpRetrieverException($"Received a non success response from the BAP service with request id {requestId} for tenant {tenantId} code was {response?.StatusCode}, response was {responseString}", error?.Error?.Code, error?.Error?.Message);
        }
    }
}
