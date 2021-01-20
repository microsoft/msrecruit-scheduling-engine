//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="BapClientConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.BapClient.Configuration
{
    /// <summary> BAP Service Client Configuration. </summary>
    public class BapClientConfiguration
    {
        /// <summary> Gets or sets the BAP service resource. </summary>
        public string BapResourceId { get; set; }

        /// <summary> Gets or sets the BAP relative url </summary>
        public string BapRelativeUrl { get; set; }

        /// <summary> Gets or sets ServicePrincipalClientId </summary>
        public string BapServicePrincipalClientId { get; set; }

        /// <summary> Gets or sets Tenant Id. </summary>
        public string BapAADTenantId { get; set; }

        /// <summary> Gets or sets the ID scoping a creator. </summary>
        public string BapAreaId { get; set; }

        /// <summary> Gets or sets a prefix that defines the BAP environment for the tenant. </summary>
        public string BapEnvironmentPrefix { get; set; }

        /// <summary> Gets or sets the BAP API version. </summary>
        public string BapApiVersion { get; set; }

        /// <summary>Gets or sets the bap base url.</summary>
        public string BapBaseUrl { get; set; }

        /// <summary> Gets or sets the BAP XRM API version. </summary>
        public string BapXRMApiVersion { get; set; }
    }
}