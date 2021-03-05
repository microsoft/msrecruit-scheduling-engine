//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace ServicePlatform.Context
{
    /// <summary>
    /// Contract for ServicePlatform Environment Context
    /// </summary>
    public interface IEnvironmentContext
    {
        /// <summary>
        /// Gets the application name (e.g. "CDM", "HCM")
        /// </summary>
        string Application { get; }

        /// <summary>
        /// Gets the service name (e.g. "CDMDataService.exe")
        /// </summary>
        string Service { get; }

        /// <summary>
        /// Gets the code package version (e.g. "1.0.8.0")
        /// </summary>
        string CodePackageVersion { get; }

        /// <summary>
        /// Gets the partition Id
        /// </summary>
        string PartitionId { get; }

        /// <summary>
        /// Gets the replica or instance Id (e.g. "131360817915785499")
        /// </summary>
        string ReplicaOrInstanceId { get; }

        /// <summary>
        /// Gets the listener name
        /// </summary>
        string ListenerName { get; }

        /// <summary>
        /// Gets the Service Fabric cluster name
        /// </summary>
        string Cluster { get; }

        /// <summary>
        /// Gets the Computer name of the Service Fabric VM (e.g. "NT1VM000000")
        /// </summary>
        string RoleInstance { get; }

        /// <summary>
        /// Gets the account for metrics in hot path. 
        /// </summary>
        string MdmAccount { get; }

        /// <summary>
        /// Gets the logical container for the metrics in the hot path account
        /// </summary>
        string MdmNamespace { get; }
    }
}
