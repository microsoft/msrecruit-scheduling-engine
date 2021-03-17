//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommonLibrary.ServicePlatform.GlobalService.Contracts
{
    /// <summary>
    /// Types of physical clusters
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PhysicalClusterType
    {
        /// <summary>
        /// Represents all physical cluster types
        /// </summary>
        All,

        /// <summary>
        /// Represents a Common Data Service cluster
        /// </summary>
        CommonDataService,

        /// <summary>
        /// Represents a Dynamics 365 cluster
        /// </summary>
        Dynamics365,

        /// <summary>
        /// Represents a Dynamics 365 for Talent cluster
        /// </summary>
        Dynamics365ForTalent,
    }
}
