//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferAcceptanceRedirectionSettings.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Talent.FalconEntities.Offer.Entity
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The post offer redirection setting data contract.
    /// </summary>
    [DataContract]
    public class JobOfferAcceptanceRedirectionSettings
    {
        /// <summary>
        /// Gets or sets WebAddressUri
        /// </summary>
        [DataMember(Name = "webAddressUri", IsRequired = false)]
        public string WebAddressUri { get; set; }
    }
}
