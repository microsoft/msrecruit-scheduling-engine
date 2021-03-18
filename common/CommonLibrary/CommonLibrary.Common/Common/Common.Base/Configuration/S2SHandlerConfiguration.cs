//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace CommonLibrary.Common.Web.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Base.Configuration;
    using CommonDataService.Common.Internal;
    using CommonLibrary.ServicePlatform.Configuration;
    using S2SHandler.V2;

    /// <summary>The s 2 s handler configuration.</summary>
    [SettingsSection("S2SHandlerConfiguration")]
    public class S2SHandlerConfiguration
    {
        // PLEASE ALPHABETIZE
        public static Dictionary<ServiceUrls, string> ApplicationNameMap => new Dictionary<ServiceUrls, string>
        {
            { ServiceUrls.AdminService, "AdminService" },
            { ServiceUrls.EmailService, "EmailServiceApp" },
            { ServiceUrls.FlightingService, "FlightingServiceApp" },
            { ServiceUrls.GlobalService, "GlobalService" },
            { ServiceUrls.InsightsService, "InsightsServiceApp" },
            { ServiceUrls.IntegrationService, "IntegrationServiceApp" },
            { ServiceUrls.JobPostingService, "JobPostingServiceApp" },
            { ServiceUrls.LinkedInIntegrationService, "LinkedInIntegrationServiceApp" },
            { ServiceUrls.OfferManagementService, "OfferManagementServiceApp" },
            { ServiceUrls.OfferRuleService, "OfferRuleServiceApp" },
            { ServiceUrls.OnboardingService, "OnboardingServiceApp" },
            { ServiceUrls.ProvisioningService, "ProvisioningServiceApp" },
            { ServiceUrls.ReferralService, "ReferralServiceApp" },
            { ServiceUrls.SchedulingService, "SchedulingServiceApp" },
            { ServiceUrls.SyncService, "SyncServiceApp" },
            { ServiceUrls.TalentEngagementService, "TalentEngagementServiceApp" },
            { ServiceUrls.TaskService, "TaskService" },
            { ServiceUrls.TokenService, "TokenService" },
            { ServiceUrls.UserDirectoryService, "UserDirectoryServiceApp" },
            { ServiceUrls.WebNotificationService, "WebNotificationServiceApp" },
            { ServiceUrls.MetadataService, "MetadataService" },
        };

        // PLEASE ALPHABETIZE
        public static Dictionary<ServiceUrls, string> ServiceNameMap => new Dictionary<ServiceUrls, string>
        {
            { ServiceUrls.AdminService, "api" },
            { ServiceUrls.EmailService, "EmailService" },
            { ServiceUrls.FlightingService, "FlightingService" },
            { ServiceUrls.GlobalService, "GlobalService" },
            { ServiceUrls.InsightsService, "InsightsService" },
            { ServiceUrls.IntegrationService, "IntegrationService" },
            { ServiceUrls.JobPostingService, "JobPostingService" },
            { ServiceUrls.LinkedInIntegrationService, "LinkedInIntegrationService" },
            { ServiceUrls.OfferManagementService, "OfferManagementService" },
            { ServiceUrls.OfferRuleService, "OfferRuleService" },
            { ServiceUrls.OnboardingService, "OnboardingService" },
            { ServiceUrls.ProvisioningService, "ProvisioningService" },
            { ServiceUrls.ReferralService, "ReferralService" },
            { ServiceUrls.SchedulingService, "SchedulingService" },
            { ServiceUrls.SyncService, "SyncService" },
            { ServiceUrls.TalentEngagementService, "TalentEngagementService" },
            { ServiceUrls.TaskService, "api" },
            { ServiceUrls.TokenService, "api" },
            { ServiceUrls.UserDirectoryService, "UserDirectoryService" },
            { ServiceUrls.WebNotificationService, "WebNotificationService" },
            { ServiceUrls.MetadataService, "api" },
        };

        private Dictionary<string, string> applicationNameOverridesParsed;

        /// <summary>Gets or sets the cluster url.</summary>
        public string ClusterUrl { get; set; }

        /// <summary>Gets or sets the application name overrides.</summary>
        public string ApplicationNameOverrides { get; set; }

        /// <summary>Gets or sets whether the service is running as a console application.</summary>
        public bool TargetConsoleApplication { get; set; }

        public string GetApplicationName(ServiceUrls serviceUrl)
        {
            if (!ApplicationNameMap.ContainsKey(serviceUrl))
            {
                throw new Exception($"The key {serviceUrl} does not exist in the application name map.");
            }

            var applicationName = ApplicationNameMap[serviceUrl];

            return this.GetApplicationName(applicationName);
        }

        public string GetApplicationName(string applicationName)
        {
            Contract.CheckNonEmpty(applicationName, nameof(applicationName), "Application name cannot be empty");

            this.ParseApplicationNameOverrides();

            if (this.applicationNameOverridesParsed != null && this.applicationNameOverridesParsed.ContainsKey(applicationName))
            {
                applicationName = this.applicationNameOverridesParsed[applicationName];
            }

            return applicationName;
        }

        public string GetServiceName(ServiceUrls serviceUrl)
        {
            if (!ServiceNameMap.ContainsKey(serviceUrl))
            {
                throw new Exception($"The key {serviceUrl} does not exist in the service name map.");
            }

            return ServiceNameMap[serviceUrl];
        }

        public string GetFabricUrl(ServiceUrls serviceUrl, string serviceName = null)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                serviceName = this.GetServiceName(serviceUrl);
            }

            var applicationName = this.GetApplicationName(serviceUrl);

            return this.GetFabricUrl(applicationName, serviceName);
        }

        public string GetFabricUrl(string applicationName, string serviceName)
        {
            Contract.CheckValue(applicationName, nameof(applicationName), "Application name cannot be empty");
            Contract.CheckValue(serviceName, nameof(serviceName), "Service name cannot be empty");

            return $"fabric:/{this.GetApplicationName(applicationName)}/{serviceName}";
        }

        public string GetFullServiceUrl(ServiceUrls serviceUrl, string serviceName = null, string environmentName = null)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                serviceName = this.GetServiceName(serviceUrl);
            }

            return this.GetFullServiceUrl(this.GetApplicationName(serviceUrl), serviceName, environmentName);
        }

        public string GetFullServiceUrl(string applicationName, string serviceName, string environmentName = null)
        {
            Contract.CheckValue(applicationName, nameof(applicationName), "Application name cannot be empty");
            Contract.CheckValue(serviceName, nameof(serviceName), "Service name cannot be empty");

            return $"{this.GetBaseUrl(environmentName)}/{applicationName}/{serviceName}";
        }

        public string GetBaseUrl(string environmentName = null)
        {
            if (!string.IsNullOrEmpty(this.ClusterUrl))
            {
                return this.ClusterUrl;
            }

            // Default to dev
            if (string.IsNullOrEmpty(environmentName))
            {
                environmentName = Constants.DevEnvironmentName;
            }

            environmentName = environmentName.ToUpper();

            switch (environmentName)
            {
                case Constants.LocalFabricEnvironmentName:
                    return @"http://localhost:19081";
                case Constants.DevEnvironmentName:
                    return @"https://d365-atm-hcm-dev.rsu.int.powerapps.com";
                case Constants.IntEnvironmentName:
                    return @"https://d365-atm-hcm-int.rsu.int.powerapps.com";
                default:
                    throw new Exception($"No base url defined for environment {environmentName}");
            }
        }

        // Set the S2S handler overrides so that the runner knows to call the right app.
        // i.e if the fullname is 123456-ProvisioningServiceApp then we should set the config to be = ";ProvisioningServiceApp=123456-ProvisioningServiceApp
        public void SetApplicationNameOverrides(EnvironmentConfiguration environmentConfiguration)
        {
            Contract.CheckValue(environmentConfiguration, nameof(environmentConfiguration));
            
            if (!string.IsNullOrEmpty(environmentConfiguration?.FullName))
            {
                var split = environmentConfiguration.FullName.Split('-');

                if (split.Length >= 2)
                {
                    this.ApplicationNameOverrides = this.ApplicationNameOverrides + ";" + $"{split.Last()}={environmentConfiguration.FullName}";
                }
            }
        }

        private void ParseApplicationNameOverrides()
        {
            if (this.applicationNameOverridesParsed == null)
            {
                this.applicationNameOverridesParsed = this
                    .ApplicationNameOverrides
                    ?.Split(';')
                    ?.Where(s => s != string.Empty)
                    .ToDictionary(
                        pair => pair.Split('=')[0],
                        pair => pair.Split('=')[1]);
            }
        }
    }
}
