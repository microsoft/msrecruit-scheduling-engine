//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Data
{
    using System.Threading.Tasks;
    using Common.DocumentDB;
    using Common.TalentAttract.Contract;

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
