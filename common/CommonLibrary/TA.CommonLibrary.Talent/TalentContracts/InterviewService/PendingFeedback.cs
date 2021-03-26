//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Talent.TalentContracts.InterviewService
{
    using TA.CommonLibrary.Common.TalentAttract.Contract;
    using TA.CommonLibrary.TalentEntities.Enum;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// The Pending Feedback data contract.
    /// </summary>
    [DataContract]
    public class PendingFeedback
    {
        /// <summary>Gets or sets End date time.</summary>
        [DataMember(Name = "StartDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? StartDateTime { get; set; }

        /// <summary>Gets or sets Mode of the interview.</summary>
        [DataMember(Name = "ModeOfInterview", EmitDefaultValue = false, IsRequired = false)]
        public InterviewMode? ModeOfInterview { get; set; }

        /// <summary>Gets or sets the Job Opening Position.</summary>
        [DataMember(Name = "PositionTitle", EmitDefaultValue = false, IsRequired = false)]
        public string PositionTitle { get; set; }

        /// <summary>Gets or sets the Interviewer Name.</summary>
        [DataMember(Name = "InterviewerName", EmitDefaultValue = false, IsRequired = false)]
        public string InterviewerName { get; set; }

        /// <summary>Gets or sets the Candidate Name.</summary>
        [DataMember(Name = "CandidateName", EmitDefaultValue = false, IsRequired = false)]
        public string CandidateName { get; set; }

        /// <summary> Gets or sets the Interviewer Oid/// </summary>
        [DataMember(Name = "InterviewerOID", EmitDefaultValue = false, IsRequired = false)]
        public string InterviewerOID { get; set; }

        /// <summary>
        /// Gets or sets a value for the Job Application ID
        /// </summary>
        [DataMember(Name = "jobApplicationID", IsRequired = false, EmitDefaultValue = false)]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets a role of user in this job application
        /// </summary>
        [DataMember(Name = "Roles", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobParticipantRole?> Roles { get; set; }
    }
}
