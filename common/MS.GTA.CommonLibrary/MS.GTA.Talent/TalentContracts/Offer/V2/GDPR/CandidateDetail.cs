//----------------------------------------------------------------------------
// <copyright file="CandidateDetail.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2.Gdpr
{
    /// <summary>
    /// Contract for Candidate informations details
    /// </summary>
    public class CandidateDetail
    {
        /// <summary>Gets or sets the value of given name</summary>
        public string GivenName { get; set; }

        /// <summary>Gets or sets the value of middle name</summary>
        public string MiddleName { get; set; }

        /// <summary>Gets or sets the value of surname</summary>
        public string Surname { get; set; }

        /// <summary>Gets or sets the value of primary email</summary>
        public string EmailPrimary { get; set; }

        /// <summary>Gets or sets the value of alternate email</summary>
        public string EmailAlternate { get; set; }

        /// <summary>Gets or sets the value of mobile phone</summary>
        public string MobilePhone { get; set; }

        /// <summary>Gets or sets the value of mobile phone</summary>
        public string WorkPhone { get; set; }

        /// <summary>Gets or sets the value of mobile phone</summary>
        public string HomePhone { get; set; }

        /// <summary>Gets or sets the value of FacebookId</summary>
        public string FacebookId { get; set; }

        /// <summary>Gets or sets the value of LinkedInId</summary>
        public string LinkedInId { get; set; }

        /// <summary>Gets or sets the value of TwitterId</summary>
        public string TwitterId { get; set; }
    }
}
