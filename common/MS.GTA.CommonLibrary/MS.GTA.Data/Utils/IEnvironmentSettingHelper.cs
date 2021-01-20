//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IEnvironmentSettingHelper.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Data
{
    using System.Threading.Tasks;
    using MS.GTA.Common.DocumentDB;
    using MS.GTA.Common.TalentAttract.Contract;

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