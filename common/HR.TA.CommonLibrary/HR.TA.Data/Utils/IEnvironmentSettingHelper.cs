//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Data
{
    using System.Threading.Tasks;
    using HR.TA.Common.DocumentDB;
    using HR.TA.Common.TalentAttract.Contract;

    public interface IEnvironmentSettingHelper
    {
        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <returns></returns>
        Task<IHcmDocumentClient> GetClient();

        /// <summary>
        /// Gets the environment settings.
        /// </summary>
        /// <returns></returns>
        Task<EnvironmentSettings> GetEnvironmentSettings();

        /// <summary>
        /// Gets the user environment settings document identifier.
        /// </summary>
        /// <returns></returns>
        string GetUserEnvironmentSettingsDocumentId();
    }
}