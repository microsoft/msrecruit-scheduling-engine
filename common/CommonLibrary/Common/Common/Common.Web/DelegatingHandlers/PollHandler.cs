//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Web.DelegatingHandlers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using ServicePlatform.Context;
    using ServicePlatform.Exceptions;
    using Microsoft.Extensions.Logging;

    /// <summary>The poll handler.</summary>
    public class PollHandler : DelegatingHandler
    {
        /// <summary>The logger.</summary>
        private readonly ILogger<PollHandler> logger;

        /// <summary>The get token function.</summary>
        private readonly Func<Task<string>> getTokenFunction;

        /// <summary>Initializes a new instance of the <see cref="PollHandler"/> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="getTokenFunction">The get token function.</param>
        public PollHandler(ILogger<PollHandler> logger, Func<Task<string>> getTokenFunction)
        {
            this.getTokenFunction = getTokenFunction;
            this.logger = logger;
        }

        /// <summary>The send async.</summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await this.logger.ExecuteAsync(
                "PollHandler",
                async () =>
                {
                    try
                    {
                        var response = await base.SendAsync(request, cancellationToken);

                        this.logger.LogInformation($"Polling for OK status.");

                        if (response.StatusCode == HttpStatusCode.OK || string.IsNullOrEmpty(response?.Headers?.Location?.ToString()))
                        {
                            this.logger.LogInformation($"Already recieved OK response or no poll location from service url {response?.RequestMessage?.RequestUri}");

                            return response;
                        }

                        var delay = TimeSpan.FromSeconds(1);

                        // Note: On a pending request the cancellation token will be hit long before this.
                        var timeout = DateTime.UtcNow + TimeSpan.FromMinutes(10);

                        while (DateTime.UtcNow < timeout)
                        {
                            var newLocation = response.Headers.Location;
                            var newRequest = new HttpRequestMessage(HttpMethod.Get, newLocation);

                            // We need to dispose of the previous response otherwise things hang.
                            response.Dispose();
                            response = null;

                            this.logger.LogInformation($"Sending poll request to {newLocation}");

                            var token = await this.getTokenFunction();
                            newRequest.Headers.Authorization = new AuthenticationHeaderValue(Common.Base.Constants.BearerAuthenticationScheme, token);

                            response = await base.SendAsync(newRequest, cancellationToken);

                            if (response.StatusCode == HttpStatusCode.OK || string.IsNullOrEmpty(response?.Headers?.Location?.ToString()))
                            {
                                this.logger.LogInformation($"Recieved ok response or no location from url {response?.RequestMessage?.RequestUri}");

                                return response;
                            }

                            this.logger.LogInformation($"Waiting {delay} before sending next poll request.");

                            await Task.Delay(delay);
                        }

                        this.logger.LogWarning($"Timeout reached for polling.");

                        return response;
                    }
                    catch (Exception e)
                    {
                        this.logger.LogError($"Encountered error while polling: {e}");

                        throw new PollHandlerException(e);
                    }
                });
        }
    }

    /// <summary>
    /// The poll handler exception. Used to wrap the poll handler code so we don't throw log an exception except one level up.
    /// </summary>
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "PollHandler", "Error", MonitoredExceptionKind.Service)]
    public class PollHandlerException : MonitoredException
    {
        /// <summary>Initializes a new instance of the <see cref="PollHandlerException"/> class.</summary>
        /// <param name="innerException">The inner exception.</param>
        public PollHandlerException(Exception innerException) : base("An error occured while polling", innerException)
        {
        }
    }
}
