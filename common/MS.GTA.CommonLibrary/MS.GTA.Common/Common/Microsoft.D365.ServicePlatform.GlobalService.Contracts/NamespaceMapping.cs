//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.ServicePlatform.GlobalService.Contracts.Client
{
    /// <summary>
    /// Namespace Domain Mapping POCO
    /// </summary>
    public sealed class NamespaceMapping
    {
        /// <summary>
        /// Gets or sets the namespace identifier for the mapping.
        /// </summary>
        public string NamespaceId { get; set; }

        /// <summary>
        /// Gets or sets the environment identifier for the mapping.
        /// </summary>
        public string EnvironmentId { get; set; }

        /// <summary>
        /// Gets or sets the Custer ID.
        /// </summary>
        public string ClusterId { get; set; }

        /// <summary>
        /// Gets or sets the Domain.
        /// </summary>
        public string Domain { get; set; }
    }
}
