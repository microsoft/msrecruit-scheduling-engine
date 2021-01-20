//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ApplicantProfile.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The Applicant data contract.
    /// </summary>
    [DataContract]
    public class ApplicantProfile : Person
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets guid.
        /// </summary>
        [DataMember(Name = "guid", IsRequired = false)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets b2c objectid.
        /// </summary>
        [DataMember(Name = "b2cObjectId", IsRequired = false)]
        public string B2CObjectId { get; set; }

        /// <summary>
        /// Gets or sets is internal candidate.
        /// </summary>
        [DataMember(Name = "isInternal", IsRequired = false)]
        public bool? IsInternal { get; set; }

        /// <summary>
        /// Gets or sets status of the candidate.
        /// </summary>
        [DataMember(Name = "status", IsRequired = false)]
        public CandidateStatus Status { get; set; }

        /// <summary>
        /// Gets or sets status reason of the candidate.
        /// </summary>
        [DataMember(Name = "statusReason", IsRequired = false)]
        public CandidateStatusReason StatusReason { get; set; }

        /// <summary>
        /// Gets or sets external candidate source.
        /// </summary>
        [DataMember(Name = "externalSource", IsRequired = false)]
        public CandidateExternalSource? ExternalSource { get; set; }

        /// <summary>
        /// Gets or sets preferred phone.
        /// </summary>
        [DataMember(Name = "preferredPhone", IsRequired = false)]
        public CandidatePreferredPhone PreferredPhone { get; set; }

        /// <summary>
        /// Gets or sets a value for the gender of the candidate
        /// </summary>
        [DataMember(Name = "gender", IsRequired = false)]
        public CandidateGender Gender { get; set; }

        /// <summary>
        /// Gets or sets a value for the ethnic origin of the candidates
        /// </summary>
        [DataMember(Name = "ethnicOrigin", IsRequired = false)]
        public CandidateEthnicOrigin EthnicOrigin { get; set; }

        /// <summary>
        /// Gets or sets attachments.
        /// </summary>
        [DataMember(Name = "attachments", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<ApplicantAttachment> Attachments { get; set; }

        /// <summary>
        /// Gets or sets a value for flag to alert on application change.
        /// </summary>
        [DataMember(Name = "alertOnApplicationChange", IsRequired = false, EmitDefaultValue = false)]
        public bool? AlertOnApplicationChange { get; set; }

        /// <summary>
        /// Gets or sets ExtendedAttributes.
        /// </summary>
        [DataMember(Name = "extendedAttributes", IsRequired = false, EmitDefaultValue = false)]
        public Dictionary<string, string> ExtendedAttributes { get; set; }

        /// <summary>
        /// Gets or sets the various work experiences for the applicant.
        /// </summary>
        [DataMember(Name = "workExperience", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<WorkExperience> WorkExperience { get; set; }

        /// <summary>
        /// Gets or sets the various education experiences for the applicant.
        /// </summary>
        [DataMember(Name = "education", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Education> Education { get; set; }

        /// <summary>
        /// Gets or sets the various skills for the applicant.
        /// </summary>
        [DataMember(Name = "skillSet", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<string> SkillSet { get; set; }

        /// <summary>
        /// Gets or sets the Talent source
        /// </summary>
        [DataMember(Name = "talentSource", IsRequired = false, EmitDefaultValue = false)]
        public TalentSource TalentSource { get; set; }
    }
}