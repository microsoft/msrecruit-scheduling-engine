//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Email properties
    /// </summary>
    [DataContract]
    public class OfferExpiryCallback
    {
        /// <summary>
        /// Gets or sets offerid.
        /// </summary>
        [DataMember(Name = "offerID", IsRequired = true)]
        public string OfferID { get; set; }

        /// <summary>
        /// Gets or sets company name.
        /// </summary>
        [DataMember(Name = "companyName", IsRequired = true)]
        public string CompanyName { get; set; }
    }
}
