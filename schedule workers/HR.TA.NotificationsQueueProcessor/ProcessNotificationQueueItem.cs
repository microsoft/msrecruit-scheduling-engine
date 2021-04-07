//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.NotificationsQueueProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Newtonsoft.Json;

    /// <summary>
    /// Type of the notification supported by system.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Mail
        /// </summary>
        Mail,

        /// <summary>
        /// Meet
        /// </summary>
        Meet,
    }

    /// <summary>
    /// Function to process notification queue items.
    /// </summary>
    public static class ProcessNotificationQueueItem
    {
        /// <summary>
        /// Trigger method invoked when a notification item is added to the queue.
        /// </summary>
        /// <param name="notifQueueItem">Serialized queue item.</param>
        /// <param name="log">An instance of logger.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns>
        [FunctionName("ProcessNotificationQueueItem")]
        public static async Task Run([QueueTrigger("notifications-queue", Connection = "AzureWebJobsStorage")]string notifQueueItem, ILogger log)
        {
            log.LogInformation($"ProcessNotificationQueueItem started processing: {notifQueueItem}");

            if (string.IsNullOrEmpty(notifQueueItem))
            {
                throw new ArgumentException("message", nameof(notifQueueItem));
            }

            QueueNotificationItem queueNotificationItem = JsonConvert.DeserializeObject<QueueNotificationItem>(notifQueueItem);

            if (queueNotificationItem != null)
            {
                var notifType = queueNotificationItem.NotificationType == NotificationType.Mail ? Constants.EmailNotificationType : Constants.MeetingNotificationType;

                var stringContent = new StringContent(JsonConvert.SerializeObject(queueNotificationItem.NotificationIds), Encoding.UTF8, Constants.JsonMIMEType);
                string notificationServiceEndpoint = $"{Environment.GetEnvironmentVariable(Constants.EnvVariableNotificationServiceEndpoint)}";
                log.LogInformation($"ProcessNotificationQueueItem fetching token to call notification service endpoint...");
                string bearerToken = await HttpAuthenticationAsync(Environment.GetEnvironmentVariable(Constants.EnvVariableAuthority), Environment.GetEnvironmentVariable(Constants.EnvVariableClientId));
                if (bearerToken != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                        log.LogInformation($"ProcessNotificationQueueItem calling notification service endpoint...");
                        var response = await client.PostAsync($"{notificationServiceEndpoint}v1/{notifType}/process/{queueNotificationItem.Application}", stringContent);
                        log.LogInformation($"ProcessNotificationQueueItem received response from notification service endpoint.");
                        if (!response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            log.LogError($"An error occurred while processing {notifQueueItem} in ProcessNotificationQueueItem. Details: {content}.");
                        }
                    }
                }
                else
                {
                    log.LogWarning($"Unable to generate token for {Environment.GetEnvironmentVariable(Constants.EnvVariableClientId)} in ProcessNotificationQueueItem. Skipping the processing of the notification ids : {queueNotificationItem.NotificationIds}");
                }
            }
            else
            {
                log.LogWarning("Invalid queue item received by the processor.");
            }

            log.LogInformation($"ProcessNotificationQueueItem finished processing: {notifQueueItem}");
        }

        private static async Task<string> HttpAuthenticationAsync(string authority, string clientId)
        {
            var authContext = new AuthenticationContext(authority);
            var authResult = await authContext.AcquireTokenAsync(clientId, new ClientCredential(clientId, Environment.GetEnvironmentVariable(Constants.EnvVariableClientSecret)));
            return authResult.AccessToken;
        }
    }
}