//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Talent.TalentContracts.InterviewService
{
    using System.Runtime.Serialization;
    using CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// The user related applicant data contract.
    /// </summary>
    [DataContract]
    public class IVApplicant
    {
        /// <summary>Gets or sets job application id.</summary>
        [DataMember(Name = "JobApplicationId", EmitDefaultValue = false, IsRequired = false)]
        public string JobApplicationID { get; set; }

        /// <summary>Gets or sets ExternalJobOpeningId (Requisition Id).</summary>
        [DataMember(Name = "ExternalJobOpeningId", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalJobOpeningId { get; set; }

        /// <summary>Gets or sets ExternalJobApplicationId (Candidacy Id).</summary>
        [DataMember(Name = "ExternalJobApplicationId", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalJobApplicationId { get; set; }

        /// <summary>Gets or sets job title.</summary>
        [DataMember(Name = "JobTitle", EmitDefaultValue = false, IsRequired = false)]
        public string JobTitle { get; set; }

        /// <summary>Gets or sets candidate.</summary>
        [DataMember(Name = "Candidate", EmitDefaultValue = false, IsRequired = false)]
        public CandidateInformation Candidate { get; set; }

        /// <summary>Gets or sets name of hiring manager.</summary>
        [DataMember(Name = "HiringManager", EmitDefaultValue = false, IsRequired = false)]
        public string HiringManager { get; set; }

        /// <summary>Gets or sets name of recruiter.</summary>
        [DataMember(Name = "Recruiter", EmitDefaultValue = false, IsRequired = false)]
        public string Recruiter { get; set; }

        /// <summary>Gets or sets the role of user.</summary>
        [DataMember(Name = "participantRole", EmitDefaultValue = false, IsRequired = false)]
        public JobParticipantRole? participantRole { get; set; }

        /// <summary>Gets or sets current stage.</summary>
        [DataMember(Name = "CurrentStage", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationActivityType? CurrentStage { get; set; }

        /// <summary>Gets or sets job description.</summary>
        [DataMember(Name = "JobDescription", EmitDefaultValue = false, IsRequired = false)]
        public string JobDescription { get; set; }
    }
}
