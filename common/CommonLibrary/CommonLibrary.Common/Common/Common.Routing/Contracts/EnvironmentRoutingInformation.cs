//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Routing.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The environment routing information
    /// </summary>
    [DataContract]
    public class EnvironmentRoutingInformation
    {
        /// <summary>
        /// Gets or sets the Environment Id
        /// </summary>
        [DataMember(Name = "environmentId")]
        public string EnvironmentId { get; set; }

        /// <summary>
        /// Gets or sets the environment's cluster URI
        /// </summary>
        [DataMember(Name = "clusterUri")]
        public string ClusterUri { get; set; }

        /// <summary>
        /// Gets or sets the BAP location for the environment
        /// </summary>
        [DataMember(Name = "bapLocation")]
        public string BapLocation{ get; set; }

        /// <summary>Gets or sets the partition id.</summary>
        [DataMember(Name = "partitionId")]
        public string PartitionId { get; set; }
    }
}
