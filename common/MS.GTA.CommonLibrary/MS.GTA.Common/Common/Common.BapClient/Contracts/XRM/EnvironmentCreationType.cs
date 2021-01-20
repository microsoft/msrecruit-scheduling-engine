// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnvironmentCreationType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.BapClient.Contracts.XRM
{
    using Base.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// The environment creation type
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter))]
    public enum EnvironmentCreationType
    {
        /// <summary>
        /// The environment creation type is not specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// The legacy environment
        /// </summary>
        Legacy,

        /// <summary>
        /// The default environment in the tenant
        /// </summary>
        DefaultTenant,

        /// <summary>
        /// The user environment
        /// </summary>
        User,

        /// <summary>
        /// The common data model legacy environment.
        /// </summary>
        CommonDataModelLegacy,

        /// <summary>
        /// The environment was created by a partner team.
        /// </summary>
        Partner,

        /// <summary>
        /// The developer environment.
        /// </summary>
        Developer,
    }
}
