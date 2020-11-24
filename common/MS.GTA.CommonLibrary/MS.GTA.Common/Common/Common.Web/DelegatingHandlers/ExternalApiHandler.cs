// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ExternalApiHandler.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using MS.GTA.CommonDataService.Common;
using MS.GTA.Common.Web.Exceptions;
using MS.GTA.ServicePlatform.Communication.Http.Handlers;
using MS.GTA.ServicePlatform.Context;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace MS.GTA.Common.Web.DelegatingHandlers
{
    /// <summary>The external API handler.</summary>
    public class ExternalApiHandler : DelegatingHandler
    {
        /// <summary>The logger.</summary>
        private readonly ILogger<ExternalApiHandler> logger;

        /// <summary>Initializes a new instance of the <see cref="ExternalApiHandler"/> class.</summary>
        /// <param name="logger">The logger.</param>
        public ExternalApiHandler(ILogger<ExternalApiHandler> logger)
        {
            this.logger = logger;
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

                    this.logger.LogInformation($"Sending {request.Method} request to {request.RequestUri}");

                    try
                    {
                        var response = await base.SendAsync(request, cancellationToken);

                        this.logger.LogInformation($"Finished sending {request.Method} request to {request.RequestUri} response code was {response.StatusCode}");

                        if (!response.IsSuccessStatusCode)
                        {
                            var content = response?.Content != null ? await response.Content.ReadAsStringAsync() : null;
                            var message = $"Received nonsuccess response during request, response was:\n '{content}'\n. Headers were:\n {JsonConvert.SerializeObject(response.Headers)}";
                            this.logger.LogError(message);

                            throw new ExternalApiException(content, response.Headers, response.StatusCode);
                        }

                        return response;
                    }
                    catch (TaskCanceledException e)
                    {
                        throw new ExternalApiTaskCancelledException($"{e}");
                    }
                });
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
