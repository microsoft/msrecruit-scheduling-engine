//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.BusinessLibrary.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using HR.TA.Common.MSGraph.Configuration;
    using HR.TA.CommonDataService.Common.Internal;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using HR.TA.Common.Base.Configuration;
    using HR.TA.Common.MSGraph;
    using HR.TA.ScheduleService.BusinessLibrary.Configurations;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Utils;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.ServicePlatform.Context;
    using Newtonsoft.Json;
    using HR.TA.ServicePlatform.Exceptions;
    using HR.TA.ScheduleService.Contracts;

    /// <summary>
    /// Class to work on room resources
    /// </summary>
    public class RoomResourceProvider : IRoomResourceProvider
    {
        /// <summary>
        /// Base graph url for making API calls.
        /// </summary>
        private readonly string graphBaseUrl;

        /// <summary>
        /// Configuration manager instance
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// JSON serializer settings for http calls.
        /// </summary>
        private readonly JsonSerializerSettings jsonSerializerSettings;

        /// <summary>
        /// Gets or sets the Metric Logger
        /// </summary>
        private readonly ILogger<RoomResourceProvider> logger;

        /// <summary>
        /// Gets or sets email client Logger
        /// </summary>
        private readonly IEmailClient emailClient;

        private readonly ITokenCacheService tokenCacheService;

        /// <summary>
        /// Http client for room resource provider.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomResourceProvider"/> class
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="emailClient">The email client class.</param>
        /// <param name="tokenCacheService">Instance of token cache service</param>
        /// <param name="logger">The logger.</param>
        /// <param name="httpClientFactory">http client instance</param>
        public RoomResourceProvider(
            IConfigurationManager configurationManager,
            IEmailClient emailClient,
            ITokenCacheService tokenCacheService,
            ILogger<RoomResourceProvider> logger,
            System.Net.Http.IHttpClientFactory httpClientFactory)
        {
            Contract.CheckValue(tokenCacheService, nameof(tokenCacheService));
            Contract.CheckValue(httpClientFactory, nameof(httpClientFactory));

            this.graphBaseUrl = configurationManager.Get<MsGraphSetting>().GraphResourceId + "/beta/";
            this.configurationManager = configurationManager;
            this.emailClient = emailClient;
            this.logger = logger;
            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };
            this.tokenCacheService = tokenCacheService;
            this.httpClient = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Gets the room list collection
        /// </summary>
        /// <returns>A list of rooms</returns>
        public async Task<List<Room>> GetRoomLists()
        {
            var rooms = new List<Room>();
            await this.logger.ExecuteAsync(
            "HcmSSGetRmLst",
            async () =>
            {
                var relativePath = $"/me/findRoomLists";
                var url = $"{this.graphBaseUrl}{relativePath}";
                var serviceAccountName = this.configurationManager.Get<SchedulerConfiguration>().UserProfileEmailAddress;
                var accessToken = await this.emailClient.GetServiceAccountTokenByEmail(serviceAccountName);
                rooms = await this.GetRoomsInternal(url, accessToken, serviceAccountName);
            });

            return rooms;
        }

        /// <summary>
        /// Gets rooms collection
        /// </summary>
        /// <param name="buildingName">Building name</param>
        /// <returns>A list of rooms.</returns>
        public async Task<List<Room>> GetRooms(string buildingName)
        {
            Contract.CheckValue(buildingName, nameof(buildingName));

            var rooms = new List<Room>();
            await this.logger.ExecuteAsync(
            "HcmSSGetRms",
            async () =>
            {
                var relativePath = $"/me/findRooms(roomList=\'{buildingName}\')";
                var url = $"{this.graphBaseUrl}{relativePath}";
                var serviceAccountName = this.configurationManager.Get<SchedulerConfiguration>().UserProfileEmailAddress;
                var accessToken = await this.emailClient.GetServiceAccountTokenByEmail(serviceAccountName);
                rooms = await this.GetRoomsInternal(url, accessToken, serviceAccountName);
            });

            return rooms;
        }

        /// <summary>
        /// Get rooms internal method
        /// </summary>
        /// <param name="url">The graph url.</param>
        /// <param name="userAccessToken">User access token.</param>
        /// <param name="serviceAccountName">Service account email</param>
        /// <returns>List of rooms.</returns>
        private async Task<List<Room>> GetRoomsInternal(string url, string userAccessToken, string serviceAccountName)
        {
            var schedulerGraphToken = await this.GetBearerTokenFromUserToken(userAccessToken, serviceAccountName, TokenCachingOptions.ForceRefreshCache);
            var exceptions = new List<Exception>();

            try
            {
                this.httpClient.DefaultRequestHeaders.Authorization = schedulerGraphToken;

                for (int i = 1; i <= HR.TA.Common.Email.Constants.MaxRetryCount; i++)
                {
                    var message = new HttpRequestMessage(HttpMethod.Get, url);
                    message.Headers.Authorization = schedulerGraphToken;

                    using (var httpResponse = await this.httpClient.SendAsync(message))
                    {
                        if (httpResponse?.Content != null)
                        {
                            var content = await httpResponse.Content?.ReadAsStringAsync();

                            if (!httpResponse.IsSuccessStatusCode)
                            {
                                var exception = new GraphException(HttpMethod.Get.Method, url, httpResponse.StatusCode, content);

                                if (EmailUtils.ShouldRetryOnGraphException(httpResponse.StatusCode))
                                {
                                    this.logger.LogWarning($"Retry # - {i}. Get rooms call for GetRoomsEvent failed with StatusCode: {httpResponse.StatusCode}, error: {content}");
                                    exceptions.Add(exception);
                                    await EmailUtils.ExponentialDelay(httpResponse, i);
                                }
                                else if (httpResponse.StatusCode == HttpStatusCode.Unauthorized || httpResponse.StatusCode == HttpStatusCode.Forbidden)
                                {
                                    this.logger.LogError($"Get rooms call with ${url} failed with status {httpResponse.StatusCode}  with error message {content}");
                                    throw new GetRoomsUnAuthorizedOrForbiddenException("The user is not authorized to make get rooms request").EnsureLogged(this.logger);
                                }
                                else if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                                {
                                    this.logger.LogWarning($"Get room call failed with meeting room not found. {content}");
                                    return new List<Room>();
                                }
                                else
                                {
                                    this.logger.LogError($"Get rooms call with ${url} failed with status {httpResponse.StatusCode}  with error message {content}");
                                    throw exception.EnsureLogged(this.logger);
                                }
                            }
                            else
                            {
                                var eventResponse = JsonConvert.DeserializeObject<RoomResponse>(content, this.jsonSerializerSettings);
                                var rooms = eventResponse.Rooms;
                                return rooms;
                            }
                        }
                    }
                }

                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
                else
                {
                    throw new SchedulerGetRoomsException("Issue while trying to get rooms");
                }
            }
            catch (GetRoomsUnAuthorizedOrForbiddenException e)
            {
                this.logger.LogError($"GetRoomsInternal failed with error  stackTrace {e.StackTrace} InnerException: {e.InnerException}");
                throw new GetRoomsUnAuthorizedOrForbiddenException($"The user is not authorized to make get rooms request: {e.Message}").EnsureLogged(this.logger);
            }
            catch (Exception e)
            {
                this.logger.LogError($"GetRoomsInternal failed with error {e.Message} , stackTrace {e.StackTrace} , InnerException: {e.InnerException}");
                throw new SchedulerGetRoomsException($"Issue while trying to get rooms : {e.Message}").EnsureLogged(this.logger);
            }
        }

        /// <summary>
        /// Creates an authentication token for the graph resource from the user access token.
        /// </summary>
        /// <param name="userAccessToken">User access token.</param>
        /// <param name="serviceAccountName">Service account email</param>
        /// <param name="tokenCachingOptions">Token caching options.</param>
        /// <returns>Authentication token for the graph resource defined in the graph provider.</returns>
        private async Task<AuthenticationHeaderValue> GetBearerTokenFromUserToken(string userAccessToken, string serviceAccountName, TokenCachingOptions tokenCachingOptions = TokenCachingOptions.PreferCache)
        {
            if (userAccessToken.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
            {
                userAccessToken = userAccessToken.Remove(0, 7);
            }

            var resourceToken = await this.GetResourceAccessTokenFromUserToken(userAccessToken, serviceAccountName);

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
            UserAssertion userAssertion = new UserAssertion(userAccessToken, "urn:ietf:params:oauth:grant-type:jwt-bearer");
            string aadInstance = this.configurationManager.Get<AADClientConfiguration>().AADInstance;
            string tenant = this.configurationManager.Get<AADClientConfiguration>().TenantID;
            string authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

            var clientValue = this.configurationManager.Get<MsGraphSetting>().ClientCredential;
            var clientId = this.configurationManager.Get<MsGraphSetting>().ClientId;
            ClientCredential clientCredential = new ClientCredential(clientId, clientValue);

            AuthenticationContext authContext = new AuthenticationContext(
                authority,
                await this.tokenCacheService.GetCacheAsync($"Email:{userEmail}::ClientId:{clientId}").ConfigureAwait(false));

            var result = await authContext.AcquireTokenAsync("https://graph.microsoft.com", clientCredential, userAssertion);
            return result.AccessToken;
        }
    }
}
