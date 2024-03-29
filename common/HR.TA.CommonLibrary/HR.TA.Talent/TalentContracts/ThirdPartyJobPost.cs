﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using HR.TA.Common.Attract.Data.DocumentDB;
    using System.Runtime.Serialization;

    /// <summary>
    /// The job post class
    /// </summary>
    [DataContract]
    public class ThirdPartyJobPost : JobPost
    {
        /// <summary>Gets or sets company.</summary>
        [DataMember(Name = "company", IsRequired = false)]
        public string Company { get; set; }

        /// <summary>Gets or sets supplier.</summary>
        [DataMember(Name = "supplier", IsRequired = false)]
        public string Supplier { get; set; }

        /// <summary>Gets or sets countryCode.</summary>
        [DataMember(Name = "countryCode", IsRequired = false)]
        public string CountryCode { get; set; }

        /// <summary>Gets or sets postalCode.</summary>
        [DataMember(Name = "postalCode", IsRequired = false)]
        public string PostalCode { get; set; }
    }
}