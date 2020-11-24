// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ExternalApiHandler.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.ServicePlatform.Communication.Http.Handlers
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using CommonDataService.Common.Internal;
    using Context;
    using Exceptions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>The external API handler.</summary>
    public class ExternalApiHandler : DelegatingHandler
    {
        /// <summary>The logger.</summary>
        private readonly ILogger<ExternalApiHandler> logger;

        private readonly ILoggerFactory loggerFactory;

        /// <summary>Initializes a new instance of the <see cref="ExternalApiHandler"/> class.</summary>
        /// <param name="loggerFactory">The instnce for <see cref="ILoggerFactory"/>.</param>
        public ExternalApiHandler(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.logger = loggerFactory.CreateLogger<ExternalApiHandler>();
        }

        /// <summary>The send async.</summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Contract.CheckValue(request, nameof(request));

            return await this.logger.ExecuteAsync(
                "ExternalApiHandler",
                async () =>
                {
                    // Note: This could be somewhat dangerous if we use this outside of graph?
                    // We want to reset requests to be relative if we are using a router. We are assuming here that the base url is setup correctly.
                    if (request?.RequestUri != null && request.RequestUri.IsAbsoluteUri)
                    {
                        var routingHandler = this.GetRoutingHandler();
                        if (routingHandler != null)
                        {
                            request.RequestUri = new Uri(request.RequestUri.PathAndQuery, UriKind.Relative);
                        }
                    }

                    this.logger.LogInformation($"Sending {request?.Method} request to {request?.RequestUri}");

                    try
                    {
                        var response = await base.SendAsync(request, cancellationToken);

                        this.logger.LogInformation($"Finished sending {request?.Method} request to {request?.RequestUri} response code was {response?.StatusCode}");

                        if (!response.IsSuccessStatusCode)
                        {
                            var content = response.Content != null ? await response.Content.ReadAsStringAsync() : null;
                            var message = $"Received nonsuccess response '{response.StatusCode}' during request, response was:\n '{content}'\n. Headers were:\n {JsonConvert.SerializeObject(response.Headers)}";
                            this.logger.LogError(message);

                            throw await NonSuccessHttpResponseException.CreateAsync(response);
                        }

                        return response;
                    }
                    catch (TaskCanceledException e)
                    {
                        throw new ExternalApiTaskCancelledException($"{e}");
                    }
                }, new [] { typeof(NonSuccessHttpResponseException) });
        }

        /// <summary>The get routing handler. And determine if it's using a router.</summary>
        /// <returns>The <see cref="HttpClientRoutingHandler"/>.</returns>
        private HttpClientRoutingHandler GetRoutingHandler()
        {
            var nextHandler = this.InnerHandler;

            while (nextHandler != null)
            {
                if (nextHandler is HttpClientRoutingHandler handler)
                {
                    return handler;
                }

                if (nextHandler is DelegatingHandler nextHandlerCasted)
                {
                    nextHandler = nextHandlerCasted.InnerHandler;
                    continue;
                }

                break;
            }

            return null;
        }
    }
}
