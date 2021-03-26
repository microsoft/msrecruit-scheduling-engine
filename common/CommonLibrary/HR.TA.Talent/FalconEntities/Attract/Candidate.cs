//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..Common.DocumentDB.Contracts;
    using HR.TA..Common.TalentEntities.Common;
    using HR.TA..TalentEntities.Enum;
    using HR.TA..Common.TalentEntities.Enum.Common;
    using HR.TA..Talent.FalconEntities.Attract;

    [DataContract]
    public class Candidate : DocDbEntity
    {
        [DataMember(Name = "CandidateId", EmitDefaultValue = false, IsRequired = false)]
        public string CandidateID { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public CandidateStatus? Status { get; set; }

        [DataMember(Name = "StatusReason", EmitDefaultValue = false, IsRequired = false)]
        public CandidateStatusReason? StatusReason { get; set; }

        [DataMember(Name = "ExternalCandidateID", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalCandidateID { get; set; }

        [DataMember(Name = "ExternalCandidateSource", EmitDefaultValue = false, IsRequired = false)]
        public CandidateExternalSource? ExternalCandidateSource { get; set; }

        [DataMember(Name = "ImportedDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ImportedDate { get; set; }

        [DataMember(Name = "IsWillingToRelocate", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsWillingToRelocate { get; set; }

        [DataMember(Name = "IsOpenToNewJob", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsOpenToNewJob { get; set; }

        [DataMember(Name = "InternalCandidate", EmitDefaultValue = false, IsRequired = false)]
        public Worker InternalCandidate { get; set; }

        [DataMember(Name = "LanguagePrimary", EmitDefaultValue = false, IsRequired = false)]
        public string LanguagePrimary { get; set; }

        [DataMember(Name = "CitizenshipPrimary", EmitDefaultValue = false, IsRequired = false)]
        public CountryCode? CitizenshipPrimary { get; set; }

        [DataMember(Name = "EthnicOrigin", EmitDefaultValue = false, IsRequired = false)]
        public CandidateEthnicOrigin? EthnicOrigin { get; set; }

        [DataMember(Name = "DisabilityStatus", EmitDefaultValue = false, IsRequired = false)]
        public CandidateDisabilityStatus? DisabilityStatus { get; set; }

        [DataMember(Name = "FullName", EmitDefaultValue = false, IsRequired = false)]
        public PersonName FullName { get; set; }

        [DataMember(Name = "EmailPrimary", EmitDefaultValue = false, IsRequired = false)]
        public string EmailPrimary { get; set; }

        [DataMember(Name = "EmailAlternate", EmitDefaultValue = false, IsRequired = false)]
        public string EmailAlternate { get; set; }

        [DataMember(Name = "PhonePrimary", EmitDefaultValue = false, IsRequired = false)]
        public string PhonePrimary { get; set; }

        [DataMember(Name = "HomePhone", EmitDefaultValue = false, IsRequired = false)]
        public string HomePhone { get; set; }

        [DataMember(Name = "WorkPhone", EmitDefaultValue = false, IsRequired = false)]
        public string WorkPhone { get; set; }

        [DataMember(Name = "MobilePhone", EmitDefaultValue = false, IsRequired = false)]
        public string MobilePhone { get; set; }

        [DataMember(Name = "LinkedInID", EmitDefaultValue = false, IsRequired = false)]
        public string LinkedInID { get; set; }

        [DataMember(Name = "LinkedInAPIURL", EmitDefaultValue = false, IsRequired = false)]
        public string LinkedInAPIURL { get; set; }

        [DataMember(Name = "FacebookID", EmitDefaultValue = false, IsRequired = false)]
        public string FacebookID { get; set; }

        [DataMember(Name = "TwitterID", EmitDefaultValue = false, IsRequired = false)]
        public string TwitterID { get; set; }

        [DataMember(Name = "OID", EmitDefaultValue = false, IsRequired = false)]
        public string OID { get; set; }

        [DataMember(Name = "B2CObjectId", EmitDefaultValue = false, IsRequired = false)]
        public string B2CObjectId { get; set; }

        [DataMember(Name = "ResidentialPostalAddress", EmitDefaultValue = false, IsRequired = false)]
        public Address ResidentialPostalAddress { get; set; }

        [DataMember(Name = "OtherPostalAddress", EmitDefaultValue = false, IsRequired = false)]
        public Address OtherPostalAddress { get; set; }

        [DataMember(Name = "Gender", EmitDefaultValue = false, IsRequired = false)]
        public CandidateGender? Gender { get; set; }

        [DataMember(Name = "Birthdate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? Birthdate { get; set; }

        [DataMember(Name = "Comment", EmitDefaultValue = false, IsRequired = false)]
        public string Comment { get; set; }

        [DataMember(Name = "PreferredPhone", EmitDefaultValue = false, IsRequired = false)]
        public CandidatePreferredPhone? PreferredPhone { get; set; }

        [DataMember(Name = "Skills", EmitDefaultValue = false, IsRequired = false)]
        public IList<CandidateSkill> Skills { get; set; }

        [DataMember(Name = "JobApplications", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplication> JobApplications { get; set; }

        [DataMember(Name = "WorkExperiences", EmitDefaultValue = false, IsRequired = false)]
        public IList<CandidateWorkExperience> WorkExperiences { get; set; }

        [DataMember(Name = "Educations", EmitDefaultValue = false, IsRequired = false)]
        public IList<CandidateEducation> Educations { get; set; }

        [DataMember(Name = "Notes", EmitDefaultValue = false, IsRequired = false)]
        public IList<CandidateNote> Notes { get; set; }

        [DataMember(Name = "Attachments", EmitDefaultValue = false, IsRequired = false)]
        public IList<CandidateAttachment> Attachments { get; set; }

        [DataMember(Name = "CandidateExtendedAttributes", EmitDefaultValue = false, IsRequired = false)]
        public IList<CustomAttributes> CandidateExtendedAttributes { get; set; }

        [DataMember(Name = "IsSyncedWithLinkedIn", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsSyncedWithLinkedIn { get; set; }
    }
}
