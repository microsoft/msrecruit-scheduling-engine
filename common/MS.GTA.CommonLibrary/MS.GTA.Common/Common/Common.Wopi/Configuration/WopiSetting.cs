//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="WopiSetting.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Configuration
{
    using System;
    using System.Collections.Generic;
    using MS.GTA.Common.Base.Helper;
    using MS.GTA.ServicePlatform.Configuration;

    /// <summary>
    /// Microsoft Graph settings class
    /// </summary>
    [SettingsSection("WopiSetting")]
    public class WopiSetting
    {
        /// <summary>
        /// Gets or sets the proof validity window
        /// </summary>
        public int WopiProofValidityWindowInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the WOPI server/host URL
        /// </summary>
        public string WopiServerUrl { get; set; }

        /// <summary>
        /// Gets or sets the WOPI client URL
        /// </summary>
        public string WopiClientUrl { get; set; }

        /// <summary>
        /// Gets or sets the WOPI proof validation feature flag
        /// </summary>
        public string WopiProofFeatureFlag { get; set; }

        /// <summary>
        /// Gets or sets the audience to use in the WOPI JWT token
        /// </summary>
        public string WopiValidAudience { get; set; }

        /// <summary>
        /// Gets or sets the issuer to use in the WOPI JWT token
        /// </summary>
        public string WopiValidIssuer { get; set; }

        /// <summary>
        /// Gets or sets the lifetime for WOPI JWT Tokens
        /// </summary>
        public int WopiTokenValidityInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the certificate thumbprint for signing JWT Tokens
        /// </summary>
        [Obsolete("Use CertThumbprints to access thumbprints.")]
        public string WopiTokenCertThumbprint { get; set; }

        /// <summary>
        /// Gets or sets certificate thumbprints for signing JWT Tokens
        /// </summary>
        public string WopiTokenCertThumbprints { get; set; }

        /// <summary>
        /// Access the certificate thumbprints as list.
        /// </summary>
        public IList<string> CertThumbprintList => MultipleCertificateThumprints.GetThumbprints(WopiTokenCertThumbprints, WopiTokenCertThumbprint);
    }
}
