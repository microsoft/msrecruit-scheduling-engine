using System;
using Newtonsoft.Json;

namespace MS.GTA.Common.BapClient.Contracts.XRM
{
    /// <summary>
    /// The environment entity properties.
    /// </summary>
    public class EnvironmentProperties
    {
        /// <summary>
        /// Gets or sets the api hub management account resource id.
        /// </summary>
        public string ApiManagementAccountResourceId { get; set; }

        /// <summary>
        /// Gets or sets the azure region hint for the region.
        /// </summary>
        /// <remarks>Value is only populated on calls of the service context type.</remarks>
        [JsonProperty]
        public string AzureRegionHint { get; set; }

        /// <summary>
        /// Gets or sets the environment display name.
        /// </summary>
        [JsonProperty]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the environment description.
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the created time.
        /// </summary>
        [JsonProperty]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        [JsonProperty]
        public Principal CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified time.
        /// </summary>
        [JsonProperty]
        public DateTime? LastModifiedTime { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        [JsonProperty]
        public Principal LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the provisioning state
        /// </summary>
        [JsonProperty]
        public EnvironmentProvisioningState ProvisioningState { get; set; }

        /// <summary>
        /// Gets or sets the environment provisioning details.
        /// </summary>
        [JsonProperty]
        public EnvironmentProvisioningDetails ProvisioningDetails { get; set; }

        /// <summary>
        /// Gets or sets the creation type
        /// </summary>
        [JsonProperty]
        public EnvironmentCreationType CreationType { get; set; }

        /// <summary>
        /// Gets or sets the SKU
        /// </summary>
        [JsonProperty]
        public EnvironmentSku EnvironmentSku { get; set; }

        /// <summary>
        /// Gets or sets the environment type
        /// </summary>
        [JsonProperty]
        public EnvironmentType EnvironmentType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this environment is the default environment
        /// </summary>
        [JsonProperty]
        public bool? IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the expiration time for the environment.
        /// </summary>
        /// <remarks>
        /// Only applicable to temporary environments.
        /// </remarks>
        [JsonProperty]
        public DateTime? ExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets the linked environment metadata
        /// </summary>
        [JsonProperty]
        public LinkedEnvironmentMetadata LinkedEnvironmentMetadata { get; set; }

        /// <summary>
        /// Gets or sets the CDS1.0 namespace in the environment.
        /// </summary>
        /// <remarks>
        /// Only available when $expand=Namespace is given on the request.
        /// </remarks>
        [JsonProperty]
        public NamespaceMetadata Namespace { get; set; }

        /// <summary>
        /// Gets or sets the soft-deletion time
        /// </summary>
        [JsonProperty]
        public DateTime? SoftDeletedTime { get; set; }

        /// <summary>
        /// Gets or sets a dictionary of permissions that the calling user has on the given environment
        /// </summary>
        /// <remarks>
        /// Only available when $expand=Permissions is given on the request.
        /// </remarks>
        [JsonProperty]
        public Permissions Permissions { get; set; }
    }
}
