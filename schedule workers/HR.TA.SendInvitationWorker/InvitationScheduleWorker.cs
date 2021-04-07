//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleWorker
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Core;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class SendInvitation
    {
        private static int MaxRetries = 10;
        private static int MaxRandomAdditionalDelaySeconds = 5;
        private static long lockRecordMaxTimeInSeconds = Convert.ToInt64(Environment.GetEnvironmentVariable("LockRecordMaxTimeInSeconds"));
        private static readonly string serviceBusEndpoint = Environment.GetEnvironmentVariable("ServiceBusEndpoint");
        private static readonly string serviceAccountName = Environment.GetEnvironmentVariable("ServiceAccountName");
        private static readonly string serviceBusQueue = Environment.GetEnvironmentVariable("ServiceBusQueue");

        [FunctionName("SendInvitation")]
        public static async Task Run([TimerTrigger("%TimerSchedule%", RunOnStartup = true)]TimerInfo myTimer, ILogger log)
        {
            var scheduleIdLockTokenMap = new Dictionary<string, string>();
            log.LogInformation($"SendInvitation: Fetching messages from {serviceBusQueue} queue.");
            IMessageReceiver messageReceiver = new MessageReceiver(serviceBusEndpoint, serviceBusQueue, ReceiveMode.PeekLock);

            try
            {
                Message sbqMessage = new Message();
                IList<Task> updateMessageStatusTasks = new List<Task>();
                sbqMessage = await messageReceiver.ReceiveAsync();

                if (sbqMessage == null)
                {
                    log.LogInformation($"SendInvitation: Couldn't find any messages in {serviceBusQueue} queue.");
                    return;
                }

                log.LogInformation($"SendInvitation: Fetched message from {serviceBusQueue} queue.");
                IList<string> scheduleIds = new List<string>();
                string rootActivityId = Guid.NewGuid().ToString();
                Dictionary<string, ConnectorIntegrationDetails> connectorIntegrationDetails = new Dictionary<string, ConnectorIntegrationDetails>();

                string content = Encoding.UTF8.GetString(sbqMessage.Body);
                ConnectorIntegrationDetails messageDetails = JsonConvert.DeserializeObject<ConnectorIntegrationDetails>(content);

                connectorIntegrationDetails.Add(sbqMessage.MessageId, messageDetails);

                if (messageDetails == null)
                {
                    log.LogError($"SendInvitation: Couldn't deserialized recieved message with messageId: {sbqMessage.MessageId}, rootActivityId: {rootActivityId}");
                    updateMessageStatusTasks.Add(messageReceiver.DeadLetterAsync(sbqMessage.SystemProperties.LockToken, "Failed to deserialzed message"));
                }

                if (string.IsNullOrEmpty(messageDetails.ScheduleID))
                {
                    log.LogError($"SendInvitation: Recieved message with null schedule id, messgaeId: {sbqMessage.MessageId}, rootActivityId: {rootActivityId}.");
                    updateMessageStatusTasks.Add(messageReceiver.CompleteAsync(sbqMessage.SystemProperties.LockToken));
                }

                if (scheduleIdLockTokenMap.ContainsKey(messageDetails.ScheduleID))
                {
                    log.LogInformation($"SendInvitation: Recieved duplicate request to process message with scheduleId: {messageDetails.ScheduleID}, actionType: {messageDetails.ScheduleAction}, rootActivityId: {rootActivityId}.");
                    await messageReceiver.AbandonAsync(sbqMessage?.SystemProperties.LockToken);
                }

                log.LogInformation($"SendInvitation: Recieved request to process message with scheduleId: {messageDetails.ScheduleID}, actionType: {messageDetails.ScheduleAction}, rootActivityId: {rootActivityId}.");
                scheduleIdLockTokenMap.Add(messageDetails.ScheduleID, sbqMessage.SystemProperties.LockToken);

                scheduleIds.Add(messageDetails.ScheduleID);

                var acquiredLockScheduleIds = await AcquireLocks(scheduleIds, rootActivityId, log);

                var failedScheduleIds = scheduleIds?.Where(a => !acquiredLockScheduleIds.Contains(a));
                foreach (var scheduleId in failedScheduleIds)
                {
                    if (scheduleIdLockTokenMap.ContainsKey(scheduleId))
                    {
                        updateMessageStatusTasks.Add(messageReceiver.AbandonAsync(scheduleIdLockTokenMap[scheduleId]));
                    }
                }

                if (!acquiredLockScheduleIds?.Any() ?? true)
                {
                    log.LogInformation($"SendInvitation: Couldn't Acquire valid locks for any message, rootActivityId: {rootActivityId}.");
                    await Task.WhenAll(updateMessageStatusTasks);
                    return;
                }

                string schedulingServiceEnpoint = $"{Environment.GetEnvironmentVariable("SchedulingServiceEndpoint")}";

                Uri uri = new Uri($"{schedulingServiceEnpoint}v1/schedulemeeting/sendcalendarinvite?serviceAccountName={serviceAccountName}");
                string bearerToken = await HttpAuthenticationAsync(Environment.GetEnvironmentVariable("Authority"), Environment.GetEnvironmentVariable("ClientId"));

                var serviceResponse = await SendRequestWithRetry(HttpMethod.Post, uri, acquiredLockScheduleIds, bearerToken, rootActivityId, log);

                var processedScheduleIds = new List<string>();
                if (serviceResponse.IsSuccessStatusCode)
                {
                    var serviceResponseContent = await serviceResponse.Content.ReadAsStringAsync();
                    processedScheduleIds = JsonConvert.DeserializeObject<List<string>>(serviceResponseContent);
                    foreach (var scheduleId in processedScheduleIds)
                    {
                        if (scheduleIdLockTokenMap.ContainsKey(scheduleId))
                        {
                            log.LogInformation($"SendInvitation: Successfully processed send invitation for scheduleId: {scheduleId}.");
                            updateMessageStatusTasks.Add(messageReceiver.CompleteAsync(scheduleIdLockTokenMap[scheduleId]));
                        }
                    }
                }
                else
                {
                    var serviceResponseContent = await serviceResponse.Content.ReadAsStringAsync();
                }

                failedScheduleIds = acquiredLockScheduleIds?.Where(a => !processedScheduleIds.Contains(a));
                foreach (var scheduleId in failedScheduleIds)
                {
                    if (scheduleIdLockTokenMap.ContainsKey(scheduleId))
                    {
                        log.LogError($"SendInvitation: Failed to process send invitation for scheduleId: {scheduleId}.");
                        updateMessageStatusTasks.Add(messageReceiver.AbandonAsync(scheduleIdLockTokenMap[scheduleId]));

                        string failedMessageId = connectorIntegrationDetails.Where(a => a.Value.ScheduleID.Equals(scheduleId)).FirstOrDefault().Key;

                        if (sbqMessage?.SystemProperties.DeliveryCount == MaxRetries)
                        {
                            ////Invoke Fail Notification
                            SendFailNotfication(scheduleId, acquiredLockScheduleIds, rootActivityId, log);
                        }
                    }
                }
                await messageReceiver.CloseAsync();
                if (updateMessageStatusTasks != null && updateMessageStatusTasks.Count > 0)
                {
                    await Task.WhenAll(updateMessageStatusTasks);
                }

                await ReleaseLocks(scheduleIds, rootActivityId, log);
            }
            catch (Exception ex)
            {
                IList<Task> updateMessageStatusTasks = new List<Task>();

                foreach (var scheduleIdLockToken in scheduleIdLockTokenMap)
                {
                    log.LogError($"SendInvitation: Failed to process send invitation for scheduleId: {scheduleIdLockToken.Key}.");
                    updateMessageStatusTasks.Add(messageReceiver.AbandonAsync(scheduleIdLockToken.Value));
                }

                log.LogError($"SendInvitation: An error occurred while executing Invitation Schedule Worker. Error Message: {ex.Message} and Stack Trace: {ex.StackTrace}");
            }
        }

        public static async Task<HttpResponseMessage> SendRequestWithRetry(HttpMethod method, Uri uri, Object obj, string bearerToken, string rootActivityId, ILogger log)
        {
            log.LogInformation($"SendInvitation.SendRequestWithRetry: recieved request to send http request to schedulingservice with rootActivityId: {rootActivityId}");

            var attempt = 0;
            var random = new Random();

            using (var httpClient = new HttpClient(new HttpClientHandler()))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                while (true)
                {
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

                        log.LogInformation($"SendInvitation.SendRequestWithRetry: Got status {response.StatusCode} ({response.ReasonPhrase}) on attempt {attempt}, retrying after {delay} (retry after: {response.Headers.RetryAfter.Delta.Value}), rootActivityId: {rootActivityId}");

                        // Clean up the non-returned response object.
                        response.Dispose();

                        // Delay before retrying.
                        await Task.Delay(delay);
                        continue;
                    }

                    log.LogInformation($"SendInvitation.SendRequestWithRetry: Returning response with status {response.StatusCode} ({response.ReasonPhrase}) for {uri} on attempt {attempt}, rootActivityId: {rootActivityId} ");
                    return response;
                }
            }
        }

        private async static void SendFailNotfication(string scheduleId, List<string> acquiredLockScheduleIds, string rootActivityId, ILogger log)
        {
            try
            {
                log.LogInformation("SendFailNotfication for the schedule :" + scheduleId + " started");
                string schedulingServiceEnpoint = $"{Environment.GetEnvironmentVariable("SchedulingServiceEndpoint")}";

                Uri emailuri = new Uri($"{schedulingServiceEnpoint}v1/email/invitefailremind/scheduler?scheduleId={scheduleId}");
                string emailToken = await HttpAuthenticationAsync(Environment.GetEnvironmentVariable("Authority"), Environment.GetEnvironmentVariable("ClientId"));

                await SendRequestWithRetry(HttpMethod.Post, emailuri, acquiredLockScheduleIds, emailToken, rootActivityId, log);
                log.LogInformation("SendFailNotfication for the schedule :" + scheduleId + " completed");
            }
            catch (Exception ex)
            {
                log.LogError("SendFailNotfication for the schedule :" + scheduleId + " failed. Reason: " + ex.Message + " and Stack Trace: " + ex.StackTrace);
            }
        }

        private static async Task<string> HttpAuthenticationAsync(string authority, string clientId)
        {
            var authContext = new AuthenticationContext(authority);

            var authResult = await authContext
             .AcquireTokenAsync(clientId, new ClientCredential(clientId, Environment.GetEnvironmentVariable("ClientSecret")));
            return authResult.AccessToken;
        }

        private static async Task<List<string>> AcquireLocks(IList<string> scheduleIds, string rootActivityId, ILogger log)
        {
            if (!scheduleIds?.Any() ?? true)
            {
                log.LogError($"SendInvitation.AcquireLock: Received request to acquire locks for invalid scheduleIds. rootActivityId: {rootActivityId}");
                return null;
            }

            log.LogInformation($"SendInvitation.AcquireLock: Received request to acquire locks for {scheduleIds?.Count} messages. rootActivityId: {rootActivityId}");

            string schedulingServiceEnpoint = $"{Environment.GetEnvironmentVariable("SchedulingServiceEndpoint")}";

            Uri getlocksuri = new Uri($"{schedulingServiceEnpoint}v1/lock/getinvitationlocks");
            string bearerToken = await HttpAuthenticationAsync(Environment.GetEnvironmentVariable("Authority"), Environment.GetEnvironmentVariable("ClientId"));

            var serviceResponse = await SendRequestWithRetry(HttpMethod.Post, getlocksuri, scheduleIds, bearerToken, rootActivityId, log);


            IEnumerable<SendInvitationLock> lockedRecords = null;
            if (serviceResponse.IsSuccessStatusCode)
            {
                var content = await serviceResponse.Content.ReadAsStringAsync();
                lockedRecords = JsonConvert.DeserializeObject<List<SendInvitationLock>>(content);
            }

            var sendInvitationLocks = lockedRecords?.Distinct().ToList();
            var lockedScheduleIds = sendInvitationLocks?.Select(a => a.ScheduleId)?.ToList();
            log.LogInformation($"SendInvitation.AcquireLock: Lock Records exists for {lockedScheduleIds?.Count} messages. rootActivityId: {rootActivityId}");
            var acquiredLockScheduleIds = new List<string>();

            foreach (var scheduleId in scheduleIds)
            {
                if (!lockedScheduleIds?.Contains(scheduleId) ?? true)
                {
                    var lockRecord = new SendInvitationLock()
                    {
                        ScheduleId = scheduleId,
                        LockedTime = DateTime.UtcNow
                    };
                    log.LogInformation($"SendInvitation.AcquireLocks: No lock exists.Locking message with scheduleId: {scheduleId}, rootActivityId: {rootActivityId}");
                    acquiredLockScheduleIds.Add(scheduleId);

                    Uri createLockuri = new Uri($"{schedulingServiceEnpoint}v1/lock/createinvitationlock");
                    var createResponse = await SendRequestWithRetry(HttpMethod.Post, createLockuri, lockRecord, bearerToken, rootActivityId, log);
                }
            }

            if (sendInvitationLocks != null)
            {
                foreach (var lockedRecord in sendInvitationLocks)
                {
                    if (DateTime.UtcNow.Subtract(lockedRecord.LockedTime).TotalSeconds > lockRecordMaxTimeInSeconds)
                    {
                        log.LogInformation(
                            $"SendInvitation.AcquireLocks: The Message has been locked for more than the allotted time. scheduleId: {lockedRecord.ScheduleId}, rootActivityId: {rootActivityId}");

                        Uri deleteLockuri = new Uri($"{schedulingServiceEnpoint}v1/lock/deleteinvitationlock/{lockedRecord.Id}");
                        var deleteResponse = await SendRequestWithRetry(HttpMethod.Delete, deleteLockuri, null, bearerToken, rootActivityId, log);

                        var lockRecord = new SendInvitationLock()
                        {
                            ScheduleId = lockedRecord.ScheduleId,
                            LockedTime = DateTime.UtcNow
                        };
                        acquiredLockScheduleIds.Add(lockedRecord.ScheduleId);

                        Uri createLockuri = new Uri($"{schedulingServiceEnpoint}v1/lock/createinvitationlock");
                        var createResponse = await SendRequestWithRetry(HttpMethod.Post, createLockuri, lockRecord, bearerToken, rootActivityId, log);
                    }
                    else
                    {
                        log.LogInformation(
                            $"SendInvitation.AcquireLocks: Lock already exist. scheduleId: {lockedRecord.ScheduleId}, rootActivityId: {rootActivityId}");
                    }
                }
            }

            return acquiredLockScheduleIds;
        }

        private static async Task ReleaseLocks(IList<string> scheduleIds, string rootActivityId, ILogger log)
        {
            try
            {
                log.LogInformation($"SendInvitation.ReleaseLock: Received request to release lock, rootActivityId: {rootActivityId}");
                var releaseLockTasks = new List<Task>();
                string schedulingServiceEnpoint = $"{Environment.GetEnvironmentVariable("SchedulingServiceEndpoint")}";
                string bearerToken = await HttpAuthenticationAsync(Environment.GetEnvironmentVariable("Authority"), Environment.GetEnvironmentVariable("ClientId"));

                Uri deleteLockuri = new Uri($"{schedulingServiceEnpoint}v1/lock/deleteinvitationlocks");
                await SendRequestWithRetry(HttpMethod.Delete, deleteLockuri, scheduleIds, bearerToken, rootActivityId, log);

                log.LogInformation($"SendInvitation.ReleaseLock: Successfully released lock, rootActivityId: {rootActivityId}");
            }
            catch (Exception ex)
            {
                log.LogError($"SendInvitation.ReleaseLock: Message lock release request failed, rootActivityId: {rootActivityId}, Error Message : {ex.Message}, InnerException :{ex.InnerException}, StackTrace: {ex.StackTrace}");
            }
        }
    }
}
