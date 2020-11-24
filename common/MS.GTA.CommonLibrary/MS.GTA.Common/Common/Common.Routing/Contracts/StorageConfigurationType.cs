//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="StorageConfigurationType.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Routing.Contracts
{
    /// <summary>
    /// The storage configuration type
    /// </summary>
    public enum StorageConfigurationType
    {
        /// <summary>
        /// Represents a storage configuration tied to a particular environment
        /// </summary>
        EnvironmentStorageConfiguration,

        /// <summary>
        /// Represents a storage configuration tied to a logical cluster
        /// </summary>
        LogicalClusterStorageConfiguration,
    }
}
