//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.CommonDataService.Instrumentation;
using MS.GTA.ServicePlatform.Communication.Http.Internal;
using MS.GTA.ServicePlatform.Communication.Http.Routers;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.ServicePlatform.Exceptions;
using MS.GTA.ServicePlatform.Fabric;
using MS.GTA.ServicePlatform.Utils;
using Microsoft.Extensions.Logging;

namespace MS.GTA.ServicePlatform.Communication.Http.Handlers
{
    /// <summary>
    /// A routing handler that executes an inner retry loop around an instance of <see cref="IHttpRouter"/> provided
    /// during construction. 
    /// 
    /// Before the retry loop starts, the handler creates a router request and asks for the first endpoint. This lets
    /// routers create their <see cref="IHttpRouterRequest"/> holding the retry state for a single request execution. The 
    /// initial call to <see cref="IHttpRouterRequest.GetNextEndpointAsync(HttpRequestMessage, HttpResponseMessage, HttpCommunicationException, CancellationToken)"/>
    /// will have both <see cref="HttpResponseMessage"/> and <see cref="HttpCommunicationException"/> set to null. The routers
    /// are expected to provide the initial base URL for the first attempt.
    /// 
    /// If the request may need to be retried, the handler will ask the <see cref="IHttpRouterRequest"/> for 
    /// the next endpoint again. This time however either the <see cref="HttpResponseMessage"/> or the 
    /// <see cref="HttpCommunicationException"/> will be provided, but never both. 
    /// 
    /// A retry is only initiated if one of the following occurs:
    /// 
    ///     - A non-success <see cref="HttpResponseMessage"/> is returned
    ///     - A monitored <see cref="HttpCommunicationException"/> is thrown
    ///     - An unmonitored <see cref="HttpRequestException"/> is thrown (will be wrapped in the appropriate <see cref="HttpCommunicationException"/> for the GetNextEndpointAsync call)
    /// 
    /// The router has three options to handle any call to GetNextEndpointAsync:
    /// 
    ///     1) Return null
    ///     2) Return a base URL 
    ///     3) Throw an exception
    /// 
    /// Returning null means that the router does not want to continue the retry loop because there are no more eligible 
    /// endpoints or attempts left. In this case the non-success response from the last attempt will be returned, or 
    /// the original exception from the last attempt will be re-thrown (Monitored or unmonitored. Unmonitored 
    /// <see cref="HttpRequestException"/> is only wrapped in a monitored <see cref="HttpCommunicationException"/>
    /// for the GetNextEndpointAsync call in this case).
    /// 
    /// Throwing an exception from the GetNextEndpointAsync call on the other hand means that there was an exception in the 
    /// router itself. In this case this exception will be directly thrown out of the routing handler without any additional
    /// processing.
    /// 
    /// Returning a base URL will result in a new request attempt to be initiated against this host.
    /// </summary>
    /// <remarks>
    /// The <see cref="IHttpRouter"/> and its <see cref="IHttpRouterRequest"/> are responsible for breaking the retry loop. The handler
    /// will throw a <see cref="RoutingHandlerRetryLimitReachedException"/> in case the retry loop is not broken after 100 attempts.
    /// 
    /// Even though the handler wraps <see cref="HttpRequestException"/>s in the appropriate <see cref="HttpCommunicationException"/> for 
    /// the GetNextEndpointAsync call, the original exception is re-thrown if the <see cref="IHttpRouterRequest"/> returns null.
    /// 
    /// The <see cref="HttpRequestMessage"/> is reused across retries. This means that messages containing streamed content may not be 
    /// retriable even though the handler may attempt to retry them. It is up to the consumer to resolve this situation.
    /// 
    /// Because the <see cref="HttpRequestMessage"/> comes from outside of the handler, the handler will not dispose it. 
    /// 
    /// The handler will dispose any <see cref="HttpResponseMessage"/> objects that are not propagated to the callers due to a
    /// retry or an exception.
    /// </remarks>
    public sealed class HttpClientRoutingHandler : DelegatingHandler
    {
        // This is a limit that should in reality never be reached as the router
        // should exhaust its own retries much sooner
        private const int RetryAttemptsLimit = 100;
        private readonly IHttpRouter router;
        private readonly ILogger logger;
        private readonly ILoggerFactory loggerFactory;
        private bool disposed;

