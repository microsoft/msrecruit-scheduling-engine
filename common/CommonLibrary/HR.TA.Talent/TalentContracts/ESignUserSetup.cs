//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for ESign Account
    /// </summary>
    [DataContract]
    public class ESignUserSetup
    {
        /// <summary>
        /// Gets or sets a value indicating where authcode enabled or not
        /// </summary>
        [DataMember(Name = "authcode", IsRequired = false)]
        public string Authcode { get; set; }

        /// <summary>
        /// Gets or sets a value for Redirect URI
        /// </summary>
        [DataMember(Name = "redirectUri", IsRequired = false)]
        public string RedirectUri { get; set; }

        /// <summary>
        /// Gets or sets a value for API Access Point
        /// </summary>
        [DataMember(Name = "apiAccessPoint", IsRequired = false)]
        public string ApiAccessPoint { get; set; }

        /// <summary>
        /// Gets or sets a value for Web Access Point
        /// </summary>
        [DataMember(Name = "webAccessPoint", IsRequired = false)]
        public string WebAccessPoint { get; set; }
    }
}
