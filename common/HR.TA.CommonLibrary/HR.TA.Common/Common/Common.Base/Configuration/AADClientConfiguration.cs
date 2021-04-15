//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.Base.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using ServicePlatform.Configuration;

    /// <summary>
    /// AAD Client Configuration
    /// </summary>
    [SettingsSection(nameof(AADClientConfiguration))]
    public class AADClientConfiguration
    {
        /// <summary>
        /// Gets or sets AAD App ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets AAD client certificate for token issuance
        /// </summary>
        public string ClientCertificateThumbprints { get; set; }

        /// <summary>
        /// Gets or sets AAD client credential for token issuance
        /// </summary>
        public string ClientCredential { get; set; }

        /// <summary>
        /// Gets or sets graph AAD instance URL
        /// </summary>
        public string AADInstance { get; set; }

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

                return thumbprints;
            }
        }

        /// <summary>
        /// Gets or sets the XRM Resource ID for token issuance
        /// </summary>
        public string XRMResource { get; set; }

        /// <summary>
        /// Gets or sets Tenant ID for token issuance
        /// </summary>
        public string TenantID { get; set; }
    }
}
