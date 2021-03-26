//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Data
{
    using System.Threading.Tasks;
    using TA.CommonLibrary.Common.DocumentDB;
    using TA.CommonLibrary.Common.TalentAttract.Contract;

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
