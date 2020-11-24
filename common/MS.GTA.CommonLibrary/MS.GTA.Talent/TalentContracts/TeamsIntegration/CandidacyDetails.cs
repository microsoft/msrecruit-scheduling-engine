//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidacyDetails.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Talent.TalentContracts.TeamsIntegration
{
    using System.Runtime.Serialization;
    using MS.GTA.Talent.TalentContracts.InterviewService;

    /// <summary>
    /// Candidacy Details
    /// </summary>
    [DataContract]
    public class CandidacyDetails
    {
        /// <summary>
        /// Gets or sets Candidacy Id.
        /// </summary>
        [DataMember(Name = "CandidacyId", EmitDefaultValue = false, IsRequired = false)]
        public string CandidacyId { get; set; }

        /// <summary>
        /// Gets or sets Requisition Id.
        /// </summary>
        [DataMember(Name = "RequisitionId", EmitDefaultValue = false, IsRequired = false)]
        public string RequisitionId { get; set; }

        /// <summary>
        /// Gets or sets job title.
        /// </summary>
        [DataMember(Name = "JobTitle", EmitDefaultValue = false, IsRequired = false)]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets candidate.
        /// </summary>
        [DataMember(Name = "Candidate", EmitDefaultValue = false, IsRequired = false)]
        public CandidateInformation Candidate { get; set; }

        /// <summary>
        /// Gets or sets Hiring Manager
        /// </summary>
        [DataMember(Name = "HiringManager", EmitDefaultValue = false, IsRequired = false)]
        public string HiringManager { get; set; }

        /// <summary>
        /// Gets or sets Recruiter
        /// </summary>
        [DataMember(Name = "Recruiter", EmitDefaultValue = false, IsRequired = false)]
        public string Recruiter { get; set; }

        /// <summary>
        /// Gets or sets current stage.
        /// </summary>
        [DataMember(Name = "CandidacyStage", EmitDefaultValue = false, IsRequired = false)]
        public CandidacyStage? CandidacyStage { get; set; }

        /// <summary>
        /// Gets or sets current stage.
        /// </summary>
        [DataMember(Name = "CandidacyStatus", EmitDefaultValue = false, IsRequired = false)]
        public string CandidacyStatus { get; set; }
    }
}
