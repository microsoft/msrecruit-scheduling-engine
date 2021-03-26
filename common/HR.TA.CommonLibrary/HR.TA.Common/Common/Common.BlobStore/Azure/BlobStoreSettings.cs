//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.BlobStore.Configuration
{
    using ServicePlatform.Configuration;

    /// <summary>
    /// BlobStore settings class
    /// </summary>
    [SettingsSection(nameof(BlobStoreSettings))]
    public class BlobStoreSettings
    {
        /// <summary>
        /// Gets or sets the Container for Candidate Resume
        /// </summary>
        public string CandidateResumeContainer { get; set; }

        /// <summary>Gets or sets the name of the blob store container.</summary>
        public string BlobStoreContainerName { get; set; }

        /// <summary>Gets or sets the name of KeyVault Secret Name where PrimaryConnectionString is stored.</summary>
        public string KeyVaultSecretNameForPrimaryConnectionString { get; set; }

        /// <summary>Gets or sets the name of KeyVault Secret Name where SecondaryConnectionString is stored.</summary>
        public string KeyVaultSecretNameForSecondaryConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the blob key vault URI
        /// </summary>
        public string BlobKeyVaultUri { get; set; }
    }
}
