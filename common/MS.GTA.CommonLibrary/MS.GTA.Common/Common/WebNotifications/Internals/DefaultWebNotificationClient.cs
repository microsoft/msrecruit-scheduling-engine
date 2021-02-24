//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.WebNotifications.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Common.WebNotifications.Configurations;
    using MS.GTA.Common.WebNotifications.Interfaces;
    using MS.GTA.Common.WebNotifications.Models;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.ServicePlatform.Configuration;
    using Newtonsoft.Json;

    /// <summary>
    /// The <see cref="DefaultWebNotificationClient"/> class communicates with web notifications service.
    /// </summary>
    /// <seealso cref="IWebNotificationClient" />
    internal class DefaultWebNotificationClient : IWebNotificationClient
    {
        /// <summary>
        /// The maximum container size constant.
        /// </summary>
        private const int MaxContainerSize = 10;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<DefaultWebNotificationClient> logger;

        /// <summary>
        /// The HTTP client
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// The web notification service configuration.
        /// </summary>
        private readonly WebNotificationServiceConfiguration webNotificationServiceConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWebNotificationClient"/> class.
        /// </summary>
        /// <param name="httpClient">The instance of <see cref="HttpClient"/>.</param>
        /// <param name="configurationManager">The instance of <see cref="IConfigurationManager"/>.</param>
        /// <param name="logger">The intance for <see cref="ILogger{DefaultWebNotificationClient}"/>.</param>
        /// <exception cref="ArgumentNullException">logger</exception>
        public DefaultWebNotificationClient(HttpClient httpClient, IConfigurationManager configurationManager, ILogger<DefaultWebNotificationClient> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.webNotificationServiceConfiguration = configurationManager?.Get<WebNotificationServiceConfiguration>() ?? throw new ArgumentNullException(nameof(configurationManager));
        }

        /// <summary>
        /// Posts the notifications to the service.
        /// </summary>
        /// <param name="webNotificationRequests">The instance for <see cref="IEnumerable{WebNotificationRequest}"/>.</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        public async Task PostNotificationsAsync(IEnumerable<WebNotificationRequest> webNotificationRequests)
        {
            this.logger.LogInformation($"Started {nameof(this.PostNotificationsAsync)} method in {nameof(DefaultWebNotificationClient)}");
            Contract.CheckValue(webNotificationRequests, nameof(webNotificationRequests));
            await this.PostNotificationsInternalAsync(webNotificationRequests).ConfigureAwait(false);
            this.logger.LogInformation($"Finished {nameof(this.PostNotificationsAsync)} method in {nameof(DefaultWebNotificationClient)}");
        }

        /// <summary>
        /// Posts the web notifications to the service in batches.
        /// </summary>
        /// <param name="webNotificationRequests">The instance for <see cref="IEnumerable{WebNotificationRequest}"/>.</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        private async Task PostNotificationsInternalAsync(IEnumerable<WebNotificationRequest> webNotificationRequests)
        {
            Debug.Assert(webNotificationRequests?.Any() ?? false, "Missing web notifications.");
            WebNotificationRequestContainer webNotificationRequestContainer = new WebNotificationRequestContainer();
            int currentConainerSize = webNotificationRequestContainer.Notifications.Count;
            foreach (WebNotificationRequest webNotificationRequest in webNotificationRequests)
            {
                if (webNotificationRequest != null)
                {
                    webNotificationRequestContainer.Notifications.Add(webNotificationRequest);
                    currentConainerSize++;
                    if (currentConainerSize >= MaxContainerSize)
                    {
                        await this.PostNotificationContainerInternalAsync(webNotificationRequestContainer).ConfigureAwait(false);
                        webNotificationRequestContainer = new WebNotificationRequestContainer();
                    }
                }
            }

            if (webNotificationRequestContainer.Notifications.Any())
            {
                await this.PostNotificationContainerInternalAsync(webNotificationRequestContainer).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Posts the web notification container internally asynchronously.
        /// </summary>
        /// <param name="webNotificationRequestContainer">The instance of <see cref="WebNotificationRequestContainer"/>.</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        private async Task PostNotificationContainerInternalAsync(WebNotificationRequestContainer webNotificationRequestContainer)
        {
            string errorResponse = string.Empty;
            string serializedContainer = JsonConvert.SerializeObject(webNotificationRequestContainer);
            StringContent requestBody = new StringContent(serializedContainer, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await this.httpClient.PostAsync(this.webNotificationServiceConfiguration.PostWebNotificationsRelativeUrl, requestBody).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
            {
                if (responseMessage.Content != null)
                {
                    errorResponse = await responseMessage.Content?.ReadAsStringAsync();
                }

                // Do not throw exception as the event notification failure does not translate as invoking API endpoint failure.
                this.logger.LogError($"Failure response on posting web notifications container to '{this.webNotificationServiceConfiguration.PostWebNotificationsRelativeUrl}'. Error Response : {errorResponse} Status Code: {responseMessage.StatusCode}");
            }
        }
    }
}
