﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Data.DataAccess
{
    using System.Threading.Tasks;
    using HR.TA.Common.TalentAttract.Contract;

    /// <summary>
    /// Settings Manager Interface
    /// </summary>
    public interface IEsignSettingsManager
    {
        /// <summary>Gets the e sign user status.</summary>
        /// <param name="esignType">Type of the esign.</param>
        /// <param name="keyVaultUri">The key vault URI.</param>
        /// <param name="clientSecretName">Name of the client secret.</param>
        /// <param name="integrationSecretName">Name of the integration secret.</param>
        /// <returns>ESign User Status</returns>
        Task<ESignUserStatus> GetESignUserStatus(ESignType esignType, string keyVaultUri, string clientSecretName, string integrationSecretName);

        /// <summary>
        /// Gets the enabled e sign settings.
        /// </summary>
        /// <returns>Esign Settings</returns>
        Task<ESignSettings> GetEnabledESignSettings();
    }
}
