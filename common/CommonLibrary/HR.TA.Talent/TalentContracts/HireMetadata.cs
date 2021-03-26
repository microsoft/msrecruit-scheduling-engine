//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The client data contract with the data to show the welcome page.
    /// </summary>
    [DataContract]
    public class HireMetadata
    {
        /// <summary>
        /// Gets or sets the collection of candidates
        /// </summary>
        [DataMember(Name = "candidates", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Applicant> Candidates { get; set; }

        /// <summary>
        /// Gets or sets the collection of job openings
        /// </summary>
        [DataMember(Name = "jobs", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Job> Jobs { get; set; }

        /// <summary>
        /// Gets or sets number of candidates in applied stage for job openings where user is participating.
        /// </summary>
        [DataMember(Name = "candidatesAppliedCount")]
        public int CandidatesAppliedCount { get; set; }

        /// <summary>
        /// Gets or sets number of candidates in screen stage for job openings where user is participating.
        /// </summary>
        [DataMember(Name = "candidatesScreenCount")]
        public int CandidatesScreenCount { get; set; }

        /// <summary>
        /// Gets or sets number of candidates in interview stage for job openings where user is participating.
        /// </summary>
        [DataMember(Name = "candidatesInterviewCount")]
        public int CandidatesInterviewCount { get; set; }

        /// <summary>
        /// Gets or sets number of candidates in offer stage for job openings where user is participating.
        /// </summary>
        [DataMember(Name = "candidatesOfferCount")]
        public int CandidatesOfferCount { get; set; }

        /// <summary>
        /// Gets or sets number of candidates in assessment stage for job openings where user is participating.
        /// </summary>
        [DataMember(Name = "candidatesAssessmentCount")]
        public int CandidatesAssessmentCount { get; set; }
    }
}