        /// <summary>
        /// Creates a new HTTP routing handler given the provided <paramref name="router"/>.
        /// </summary>
        [Obsolete("Please use the method with the logger factory to facilitate switching off of trace source")]
        public HttpClientRoutingHandler(IHttpRouter router)
        {
            Contract.CheckValue(router, nameof(router));
            
            this.router = router;
        }

        /// <summary>
        /// Creates a new HTTP routing handler given the provided <paramref name="router"/> and <paramref name="metricLogger"/>.
        /// </summary>
        [Obsolete("Please use the method with the logger factory to correctly list the source name")]
        public HttpClientRoutingHandler(IHttpRouter router, ILogger logger) : this(router)
        {
            Contract.CheckValue(logger, nameof(logger));
            
            this.logger = logger;
        }
        
        /// <summary>
        /// Creates a new HTTP routing handler given the provided <paramref name="router"/> and <paramref name="metricLogger"/>.
        /// TODO: When ready. Switch the logger in this class to explicity require ILogger<HttpClientRoutingHandler> to correctly list the SourceName in kusto
        /// </summary>
        public HttpClientRoutingHandler(IHttpRouter router, ILoggerFactory loggerFactory) : this(router, loggerFactory.CreateLogger<HttpClientRoutingHandler>())
        {
            this.loggerFactory = loggerFactory;
        }

        // Test hook to validate the router instance
        internal IHttpRouter Router
        {
            get { return router; }
        }

        /// <inheritdoc />
        protected sealed override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.logger == null)
            {
                return ServiceContext.Activity.ExecuteAsync(
                    RoutingHandlerActivityType.Instance,
                    () => SendLoopAsync(request, cancellationToken));                
            }
            
