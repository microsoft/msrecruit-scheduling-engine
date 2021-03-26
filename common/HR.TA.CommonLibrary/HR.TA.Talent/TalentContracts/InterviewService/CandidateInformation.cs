//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.TalentContracts.InterviewService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The candidate data contract.
    /// </summary>
    [DataContract]
    public class CandidateInformation
    {
        /// <summary>Gets or sets job application id.</summary>
        [DataMember(Name = "CandidateID", EmitDefaultValue = false, IsRequired = false)]
        public string CandidateID { get; set; }

        /// <summary>Gets or sets job application id.</summary>
        [DataMember(Name = "ExternalCandidateID", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalCandidateID { get; set; }

        /// <summary>Gets or sets the Office Graph Identifier.</summary>
        [DataMember(Name = "OID")]
        public string OID { get; set; }

        /// <summary>Gets or sets the value of full name </summary>
        [DataMember(Name = "FullName")]
        public string FullName { get; set; }

        /// <summary> Gets or sets the value of given name </summary>
        [DataMember(Name = "GivenName")]
        public string GivenName { get; set; }

        /// <summary>Gets or sets the value of middle name </summary>
        [DataMember(Name = "MiddleName")]
        public string MiddleName { get; set; }

        /// <summary>Gets or sets the value of surname </summary>
        [DataMember(Name = "Surname")]
        public string Surname { get; set; }

        /// <summary>Gets or sets the value of primary email </summary>
        [DataMember(Name = "EmailPrimary")]
        public string EmailPrimary { get; set; }

        /// <summary>Gets or sets the value of Phone Primary </summary>
        [DataMember(Name = "PhonePrimary")]
        public string PhonePrimary { get; set; }

        /// <summary>
        /// Gets or sets the value of AttachmentID
        /// </summary>
        [DataMember(Name = "AttachmentID")]
        public string AttachmentID { get; set; }

        /// <summary>
        /// Gets or sets the value of AttachmentName
        /// </summary>
        [DataMember(Name = "AttachmentName")]
        public string AttachmentName { get; set; }

        /// <summary>
        /// Gets or sets the value of list of candidate education
        /// </summary>
        [DataMember(Name = "Educations")]
        public IList<CandidateEducationInformation> Educations { get; set; }

        /// <summary>
        /// Gets or sets the value of list of candidate work experience
        /// </summary>
        [DataMember(Name = "WorkExperiences")]
        public IList<CandidateWorkExperienceInformation> WorkExperiences { get; set; }

        /// <summary>
        /// Gets or sets the value of list of candidate skills
        /// </summary>
        [DataMember(Name = "Skills")]
        public IList<CandidateSkillInformation> Skills { get; set; }
    }


    /// <summary>
    /// The candidate education data contract.
    /// </summary>
    [DataContract]
    public class CandidateEducationInformation
    {
        [DataMember(Name = "School")]
        public string School { get; set; }

        [DataMember(Name = "Degree")]
        public string Degree { get; set; }

        [DataMember(Name = "FieldOfStudy")]
        public string FieldOfStudy { get; set; }

        [DataMember(Name = "Grade")]
        public string Grade { get; set; }

        [DataMember(Name = "ActivitiesSocieties")]
        public string ActivitiesSocieties { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "FromMonthYear")]
        public DateTime? FromMonthYear { get; set; }

        [DataMember(Name = "ToMonthYear")]
        public DateTime? ToMonthYear { get; set; }
    }

    /// <summary>
    /// The candidate work experience data contract.
    /// </summary>
    [DataContract]
    public class CandidateWorkExperienceInformation
    {
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "Organization")]
        public string Organization { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "IsCurrentPosition")]
        public bool? IsCurrentPosition { get; set; }

        [DataMember(Name = "FromMonthYear")]
        public DateTime? FromMonthYear { get; set; }

        [DataMember(Name = "ToMonthYear")]
        public DateTime? ToMonthYear { get; set; }
    }

    /// <summary>
    /// The candidate skills data contract.
    /// </summary>
    [DataContract]
    public class CandidateSkillInformation
    {
        [DataMember(Name = "Skill")]
        public string Skill { get; set; }
    }
}
