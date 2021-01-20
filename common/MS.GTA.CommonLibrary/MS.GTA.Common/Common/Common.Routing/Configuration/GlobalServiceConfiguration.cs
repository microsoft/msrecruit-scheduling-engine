//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="GlobalServiceConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Routing.Configuration
{
    /// <summary> Global service configuration </summary>
    public sealed class GlobalServiceConfiguration
    {
        /// <summary>
        /// Gets or sets the HCM App ID to use for the global service
        /// </summary>
        public string HCMAppAuthorityId { get; set; }

        /// <summary>
        /// Gets or sets the the global service resource
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the the global service AAD tenant Id
        /// </summary>
        public string AADTenantId { get; set; }

        /// <summary>
        /// Gets or sets the the global service cluster Uri
        /// </summary>
        public string ClusterUri { get; set; }
    }
}
