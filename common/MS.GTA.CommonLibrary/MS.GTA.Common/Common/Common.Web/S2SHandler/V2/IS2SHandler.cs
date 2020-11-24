//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IS2SHandler.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Web.S2SHandler.V2
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using MS.GTA.Common.Contracts;
    using MS.GTA.ServicePlatform.Communication.Http;
    using MS.GTA.ServicePlatform.Communication.Http.Routers;

    /// <summary>The S2SHandler interface.</summary>
    public interface IS2SHandler
    {
        /// <summary>The get full service URL. For scenarios outside of service fabric routing.</summary>
        /// <param name="serviceUrlsEnum">The service URLS enumerable.</param>
        /// <returns>The <see cref="string"/>.</returns>
        /// <remarks>
        /// In the future if we use something other than service fabric
        /// this should be updated to point to the correct routes based on service location.
        /// </remarks>
        string GetFullServiceUrl(ServiceUrls serviceUrlsEnum);

        /// <summary>Make a service to service get call to another internal service.
        /// Will make a determination based on running mode where to call out to.</summary>
        /// <param name="serviceUrlsEnum">The service URLS ENUM.</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="T">The type to decode the response to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> MakeGetCall<T>(ServiceUrls serviceUrlsEnum, string relativeUrl, CancellationToken cancellationToken = default(CancellationToken))
            where T : class;

        /// <summary>Make a service to service post call to another internal service.
        /// Will make a determination based on running mode where to call out to.</summary>
        /// <param name="serviceUrlsEnum">The service URLS enumerable.</param>
        /// <param name="relativeUrl">The relative URLS.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="T">The type to deserialize the response back to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> MakePostCall<T>(
            ServiceUrls serviceUrlsEnum,
            string relativeUrl,
            object payload,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : class;

        /// <summary>Make a service to service get call to another internal service in the context of a user.
        /// Will make a determination based on running mode where to call out to.</summary>
        /// <param name="serviceUrlsEnum">The service URLS ENUM.</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="T">The type to decode the response to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> MakeUserGetCall<T>(ServiceUrls serviceUrlsEnum, string relativeUrl, CancellationToken cancellationToken = default(CancellationToken))
            where T : class;

        /// <summary>Make a service to service post call to another internal service in the context of a user.
        /// Will make a determination based on running mode where to call out to.</summary>
        /// <param name="serviceUrlsEnum">The service URLS enumerable.</param>
        /// <param name="relativeUrl">The relative URLS.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="T">The type to deserialize the response back to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> MakeUserPostCall<T>(
            ServiceUrls serviceUrlsEnum,
            string relativeUrl,
            object payload,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : class;

        /// <summary>Make a S2S with a service url enumerable call.
        /// NOTE: Prefer enumerable version for type safety and auto router generation.</summary>
        /// <param name="serviceUrlsEnum">The service URLS enumerable.</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="method">The method.</param>
        /// <param name="body">The body.</param>
        /// <param name="requestContext">The value from <see cref="RequestContext"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="environmentId">The d365 environment id</param>
        /// <typeparam name="T">The type to decode the response to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> MakeCall<T>(
            ServiceUrls serviceUrlsEnum,
            string relativeUrl,
            HttpMethod method = null,
            HttpContent body = null,
            RequestContext requestContext = RequestContext.App,
            CancellationToken cancellationToken = default(CancellationToken),
            string environmentId = null)
            where T : class;

        /// <summary>The make call with a router.</summary>
        /// <param name="router">The router.</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="method">The method.</param>
        /// <param name="body">The body.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="requestContext">The value from <see cref="RequestContext"/>.</param>
        /// <typeparam name="T">The type to decode the response to.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<T> MakeCall<T>(
            IHttpRouter router,
            string relativeUrl,
            HttpMethod method = null,
            HttpContent body = null,
            RequestContext requestContext = RequestContext.App,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : class;

        /// <summary>
        /// Make a S2S with a service url enumerable call.
        /// NOTE: Prefer enumerable version for type safety and auto router generation.
        /// </summary>
        /// <param name="serviceUrlsEnum">The service URLS enumerable.</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="method">The method.</param>
        /// <param name="body">The body.</param>
        /// <param name="requestContext">The request Context.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="environmentId">The d365 environment id</param>
        /// <param name="environmentMode">The d365 environment mode</param>
        /// <param name="authenticationToken">The base64 encoded token from an Authentication header, if null, it'll be pulled from the RequestContext</param>
        /// <param name="tenantId">TenantId value to pass through via header</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<HttpResponseMessage> MakeCall(
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
            string tenantId = null);

        /// <summary>The make call.</summary>
        /// <param name="router">The router.</param>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="method">The method.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="requestContext">The request Context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="environmentId">The d365 environment id</param>
        /// <param name="environmentMode">The d365 environment mode</param>
        /// <param name="authenticationToken">The base64 encoded token from an Authentication header, if null, it'll be pulled from the RequestContext</param>
        /// <param name="tenantId">TenantId value to pass through via header</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<HttpResponseMessage> MakeCall(
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
            string tenantId = null);

        /// <summary>The make call.</summary>
        /// <param name="router">The router.</param>
        /// <param name="requestMessage">The request message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<HttpResponseMessage> MakeCall(IHttpRouter router, HttpRequestMessage requestMessage, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>The make call.</summary>
        /// <param name="router">The router.</param>
        /// <param name="requestMessage">The request message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="httpCommunicationClientOptions"></param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<HttpResponseMessage> MakeCall(IHttpRouter router, HttpRequestMessage requestMessage, HttpCommunicationClientOptions httpCommunicationClientOptions, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets an <see cref="IHttpRouter"/> instance for the service.
        /// </summary>
        /// <param name="serviceUrlsEnum">The service to connect to.</param>
        /// <param name="requestRetryOptions">The request retry options; optional.</param>
        /// <returns>An <see cref="IHttpRouter"/> instance.</returns>
        IHttpRouter GetRouter(ServiceUrls serviceUrlsEnum, RequestRetryOptions requestRetryOptions = null);
    }
}
