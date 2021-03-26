//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// The Sample data contract.
    /// </summary>
    [DataContract]
    public class Sample
    {
        /// <summary>
        /// Gets or sets a value indicating whether to delete all jobs or not
        /// </summary>
        [DataMember(Name = "deleteJobs", IsRequired = false)]
        public bool DeleteJobs { get; set; }

        /// <summary>Gets or sets the name.</summary>
        [DataMember(Name = "job")]
        public Job Job { get; set; }

        /// <summary>
        /// Gets or sets candidate
        /// </summary>
        [DataMember(Name = "teams", IsRequired = false)]
        public TeamMember[] Teams { get; set; }

        /// <summary>
        /// Gets or sets candidate
        /// </summary>
        [DataMember(Name = "candidates", IsRequired = false)]
        public Applicant[] Candidates { get; set; }

        /// <summary>
        /// Gets or sets candidate stages
        /// </summary>
        [DataMember(Name = "advanceStages", IsRequired = false)]
        public Dictionary<string, List<JobStage>> AdvanceStages { get; set; }

        /// <summary>
        /// Gets or sets candidate stages
        /// </summary>
        [DataMember(Name = "advanceStageOrder", IsRequired = false)]
        public Dictionary<string, List<int>> AdvanceStagesOrder { get; set; }

        /// <summary>
        /// Gets or sets candidate assessments
        /// </summary>
        [DataMember(Name = "assessments", IsRequired = false)]
        public Dictionary<string, Feedback[]> Assessments { get; set; }

        /// <summary>
        /// Gets or sets candidate assessments
        /// </summary>
        [DataMember(Name = "rejectedCandidates", IsRequired = false)]
        public Dictionary<string, JobApplicationStatusReasonPayload> RejectedCandidates { get; set; }
    }
}
