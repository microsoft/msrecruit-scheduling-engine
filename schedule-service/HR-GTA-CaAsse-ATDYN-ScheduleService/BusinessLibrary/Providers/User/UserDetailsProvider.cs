//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.BusinessLibrary.Providers.User
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using MS.GTA.Common.Base.Configuration;
    using MS.GTA.Common.MSGraph;
    using MS.GTA.Common.MSGraph.Configuration;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.ScheduleService.BusinessLibrary.Configurations;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Exceptions;
    using Newtonsoft.Json;

    /// <summary>
    /// User details provider implmentation
    /// </summary>
    public class UserDetailsProvider : IUserDetailsProvider
    {
        /// <summary>
        /// SearchUserApi
        /// </summary>
        private const string SearchUserApiByMail = "/users?$filter=mail eq '{0}' or userPrincipalName eq '{0}'";

        /// <summary>
        /// Search user api
        /// </summary>
        private const string SearchUserApi = "/users/{0}";

        /// <summary>
        /// User photo api
        /// </summary>
        private const string UserPhotoApi = "/users/{0}/photo/$value";

        /// <summary>
        /// Base graph url for making API calls.
        /// </summary>
        private readonly string graphBaseUrl;

        /// <summary>
        /// Gets or sets the Metric Logger
        /// </summary>
        private readonly ILogger<UserDetailsProvider> logger;

        /// <summary>
        /// Gets or sets email client Logger
        /// </summary>
        private readonly IEmailClient emailClient;

        /// <summary>
        /// Configuration manager instance
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        private readonly HttpClient httpClient;

        private readonly string serviceAccountName;

        private readonly ITokenCacheService tokenCacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetailsProvider"/> class
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="clientFactory">client Factory</param>
        /// <param name="emailClient">The email client class.</param>
        /// <param name="tokenCacheService">Token cache service</param>
        /// <param name="logger">The logger.</param>
        public UserDetailsProvider(
            IConfigurationManager configurationManager,
            System.Net.Http.IHttpClientFactory clientFactory,
            IEmailClient emailClient,
            ITokenCacheService tokenCacheService,
            ILogger<UserDetailsProvider> logger)
        {
            this.graphBaseUrl = configurationManager.Get<MsGraphSetting>()?.GraphResourceId + "/v1.0/";
            this.serviceAccountName = configurationManager.Get<SchedulerConfiguration>()?.UserProfileEmailAddress;
            this.configurationManager = configurationManager;
            this.emailClient = emailClient;
            this.logger = logger;
            this.tokenCacheService = tokenCacheService;
            this.httpClient = clientFactory?.CreateClient();
        }

        /// <summary>
        /// GetUserPhoto
        /// </summary>
        /// <param name="userObjectId">userObjectId</param>
        /// <returns>photo</returns>
        public async Task<string> GetUserPhotoAsync(string userObjectId)
        {
            if (string.IsNullOrWhiteSpace(userObjectId))
            {
                throw new InvalidRequestDataValidationException("GetUserPhotoAsync: Invalid input parameter. userObjectId must not be null.").EnsureTraced();
            }

            var accessToken = await this.emailClient.GetServiceAccountTokenByEmail(this.serviceAccountName);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                this.logger.LogWarning($"GetUserPhotoAsync: AccessToken is null.");
                return null;
            }

            string result = string.Empty;
            await this.logger.ExecuteAsync(
                "GTAGetUserPhotoAsync",
                async () =>
                {
                    var relativePath = string.Format(UserPhotoApi, userObjectId);

                    // Force refreshing cache so that the cached resource token from the user principle isn't returned
                    var schedulerGraphToken = await this.GetBearerTokenFromUserToken(accessToken, this.serviceAccountName, TokenCachingOptions.ForceRefreshCache);

                    if (this.httpClient != null)
                    {
                        using (var message = new HttpRequestMessage(HttpMethod.Get, $"{this.graphBaseUrl}{relativePath}"))
                        {
                            this.httpClient.DefaultRequestHeaders.Authorization = schedulerGraphToken;

                            message.Headers.Authorization = schedulerGraphToken;

                            using (var httpResponse = await this.httpClient.SendAsync(message))
                            {
                                this.logger.LogInformation($"GetUserPhotoAsync response headers: {httpResponse.Headers}");

                                if (httpResponse.IsSuccessStatusCode)
                                {
                                    Stream photo = await httpResponse.Content.ReadAsStreamAsync();
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        photo.CopyTo(ms);
                                        byte[] buffer = ms.ToArray();
                                        result = Convert.ToBase64String(buffer);
                                    }
                                }
                                else
                                {
                                    this.logger.LogError($"GetUserPhotoAsync(): Failed to retrive user details. Status Code: {httpResponse.StatusCode}");
                                }
                            }
                        }
                    }
                });

            return result;
        }

        /// <summary>
        /// GetUser
        /// </summary>
        /// <param name="userObjectId">userObjectId</param>
        /// <returns>user</returns>
        public async Task<Microsoft.Graph.User> GetUserAsync(string userObjectId)
        {
            if (string.IsNullOrWhiteSpace(userObjectId))
            {
                throw new InvalidRequestDataValidationException("GetUserAsync: Invalid input parameter. userObjectId must not be null.").EnsureTraced();
            }

            var accessToken = await this.emailClient.GetServiceAccountTokenByEmail(this.serviceAccountName);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                this.logger.LogWarning($"GetUserAsync: AccessToken is null.");

                return null;
            }

            Microsoft.Graph.User graphResponse = new Microsoft.Graph.User();
            await this.logger.ExecuteAsync(
                "GTAGetUserAsync",
                async () =>
                {
                    var relativePath = string.Format(SearchUserApi, userObjectId);

                    // Force refreshing cache so that the cached resource token from the user principle isn't returned
                    var schedulerGraphToken = await this.GetBearerTokenFromUserToken(accessToken, this.serviceAccountName, TokenCachingOptions.ForceRefreshCache);

                    if (this.httpClient != null)
                    {
                        using (var message = new HttpRequestMessage(HttpMethod.Get, $"{this.graphBaseUrl}{relativePath}"))
                        {
                            this.httpClient.DefaultRequestHeaders.Authorization = schedulerGraphToken;

                            message.Headers.Authorization = schedulerGraphToken;

                            using (var httpResponse = await this.httpClient.SendAsync(message))
                            {
                                this.logger.LogInformation($"GetUserAsync response headers: {httpResponse.Headers}");
                                this.logger.LogInformation($"GetUserAsync call made with account {this.serviceAccountName}");

                                if (httpResponse.IsSuccessStatusCode)
                                {
                                    string stringResult = await httpResponse.Content.ReadAsStringAsync();
                                    graphResponse = JsonConvert.DeserializeObject<Microsoft.Graph.User>(stringResult);
                                }
                                else
                                {
                                    this.logger.LogError($"GetUserAsync(): Failed to retrive user details. Status Code: {httpResponse.StatusCode}");
                                }
                            }
                        }
                    }
                });

            return graphResponse;
        }

        /// <summary>
        /// SearchUser ByEmail
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="serviceAccountName">serviceAccountName</param>
        /// <returns>graph response</returns>
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
                    var relativePath = string.Format(SearchUserApiByMail, email);

                    // Force refreshing cache so that the cached resource token from the user principle isn't returned
                    var schedulerGraphToken = await this.GetBearerTokenFromUserToken(accessToken, serviceAccountName, TokenCachingOptions.ForceRefreshCache);

                    if (this.httpClient != null)
                    {
                        using (var message = new HttpRequestMessage(HttpMethod.Get, $"{this.graphBaseUrl}{relativePath}"))
                        {
                            this.httpClient.DefaultRequestHeaders.Authorization = schedulerGraphToken;

                            message.Headers.Authorization = schedulerGraphToken;

                            using (var httpResponse = await this.httpClient.SendAsync(message))
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
                    }
                });

            return graphResponse;
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