            return this.logger.ExecuteAsync(
                RoutingHandlerActivityType.Instance,
                () => SendLoopAsync(request, cancellationToken),
                new [] { typeof(HttpCommunicationException), typeof(MonitoredFabricException) });
        }

        // The main SendAsync retry loop
        private async Task<HttpResponseMessage> SendLoopAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(HttpClientRoutingHandler));
            }

            Contract.CheckValue(request, nameof(request));
            Contract.CheckValue(request, nameof(request));
            Contract.CheckValue(request.RequestUri, nameof(request) + "." + nameof(request.RequestUri));
            Contract.Check(!request.RequestUri.IsAbsoluteUri, "Expected relative request URI");

            // Needs to be kept aside because we'll be changing the URL on the request object
            Uri relativeRequestUri = request.RequestUri;
            HttpResponseMessage responseToReturn = null;
            IHttpRouterRequest routerRequest = null;

            try
            {
                CommunicationTraceSource.Instance.TraceInformation("Creating router request");
                routerRequest = router.CreateRouterRequest(this.loggerFactory);

                CommunicationTraceSource.Instance.TraceInformation("Requesting router endpoint for the first time");
                Uri nextEndpoint = await routerRequest.GetNextEndpointAsync(
                    request: request,
                    lastResponse: null,
                    lastException: null,
                    cancellationToken: cancellationToken);

                // We loop until the operation succeeds or the router decides to stop the loop. It is up to the 
                // router to decide if it needs to pause, and for how long.
                var attemptCount = 0;

                while (++attemptCount <= RetryAttemptsLimit)
                {
                    if (this.logger == null)
                    {
                        responseToReturn = await ServiceContext.Activity.ExecuteAsync(
                            SendRoutedRequestActivityType.Instance,
                            async () =>
                            {
                                // Before making the request we need to combine the router base URL and the request relative URL
                                request.RequestUri = CreateUri(nextEndpoint, relativeRequestUri);
                                HttpResponseMessage currentResponse = null;
                    
                                try
                                {
                                    // Not using Task.ContinueWith since when SendAsync throw exception we will receive 
                                    // aggregation exception in the continuation task which is not expected
                                    CommunicationTraceSource.Instance.TraceInformation(String.Format(
                                        "Executing HTTP request: RequestUri={0}, TargetHostUri={1}, RelativeRequestUri={2}, Attempt={3}",
                                        request.RequestUri,
                                        nextEndpoint,
                                        relativeRequestUri,
                                        attemptCount));

                                    var stopwatch = Stopwatch.StartNew();
                                    currentResponse = await base.SendAsync(request, cancellationToken);
                                    ServiceContext.Activity.Current.AddHttpStatusCode((int)currentResponse.StatusCode);
                                    logger?.LogMetric(DefaultMetricConstants.MetricNamespace, DefaultMetricConstants.OutgoingHttpOperationDurationMetricName, new SortedList<string, string> { { DefaultMetricConstants.HttpStatusCodeDimension, currentResponse.StatusCode.ToString() } }, stopwatch.ElapsedMilliseconds);
                    
                                    // If successful response, just return it
                                    if (currentResponse.IsSuccessStatusCode)
                                        return currentResponse;
                    
                                    // Otherwise ask the router on non-success response whether we should retry again
                                    // To make this decision, the router is given the endpoint used for the last attempt and 
                                    // the response we received from the server.
                                    // 
                                    // Note that routers are free to use whatever logic necessary to obtain endpoint (including internal retries, etc.)
                                    CommunicationTraceSource.Instance.TraceInformation($"Requesting router endpoint for non-successful response. StatusCode={currentResponse.StatusCode}");
                                    request.RequestUri = relativeRequestUri;
                                    nextEndpoint = await routerRequest.GetNextEndpointAsync(
                                        request: request,
                                        lastResponse: currentResponse,
                                        lastException: null,
                                        cancellationToken: cancellationToken);
                    
                                    if (nextEndpoint == null)
                                    {
                                        CommunicationTraceSource.Instance.TraceInformation("Received response will be returned because router did not return another endpoint");
                                        return currentResponse;
                                    }
                    
                                    // Will be retried with the new endpoint, release the response
                                    CommunicationTraceSource.Instance.TraceInformation($"Router returned the next endpoint to try: '{nextEndpoint}'");
                    
                                    currentResponse.Dispose();
                                    currentResponse = null;
                                    return null;
                                }
                                catch (Exception ex) when (!ex.IsFatal() && currentResponse == null)
                                {
                                    // Only capture exceptions out of SendAsync here, i.e. (lastResponse == null) to avoid invoking router's 
                                    // GetNextEndpointAsync for exceptions thrown out of that method. We also only capture HTTP communication
                                    // exceptions that get thrown by the base handler in place of HttpRequestException
                                    //
                                    CommunicationTraceSource.Instance.TraceError("An exception encountered while sending the request: {0}", ex.ToString());
                    
                                    // Only attempt a router retry on HTTP communication exceptions
                                    //
                                    HttpCommunicationException communicationException;
                                    if (!HttpCommunicationException.TryCreate(ex, out communicationException))
                                    {
                                        throw;
                                    }
                    
                                    // Ask the router on exception from the http message handler stack whether we should retry again.
                                    // To make this decision, the router is given the exception that was thrown. Note that routers are 
                                    // free to use whatever logic necessary to obtain endpoint (including internal retries, etc.)
                                    // 
                                    // Note that HttpClient stack only throws an HttpRequestException in case the request cannot be completed. 
                                    // That is for example when the endpoint is unreachable, the connection gets terminated in the middle of the 
                                    // request, the content length is incorrect etc. It does NOT throw any exceptions when the request finishes 
                                    // with a non-successful HTTP response from the server like for example HTTP 500 Internal Server Error. This
                                    // situation is already handled above.
                                    //
                                    request.RequestUri = relativeRequestUri;
                                    nextEndpoint = await routerRequest.GetNextEndpointAsync(
                                        request: request,
                                        lastResponse: null,
                                        lastException: communicationException,
                                        cancellationToken: cancellationToken);
                    
                                    if (nextEndpoint == null)
                                    {
                                        // Will not be retried anymore
                                        CommunicationTraceSource.Instance.TraceVerbose("Rethrowing exception because the router did not return another endpoint");
                                        throw;
                                    }
                    
                                    // Will be retried with the new endpoint
                                    CommunicationTraceSource.Instance.TraceInformation($"Router returned the next endpoint to try: '{nextEndpoint}'");
                                    return null;
                                }
                                catch when (currentResponse != null)
                                {
                                    // If there's an exception being thrown while we're still holding onto 
                                    // a response then make sure to dispose it
                                    CommunicationTraceSource.Instance.TraceVerbose("Disposing the current response object");
                                    currentResponse.Dispose();
                                    currentResponse = null;
                    
                                    throw;
                                }
                            });
                    }
                    else
                    {
                        responseToReturn = await this.logger.ExecuteAsync(
                            SendRoutedRequestActivityType.Instance,
                            async () =>
                            {
                                // Before making the request we need to combine the router base URL and the request relative URL
                                request.RequestUri = CreateUri(nextEndpoint, relativeRequestUri);
                                HttpResponseMessage currentResponse = null;
                    
                                try
                                {
                                    // Not using Task.ContinueWith since when SendAsync throw exception we will receive 
                                    // aggregation exception in the continuation task which is not expected
                                    CommunicationTraceSource.Instance.TraceInformation(
                                        "Executing HTTP request: RequestUri={0}, TargetHostUri={1}, RelativeRequestUri={2}, Attempt={3}",
                                        request.RequestUri,
                                        nextEndpoint,
                                        relativeRequestUri,
                                        attemptCount);
                    
                                    var stopwatch = Stopwatch.StartNew();
                                    currentResponse = await base.SendAsync(request, cancellationToken);
                                    ServiceContext.Activity.Current.AddHttpStatusCode((int)currentResponse.StatusCode);
                                    logger?.LogMetric(DefaultMetricConstants.MetricNamespace, DefaultMetricConstants.OutgoingHttpOperationDurationMetricName, new SortedList<string, string> { { DefaultMetricConstants.HttpStatusCodeDimension, currentResponse.StatusCode.ToString() } }, stopwatch.ElapsedMilliseconds);
                    
                                    // If successful response, just return it
                                    if (currentResponse.IsSuccessStatusCode)
                                        return currentResponse;
                    
                                    // Otherwise ask the router on non-success response whether we should retry again
                                    // To make this decision, the router is given the endpoint used for the last attempt and 
                                    // the response we received from the server.
                                    // 
                                    // Note that routers are free to use whatever logic necessary to obtain endpoint (including internal retries, etc.)
                                    CommunicationTraceSource.Instance.TraceInformation($"Requesting router endpoint for non-successful response. StatusCode={currentResponse.StatusCode}");
                                    request.RequestUri = relativeRequestUri;
                                    nextEndpoint = await routerRequest.GetNextEndpointAsync(
                                        request: request,
                                        lastResponse: currentResponse,
                                        lastException: null,
                                        cancellationToken: cancellationToken);
                    
                                    if (nextEndpoint == null)
                                    {
                                        CommunicationTraceSource.Instance.TraceInformation("Received response will be returned because router did not return another endpoint");
                                        return currentResponse;
                                    }
                    
                                    // Will be retried with the new endpoint, release the response
                                    CommunicationTraceSource.Instance.TraceInformation($"Router returned the next endpoint to try: '{nextEndpoint}'");
                    
                                    currentResponse.Dispose();
                                    currentResponse = null;
                                    return null;
                                }
                                catch (Exception ex) when (!ex.IsFatal() && currentResponse == null)
                                {
                                    // Only capture exceptions out of SendAsync here, i.e. (lastResponse == null) to avoid invoking router's 
                                    // GetNextEndpointAsync for exceptions thrown out of that method. We also only capture HTTP communication
                                    // exceptions that get thrown by the base handler in place of HttpRequestException
                                    //
                                    CommunicationTraceSource.Instance.TraceError("An exception encountered while sending the request: {0}", ex.ToString());
                    
                                    // Only attempt a router retry on HTTP communication exceptions
                                    //
                                    HttpCommunicationException communicationException;
                                    if (!HttpCommunicationException.TryCreate(ex, out communicationException))
                                    {
                                        throw;
                                    }
                    
                                    // Ask the router on exception from the http message handler stack whether we should retry again.
                                    // To make this decision, the router is given the exception that was thrown. Note that routers are 
                                    // free to use whatever logic necessary to obtain endpoint (including internal retries, etc.)
                                    // 
                                    // Note that HttpClient stack only throws an HttpRequestException in case the request cannot be completed. 
                                    // That is for example when the endpoint is unreachable, the connection gets terminated in the middle of the 
                                    // request, the content length is incorrect etc. It does NOT throw any exceptions when the request finishes 
                                    // with a non-successful HTTP response from the server like for example HTTP 500 Internal Server Error. This
                                    // situation is already handled above.
                                    //
                                    request.RequestUri = relativeRequestUri;
                                    nextEndpoint = await routerRequest.GetNextEndpointAsync(
                                        request: request,
                                        lastResponse: null,
                                        lastException: communicationException,
                                        cancellationToken: cancellationToken);
                    
                                    if (nextEndpoint == null)
                                    {
                                        // Will not be retried anymore
                                        CommunicationTraceSource.Instance.TraceVerbose("Rethrowing exception because the router did not return another endpoint");
                                        throw;
                                    }
                    
                                    // Will be retried with the new endpoint
                                    CommunicationTraceSource.Instance.TraceInformation($"Router returned the next endpoint to try: '{nextEndpoint}'");
                                    return null;
                                }
                                catch when (currentResponse != null)
                                {
                                    // If there's an exception being thrown while we're still holding onto 
                                    // a response then make sure to dispose it
                                    CommunicationTraceSource.Instance.TraceVerbose("Disposing the current response object");
                                    currentResponse.Dispose();
                                    currentResponse = null;
                    
                                    throw;
                                }
                            });
                    }

                    if (responseToReturn != null)
                        return responseToReturn;
                }
            }
            catch when (responseToReturn != null)
            {
                CommunicationTraceSource.Instance.TraceVerbose("Disposing the returned response object");
                responseToReturn.Dispose();
                responseToReturn = null;

                throw;
            }
            finally
            {
                if (routerRequest != null)
                {
                    CommunicationTraceSource.Instance.TraceVerbose("Disposing the router request");
                    routerRequest.Dispose();
                    routerRequest = null;
                }
            }

            // This should not really happen in practice -- routers should stop retrying before RetryAttemptsLimit is reached
            CommunicationTraceSource.Instance.TraceError("Exceeded maximum retry attempts");
            throw new RoutingHandlerRetryLimitReachedException(router);
        }

        /// <summary>
        /// Stateful Service Fabric services have endpoints resolved by the Naming Service looking like the following:
        /// http://host:port/partition/replica/guid.
        /// 
        /// Creating a new Uri using this with a relative Uri removes the last segment of the request (in this case "guid").
        /// As such, we need to build up a Uri ourselves, that can handle simple stateless services in addition to the 
        /// stateful services.
        /// 
        /// Secondly, kestrel returns a 404 if you have a // backslash in the uri, such as http://host:port/segment1//segment2.
        /// Thus, we need to ensure that if Service Fabric resolves the target URI with a backslash, and the relative URI is
        /// provided with a backslash, that the resulting URI only contains a single backslash.
        /// </summary>
        private Uri CreateUri(Uri targetUri, Uri relativeUri)
        {
            var targetUriString = targetUri.ToString();
            var stringBuilder = new StringBuilder(targetUriString);
            if (targetUriString.EndsWith("/"))
            {
                stringBuilder.Length--;
            }

            var relativeUriString = relativeUri.ToString();
            if (!relativeUriString.StartsWith("/"))
            {
                stringBuilder.Append("/");
            }

            stringBuilder.Append(relativeUriString);
            return new Uri(stringBuilder.ToString());
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            // Nothing to actually dispose, just update the state and call parent
            disposed = true;

            base.Dispose(disposing);
        }

        private sealed class RoutingHandlerActivityType : SingletonActivityType<RoutingHandlerActivityType>
        {
            public RoutingHandlerActivityType()
                : base("SP.Communication.RoutingHandler", ActivityKind.ClientProxy)
            {
            }
        }

        private sealed class SendRoutedRequestActivityType : SingletonActivityType<SendRoutedRequestActivityType>
        {
            public SendRoutedRequestActivityType()
                : base("SP.Communication.RoutingHandler.SendRequest", ActivityKind.ClientProxy)
            {
            }
        }
    }
}
