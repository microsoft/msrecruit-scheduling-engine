//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Notification
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Resources;
    using HR.TA.CommonDataService.Common.Internal;
    using HR.TA.Common.Email;
    using HR.TA.ServicePlatform.Azure.Security;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.Common.Base.Utilities;
    using HR.TA.Common.Email.GraphContracts;
    using HR.TA.Common.Email.SendGridContracts;
    using HR.TA.ScheduleService.BusinessLibrary.Providers;
    using HR.TA.ScheduleService.BusinessLibrary.Utils;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ServicePlatform.Exceptions;
    using HR.TA.ScheduleService.BusinessLibrary.Strings;
    using HR.TA.ServicePlatform.AspNetCore.Mvc.Filters;
    using HR.TA.Talent.EnumSetModel.SchedulingService;
    using HR.TA.Common.Base.Security;
    using HR.TA.Common.Base.Configuration;
    using HR.TA.ScheduleService.BusinessLibrary.Configurations;
    using HR.TA.ServicePlatform.Context;
    using HR.TA.Common.Common.Common.Email.Contracts;
    using Newtonsoft.Json;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using HR.TA.Common.MSGraph;
    using Microsoft.Extensions.Logging;

    /// <summary>The Email Client implementation of notification interface.</summary>
    public class NotificationClient : INotificationClient
    {
        /// <summary>
        /// Http Client for Graph Provider.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>The Configuration Manager.</summary>
        private readonly IConfigurationManager configurationManager;

        private readonly AADClientConfiguration aadConfiguration;

        /// <summary>The email Service.</summary>
        private readonly IEmailService emailService;

        /// <summary>The graph provider.</summary>
        private readonly IMsGraphProvider graphProvider;

        private readonly ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient azureActiveDirectoryClient;

        private readonly NotificationServiceConfiguration notificationServiceConfiguration;

        private readonly ServiceBusClientConfiguration servicebusClientConfiguration;

        /// <summary>Secret manager class.</summary>
        private readonly ISecretManager secretManager;

        /// <summary>
        /// The instance for <see cref="ILogger{NotificationClient}"/>
        /// </summary>
        private readonly ILogger<NotificationClient> logger;

        private readonly ITokenCacheService tokenCacheService;

        /// <summary>Initializes a new instance of the <see cref="NotificationClient"/> class. </summary>
        /// <param name="configurationManager">The configuration Manager.</param>
        /// <param name="emailService">The email service</param>
        /// <param name="graphProvider">The graph provider</param>
        /// <param name="azureClient">azure client</param>
        /// <param name="secretManager">The secret manager class.</param>
        /// <param name="logger">The instance for <see cref="ILogger{NotificationClient}"/>.</param>
        /// <param name="tokenCacheService">the token service</param>/>
        /// <param name="httpClientFactory">http client instance</param>
        public NotificationClient(
            IConfigurationManager configurationManager,
            IEmailService emailService,
            IMsGraphProvider graphProvider,
            ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient azureClient,
            ISecretManager secretManager,
            ILogger<NotificationClient> logger,
            ITokenCacheService tokenCacheService,
            System.Net.Http.IHttpClientFactory httpClientFactory)
        {
            Contract.AssertValue(configurationManager, nameof(configurationManager));
            Contract.AssertValue(emailService, nameof(emailService));
            Contract.AssertValue(graphProvider, nameof(graphProvider));
            Contract.AssertValue(secretManager, nameof(secretManager));
            Contract.CheckValue(tokenCacheService, nameof(tokenCacheService));
            Contract.CheckValue(httpClientFactory, nameof(httpClientFactory));

            this.logger = logger;
            this.configurationManager = configurationManager;
            this.azureActiveDirectoryClient = azureClient;
            this.emailService = emailService;
            this.graphProvider = graphProvider;
            this.secretManager = secretManager;
            this.servicebusClientConfiguration = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<ServiceBusClientConfiguration>();
            this.aadConfiguration = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<AADClientConfiguration>();
            this.notificationServiceConfiguration = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<NotificationServiceConfiguration>();
            if (!string.IsNullOrEmpty(this.servicebusClientConfiguration?.KeyVaultUri))
            {
                this.secretManager = new SecretManager(
                    this.azureActiveDirectoryClient,
                    new KeyVaultConfiguration { KeyVaultUri = this.servicebusClientConfiguration.KeyVaultUri });
            }

            this.tokenCacheService = tokenCacheService;
            this.httpClient = httpClientFactory?.CreateClient();
        }

        /// <summary>Transform from email address with HCM domain .</summary>
        /// <param name="email">The email.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string TransformFromEmail(string email)
        {
            var fromEmailNamePortion = email?.Replace("@", "_at_");
            var fromEmail = $"{fromEmailNamePortion}@{this.emailService.DefaultDomainToSendFrom}";
            return fromEmail;
        }

        /// <summary>
        /// send email
        /// </summary>
        /// <param name="notificationItems">notification items</param>
        /// <param name="trackingId">trackingId</param>
        /// <returns>response</returns>
        [MonitorWith("GTASendEmail")]
        public async Task<bool> SendEmail(List<NotificationItem> notificationItems, string trackingId)
        {
            ResourceManager rm = new ResourceManager(typeof(ScheduleServiceEmailTemplate).Namespace + ".ScheduleServiceEmailTemplate", typeof(ScheduleServiceEmailTemplate).Assembly);
            var emailStyleTemplate = rm.GetString(BusinessConstants.EmailTemplateWithoutButton);

            notificationItems?.ForEach(notificationItem =>
            {
                var templateParams = new Dictionary<string, string>
                {
                    { "EmailHeaderUrl", BusinessConstants.MicrosoftLogoUrl },
                    { "EmailBodyContent", notificationItem.Body },
                    { "CompanyInfoFooter", BusinessConstants.MicrosoftInfoFooter },
                    { "Company_Name", "Microsoft" },
                    { "Privacy_Policy_Link", BusinessConstants.PrivacyPolicyUrl },
                    { "Terms_And_Conditions_Link", BusinessConstants.TermsAndConditionsUrl },
                };
                notificationItem.Body = this.ParseTemplate(emailStyleTemplate, templateParams);
                if (string.IsNullOrEmpty(notificationItem.TrackingId) && !string.IsNullOrEmpty(trackingId))
                {
                    notificationItem.TrackingId = trackingId;
                }
            });

            try
            {
                var serializedObject = JsonConvert.SerializeObject(notificationItems);
                var stringContent = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                string aadInstance = this.configurationManager.Get<AADClientConfiguration>().AADInstance;
                string tenant = this.configurationManager.Get<AADClientConfiguration>().TenantID;
                string authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

                string clientId = this.configurationManager.Get<AADClientConfiguration>().ClientId;
                var clientValue = await this.secretManager.TryGetSecretAsync(this.configurationManager.Get<AADClientConfiguration>().ClientCredential);

                var accessToken = await this.GetAccessToken(authority, clientId, clientId, clientValue?.Result?.Value);
                if (this.httpClient != null)
                {
                    this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var requestUrl = $"{this.notificationServiceConfiguration.EndpointUrl}/{string.Format(this.notificationServiceConfiguration.QueueNotificationUri, this.notificationServiceConfiguration.ApplicationName)}";

                    using (var httpResponseMessage = await this.httpClient.PostAsync(requestUrl, stringContent))
                    {
                        if (!httpResponseMessage.IsSuccessStatusCode)
                        {
                            this.logger.LogInformation(await httpResponseMessage.Content.ReadAsStringAsync());
                            return false;
                        }
                        else
                        {
                            this.logger.LogInformation("Mail sent successfully.");
                            return true;
                        }
                    }
                }
                else
                {
                    this.logger.LogError("SendEmail failed. Error: HTTP client connection is not established.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("SendEmail failed. Error: " + ex.Message + " and stack trace: " + ex.StackTrace);
                return false;
            }
        }

        private async Task<string> GetAccessToken(string authority, string resource, string clientId, string clientSecret)
        {
            AuthenticationContext authContext = new AuthenticationContext(
                authority,
                await this.tokenCacheService.GetCacheAsync($"ClientId:{clientId}").ConfigureAwait(false));
            var token = await authContext.AcquireTokenAsync(resource, new ClientCredential(clientId, clientSecret));
            return token.AccessToken;
        }

        /// <summary>Parses the subject and body templates.</summary>
        /// <param name="templateContent">The message Subject Template.</param>
        /// <param name="templateParams">The message Body Template.</param>
        /// <returns>returns parsed template</returns>
        private string ParseTemplate(string templateContent, Dictionary<string, string> templateParams)
        {
            foreach (var key in templateParams.Keys)
            {
                var value = templateParams[key] ?? string.Empty;
                templateContent = templateContent.Replace($"{{{key}}}", value, StringComparison.InvariantCultureIgnoreCase);
            }

            return templateContent;
        }
    }
}
