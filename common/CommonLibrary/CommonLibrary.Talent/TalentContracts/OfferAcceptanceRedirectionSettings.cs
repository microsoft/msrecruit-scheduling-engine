//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The post offer redirection setting data contract.
    /// </summary>
    [DataContract]
    public class OfferAcceptanceRedirectionSettings
    {
        /// <summary>
        /// Gets or sets WebAddressUri
        /// </summary>
        [DataMember(Name = "webAddressUri", IsRequired = false)]
        public string WebAddressUri { get; set; }
    }
}
