//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.SubscribeToInbox
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class SubscribeToInbox
    {
        private static int MaxRetries = 10;
        private static int MaxRandomAdditionalDelaySeconds = 5;

        [FunctionName("SubscribeToInbox")]
        public static async Task Run([TimerTrigger("%TimerSchedule%", RunOnStartup =true)]TimerInfo myTimer, ILogger log)
        {

            try
            {
                log.LogInformation($"SubscribeToInbox: Timer trigger function executed at: {DateTime.Now}");

                string schedulingServiceEndpoint = $"{Environment.GetEnvironmentVariable("SchedulingServiceEndpoint")}";
                
                var schedulerUserEmails = Environment.GetEnvironmentVariable("ServiceAccountEmails")?.Split(',').ToList();
                string bearerToken = await HttpAuthenticationAsync(Environment.GetEnvironmentVariable("Authority"), Environment.GetEnvironmentVariable("ClientId"));

                if (schedulerUserEmails != null)
                {
                    foreach (var serviceAccount in schedulerUserEmails)
                    {
                        Uri uri = new Uri(
                            $"{schedulingServiceEndpoint}v1/subscription/subscribeToInbox?serviceAccountName={serviceAccount}");


                        log.LogInformation($"SubscribeToInbox: Subscribing to service account: {serviceAccount}");

                        var serviceResponse =
                            await SendRequestWithRetry(HttpMethod.Post, uri, string.Empty, bearerToken, log);

                        if (serviceResponse.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            log.LogError(
                                $"SubscribeToInbox: Scheduling Service response code: {serviceResponse.StatusCode}, while subscribing to service account: {serviceAccount}, error Message: {JObject.Parse(serviceResponse?.Content?.ReadAsStringAsync()?.Result)?.ToString()}");
                        }

                        log.LogInformation(
                            $"SubscribeToInbox: Subscribed to service account: {serviceAccount} successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError($"SubscribeToInbox: An error occurred while executing Invitation Schedule Worker. Error Message: {ex.Message} and stack trace : {ex.StackTrace}");
            }

        }
        public static async Task<HttpResponseMessage> SendRequestWithRetry(HttpMethod method, Uri uri, Object obj, string bearerToken, ILogger log)
        {
            log.LogInformation($"SubscribeToInbox: Received request to send http request to schedulingservice");

            var attempt = 0;
            var random = new Random();

            using (var httpClient = new HttpClient(new HttpClientHandler()))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                while (true)
                {
                    log.LogInformation($"SubscribeToInbox: Sending http request to schedulingservice");

                    var request = new HttpRequestMessage(method, uri)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"),
                    };

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

                        log.LogInformation($"SubscribeToInbox: Got status {response.StatusCode} ({response.ReasonPhrase}) on attempt {attempt}, retrying after {delay} (retry after: {response.Headers.RetryAfter.Delta.Value})");

                        // Clean up the non-returned response object.
                        response.Dispose();

                        // Delay before retrying.
                        await Task.Delay(delay);
                        continue;
                    }

                    log.LogInformation($"SubscribeToInbox: Returning response with status {response.StatusCode} ({response.ReasonPhrase}) for {uri} on attempt {attempt}");
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
    }
}
