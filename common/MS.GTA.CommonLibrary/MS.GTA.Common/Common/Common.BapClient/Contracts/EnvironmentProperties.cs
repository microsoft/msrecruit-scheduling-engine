//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnvironmentProperties.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.BapClient.Contracts
{
    using System;

    /// <summary>
    /// The environment properties contract
    /// </summary>
    public class EnvironmentProperties
    {
        /// <summary>
        /// Gets or sets the management account resource identifier.
        /// </summary>
        public string ApiManagementAccountResourceId { get; set; }

        /// <summary>
        /// Gets or sets the Azure Region hint for the environment.
        /// </summary>
        public string AzureRegionHint { get; set; }

        /// <summary>
        /// Gets or sets the environment display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the environment description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the created time.
        /// </summary>
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public Principal CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified time.
        /// </summary>
        public DateTime? LastModifiedTime { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        public Principal LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this environment is the default environment
        /// </summary>
        public bool? IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the provisioning state
        /// </summary>
        public string ProvisioningState { get; set; }

        /// <summary>
        /// Gets or sets the environment type
        /// </summary>
        public EnvironmentType EnvironmentType { get; set; }

        /// <summary>
        /// Gets or sets the environment SKU
        /// </summary>
        public EnvironmentSku EnvironmentSku { get; set; }

        /// <summary>
        /// Gets or sets the expiration time
        /// </summary>
        public DateTime? ExpirationTime { get; set; }

        /// <summary>Gets or sets the permissions.</summary>
        public Permissions Permissions { get; set; }
    }
}
