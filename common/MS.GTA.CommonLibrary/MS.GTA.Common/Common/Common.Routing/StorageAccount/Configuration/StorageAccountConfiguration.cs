using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.GTA.Common.Routing.StorageAccount.Configuration
{
    using ServicePlatform.Configuration;

    /// <summary>
    /// StorageAccount settings class
    /// </summary>
    [SettingsSection(nameof(StorageAccountConfiguration))]
    public class StorageAccountConfiguration
    {
        /// <summary>Gets or sets the name of KeyVault Secret Name where PrimaryConnectionString is stored.</summary>
        public string KeyVaultSecretNameForPrimaryConnectionString { get; set; }

        /// <summary>Gets or sets the name of KeyVault Secret Name where SecondaryConnectionString is stored.</summary>
        public string KeyVaultSecretNameForSecondaryConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the StorageAccount key vault URI
        /// </summary>
        public string StorageAccountKeyVaultUri { get; set; }
    }
}
