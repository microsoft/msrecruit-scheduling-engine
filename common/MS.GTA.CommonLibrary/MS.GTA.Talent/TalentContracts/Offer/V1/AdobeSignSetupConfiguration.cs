//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AdobeSignSetupConfiguration.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for Adobe Sign Setup Integration
    /// </summary>
    [DataContract]
    public class AdobeSignSetupConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether for adobe code
        /// </summary>
        [DataMember(Name = "code", IsRequired = true)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets Redirect URI
        /// </summary>
        [DataMember(Name = "redirectUri", IsRequired = true)]
        public string RedirectUri { get; set; }

        /// <summary>
        /// Gets or sets Client ID
        /// </summary>
        [DataMember(Name = "clientId", IsRequired = false)]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets Client Secret
        /// </summary>
        [DataMember(Name = "clientSecret", IsRequired = false)]
        public string ClientSecret { get; set; }
    }
}
