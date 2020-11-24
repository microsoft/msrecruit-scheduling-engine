//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ApplicantProfileWithLoginDetails.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The Applicant data contract.
    /// </summary>
    [DataContract]
    public class ApplicantProfileWithLoginDetails
    {
        /// <summary>
        /// Gets or sets identityProvider.
        /// </summary>
        [DataMember(Name = "identityProvider", IsRequired = true)]
        public string IdentityProvider { get; set; }

        /// <summary>
        /// Gets or sets identityProviderUsername.
        /// </summary>
        [DataMember(Name = "identityProviderUsername", IsRequired = true)]
        public string IdentityProviderUsername { get; set; }

        /// <summary>
        /// Gets or sets applicant profile.
        /// </summary>
        [DataMember(Name = "applicantProfile", IsRequired = true)]
        public ApplicantProfile ApplicantProfile { get; set; }
    }
}