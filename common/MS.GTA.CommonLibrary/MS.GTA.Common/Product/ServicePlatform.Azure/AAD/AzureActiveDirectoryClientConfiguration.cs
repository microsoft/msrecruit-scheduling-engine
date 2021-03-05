//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using ServicePlatform.Configuration;

namespace ServicePlatform.Azure.AAD
{
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Settings class for binding Azure Active Directory related configuration values.
    /// </summary>
    [SettingsSection("GTA:ServicePlatform:AzureActiveDirectoryClientConfiguration")]
    public sealed class AzureActiveDirectoryClientConfiguration
    {
        /// <summary>
        /// Instance of AAD (Usually https://login.windows.net/common or https://login.windows.net/{tenant})
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Identifier of application registered with Azure Active Directory
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Identifier of the application used for Graph API 
        /// </summary>
        public string NativeAppClientId { get; set; }
        
        /// <summary>
        /// Service account used for the Native App for accessing graph
        /// </summary>
        public string ServiceAccountUserId { get; set; }
        /// <summary>
        /// Certificate thumbprint associated with client id.
        /// </summary>
        [Obsolete("Use ClientCertificateThumbprints to access thumbprints.")]
        public string ClientCertificateThumbprint { get; set; }

        /// <summary>
        /// Gets or sets the certificate store name, defaulting to My.
        /// </summary>
        public StoreName ClientCertificateStoreName { get; set; } = StoreName.My;

        /// <summary>
        /// Gets or sets the certificate store location, defaulting to CurrentUser.
        /// </summary>
        public StoreLocation ClientCertificateStoreLocation { get; set; } = StoreLocation.CurrentUser;

        /// <summary>
        /// Comma separated list of certificate thumbprints associated with client id.
        /// </summary>
        public string ClientCertificateThumbprints { get; set; }

        /// <summary>
        /// Access the certificate thumbprints as list.
        /// </summary>
        public IList<string> ClientCertificateThumbprintList
        {
            get
            {
                var thumbprints = new List<string>();

                if (!string.IsNullOrWhiteSpace(ClientCertificateThumbprints))
                {
                    thumbprints.AddRange(ClientCertificateThumbprints.Split(',').Select(t => t.Trim()));
                }

#pragma warning disable 618
                if (!string.IsNullOrWhiteSpace(ClientCertificateThumbprint) && !thumbprints.Contains(ClientCertificateThumbprint.Trim()))
                {
                    thumbprints.Add(ClientCertificateThumbprint.Trim());
                }
#pragma warning restore 618

                return thumbprints;
            }
        } 
    }
}
