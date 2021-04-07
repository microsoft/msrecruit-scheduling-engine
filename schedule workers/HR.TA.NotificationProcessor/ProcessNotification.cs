//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using  MS.GTA.NotificationProcessor.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace MS.GTA.NotificationProcessor
{
    public static class ProcessNotification
    {
        private static readonly int MaxRetries = 10;
        private static readonly int MaxRandomAdditionalDelaySeconds = 5;
        private static readonly long lockRecordMaxTimeInSeconds = Convert.ToInt64(Environment.GetEnvironmentVariable("LockRecordMaxTimeInSeconds"));

        [FunctionName("ProcessNotification")]
        public static async Task RunAsync([ServiceBusTrigger("graphnotification", Connection = "ServiceBusEndpoint")]string message, string MessageId,IDictionary<string, object> UserProperties, ILogger log)
        {
            try
            {
                string rootActivityId = UserProperties.ContainsKey("ScheduleRootActivityId") ? UserProperties["ScheduleRootActivityId"]?.ToString() : Guid.NewGuid().ToString();
                log.LogInformation($"ProcessNotification: Recieved request to process notification, MessageId: {MessageId}, rootActivityId: {rootActivityId}");

                if (string.IsNullOrWhiteSpace(message))
                {
                    log.LogError("ProcessNotification: Service Bus received an empty message.");
                    return;
                }

                var messageContent = JsonConvert.DeserializeObject<NotificationContent>(message);
                if (messageContent == null)
                {
                    log.LogError($"ProcessNotification: Couldn't deserialized service bus message received {message}, messageId {MessageId}, rootActivityId: {rootActivityId}");
                    return;
                }

                log.LogInformation($"ProcessNotification: ServiceBus queue trigger function processing message for messageId {MessageId}, rootActivityId: {rootActivityId}");
                var isRecordLocked = await AcquireLock(MessageId, log, rootActivityId);
                if (!isRecordLocked)
                {
                    log.LogInformation($"ProcessNotification: Record is already locked by another process for messageId {MessageId}, rootActivityId: {rootActivityId}");
                    return;
                }

                string schedulingServiceEnpoint = $"{Environment.GetEnvironmentVariable("SchedulingServiceEndpoint")}";
                Uri uri = new Uri($"{schedulingServiceEnpoint}v1/schedulemeeting/processNotification");

                log.LogInformation($"ProcessNotification: Request URI : {uri.AbsoluteUri}, messageId {MessageId}, rootActivityId: {rootActivityId}");

                string bearerToken = await HttpAuthenticationAsync(Environment.GetEnvironmentVariable("Authority"), Environment.GetEnvironmentVariable("ClientId"));
                var serviceResponse = await SendRequestWithRetry(HttpMethod.Post, uri, messageContent, bearerToken, rootActivityId, log);

                if (!serviceResponse.IsSuccessStatusCode)
                {
                    log.LogError($"ProcessNotification: Scheduling Service response code: {serviceResponse.StatusCode}, error Message: {JObject.Parse(serviceResponse.Content?.ReadAsStringAsync().Result)}, for message: {message}, messageId {MessageId}, rootActivityId: {rootActivityId}");
                }

                log.LogInformation($"ProcessNotification: Successfully completed request to process notification for message: {message}, messageId {MessageId}, rootActivityId: {rootActivityId}");

                await ReleaseLock(MessageId, log, rootActivityId);
            }
            catch (Exception ex)
            {
                log.LogError($"ProcessNotification: An error occurred while executing notification processor, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
        }

        public static async Task<HttpResponseMessage> SendRequestWithRetry(HttpMethod method, Uri uri, Object obj, string bearerToken, string rootActivityId, ILogger log)
        {
            log.LogInformation($"ProcessNotification.SendRequestWithRetry: recieved request to send http request to schedulingservice with rootActivityId: {rootActivityId}");

            var attempt = 0;
            var random = new Random();
            using (var httpClient = new HttpClient(new HttpClientHandler()))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                while (true)
                {
                    log.LogInformation($"ProcessNotification.SendRequestWithRetry: Sending http request to schedulingservice with rootActivityId: {rootActivityId}");

                    var request = new HttpRequestMessage(method, uri)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"),
                    };
                    request.Headers.Add("x-ms-root-activity-id", rootActivityId);

                    var response = await httpClient.SendAsync(request, CancellationToken.None);
                    request.Dispose();

                    // Retry on 429 "Too Many Requests" with a RetryAfter header.
                    if ((int)response.StatusCode == 429
                        && response.Headers.RetryAfter.Delta.HasValue
                        && ++attempt <= MaxRetries)
                    {
                        var delay = response.Headers.RetryAfter.Delta.Value;
                        // Exponential backoff to prevent contention, and some random offset to avoid thundering herd behavior.
                        delay += TimeSpan.FromSeconds((attempt * attempt) + (MaxRandomAdditionalDelaySeconds * random.NextDouble()));
                        log.LogInformation($"ProcessNotification.SendRequestWithRetry: Got status {response.StatusCode} ({response.ReasonPhrase}) on attempt {attempt}, retrying after {delay} (retry after: {response.Headers.RetryAfter.Delta.Value}), rootActivityId: {rootActivityId}");
                        // Clean up the non-returned response object.
                        response.Dispose();
                        // Delay before retrying.
                        await Task.Delay(delay);
                        continue;
                    }
                    log.LogInformation($"ProcessNotification.SendRequestWithRetry: Returning response with status {response.StatusCode} ({response.ReasonPhrase}) for {uri} on attempt {attempt}, rootActivityId: {rootActivityId}");
                    return response;
                }
            }
        }
        private static async Task<string> HttpAuthenticationAsync(string authority, string clientId)
        {
            var authContext = new AuthenticationContext(authority);
            var authResult = await authContext
             .AcquireTokenAsync(clientId, new ClientCredential(clientId, Environment.GetEnvironmentVariable("ClientSecret")));
            return authResult.AccessToken;
        }
        private static async Task<bool> AcquireLock(string messageId, ILogger log, string rootActivityId)
        {
            log.LogInformation($"ProcessNotification.AcquireLock: Received request to acquire lock for messageId: {messageId}, rootActivityId: {rootActivityId}");

            string schedulingServiceEnpoint = $"{Environment.GetEnvironmentVariable("SchedulingServiceEndpoint")}";

            Uri getlocksuri = new Uri($"{schedulingServiceEnpoint}v1/lock/getnotificationlock/{messageId}");
            string bearerToken = await HttpAuthenticationAsync(Environment.GetEnvironmentVariable("Authority"), Environment.GetEnvironmentVariable("ClientId"));

            var serviceResponse = await SendRequestWithRetry(HttpMethod.Get, getlocksuri, null, bearerToken, rootActivityId, log);
            
            NotificationMessageLock lockedRecord = null;

            if (serviceResponse.IsSuccessStatusCode)
            {
                var content = await serviceResponse.Content.ReadAsStringAsync();
                lockedRecord = JsonConvert.DeserializeObject<NotificationMessageLock>(content);
            }

            if (lockedRecord != null && DateTime.UtcNow.AddSeconds(-lockRecordMaxTimeInSeconds) > lockedRecord?.LockedTime)
            {
                log.LogInformation($"ProcessNotification.AcquireLock: The Message has been locked for more than the allotted time. messageId: {messageId}, rootActivityId: {rootActivityId}");
                await ReleaseLock(messageId, log, rootActivityId);
            }
            else if (lockedRecord != null)
            {
                log.LogError($"ProcessNotification.AcquireLock: The Message is already locked. Rejecting request to acquire lock. messageId: {messageId}, rootActivityId: {rootActivityId}");
                return false;
            }

            try
            {
                var notificationMessageLock = new NotificationMessageLock()
                {
                    ServiceBusMessageId = messageId,
                    LockedTime = DateTime.UtcNow,
                };

                Uri createLockuri = new Uri($"{schedulingServiceEnpoint}v1/lock/createnotificationlock");
                var createResponse = await SendRequestWithRetry(HttpMethod.Post, createLockuri, notificationMessageLock, bearerToken, rootActivityId, log);
            }
            catch (Exception ex)
            {
                log.LogError($"ProcessNotification.AcquireLock: Message locking request failed for messageId: {messageId}, rootActivityId: {rootActivityId}, Error Message : {ex.Message}, InnerException :{ex.InnerException}, StackTrace: {ex.StackTrace}");
            }

            log.LogInformation($"ProcessNotification.AcquireLock: Successfully acquired lock for messageId: {messageId}, rootActivityId: {rootActivityId}");
            return true;
        }

        private static async Task ReleaseLock(string messageId, ILogger log, string rootActivityId)
        {
            try
            {
                log.LogInformation($"ProcessNotification.ReleaseLock: Recieved request to release lock for messageId: {messageId}, rootActivityId: {rootActivityId}");

                string schedulingServiceEnpoint = $"{Environment.GetEnvironmentVariable("SchedulingServiceEndpoint")}";
                string bearerToken = await HttpAuthenticationAsync(Environment.GetEnvironmentVariable("Authority"), Environment.GetEnvironmentVariable("ClientId"));

                Uri deleteLockuri = new Uri($"{schedulingServiceEnpoint}v1/lock/deletenotificationlock/{messageId}");
                await SendRequestWithRetry(HttpMethod.Delete, deleteLockuri, null, bearerToken, rootActivityId, log);
            }
            catch (Exception ex)
            {
                log.LogError($"ProcessNotification.ReleaseLock: Message lock release request failed for messageId: {messageId}, rootActivityId: {rootActivityId}, Error Message : {ex.Message}, InnerException :{ex.InnerException}, StackTrace: {ex.StackTrace}");
            }

            log.LogInformation($"ProcessNotification.ReleaseLock: Successfully released lock for messageId: {messageId}, rootActivityId: {rootActivityId}");
        }
    }
}