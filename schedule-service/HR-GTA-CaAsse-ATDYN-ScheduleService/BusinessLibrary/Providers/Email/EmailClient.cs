//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using HR.TA.Common.Base.Configuration;
    using HR.TA.Common.Email;
    using HR.TA.Common.MSGraph.Configuration;
    using HR.TA.Common.TestBase;
    using HR.TA.CommonDataService.Common.Internal;
    using HR.TA.ScheduleService.BusinessLibrary.Configurations;
    using HR.TA.ScheduleService.Utils;
    using HR.TA.ServicePlatform.AspNetCore.Mvc.Filters;
    using HR.TA.ServicePlatform.Azure.Security;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.ServicePlatform.Tracing;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Email client class
    /// </summary>
    public class EmailClient : IEmailClient
    {
        /// <summary>
        /// Configuration manager instance
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>The secret manager.</summary>
        private readonly ISecretManager secretManager;

        /// <summary>
        /// The access token class.
        /// </summary>
        private readonly IAccessTokenCache accessTokenCache;

        /// <summary>
        /// The instance for <see cref="ILogger{EmailClient}"/>
        /// </summary>
        private readonly ILogger<EmailClient> logger;

        /// <summary>Initializes a new instance of the <see cref="EmailClient"/> class. </summary>
        /// <param name="emailService">The email service</param>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="secretManager">The secret manager</param>
        /// <param name="accessTokenCache">Access Token provider</param>
        /// <param name="logger">The instance for <see cref="ILogger{EmailClient}"/>.</param>
        public EmailClient(
            IEmailService emailService,
            IConfigurationManager configurationManager,
            ISecretManager secretManager,
            IAccessTokenCache accessTokenCache,
            ILogger<EmailClient> logger)
        {
            Contract.CheckValue(secretManager, nameof(secretManager));
            this.logger = logger;
            this.secretManager = secretManager;
            this.configurationManager = configurationManager;

            this.accessTokenCache = accessTokenCache;
        }

        /// <summary>
        /// Acquire scheduler service authentication token
        /// </summary>
        /// <param name="userEmail">User Email</param>
        /// <returns>The scheduler service token</returns>
        public async Task<string> GetServiceAccountTokenByEmail(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
            {
               this.logger.LogInformation($"GetServiceAccountTokenByEmail: user email is empty");
                return null;
            }

            return await this.TryGetSystemServiceAccountTokenByEmail(userEmail);
        }

        /// <summary>
        /// Acquire scheduler service authentication token
        /// </summary>
        /// <param name="userEmail">User Email</param>
        /// <returns>The scheduler service token</returns>
        private async Task<string> TryGetSystemServiceAccountTokenByEmail(string userEmail)
        {
           this.logger.LogInformation("TryGetSystemServiceAccountTokenByEmail: Request to retrieve accountToken by email.");

            return await this.accessTokenCache.GetOrAddAccessTokenAsync(userEmail, async () =>
            {
                var schedulerConfiguration = this.configurationManager.Get<SchedulerConfiguration>();
                var authConfig = this.configurationManager.Get<BearerTokenAuthentication>();
                var schedulerEmailPasswords = await this.GetSystemServiceAccountCredentialsInternal(this.secretManager, schedulerConfiguration);
                var schedulerUser = schedulerEmailPasswords.Where(email => email.UserEmail == userEmail).FirstOrDefault();
                if (schedulerUser != null)
                {
                    return await this.GetSystemServiceAccountTokenInternal(authConfig, schedulerConfiguration.NativeClientAppId, schedulerUser.UserEmail, schedulerUser.PrimaryPassword, schedulerUser.SecondaryPassword);
                }
                this.logger.LogError($"TryGetSystemServiceAccountTokenByEmail: Couldn't Found system account details for userEmail {userEmail}");
                return null;
            });
        }

        /// <summary>
        /// Acquire scheduler service authentication token
        /// </summary>
        /// <param name="authConfig">Authentication configuration</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="userEmail">Scheduler account email</param>
        /// <param name="primaryPassword">Scheduler Primary Password</param>
        /// <param name="secondaryPassword">Scheduler seconday Password</param>
        /// <returns>The scheduler service token</returns>
        [MonitorWith("GTAGetToken")]
        private async Task<string> GetSystemServiceAccountTokenInternal(BearerTokenAuthentication authConfig, string clientId, string userEmail, string primaryPassword, string secondaryPassword = null)
        {
            Contract.CheckValue(clientId, nameof(clientId));
            Contract.CheckValue(userEmail, nameof(userEmail));
            Contract.CheckValue(primaryPassword, nameof(primaryPassword));
            Contract.CheckValue(authConfig, nameof(authConfig));

            var schedulerServiceToken = string.Empty;
            var retryAttempts = 5;

            try
            {
                for (int i = 0; i < retryAttempts; i++)
                {
                    // Attempt to get token from primary password
                    try
                    {
                        this.logger.LogInformation($"Attempt {i + 1} to get scheduler service token");

                        schedulerServiceToken = await this.HttpAuthenticationAsync(authConfig.Authority, this.configurationManager.Get<MsGraphSetting>().ClientId, userEmail, primaryPassword); // authContext.AcquireTokenAsync(new HttpCommunicationClientFactory(), authContextUri, userEmail, primaryPassword, resourceId, clientId);
                        if (!string.IsNullOrWhiteSpace(schedulerServiceToken))
                        {
                           this.logger.LogInformation("Successfully retrieved access token from azure active directory using primary password");
                            return schedulerServiceToken;
                        }

                        this.logger.LogWarning($"Attempt {i + 1}: unable to generate token with primary password");
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogWarning($"Attempt: {i + 1} : Failed to get scheduler service token using primary password with error: {ex.Message} and stack trace: {ex.StackTrace}");
                    }

                    try
                    {
                        // If error attempt to get token from secondary password
                        if (!string.IsNullOrWhiteSpace(secondaryPassword))
                        {
                            this.logger.LogInformation($"Trying to get token using secondary account");

                            schedulerServiceToken = await this.HttpAuthenticationAsync(authConfig.Authority, clientId, userEmail, primaryPassword); // authContext.AcquireTokenAsync(new HttpCommunicationClientFactory(), authContextUri, userEmail, secondaryPassword, resourceId, clientId);
                            if (!string.IsNullOrWhiteSpace(schedulerServiceToken))
                            {
                               this.logger.LogInformation("Successfully retrieved access token from azure active directory using secondary Password");
                                return schedulerServiceToken;
                            }

                            this.logger.LogWarning($"Attempt {i + 1}: unable to generate token with secondary password");
                        }
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogWarning($"Attempt: {i + 1}, Failed to get scheduler service token using secondary password with error : {ex.Message} and stack trace: {ex.StackTrace}");
                    }

                    if (i + 1 < retryAttempts)
                    {
                        await EmailUtils.ExponentialDelay(null, i);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            if (string.IsNullOrWhiteSpace(schedulerServiceToken))
            {
                throw new Exception($"Failed to get scheduler service token after {retryAttempts} attempts");
            }

            return schedulerServiceToken;
        }

        private async Task<string> HttpAuthenticationAsync(string authority, string clientId, string userEmail, string userPassword)
        {
            using (HttpClient client = new HttpClient())
            {
                var tokenEndpoint = $"{authority}/oauth2/token";
                var accept = "application/json";

                client.DefaultRequestHeaders.Add("Accept", accept);
                string postBody = $"resource={clientId}&client_id={clientId}&grant_type=password&username={userEmail}&password={userPassword}& scope=openid";

                using (var response = await client.PostAsync(tokenEndpoint, new StringContent(postBody, Encoding.UTF8, "application/x-www-form-urlencoded")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonresult = JObject.Parse(await response.Content.ReadAsStringAsync());
                        return (string)jsonresult["access_token"];
                    }
                    else
                    {
                        var jsonresult = JObject.Parse(await response.Content.ReadAsStringAsync());
                        this.logger.LogError($"Couldn't generate token for system account details for userEmail {userEmail}, error : {(string)jsonresult["error"]}");
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Get Scheduler Users And Passwords
        /// </summary>
        /// <param name="secretManager">The Secret Manager</param>
        /// <param name="schedulerConfiguration">Scheduler Configuration</param>
        /// <returns>User email password secret</returns>
        private async Task<IList<UserEmailPasswordSecret>> GetSystemServiceAccountCredentialsInternal(ISecretManager secretManager, SchedulerConfiguration schedulerConfiguration)
        {
            Contract.CheckValue(schedulerConfiguration, nameof(schedulerConfiguration));
            this.logger.LogInformation($"GetSystemServiceAccountCredentialsInternal started");

            var secretValue = await secretManager.ReadSecretAsync(schedulerConfiguration.SchedulerEmailPasswordSecret);

            this.logger.LogInformation($"GetSystemServiceAccountCredentialsInternal completed.");

            return JsonConvert.DeserializeObject<IList<UserEmailPasswordSecret>>(secretValue?.Value);
        }
    }
}
