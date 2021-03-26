//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Routing.Contracts
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
