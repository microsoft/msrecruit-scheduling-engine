// <copyright file="OutlookProvider.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using MS.GTA.Common.Base.Configuration;
    using MS.GTA.Common.Base.Security;
    using MS.GTA.Common.MSGraph;
    using MS.GTA.Common.MSGraph.Configuration;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.ScheduleService.BusinessLibrary.Configurations;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.Helper;
    using MS.GTA.ScheduleService.Utils;
    using MS.GTA.ServicePlatform.AspNetCore.Mvc.Filters;
    using MS.GTA.ServicePlatform.Azure.Security;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Exceptions;
    using Newtonsoft.Json;
    using Email = MS.GTA.Common.Email;
    using Message = MS.GTA.ScheduleService.Contracts.V1.Message;

    /// <summary>
    /// The provider class for the outlook graph.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OutlookProvider : IOutlookProvider
    {
        /// <summary>
        /// SearchUserApi
        /// </summary>
        private const string SearchUserApi = "/users?$filter=mail eq '{0}' or userPrincipalName eq '{0}'";

        /// <summary>
        /// Default meeting duration if none specified.
        /// </summary>
        private const string DefaultMeetingDuration = "PT1H";

        /// <summary>
        /// Default maximum number of meeting time candidates to return if none is specified.
        /// </summary>
        private const int DefaultMaxMeetingCandidates = 99;

        /// <summary>
        /// Default minimum percentage of required attendees that must be able to attend a meeting to return the suggestion if none is specified.
        /// </summary>
        private const double DefaultMinumumAttendeePercentage = 100;

        /// <summary>
        /// Default value fi whether the reason for the suggestion is returned.
        /// </summary>
        private const bool DefaultReturnSuggestionReasons = true;

        /// <summary>
        /// Configuration manager instance
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// Outlook graph API service provider.
        /// </summary>
        private readonly IMsGraphProvider graphProvider;

        /// <summary>
        /// Base graph url for making API calls.
        /// </summary>
        private readonly string graphBaseUrl;

        /// <summary>
        /// Graph resource Id.
        /// </summary>
        private readonly string graphResourceId;

        /// <summary>
        /// Graph url for making subscriptionAPI calls.
        /// </summary>
        private readonly string subscriptionsEndpoint;

        /// <summary>
        /// The notification address
        /// </summary>
        private readonly string notificationUrl;

        /// <summary>
        /// JSON serializer settings for http calls.
        /// </summary>
        private readonly JsonSerializerSettings jsonSerializerSettings;

        /// <summary>
        /// Gets or sets the Metric Logger
        /// </summary>
        private readonly ILogger<OutlookProvider> logger;

        /// <summary>
        /// Gets or sets email client Logger
        /// </summary>
        private readonly IEmailClient emailClient;

        private readonly ITokenCacheService tokenCacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlookProvider"/> class.
        /// </summary>
        /// <param name="graphProvider">The graph provider.</param>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="secretManager">The secret manager class.</param>
        /// <param name="emailClient">The email client class.</param>
        /// <param name="tokenCacheService">Instance of token cache service</param>
        /// <param name="logger">The Logger</param>
        public OutlookProvider(
            IMsGraphProvider graphProvider,
            IConfigurationManager configurationManager,
            ISecretManager secretManager,
            IEmailClient emailClient,
            ITokenCacheService tokenCacheService,
            ILogger<OutlookProvider> logger)
        {
            Contract.CheckValue(tokenCacheService, nameof(tokenCacheService));

            this.graphProvider = graphProvider;

            this.configurationManager = configurationManager;
            this.graphBaseUrl = this.configurationManager.Get<MsGraphSetting>().GraphBaseUrl;
            this.graphResourceId = this.configurationManager.Get<MsGraphSetting>().GraphResourceId;
            this.notificationUrl = this.configurationManager.Get<MsGraphSetting>().NotificationUrl;
            this.subscriptionsEndpoint = $"{this.graphBaseUrl}/subscriptions/";
            this.logger = logger;
            this.emailClient = emailClient;
            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };

            this.tokenCacheService = tokenCacheService;
        }

        /// <summary>
        /// Sends find free busy schedule request.
        /// </summary>
        /// <param name="findFreeBusyRequest">Request body.</param>
        /// <returns>FindFreeBusySchedule Response.</returns>
        public async Task<List<FindFreeBusyScheduleResponse>> SendPostFindFreeBusySchedule(FindFreeBusyScheduleRequest findFreeBusyRequest)
        {
            Contract.Assert(findFreeBusyRequest != null, "Invalid input parameter. findFreeBusyRequest must not be null.");

            var findFreeBusyResponsePayload = new FindFreeBusyScheduleResponsePayload();
            var findFreeBusyResponses = new List<FindFreeBusyScheduleResponse>();
            var graphResourceId = this.configurationManager.Get<MsGraphSetting>().GraphResourceId;
            var serviceAccountName = this.configurationManager.Get<SchedulerConfiguration>().FreeBusyEmailAddress;
            var accessToken = await this.emailClient.GetServiceAccountTokenByEmail(serviceAccountName);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = await this.GetBearerTokenFromUserToken(accessToken, serviceAccountName, TokenCachingOptions.ForceRefreshCache);

                // Serialize the request object.
                var tasks = new List<Task>();
                if (findFreeBusyRequest != null)
                {
                    var schedules = findFreeBusyRequest.Schedules;
                    foreach (var scheduleBatch in Chunk(schedules, 20))
                    {
                        tasks.Add(this.logger.ExecuteAsync(
                        "GTAGetGraphCalendarschedule",
                        async () =>
                        {
                            for (int i = 1; i <= Email.Constants.MaxRetryCount; i++)
                            {
                                findFreeBusyRequest.Schedules = (List<string>)scheduleBatch;
                                var requestData = JsonConvert.SerializeObject(findFreeBusyRequest, this.jsonSerializerSettings);
                                using (var response = await httpClient.PostAsync(graphResourceId + "/v1.0/me/calendar/getschedule", new StringContent(requestData, Encoding.UTF8, "application/json")).ConfigureAwait(false))
                                {
                                    var responseHeaders = response.Headers.ToString();
                                    this.logger.LogInformation($"GetFreeBusySchedule: Response headers for find meeting times are {responseHeaders}");
                                    if (response.IsSuccessStatusCode)
                                    {
                                        // Read and deserialize response.
                                        var content = await response.Content.ReadAsStringAsync();

                                        findFreeBusyResponsePayload = JsonConvert.DeserializeObject<FindFreeBusyScheduleResponsePayload>(content, this.jsonSerializerSettings);

                                        if (findFreeBusyResponsePayload.Value?.Count > 0)
                                        {
                                            findFreeBusyResponses.AddRange(findFreeBusyResponsePayload.Value);
                                        }
                                        i = Email.Constants.MaxRetryCount;
                                    }
                                    else
                                    {
                                        string content = string.Empty;
                                        if (response != null)
                                        {
                                            content = await response.Content.ReadAsStringAsync();
                                        }

                                        if (i < Email.Constants.MaxRetryCount && EmailUtils.ShouldRetryOnGraphException(response.StatusCode))
                                        {
                                            this.logger.LogWarning($"GetFreeBusySchedule: Attempt {i} : Exception during {HttpMethod.Post.Method}:{this.graphResourceId}/me/calendar/getschedule call to graph. Response {response.StatusCode.ToString()} with error message {content}");
                                            await EmailUtils.ExponentialDelay(response, i);
                                        }
                                        else
                                        {
                                            this.logger.LogError($"GetFreeBusySchedule: Exception during {HttpMethod.Post.Method}:{this.graphBaseUrl}/me/getschedule call to graph. Response {response.StatusCode.ToString()} with error message {content}");
                                            break;
                                        }
                                    }
                                }
                            }
                        }));
                    }

                    Task.WaitAll(tasks.ToArray());
                    findFreeBusyRequest.Schedules = schedules;
                }
            }

            return findFreeBusyResponses;
        }

        /// <summary>
        /// Get calendar event request.
        /// </summary>
        /// <param name="userAccessToken">User Id token.</param>
        /// <param name="calendarEventId">calendar event id.</param>
        /// <param name="serviceAccountName">Service account email</param>
        /// <returns>Event that was retrieved</returns>
        public async Task<CalendarEvent> GetCalendarEvent(string userAccessToken, string calendarEventId, string serviceAccountName)
        {
            Contract.Assert(calendarEventId != null, "Invalid input parameter. CalendarEventId must not be null.");

            var eventResponse = new CalendarEvent();

            // Force refreshing cache so that the cached resource token from the user principle isn't returned
            var schedulerGraphToken = await this.GetBearerTokenFromUserToken(userAccessToken, serviceAccountName, TokenCachingOptions.ForceRefreshCache);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = schedulerGraphToken;

                var userEmail = EmailUtils.GetUserEmailFromToken(userAccessToken);

                var url = $"{this.graphBaseUrl}/users/{userEmail}/events/{calendarEventId}{SchedulerConstants.ExpandExtensionFilter}";

                eventResponse = await this.logger.ExecuteAsync<CalendarEvent>(
                    "GTAGetEvt",
                    async () =>
                    {
                        var exceptions = new List<Exception>();
                        for (int i = 1; i <= Email.Constants.MaxRetryCount; i++)
                        {
                            var getRequest = new HttpRequestMessage(HttpMethod.Get, url);
                            getRequest.Headers.Authorization = schedulerGraphToken;

                            using (var response = await httpClient.SendAsync(getRequest).ConfigureAwait(false))
                            {
                                this.logger.LogInformation($"Get calendar event call made with account {userEmail}");
                                var content = await response.Content.ReadAsStringAsync();

                                var responseHeaders = response.Headers.ToString();
                                this.logger.LogInformation($"Response headers for Get calendar event are {responseHeaders}");
                                if (response.IsSuccessStatusCode)
                                {
                                    return JsonConvert.DeserializeObject<CalendarEvent>(content, this.jsonSerializerSettings);
                                }
                                else
                                {
                                    var exception = new GraphException(HttpMethod.Get.Method, url, response.StatusCode, content);

                                    if (EmailUtils.ShouldRetryOnGraphException(response.StatusCode))
                                    {
                                        this.logger.LogWarning($"GetCalendarEvent: Get attempt #{i} failed, Status code: {response.StatusCode}, content: {content}. Will try again");
                                        exceptions.Add(exception);

                                        await EmailUtils.ExponentialDelay(response, i);
                                    }
                                    else
                                    {
                                        throw exception.EnsureLogged(this.logger);
                                    }
                                }
                            }
                        }

                        throw new AggregateException(exceptions);
                    });
            }

            return eventResponse;
        }

        /// <summary>
        /// Sends create event request.
        /// </summary>
        /// <param name="serviceAccountName">service account name.</param>
        /// <param name="eventRequest">Request body.</param>
        /// <returns>Event that was created</returns>
        public async Task<CalendarEvent> SendPostEvent(string serviceAccountName, CalendarEvent eventRequest)
        {
            Contract.Assert(eventRequest != null, "Invalid input parameter. EventRequest must not be null.");

            eventRequest.TransactionId = Guid.NewGuid();
            this.logger.LogInformation($"SendPostEvent: TransactionId used in the email: {eventRequest.TransactionId}");

            var eventResponse = new CalendarEvent();

            var accessToken = await this.emailClient.GetServiceAccountTokenByEmail(serviceAccountName);

            // Force refreshing cache so that the cached resource token from the user principle isn't returned
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = await this.GetBearerTokenFromUserToken(accessToken, serviceAccountName, TokenCachingOptions.ForceRefreshCache).ConfigureAwait(false);

                httpClient.DefaultRequestHeaders.Add("Prefer", "exchange.behavior = \"EventTransactionId\"");

                // Serialize the request object.
                var requestData = JsonConvert.SerializeObject(eventRequest, this.jsonSerializerSettings);
                var userEmail = EmailUtils.GetUserEmailFromToken(accessToken);
                this.logger.LogInformation($"Sending Calendar event(Post) using email: {userEmail}");

                var url = $"{this.graphBaseUrl}/users/{userEmail}/events";

                eventResponse = await this.logger.ExecuteAsync(
                    "GTASendPostCalendarEvent",
                    async () =>
                    {
                        List<Exception> exceptions = new List<Exception>();
                        for (int i = 1; i <= Email.Constants.MaxRetryCount; i++)
                        {
                            using (var response = await httpClient.PostAsync(url, new StringContent(requestData, Encoding.UTF8, "application/json")).ConfigureAwait(false))
                            {
                                var responseHeaders = response.Headers.ToString();
                                this.logger.LogInformation($"Response headers for post calendar event are {responseHeaders}");

                                if (response.IsSuccessStatusCode)
                                {
                                    var content = await response.Content.ReadAsStringAsync();
                                    var postEventResult = JsonConvert.DeserializeObject<CalendarEvent>(content, this.jsonSerializerSettings);

                                    this.logger.LogInformation($"Calender event id: {postEventResult.Id} created with account: {userEmail}");
                                    return postEventResult;
                                }
                                else
                                {
                                    string content = await response.Content.ReadAsStringAsync();
                                    var exception = new GraphException(HttpMethod.Post.Method, url, response.StatusCode, content);

                                    if (EmailUtils.ShouldRetryOnGraphException(response.StatusCode))
                                    {
                                        this.logger.LogWarning($"PostCalendarEvent: Post attempt #{i} failed, for user {userEmail}, status code {response.StatusCode}, response content: {content}");
                                        exceptions.Add(exception);
                                        await EmailUtils.ExponentialDelay(response, i);
                                    }
                                    else
                                    {
                                        throw exception.EnsureLogged(this.logger);
                                    }
                                }
                            }
                        }

                        throw new AggregateException(exceptions);
                    });
            }

            return eventResponse;
        }

        /// <summary>
        /// Sends update calendar event request.
        /// </summary>
        /// <param name="serviceAccountName">Service Account Name.</param>
        /// <param name="eventRequest">Request body.</param>
        /// <param name="expand"> bool to controll response expansion</param>
        /// <returns>Event that was updated</returns>
        public async Task<CalendarEvent> SendPatchEvent(string serviceAccountName, CalendarEvent eventRequest, bool expand = true)
        {
            Contract.Assert(eventRequest != null, "Invalid input parameter. EventRequest must not be null.");

            var eventResponse = new CalendarEvent();

            var accessToken = await this.emailClient.GetServiceAccountTokenByEmail(serviceAccountName);

            try
            {
                eventResponse = await this.logger.ExecuteAsync(
                    "GTAUpdateCalendarEvent",
                    async () =>
                    {
                    // var userEmail = EmailUtils.GetUserEmailFromToken(accessToken);
                    this.logger.LogInformation($"Sending patch update using email {serviceAccountName}");

                    var relativePath = $"/users/{serviceAccountName}/events/{eventRequest?.Id}";

                    var url = $"{this.graphBaseUrl}{relativePath}";

                    // Force refreshing cache so that the cached resource token from the user principle isn't returned
                    var schedulerGraphToken = await this.GetBearerTokenFromUserToken(accessToken, serviceAccountName, TokenCachingOptions.ForceRefreshCache);
                    var exceptions = new List<Exception>();
                        using (var httpClient = new HttpClient())
                        {
                            httpClient.DefaultRequestHeaders.Authorization = schedulerGraphToken;
                            var requestData = JsonConvert.SerializeObject(eventRequest, this.jsonSerializerSettings);

                            for (int i = 1; i <= Email.Constants.MaxRetryCount; i++)
                            {
                                var message = new HttpRequestMessage(new HttpMethod("PATCH"), url);
                                message.Headers.Authorization = schedulerGraphToken;
                                message.Content = new StringContent(
                                                   requestData,
                                                   Encoding.UTF8,
                                                   "application/json");

                                using (var response = await httpClient.SendAsync(message).ConfigureAwait(false))
                                {
                                    var responseHeaders = response.Headers.ToString();
                                    this.logger.LogInformation($"Response headers for patch calendar event are {responseHeaders}");

                                    if (response.IsSuccessStatusCode)
                                    {
                                        if (expand)
                                        {
                                            for (int j = 1; j <= Email.Constants.MaxRetryCount; j++)
                                            {
                                                // Do another GET so that we can return the Extensions
                                                var getMessage = new HttpRequestMessage(HttpMethod.Get, $"{url}{SchedulerConstants.ExpandExtensionFilter}");
                                                getMessage.Headers.Authorization = schedulerGraphToken;

                                                using (var extentionResponse = await httpClient.SendAsync(getMessage))
                                                {
                                                    if (extentionResponse.IsSuccessStatusCode)
                                                    {
                                                        var content = await extentionResponse.Content.ReadAsStringAsync();

                                                        return JsonConvert.DeserializeObject<CalendarEvent>(content, this.jsonSerializerSettings);
                                                    }
                                                    else
                                                    {
                                                        string content = await extentionResponse.Content.ReadAsStringAsync();
                                                        var exception = new GraphException(HttpMethod.Get.Method, $"{url}{SchedulerConstants.ExpandExtensionFilter}", extentionResponse.StatusCode, content);
                                                        if (EmailUtils.ShouldRetryOnGraphException(extentionResponse.StatusCode))
                                                        {
                                                            this.logger.LogWarning($"Retry # - {j}. Get extension for caledar event failed with StatusCode: {extentionResponse.StatusCode}, error: {content}");
                                                            exceptions.Add(exception);

                                                            await EmailUtils.ExponentialDelay(extentionResponse, j);
                                                        }
                                                        else
                                                        {
                                                            throw exception.EnsureLogged(this.logger);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return eventRequest;
                                        }

                                        throw new AggregateException(exceptions);
                                    }
                                    else
                                    {
                                        string content = await response.Content.ReadAsStringAsync();
                                        var exception = new GraphException("PATCH", url, response.StatusCode, content);

                                        if (EmailUtils.ShouldRetryOnGraphException(response.StatusCode))
                                        {
                                            this.logger.LogWarning($"Retry # - {i}. Updating calendar event failed with StatusCode: {response.StatusCode}, error: {content}");
                                            exceptions.Add(exception);

                                            await EmailUtils.ExponentialDelay(response, i);
                                        }
                                        else
                                        {
                                            throw exception.EnsureLogged(this.logger);
                                        }
                                    }
                                }
                            }
                        }

                        throw new AggregateException(exceptions);
                    });
            }
            catch (Exception e)
            {
                this.logger.LogError($"SendPatchEvent failed with error {e.Message} stackTrace {e.StackTrace} InnerException: {e.InnerException}");
                throw new SchedulerUpdateCalendarException(e).EnsureLogged(this.logger);
            }

            return eventResponse;
        }

        /// <summary>
        /// Delete calendar event given the calendar event id.
        /// </summary>
        /// <param name="serviceAccountName">Scheduler access token.</param>
        /// <param name="calendarEventId">Calendar event id.</param>
        /// <param name="isValidationRequired">Is calendar validation required</param>
        /// <returns>Status code.</returns>
        public async Task DeleteCalendarEvent(string serviceAccountName, string calendarEventId, bool isValidationRequired = false)
        {
            Contract.AssertValue(serviceAccountName, nameof(serviceAccountName));
            Contract.AssertValue(calendarEventId, nameof(calendarEventId));
            this.logger.LogInformation($"Calendar event {calendarEventId} deletion started.");

            var accessToken = await this.emailClient.GetServiceAccountTokenByEmail(serviceAccountName);

            await this.logger.ExecuteAsync(
                "GTADeleteCalendarEvent",
                async () =>
                {
                    var isValidEvent = isValidationRequired ? await this.IsValidCalendarEvent(accessToken, calendarEventId, serviceAccountName) : true;

                    if (isValidEvent)
                    {
                        var relativePath = $"/users/{serviceAccountName}/events/{calendarEventId}";

                        // Force refreshing cache so that the cached resource token from the user principle isn't returned
                        var schedulerGraphToken = await this.GetBearerTokenFromUserToken(accessToken, serviceAccountName, TokenCachingOptions.ForceRefreshCache);

                        for (int i = 1; i <= Email.Constants.MaxRetryCount; i++)
                        {
                            var message = new HttpRequestMessage(HttpMethod.Delete, $"{this.graphBaseUrl}{relativePath}");
                            using (var httpClient = new HttpClient())
                            {
                                httpClient.DefaultRequestHeaders.Authorization = schedulerGraphToken;

                                message.Headers.Authorization = schedulerGraphToken;

                                using (var httpResponse = await httpClient.SendAsync(message))
                                {
                                    this.logger.LogInformation($"Delete request headers : {httpResponse.Headers}");
                                    this.logger.LogInformation($"Delete calendar event call made with account {serviceAccountName}");

                                    if (!httpResponse.IsSuccessStatusCode)
                                    {
                                        string content = await httpResponse.Content.ReadAsStringAsync();
                                        if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                                        {
                                            this.logger.LogWarning($"Delete Calendar event resulted in 404 for id {calendarEventId}");
                                            return;
                                        }
                                        else if (EmailUtils.ShouldRetryOnGraphException(httpResponse.StatusCode))
                                        {
                                            this.logger.LogWarning($"Retry # - {i}. Deleting calendar event failed with StatusCode: {httpResponse.StatusCode}, error: {content}");
                                            await EmailUtils.ExponentialDelay(httpResponse, i);
                                        }
                                        else
                                        {
                                            this.logger.LogError($"Delete Calendar event failed for id {calendarEventId} with {httpResponse.StatusCode}, Content : {content}");
                                            throw new DeleteCalendarEventException().EnsureLogged(this.logger);
                                        }
                                    }
                                    else
                                    {
                                        this.logger.LogInformation($"Calendar event {calendarEventId} deleted successfully.");
                                        return;
                                    }
                                }
                            }
                        }

                        throw new DeleteCalendarEventException().EnsureLogged(this.logger);
                    }
                });
        }

        /// <summary>
        /// Search User By Email.
        /// </summary>
        /// <param name="email">Email of the User</param>
        /// <param name="serviceAccountName">Service Account for token generation</param>
        /// <returns><see cref="Task"/></returns>
        public async Task<GraphUserResponse> SearchUserByEmail(string email, string serviceAccountName)
        {
            Contract.Assert(email != null, "Invalid input parameter. email must not be null.");

            var accessToken = await this.emailClient.GetServiceAccountTokenByEmail(serviceAccountName);
            if (accessToken == null)
            {
                this.logger.LogWarning($"SearchUserByEmail: AccessToken is null.");

                return null;
            }

            GraphUserResponse graphResponse = new GraphUserResponse();
            await this.logger.ExecuteAsync(
                "GTASearchUserByEmail",
                async () =>
                {
                    var relativePath = string.Format(SearchUserApi, email);

                    // Force refreshing cache so that the cached resource token from the user principle isn't returned
                    var schedulerGraphToken = await this.GetBearerTokenFromUserToken(accessToken, serviceAccountName, TokenCachingOptions.ForceRefreshCache);

                    var message = new HttpRequestMessage(HttpMethod.Get, $"{this.graphBaseUrl}{relativePath}");
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = schedulerGraphToken;

                        message.Headers.Authorization = schedulerGraphToken;

                        using (var httpResponse = await httpClient.SendAsync(message))
                        {
                            this.logger.LogInformation($"SearchUserByEmail response headers: {httpResponse.Headers}");
                            this.logger.LogInformation($"SearchUserByEmail call made with account {serviceAccountName}");

                            if (httpResponse.IsSuccessStatusCode)
                            {
                                string stringResult = await httpResponse.Content.ReadAsStringAsync();
                                graphResponse = JsonConvert.DeserializeObject<GraphUserResponse>(stringResult);
                            }
                            else
                            {
                                this.logger.LogError($"SearchUserByEmail(): Failed to retrive user details. Status Code: {httpResponse.StatusCode}");
                            }
                        }
                    }
                });

            return graphResponse;
        }

        /// <inheritdoc />
        public async Task<SubscriptionViewModel> Subscribe(SubscriptionViewModel subscriptionViewModel, string serviceAccountName, bool isSubscribingToEvents = true)
        {
            Contract.AssertValue(subscriptionViewModel, nameof(subscriptionViewModel), "Subscription required to subscribe");
            var userAccessToken = await this.emailClient.GetServiceAccountTokenByEmail(serviceAccountName);

            var graphToken = await this.GetResourceAccessTokenFromUserToken(userAccessToken, serviceAccountName);
            if (string.IsNullOrEmpty(graphToken))
            {
                this.logger.LogWarning($"OutlookProvider:Subscribe(): graphToken is null");
            }
            else if (string.IsNullOrEmpty(subscriptionViewModel?.Subscription?.Id))
            {
                this.logger.LogInformation($"OutlookProvider:Subscribe(): No subscription found, create new subscription ");
                if (subscriptionViewModel != null)
                {
                    subscriptionViewModel.Subscription = await this.CreateNewSubscription(graphToken, isSubscribingToEvents);
                }
            }
            else
            {
                this.logger.LogInformation($"OutlookProvider:Subscribe(): Subscription {subscriptionViewModel?.Subscription?.Id} is found, renew subscription");
                subscriptionViewModel = await this.RenewSubscription(graphToken, subscriptionViewModel);
            }

            return subscriptionViewModel;
        }

        /// <inheritdoc />
        public async Task<bool> Unsubscribe(SubscriptionViewModel subscriptionViewModel, string serviceAccountName)
        {
            Contract.AssertValue(subscriptionViewModel, nameof(subscriptionViewModel), "Subscription required to subscribe");
            var userAccessToken = await this.emailClient.GetServiceAccountTokenByEmail(serviceAccountName);

            var graphToken = await this.GetResourceAccessTokenFromUserToken(userAccessToken, serviceAccountName);
            if (string.IsNullOrEmpty(graphToken))
            {
                this.logger.LogError($"OutlookProvider:Unsubscribe(): graphToken is null");
            }
            else if (string.IsNullOrEmpty(subscriptionViewModel?.Subscription?.Id))
            {
                this.logger.LogWarning($"OutlookProvider:Unsubscribe(): No subscription found");
                return true;
            }
            else
            {
                this.logger.LogInformation($"OutlookProvider:Unsubscribe(): Subscription {subscriptionViewModel?.Subscription?.Id} is found, delete subscription");
                return await this.DeleteSubscription(graphToken, subscriptionViewModel?.Subscription);
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<Message> GetMessageById(string messageId, string userAccessToken, string serviceAccountEmail)
        {
            this.logger.LogError($"GetMessageById(): Received request to retrieve message for messageId {messageId}");

            if (string.IsNullOrWhiteSpace(messageId))
            {
                throw new InvalidRequestDataValidationException("Invalid messageId");
            }

            var graphToken = await this.GetResourceAccessTokenFromUserToken(userAccessToken, serviceAccountEmail);

            Message message = new Message();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", graphToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = $"{this.graphBaseUrl}/{messageId}?$expand=microsoft.graph.eventMessage/event";
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string stringResult = await response.Content.ReadAsStringAsync();
                            message = JsonConvert.DeserializeObject<Message>(stringResult);
                        }
                        else
                        {
                            this.logger.LogWarning($"GetMessageById(): Failed to retrieve Message. Status Code: {response.StatusCode}");

                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                throw new MessageNotFoundException(message.Id);
                            }
                        }

                        this.logger.LogError($"GetMessageById(): Successfully retrieved message for messageId {messageId}");
                        return message;
                    }
                }
            }
        }

        /// <inheritdoc />
        public async Task<CalendarEvent> GetEventById(string eventId, string userAccessToken, string serviceAccountEmail)
        {
            this.logger.LogError($"GetEventById: Received request to retrieve message for eventId {eventId}");

            if (string.IsNullOrWhiteSpace(eventId))
            {
                throw new InvalidRequestDataValidationException("Invalid eventId");
            }

            var graphToken = await this.GetResourceAccessTokenFromUserToken(userAccessToken, serviceAccountEmail);

            CalendarEvent calendarEvent = new CalendarEvent();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", graphToken);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string url = $"{this.graphBaseUrl}/{eventId}";
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string stringResult = await response.Content.ReadAsStringAsync();
                            calendarEvent = JsonConvert.DeserializeObject<CalendarEvent>(stringResult);
                        }
                        else
                        {
                            this.logger.LogWarning($"GetMessageById: Failed to retrieve Message. Status Code: {response.StatusCode}");

                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                throw new MessageNotFoundException(calendarEvent.Id);
                            }
                        }

                        this.logger.LogError($"GetMessageById: Successfully retrieved message for messageId {eventId}");
                        return calendarEvent;
                    }
                }
            }
        }

        /// <summary>
        /// Find meeting times in case of panel interview.
        /// </summary>
        /// <param name="findMeetingTimeRequest">request object</param>
        /// <returns>Meeting suggestions that were retrieved</returns>
        public async Task<FindMeetingTimeResponse> FindMeetingTimes(FindMeetingTimeRequest findMeetingTimeRequest)
        {
            Contract.Assert(findMeetingTimeRequest != null, $"Invalid input parameter. {nameof(findMeetingTimeRequest)} must not be null in {nameof(this.FindMeetingTimes)}.");

            var findMeetingTimeResponse = new FindMeetingTimeResponse();

            var graphResourceId = this.configurationManager.Get<MsGraphSetting>().GraphResourceId;
            var serviceAccountName = this.configurationManager.Get<SchedulerConfiguration>().FreeBusyEmailAddress;
            var accessToken = await this.emailClient.GetServiceAccountTokenByEmail(serviceAccountName);
            var graphToken = await this.GetBearerTokenFromUserToken(accessToken, serviceAccountName, TokenCachingOptions.ForceRefreshCache);

            var requestData = JsonConvert.SerializeObject(findMeetingTimeRequest, this.jsonSerializerSettings);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = graphToken;
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = $"{this.graphBaseUrl}/me/findMeetingTimes";
                using (var response = await httpClient.PostAsync(url, new StringContent(requestData, Encoding.UTF8, "application/json")).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string stringResult = await response.Content.ReadAsStringAsync();
                        findMeetingTimeResponse = JsonConvert.DeserializeObject<FindMeetingTimeResponse>(stringResult);
                    }
                    else
                    {
                        this.logger.LogWarning($"{nameof(this.FindMeetingTimes)}: Failed to retrieve suggestions. Status Code: {response.StatusCode}");
                    }
                }
            }

            return findMeetingTimeResponse;
        }

        private static IEnumerable<IList<T>> Chunk<T>(List<T> elements, int chunkSize)
        {
            if (elements != null)
            {
                for (var minIndex = 0; minIndex < elements.Count; minIndex += chunkSize)
                {
                    yield return elements.GetRange(minIndex, Math.Min(chunkSize, elements.Count - minIndex));
                }
            }
        }

        /// <summary>
        /// Renew the existing subscription for another 72 hours
        /// </summary>
        /// <param name="graphToken">Graph token to access subscription.</param>
        /// <param name="subscriptionViewModel">Subscription view model with subscription to renew</param>
        /// <returns>The SubscriptionViewModel with the new subscription</returns>
        [MonitorWith("GTARenewSubs")]
        private async Task<SubscriptionViewModel> RenewSubscription(string graphToken, SubscriptionViewModel subscriptionViewModel)
        {
            Contract.AssertValue(subscriptionViewModel, nameof(subscriptionViewModel), "Subscription required to renew");
            Contract.AssertValue(subscriptionViewModel.Subscription?.Id, nameof(subscriptionViewModel.Subscription.Id), "Subscription Id required to renew");

            var subscription = new Subscription
            {
                Id = subscriptionViewModel.Subscription?.Id,
                ExpirationDateTime = DateTime.UtcNow + new TimeSpan(0, 0, 4230, 0)
            };

            var httpMethod = new HttpMethod("PATCH");

            subscriptionViewModel.Subscription = await this.CreateOrUpdateSubscription(graphToken, httpMethod, subscription);

            return subscriptionViewModel;
        }

        /// <summary>
        /// Creates or updates a subscription
        /// </summary>
        /// <param name="graphToken">Graph token to access subscription.</param>
        /// <param name="httpMethod">POST to create or PATCH to update</param>
        /// <param name="subscription">Subscription to create or renew</param>
        /// <returns>Subscription that was created or renewed</returns>
        private async Task<Subscription> CreateOrUpdateSubscription(string graphToken, HttpMethod httpMethod, Subscription subscription)
        {
            Subscription newSubscription = new Subscription();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", graphToken);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Build the request.
                using (var request = new HttpRequestMessage(httpMethod, $"{this.subscriptionsEndpoint}{subscription.Id}"))
                {
                    string contentString = JsonConvert.SerializeObject(subscription, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    request.Content = new StringContent(contentString, Encoding.UTF8, "application/json");

                    // Send the request and parse the response.
                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            // Parse the JSON response.
                            string stringResult = await response.Content.ReadAsStringAsync();
                            newSubscription = JsonConvert.DeserializeObject<Subscription>(stringResult);
                        }
                        else
                        {
                            //// TODO: Don't throw exception now, just trace so that waes tests can run. Need to fix callback address for PR custom app type name
                            this.logger.LogWarning($"{httpMethod.Method} failed subscription. Status Code: {response.StatusCode}");

                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                throw new GraphSubscriptionNotFoundException(subscription.Id).EnsureLogged(this.logger);
                            }
                        }

                        return newSubscription;
                    }
                }
            }
        }

        /// <summary>
        /// Delete a subscription
        /// </summary>
        /// <param name="graphToken">Graph token to access subscription.</param>
        /// <param name="subscription">Subscription to create or renew</param>
        /// <returns>If deletion succeeds.</returns>
        private async Task<bool> DeleteSubscription(string graphToken, Subscription subscription)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", graphToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Build the request.
                using (var request = new HttpRequestMessage(HttpMethod.Delete, $"{this.subscriptionsEndpoint}{subscription.Id}"))
                {
                    // Send the request and parse the response.
                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound)
                        {
                            return true;
                        }
                        else
                        {
                            this.logger.LogWarning($"Subscription deletion failed for {subscription?.Id}. Status Code: {response.StatusCode}");
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a new subscription
        /// </summary>
        /// <param name="graphToken">Graph token to access subscription.</param>
        /// <param name="isSubscribingToEvents">Indicate whether subscribe to events or message</param>
        /// <returns>The SubscriptionViewModel with the new subscription</returns>
        [MonitorWith("GTACreateSubs")]
        private async Task<Subscription> CreateNewSubscription(string graphToken, bool isSubscribingToEvents = true)
        {
            var subscription = new Subscription
            {
                Resource = isSubscribingToEvents ? SchedulerConstants.EventResource : SchedulerConstants.MessageResource,
                ChangeType = isSubscribingToEvents ? "updated" : "created,updated",
                NotificationUrl = this.notificationUrl,
                ClientState = Guid.NewGuid().ToString(),
                ExpirationDateTime = DateTime.UtcNow + new TimeSpan(0, 0, 4230, 0)
            };

            return await this.CreateOrUpdateSubscription(graphToken, HttpMethod.Post, subscription);
        }

        /// <summary>
        /// Adds the requester to the meeting attendees. Used when the requester should be part of the meeting invite because free times are retrieved using one user account, but the meeting invites are sent with another service account.
        /// </summary>
        /// <param name="hiringManagerMeetingTimeResponse">Find meeting times response to add requester to.</param>
        /// <param name="principal">Principal information with email and object id.</param>
        private void AddRequesterToAttendees(FindMeetingTimeResponse hiringManagerMeetingTimeResponse, IHCMApplicationPrincipal principal)
        {
            foreach (var meetingTime in hiringManagerMeetingTimeResponse.MeetingTimeSuggestions)
            {
                if (meetingTime.AttendeeAvailability == null || hiringManagerMeetingTimeResponse.EmptySuggestionsReason == "attendeesUnavailableOrUnknown")
                {
                    meetingTime.AttendeeAvailability = new List<MeetingAttendeeAvailability>();
                }

                var meetingAttendeeAvailability = new MeetingAttendeeAvailability()
                {
                    Attendee = new MeetingAttendee()
                    {
                        Type = SchedulerConstants.AttendeeTypeRequired,
                        EmailAddress = new MeetingAttendeeEmailAddress()
                        {
                            Address = principal.EmailAddress,
                            ObjectId = principal.UserObjectId,
                        }
                    },
                    Availability = meetingTime.OrganizerAvailability
                };

                meetingTime.AttendeeAvailability.Add(meetingAttendeeAvailability);
            }

            // Add a placeholder with only attendee if there are no suggestions
            if (hiringManagerMeetingTimeResponse.MeetingTimeSuggestions.Count == 0)
            {
                hiringManagerMeetingTimeResponse.MeetingTimeSuggestions.Add(new MeetingTimeSuggestion()
                {
                    AttendeeAvailability = new List<MeetingAttendeeAvailability>()
                            {
                                new MeetingAttendeeAvailability()
                                {
                                    Attendee = new MeetingAttendee()
                                    {
                                        EmailAddress = new MeetingAttendeeEmailAddress()
                                        {
                                            Address = principal.EmailAddress,
                                            ObjectId = principal.UserObjectId
                                        }
                                    }
                                }
                            }
                });
            }
        }

        /// <summary>
        /// Creates an free time response assuming always busy when free time cannot be retrieved from graph
        /// </summary>
        /// <param name="findMeetingTimeRequest">Request to create free time from.</param>
        /// <returns>A free time response will no free time in timeslots.</returns>
        private FindMeetingTimeResponse CreateUnknownFreeTimeResponse(FindMeetingTimeRequest findMeetingTimeRequest)
        {
            var attendee = findMeetingTimeRequest.Attendees?[0];

            List<MeetingAttendeeAvailability> attendeeAvailability = null;

            if (attendee != null)
            {
                attendeeAvailability = new List<MeetingAttendeeAvailability>()
                {
                    new MeetingAttendeeAvailability()
                    {
                        Availability = "busy",
                        Attendee = attendee
                    }
                };
            }

            var freeTimeResponse = new FindMeetingTimeResponse()
            {
                EmptySuggestionsReason = "attendeesUnavailableOrUnknown",

                MeetingTimeSuggestions = new List<MeetingTimeSuggestion>()
                {
                    new MeetingTimeSuggestion()
                    {
                        Confidence = 100,

                        OrganizerAvailability = "free",

                        Locations = new List<MeetingLocation>(),

                        // This must be a null object for the client to work correctly
                        MeetingTimeSlot = null,

                        AttendeeAvailability = attendeeAvailability
                    }
                }
            };

            // Remove attendee availability when time is for requester because this is added later and is empty here
            if (findMeetingTimeRequest.IsOrganizerOptional == false)
            {
                freeTimeResponse.MeetingTimeSuggestions[0].AttendeeAvailability = null;
            }

            return freeTimeResponse;
        }

        /// <summary>
        /// Check - calendar event valid or not. Check for old event
        /// </summary>
        /// <param name="userAccessToken">User access token</param>
        /// <param name="calendarEventId">Calendar Event Id</param>
        /// <param name="serviceAccountName">Service account email</param>
        /// <returns>calendar event validation</returns>
        private async Task<bool> IsValidCalendarEvent(string userAccessToken, string calendarEventId, string serviceAccountName)
        {
            Contract.AssertValue(userAccessToken, nameof(userAccessToken));
            Contract.AssertValue(calendarEventId, nameof(calendarEventId));

            var calendarEvent = await this.GetCalendarEvent(userAccessToken, calendarEventId, serviceAccountName);

            if (calendarEvent == null || DateTime.Parse(calendarEvent.Start?.DateTime) < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Set default values on find meeting times request.
        /// </summary>
        /// <param name="findMeetingTimeRequest">Find meeting times request to default from.</param>
        /// <returns>Find meeting times request with defaulted values.</returns>
        private FindMeetingTimeRequest DefaultUnsetRequestFields(FindMeetingTimeRequest findMeetingTimeRequest)
        {
            findMeetingTimeRequest.LocationConstraint = new LocationConstraintModel()
            {
                IsRequired = false,
                SuggestLocation = false,
                Locations = null,
            };

            if (string.IsNullOrEmpty(findMeetingTimeRequest.MeetingDuration))
            {
                findMeetingTimeRequest.MeetingDuration = DefaultMeetingDuration;
            }

            if (findMeetingTimeRequest.MaxCandidates == 0)
            {
                findMeetingTimeRequest.MaxCandidates = DefaultMaxMeetingCandidates;
            }

            findMeetingTimeRequest.MinimumAttendeePercentage = DefaultMinumumAttendeePercentage;
            findMeetingTimeRequest.ReturnSuggestionReasons = DefaultReturnSuggestionReasons;

            return findMeetingTimeRequest;
        }

        /// <summary>
        /// Creates an authentication token for the graph resource from the user access token.
        /// </summary>
        /// <param name="userAccessToken">User access token.</param>
        /// <param name="serviceAccountName">Service account email</param>
        /// <param name="tokenCachingOptions">Token caching options.</param>
        /// <returns>Authentication token for the graph resource defined in the graph provider.</returns>
        [MonitorWith("GTAGraphToken")]
        private async Task<AuthenticationHeaderValue> GetBearerTokenFromUserToken(string userAccessToken, string serviceAccountName, TokenCachingOptions tokenCachingOptions = TokenCachingOptions.PreferCache)
        {
            this.logger.LogInformation($"GetBearerTokenFromUserToken for user started: {serviceAccountName}");
            if (userAccessToken != null && userAccessToken.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
            {
                userAccessToken = userAccessToken.Remove(0, 7);
            }

            var resourceToken = await this.GetResourceAccessTokenFromUserToken(userAccessToken, serviceAccountName);
            this.logger.LogInformation($"GetBearerTokenFromUserToken for user completed: {serviceAccountName}");

            return new AuthenticationHeaderValue("Bearer", resourceToken);
        }

        /// <summary>
        /// Get an authentication token for the graph resource from the user access token.
        /// </summary>
        /// <param name="userAccessToken">User access token.</param>
        /// <param name="userEmail">Service account email</param>
        /// <returns>Authentication token for the graph resource defined in the graph provider.</returns>
        private async Task<string> GetResourceAccessTokenFromUserToken(string userAccessToken, string userEmail)
        {
            this.logger.LogInformation($"GetResourceAccessTokenFromUserToken started for: {userEmail}");
            UserAssertion userAssertion = new UserAssertion(userAccessToken, "urn:ietf:params:oauth:grant-type:jwt-bearer");
            string clientId = this.configurationManager.Get<MsGraphSetting>().ClientId;
            string clientValue = this.configurationManager.Get<MsGraphSetting>().ClientCredential;
            var tenantId = this.configurationManager.Get<AADClientConfiguration>().TenantID;
            string authority = string.Format(CultureInfo.InvariantCulture, "https://login.windows.net/{0}", tenantId);

            AuthenticationContext authContext = new AuthenticationContext(
                authority,
                await this.tokenCacheService.GetCacheAsync($"Email:{userEmail}::ClientId:{clientId}").ConfigureAwait(false));

            this.logger.LogInformation($"Token received from cache for: {userEmail}");

            ClientCredential clientCredential = new ClientCredential(clientId, clientValue);
            if (clientCredential == null || userAssertion == null)
            {
                this.logger.LogInformation($"GetResourceAccessTokenFromUserToken completed for: {userEmail} and token is null.");
            }

            var result = await authContext.AcquireTokenAsync("https://graph.microsoft.com", clientCredential, userAssertion);
            this.logger.LogInformation($"GetResourceAccessTokenFromUserToken completed for: {userEmail}");
            return result?.AccessToken;
        }
    }
}
