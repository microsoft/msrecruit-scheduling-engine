//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="GlobalServiceEnvironmentSettings.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.BapClient.Constants
{
    using CommonDataService.Common.Internal;
    using Configuration;
    using MS.GTA.Common.Base;
    using Exceptions;
    using ServicePlatform.Exceptions;

    /// <summary> BAP service settings and constants </summary>
    public static class BapEnvironmentSettings
    {
        /// <summary>
        /// Gets the global service environment configuration based on the environment
        /// </summary>
        /// <param name="environmentName">The environment to use</param>
        /// <returns>The <see cref="BapClientConfiguration"/>.</returns>
        public static BapClientConfiguration GetBAPClientConfiguration(string environmentName)
        {
            Contract.CheckNonEmpty(environmentName, nameof(environmentName), "environmentName has to be defined");

            var upperCasedEnvironmentName = environmentName.ToUpper();
            switch (upperCasedEnvironmentName)
            {
                case Constants.DevEnvironmentName:
                    return GetDevBAPClientConfiguration();
                case Constants.IntEnvironmentName:
                    return GetIntBAPClientConfiguration();
                case Constants.ProdEnvironmentName:
                    return GetProdBAPClientConfiguration();
                default:
                    throw new BapClientInvalidOperationException($"BapEnvironmentSettings.GetBAPClientConfiguration: Unknown environmentName - {environmentName}").EnsureTraced();
            }
        }

        /// <summary>
        /// Gets the DEV environment BAP client configuration
        /// </summary>
        /// <returns>The <see cref="BapClientConfiguration"/></returns>
        private static BapClientConfiguration GetDevBAPClientConfiguration()
        {
            return new BapClientConfiguration()
            {
                BapResourceId = "https://management.azure.com/",
                BapRelativeUrl = "providers/Microsoft.BusinessAppPlatform/scopes/service/environments?api-version=2016-11-01-alpha",
                BapApiVersion = "2016-11-01-alpha",
                BapXRMApiVersion = "2018-01-01-alpha",
                BapServicePrincipalClientId = "66832c40-f4dc-426a-8b91-d70cb1e587d0",
                BapAADTenantId = Constants.MicrosoftTenantId,
                BapAreaId = "TalentEngagement",
                BapEnvironmentPrefix = "TalentEngagement Dev",
                BapBaseUrl = "https://tip1.api.bap.microsoft.com"
            };
        }

        /// <summary>
        /// Gets the INT environment BAP client configuration
        /// </summary>
        /// <returns>The <see cref="BapClientConfiguration"/></returns>
        private static BapClientConfiguration GetIntBAPClientConfiguration()
        {
            return new BapClientConfiguration()
            {
                BapResourceId = "https://management.azure.com/",
                BapRelativeUrl = "providers/Microsoft.BusinessAppPlatform/scopes/service/environments?api-version=2016-11-01-alpha",
                BapApiVersion = "2016-11-01-alpha",
                BapXRMApiVersion = "2018-01-01-alpha",
                BapServicePrincipalClientId = "66832c40-f4dc-426a-8b91-d70cb1e587d0",
                BapAADTenantId = Constants.MicrosoftTenantId,
                BapAreaId = "TalentEngagement",
                BapEnvironmentPrefix = "TalentEngagement INT",
                BapBaseUrl = "https://tip1.api.bap.microsoft.com"
            };
        }

        /// <summary>
        /// Gets the PROD environment BAP client configuration
        /// </summary>
        /// <returns>The <see cref="BapClientConfiguration"/></returns>
        private static BapClientConfiguration GetProdBAPClientConfiguration()
        {
            return new BapClientConfiguration()
            {
                BapResourceId = "https://management.azure.com/",
                BapRelativeUrl = "providers/Microsoft.BusinessAppPlatform/scopes/service/environments?api-version=2016-11-01",
                BapApiVersion = "2016-11-01",
                BapXRMApiVersion = "2018-01-01-alpha",
                BapServicePrincipalClientId = "5712d3fd-8e22-4040-afbf-70fa18f63627",
                BapAADTenantId = Constants.MicrosoftTenantId,
                BapAreaId = "TalentEngagement",
                BapEnvironmentPrefix = "TalentEngagement",
                BapBaseUrl = "https://api.bap.microsoft.com"
            };
        }
    }
}
