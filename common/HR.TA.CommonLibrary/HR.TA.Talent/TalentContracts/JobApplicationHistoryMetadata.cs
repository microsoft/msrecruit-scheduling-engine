//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;
    using HR.TA.TalentEntities.Enum;

    /// <summary>
    /// Job Metadata Request
    /// </summary>
    [DataContract]
    public class JobApplicationHistoryMetadata
    {
        /// <summary>
        /// Job Application Status  
        /// </summary>
        [DataMember(Name = "jobApplicationStatus", IsRequired = false)]
        public JobApplicationStatus JobApplicationStatus { get; set; }

        /// <summary>
        /// Job Application date 
        /// </summary>
        [DataMember(Name = "jobApplicationDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime JobApplicationDate { get; set; }

        /// <summary>
        /// Hiring Manager Name
        /// </summary>
        [DataMember(Name = "hiringTeamMember", IsRequired = false, EmitDefaultValue = false)]
        public HiringTeamMember HiringMember { get; set; }

        /// <summary>
        /// Job Title
        /// </summary>
        [DataMember(Name = "jobTitle", IsRequired = false, EmitDefaultValue = false)]
        public string JobTitle { get; set; }

        /// <summary>
        /// Job Opening Id
        /// </summary>
        [DataMember(Name = "jobOpeningId", IsRequired = false, EmitDefaultValue = false)]
        public string JobOpeningId { get; set; }

        /// <summary>
        /// Job Application Id
        /// </summary>
        [DataMember(Name = "jobApplicationId", IsRequired = false, EmitDefaultValue = false)]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Rank of applicant within given job
        /// </summary>
        [DataMember(Name = "rank", IsRequired = false, EmitDefaultValue = false)]
        public Rank? Rank { get; set; }

        /// <summary>
        /// Gets or sets the Talent source
        /// </summary>
        [DataMember(Name = "talentSource", IsRequired = false, EmitDefaultValue = false)]
        public TalentSource TalentSource { get; set; }
    }
}
