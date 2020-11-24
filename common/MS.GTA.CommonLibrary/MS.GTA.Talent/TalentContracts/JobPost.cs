﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobPost.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

// Note: This namespace needs to stay the same since the docdb collection name depends on it
namespace MS.GTA.Common.Attract.Data.DocumentDB
{
    using System.Runtime.Serialization;
    using JobPostContract = MS.GTA.TalentJobPosting.Contract;

    /// <summary>
    /// The job post class
    /// </summary>
    [DataContract(Name = "attractJobPost")]
    public class JobPost : JobPostContract.JobPost
    {
        /// <summary>Gets or sets company.</summary>
        [DataMember(Name = "company", IsRequired = false)]
        public string Company { get; set; }

        /// <summary>Gets or sets company id.</summary>
        [DataMember(Name = "companyId", IsRequired = false)]
        public string CompanyId { get; set; }

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