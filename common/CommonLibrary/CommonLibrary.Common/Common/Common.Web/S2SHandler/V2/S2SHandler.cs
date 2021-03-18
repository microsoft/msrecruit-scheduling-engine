//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.Web.S2SHandler.V2
{
    using Microsoft.Extensions.Logging;
    using Microsoft.ServiceFabric.Services.Client;
    using CommonLibrary.Common.Base;
    using CommonLibrary.Common.Base.Configuration;
    using CommonLibrary.Common.Base.Security.V2;
    using CommonLibrary.Common.Contracts;
    using CommonLibrary.Common.Web.Configuration;
    using CommonLibrary.CommonDataService.Common;
    using CommonLibrary.ServicePlatform.Azure.AAD;
    using CommonLibrary.ServicePlatform.Communication.Http;
    using CommonLibrary.ServicePlatform.Communication.Http.Routers;
    using CommonLibrary.ServicePlatform.Configuration;
    using CommonLibrary.ServicePlatform.Context;
    using CommonLibrary.ServicePlatform.Fabric;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;


    /// <summary>
    /// The service to service handler, making calls to other internal services easier.
    /// </summary>
    public class S2SHandler : IS2SHandler
    {
        private readonly IHttpCommunicationClientFactory httpCommunicationClientFactory;
        private readonly IAzureActiveDirectoryClient azureActiveDirectoryClient;
        private readonly AADClientConfiguration aadClientConfiguration;
        private readonly S2SHandlerConfiguration s2SHandlerConfiguration;
        private readonly EnvironmentConfiguration environmentConfiguration;
        private readonly ILogger<S2SHandler> logger;
        private readonly ILoggerFactory loggerFactory;
        private readonly IHCMPrincipalRetriever hcmPrincipalRetriever;

        public S2SHandler(
            IHttpCommunicationClientFactory httpCommunicationClientFactory,
            IConfigurationManager configurationManager,
            IAzureActiveDirectoryClient azureActiveDirectoryClient,
            ILoggerFactory loggerFactory,
            IHCMPrincipalRetriever hcmPrincipalRetriever)
        {
            this.loggerFactory = loggerFactory;
            this.aadClientConfiguration = configurationManager.Get<AADClientConfiguration>();
            this.azureActiveDirectoryClient = azureActiveDirectoryClient;
            this.httpCommunicationClientFactory = httpCommunicationClientFactory ?? throw new ArgumentNullException(nameof(httpCommunicationClientFactory));
            this.s2SHandlerConfiguration = configurationManager.Get<S2SHandlerConfiguration>();
            this.environmentConfiguration = configurationManager.Get<EnvironmentConfiguration>();
            this.logger = loggerFactory.CreateLogger<S2SHandler>();
            this.hcmPrincipalRetriever = hcmPrincipalRetriever;

            this.logger.LogInformation($"Starting s2s handler as console mode: {this.environmentConfiguration?.IsConsoleApp}");
        }

        /// <summary>The get fabric service url. For fabric S2S calls.</summary>
        /// <param name="serviceUrlsEnum">The service urls enum.</param>
        /// <returns>The <see cref="string"/>.</returns>
        [Obsolete("Please use the get fabric service url in the s2s handler configuration")]
        public static string GetFabricServiceUrl(ServiceUrls serviceUrlsEnum)
        {
            return $"fabric:/{S2SHandlerConfiguration.ApplicationNameMap[serviceUrlsEnum]}/{S2SHandlerConfiguration.ServiceNameMap[serviceUrlsEnum]}";
        }

        /// <summary>The get full service URL. For scenarios outside of service fabric routing.</summary>
        /// <param name="serviceUrlsEnum">The service URLS enumerable.</param>
        /// <returns>The <see cref="string"/>.</returns>
        /// <remarks>
        /// In the future if we use something other than service fabric
        /// this should be updated to point to the correct routes based on service location.
        /// </remarks>
        [Obsolete("Please use the get full service in the s2s handler configuration")]
        public string GetFullServiceUrl(ServiceUrls serviceUrlsEnum)
        {
            return this.s2SHandlerConfiguration.GetFullServiceUrl(serviceUrlsEnum, null, this.environmentConfiguration.Name);
        }

        /// <summary>Make a service to service get call to another internal service.
        /// Will make a determination based on running mode where to call out to.</summary>
        /// <param name="serviceUrlsEnum">The service URLS ENUM.</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="T">The type to decode the response to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<T> MakeGetCall<T>(ServiceUrls serviceUrlsEnum, string relativeUrl, CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {
            var router = this.GetRouter(serviceUrlsEnum);

            var response = await this.MakeCall(router, relativeUrl, HttpMethod.Get, null, RequestContext.App, null, cancellationToken);

            return await this.DecodeResponse<T>(response);
        }

        /// <summary>Make a service to service post call to another internal service.
        /// Will make a determination based on running mode where to call out to.</summary>
        /// <param name="serviceUrlsEnum">The service URLS enumerable.</param>
        /// <param name="relativeUrl">The relative URLS.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="T">The type to deserialize the response back to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<T> MakePostCall<T>(
            ServiceUrls serviceUrlsEnum,
            string relativeUrl,
            object payload,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {
            var router = this.GetRouter(serviceUrlsEnum);

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await this.MakeCall(router, relativeUrl, HttpMethod.Post, content, RequestContext.App, null, cancellationToken);

            return await this.DecodeResponse<T>(response);
        }

        /// <summary>Make a service to service get call to another internal service in the context of a user.
        /// Will make a determination based on running mode where to call out to.</summary>
        /// <param name="serviceUrlsEnum">The service URLS ENUM.</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="T">The type to decode the response to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<T> MakeUserGetCall<T>(ServiceUrls serviceUrlsEnum, string relativeUrl, CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {
            var router = this.GetRouter(serviceUrlsEnum);

            var response = await this.MakeCall(router, relativeUrl, HttpMethod.Get, null, RequestContext.User, null, cancellationToken);

            return await this.DecodeResponse<T>(response);
        }

        /// <summary>Make a service to service post call to another internal service in the context of a user.
        /// Will make a determination based on running mode where to call out to.</summary>
        /// <param name="serviceUrlsEnum">The service URLS enumerable.</param>
        /// <param name="relativeUrl">The relative URLS.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="T">The type to deserialize the response back to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<T> MakeUserPostCall<T>(
            ServiceUrls serviceUrlsEnum,
            string relativeUrl,
            object payload,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {
            var router = this.GetRouter(serviceUrlsEnum);

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await this.MakeCall(router, relativeUrl, HttpMethod.Post, content, RequestContext.User, null, cancellationToken);

            return await this.DecodeResponse<T>(response);
        }

        public async Task<T> MakeCall<T>(
            ServiceUrls serviceUrlsEnum,
            string relativeUrl,
            HttpMethod method = null,
            HttpContent body = null,
            RequestContext requestContext = RequestContext.App,
            CancellationToken cancellationToken = default(CancellationToken),
            string environmentId = null)
            where T : class
        {
            var response = await this.MakeCall(serviceUrlsEnum, relativeUrl, method, body, requestContext, null, cancellationToken, environmentId);
            var decoded = await this.DecodeResponse<T>(response);

            return decoded;
        }

        public async Task<T> MakeCall<T>(
            IHttpRouter router,
            string relativeUrl,
            HttpMethod method = null,
            HttpContent body = null,
            RequestContext requestContext = RequestContext.App,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : class
        {
            var response = await this.MakeCall(router, relativeUrl, method, body, requestContext, null, cancellationToken);
            var decoded = await this.DecodeResponse<T>(response);

            return decoded;
        }

        public async Task<HttpResponseMessage> MakeCall(
            ServiceUrls serviceUrlsEnum,
            string relativeUrl,
            HttpMethod method = null,
            HttpContent body = null,
            RequestContext requestContext = RequestContext.App,
            Dictionary<string, string> headers = null,
            CancellationToken cancellationToken = default(CancellationToken),
            string environmentId = null,
            EnvironmentMode? environmentMode = null,
            string authenticationToken = null,
            string tenantId = null)
        {
            var router = this.GetRouter(serviceUrlsEnum);

            return await this.MakeCall(router, relativeUrl, method, body, requestContext, headers, cancellationToken, environmentId, environmentMode, authenticationToken, tenantId);
        }

        public async Task<HttpResponseMessage> MakeCall(
            IHttpRouter router,
            string relativeUrl,
            HttpMethod method = null,
            HttpContent body = null,
            RequestContext requestContext = RequestContext.App,
            Dictionary<string, string> headers = null,
            CancellationToken cancellationToken = default(CancellationToken),
            string environmentId = null,
            EnvironmentMode? environmentMode = null,
            string authenticationToken = null,
            string tenantId = null)
        {
            if (method == null)
            {
                method = HttpMethod.Get;
            }

            string authorizationHeader = authenticationToken != null ? CreateBearerHeader(authenticationToken) : await CreateAuthenticationHeaderFor(requestContext);
            var request = GenerateRequest(relativeUrl, method, authorizationHeader, headers, body, environmentId, environmentMode, tenantId);
            return await this.MakeCall(router, request, cancellationToken);
        }

        /// <summary>The make call.</summary>
        /// <param name="router">The router.</param>
        /// <param name="requestMessage">The request message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<HttpResponseMessage> MakeCall(IHttpRouter router, HttpRequestMessage requestMessage, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.MakeCall(router, requestMessage, new HttpCommunicationClientOptions(), cancellationToken);
        }

        /// <summary>The make call.</summary>
        /// <param name="router">The router.</param>
        /// <param name="requestMessage">The request message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="httpCommunicationClientOptions"></param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<HttpResponseMessage> MakeCall(IHttpRouter router, HttpRequestMessage requestMessage, HttpCommunicationClientOptions httpCommunicationClientOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.logger.ExecuteAsync(
                "S2S_MakeCall",
                async () =>
                {
                    using (var client = this.httpCommunicationClientFactory.CreateGTA(
                        router,
                        httpCommunicationClientOptions,
                        this.loggerFactory))
                    {
                        Contract.AssertValue(client, nameof(client));
                        logger.LogInformation($"Making {requestMessage.Method} call to {requestMessage.RequestUri}");

                        var response = await client?.SendAsync(requestMessage, cancellationToken);

                        this.logger.LogInformation($"Finished making call to {requestMessage.RequestUri} response was {response?.StatusCode}");

                        return response;
                    }
                },
                new[] { typeof(HttpCommunicationException), typeof(MonitoredFabricException), typeof(NonSuccessHttpResponseException) });
        }

        /// <summary>The decode response.</summary>
        /// <param name="response">The response.</param>
        /// <typeparam name="T">The type to decode the response to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task<T> DecodeResponse<T>(HttpResponseMessage response)
            where T : class
        {
            Contract.CheckValue(response, nameof(response));

            var jsonString = await response.Content.ReadAsStringAsync();

            this.logger.LogInformation(
                $"Decoding response of length {jsonString?.Length} to type {typeof(T).FullName}");

            var deserialized = JsonConvert.DeserializeObject<T>(jsonString);

            return deserialized;
        }

        public static HttpRequestMessage GenerateRequest(
            string relativeUrl,
            HttpMethod method,
            string authorizationHeader,
            HttpContent body = null,
            string environmentId = null,
            EnvironmentMode? environmentMode = null,
            string tenantId = null)
        {
            return GenerateRequest(
                relativeUrl,
                method,
                authorizationHeader,
                null,
                body,
                environmentId,
                environmentMode,
                tenantId);
        }

        public static HttpRequestMessage GenerateRequest(
            string relativeUrl,
            HttpMethod method,
            string authorizationHeader,
            Dictionary<string, string> headers = null,
            HttpContent body = null,
            string environmentId = null,
            EnvironmentMode? environmentMode = null,
            string tenantId = null)
        {
            Contract.CheckNonEmpty(relativeUrl, nameof(relativeUrl));
            Contract.CheckValue(method, nameof(method));
            if (authorizationHeader == null)
            {
                throw new ArgumentNullException(nameof(authorizationHeader));
            }

            var relativeUri = new Uri(relativeUrl, UriKind.Relative);

            var requestMessage = new HttpRequestMessage(method, relativeUri);
            requestMessage.Headers.Add("Authorization", authorizationHeader);

            if (environmentId != null)
            {
                requestMessage.Headers.Add("x-ms-environment-id", environmentId);
            }

            if (environmentMode != null)
            {
                requestMessage.Headers.Add("x-ms-environment-mode", ((int)environmentMode).ToString(CultureInfo.InvariantCulture));
            }

            if (tenantId != null)
            {
                requestMessage.Headers.Add("x-ms-tenant-id", tenantId);
            }

            if (body != null)
            {
                requestMessage.Content = body;
            }

            if (headers != null)
            {
                foreach (var headerPair in headers)
                {
                    requestMessage.Headers.Remove(headerPair.Key);
                    requestMessage.Headers.Add(headerPair.Key, headerPair.Value);
                }
            }

            return requestMessage;
        }

        public IHttpRouter GetRouter(ServiceUrls serviceUrlsEnum, RequestRetryOptions requestRetryOptions = null)
        {
            if (requestRetryOptions == null)
            {
                requestRetryOptions = new RequestRetryOptions();
            }

            if (this.s2SHandlerConfiguration.TargetConsoleApplication)
            {
                var url = this.s2SHandlerConfiguration.GetBaseUrl(this.environmentConfiguration?.Name);

                this.logger.LogInformation($"Using single uri router with url {url}");

                return new SingleUriRouter(new Uri(url), requestRetryOptions);
            }
            else if (this.environmentConfiguration?.IsConsoleApp == true)
            {
                var url = this.s2SHandlerConfiguration.GetFullServiceUrl(serviceUrlsEnum, null, this.environmentConfiguration?.Name);

                this.logger.LogInformation($"Using single uri router with url {url}");

                return new SingleUriRouter(new Uri(url), requestRetryOptions);
            }
            else
            {
                var url = this.s2SHandlerConfiguration.GetFabricUrl(serviceUrlsEnum);

                this.logger.LogInformation($"Using fabric uri router with url {url}");

                return new FabricServiceRouter(
                    ServicePartitionResolver.GetDefault(),
                    new FabricServiceEndpoint(new Uri(url)),
                    requestRetryOptions,
                    this.loggerFactory);
            }
        }

        private static string CreateBearerHeader(string token)
        {
            return "Bearer " + token;
        }

        private async Task<string> CreateAuthenticationHeaderFor(RequestContext requestContext)
        {
            if (requestContext == RequestContext.User)
            {
                this.logger.LogInformation($"Attempting to make request in the context of a user using authorization header");

                if (!string.IsNullOrEmpty(this.hcmPrincipalRetriever.Principal.Token))
                {
                    return $"{Constants.BearerAuthenticationScheme} {this.hcmPrincipalRetriever.Principal.Token}";
                }

                throw new Exception("No authorization header present");
            }

            var authenticationResult = await this.azureActiveDirectoryClient.GetAppOnlyAccessTokenAsync(
                 "https://login.microsoftonline.com/72f988bf-86f1-41af-91ab-2d7cd011db47",
                 this.aadClientConfiguration.ClientId);

            return authenticationResult.CreateAuthorizationHeader();
        }
    }
}
