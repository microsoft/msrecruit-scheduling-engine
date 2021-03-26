//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.TalentContracts.InterviewService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.Provisioning.Entities.FalconEntities.Attract;
    using HR.TA.Common.TalentEntities.Common;
    using HR.TA.TalentEntities.Enum;

    /// <summary>
    /// Applicants Metadata contract
    /// </summary>
    [DataContract]
    public class JobApplicationMetadata
    {
        /// <summary>
        /// Gets or sets the Job Application Id
        /// </summary>
        [DataMember(Name = "jobApplicationId", IsRequired = false, EmitDefaultValue = false)]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the ExternalJobOpeningId
        /// </summary>
        [DataMember(Name = "externalJobOpeningId", IsRequired = false, EmitDefaultValue = false)]
        public string ExternalJobOpeningId { get; set; }

        /// <summary>
        /// Gets or sets the JobTitle
        /// </summary>
        [DataMember(Name = "jobTitle", IsRequired = false, EmitDefaultValue = false)]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the JobDescription
        /// </summary>
        [DataMember(Name = "jobDescription", IsRequired = false, EmitDefaultValue = false)]
        public string JobDescription { get; set; }

        /// <summary>
        /// Gets or sets the CurrentJobApplicationStageStatus
        /// </summary>
        [DataMember(Name = "currentJobApplicationStageStatus", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStageStatus? CurrentJobApplicationStageStatus { get; set; }

        /// <summary>
        /// Gets or sets the CurrentJobOpeningStage
        /// </summary>
        [DataMember(Name = "currentJobOpeningStage", EmitDefaultValue = false, IsRequired = false)]
        public JobStage? CurrentJobOpeningStage { get; set; }

        /// <summary>
        /// Gets or sets the JobApplicationStatus
        /// </summary>
        [DataMember(Name = "jobApplicationStatus", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStatus? JobApplicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the Job Application Candidate
        /// </summary>
        [DataMember(Name = "candidate", EmitDefaultValue = false, IsRequired = false)]
        public CandidateInformation Candidate { get; set; }

        /// <summary>
        /// Gets or sets all  Job Application Particpants
        /// </summary>
        [DataMember(Name = "jobApplicationParticipants", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationParticipant> JobApplicationParticipants { get; set; }

        /// <summary>
        /// Gets or sets all  Job Application Particpants
        /// </summary>
        [DataMember(Name = "jobApplicationParticipantDetails", EmitDefaultValue = false, IsRequired = false)]
        public IList<IVWorker> JobApplicationParticipantDetails { get; set; }

        /// <summary>
        /// Gets or sets all  the flag for schedule summary to candidate.
        /// </summary>
        [DataMember(Name = "IsScheduleSentToCandidate", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsScheduleSentToCandidate { get; set; }

        /// <summary>
        /// Gets or sets for application hire type.
        /// </summary>
        [DataMember(Name = "HireType", EmitDefaultValue = false, IsRequired = false)]
        public string HireType { get; set; }

        /// <summary>
        /// Gets or sets for job application status reason.
        /// </summary>
        [DataMember(Name = "JobApplicationStatusReason", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationStatusReason? JobApplicationStatusReason { get; set; }

        /// <summary>
        /// Gets or sets if the user is in wob context.
        /// </summary>
        [DataMember(Name = "isWobAuthenticated", EmitDefaultValue = false, IsRequired = false)]
        public bool isWobAuthenticated { get; set; }
    }
}
