//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="SettingsManager.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------


namespace MS.GTA.Common.Data.DataAccess
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Base.Exceptions;
    using MS.GTA.Common.Base.KeyVault;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.ServicePlatform.Azure.Security;
    using MS.GTA.ServicePlatform.Tracing;
    
    public class EsignSettingsManager
    {
        /// <summary>The secret manager provider</summary>
        private readonly ISecretManager secretManager;

        /// <summary>The logger</summary>
        private readonly ILogger<SecretManagerProvider> logger;

        /// <summary> The trace source</summary>
        private readonly ITraceSource traceSource;

        /// <summary>
        /// The environment setting helper
        /// </summary>
        private readonly IEnvironmentSettingHelper environmentSettingHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EsignSettingsManager" /> class.
        /// </summary>
        /// <param name="secretManager">The secret manager.</param>
        /// <param name="environmentSettingHelper">The environment setting helper.</param>
        /// <param name="traceSource">The trace source.</param>
        public EsignSettingsManager(
            ISecretManager secretManager,
            IEnvironmentSettingHelper environmentSettingHelper,
            ITraceSource traceSource)
        {
            this.secretManager = secretManager;
            this.environmentSettingHelper = environmentSettingHelper;
            this.traceSource = traceSource;
        }

        #region Public Methods
        /// <summary>Gets the e sign user status.</summary>
        /// <param name="esignType">Type of the esign.</param>
        /// <param name="keyVaultUri">The key vault URI.</param>
        /// <param name="clientSecretName">Name of the client secret.</param>
        /// <param name="integrationSecretName">Name of the integration secret.</param>
        /// <returns>ESign User Status</returns>
        public async Task<ESignUserStatus> GetESignUserStatus(ESignType esignType, string keyVaultUri, string clientSecretName, string integrationSecretName)
        {
            var esignAccount = await this.GetESignAccountConfiguration(esignType, keyVaultUri, clientSecretName, integrationSecretName);

            if (esignAccount == null || esignAccount.IntegrationKey == null)
            {
                this.logger.LogError("The default ESign configuration is not set up.");

                throw new Exception("The default ESign configuration is not set up.");
            }

            var esignUserStatus = new ESignUserStatus()
            {
                IntegrationKey = esignAccount.IntegrationKey,
                IsEsignEnabled = false
            };

            return esignUserStatus;
        }

        /// <summary>
        /// Gets the enabled e sign settings.
        /// </summary>
        /// <returns>ESign Settings</returns>
        public async Task<ESignSettings> GetEnabledESignSettings()
        {
            var environmentSetting = await this.environmentSettingHelper.GetEnvironmentSettings();

            if (environmentSetting != null && environmentSetting.ESignSettings != null)
            {
                return environmentSetting.ESignSettings;
            }

            this.traceSource.TraceInformation($"Sending default esign settings");

            var esignSettings = new ESignSettings()
            {
                EnabledESignType = new ESignType(),
            };

            return esignSettings;
        }
        #endregion

        #region Private Methods
        /// <summary>Gets the e sign account configuration.</summary>
        /// <param name="esignType">Type of the esign.</param>
        /// <param name="keyVaultUri">The key vault URI.</param>
        /// <param name="clientSecretName">Name of the client secret.</param>
        /// <param name="integrationSecretName">Name of the integration secret.</param>
        /// <returns>ESign Account</returns>
        private async Task<ESignAccount> GetESignAccountConfiguration(ESignType esignType, string keyVaultUri, string clientSecretName, string integrationSecretName)
        {
            var secretKey = await this.secretManager.GetSecretAsync(clientSecretName, this.logger);

            if (secretKey == null)
            {
                throw new BenignException($"Client secret key not found for ESigntype : {esignType}");
            }

            var integrationKey = await this.secretManager.GetSecretAsync(integrationSecretName, this.logger);

            if (integrationKey == null)
            {
                throw new BenignException($"Client integration secret key not found for ESigntype : {esignType}");
            }

            var esignAccount = new ESignAccount()
            {
                Id = $"{ESignAccount.DocumentType}-{esignType}",
                ESignType = esignType,
                IntegrationKey = integrationKey,
                SecretKey = secretKey
            };
            return esignAccount;

        }
        #endregion
    }
}
