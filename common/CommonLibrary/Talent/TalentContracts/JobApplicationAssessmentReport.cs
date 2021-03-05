//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TalentEntities.Enum;

    /// <summary>The job application assessment report.</summary>
    [DataContract]
    public class JobApplicationAssessmentReport
    {
        /// <summary>Gets or sets the job application id.</summary>
        [DataMember(Name = "jobApplicationId")]
        public string JobApplicationId { get; set; }

        /// <summary>Gets or sets the candidate's given name.</summary>
        [DataMember(Name = "candidateGivenName")]
        public string CandidateGivenName { get; set; }

        /// <summary>Gets or sets the candidate's surname.</summary>
        [DataMember(Name = "candidateSurname")]
        public string CandidateSurname { get; set; }

        /// <summary>Gets or sets the external assessment id.</summary>
        [DataMember(Name = "externalAssessmentReportID")]
        public string ExternalAssessmentReportID { get; set; }

        /// <summary>Gets or sets the assessment status.</summary>
        [DataMember(Name = "assessmentStatus")]
        public AssessmentStatus AssessmentStatus { get; set; }

        /// <summary>Gets or sets the assessment URL.</summary>
        [DataMember(Name = "assessmentURL")]
        public string AssessmentURL { get; set; }

        /// <summary>Gets or sets the assessment Title.</summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>Gets or sets the report URL.</summary>
        [DataMember(Name = "reportURL")]
        public string ReportURL { get; set; }

        /// <summary>Gets or sets the assessment report results</summary>
        [DataMember(Name = "results")]
        public IEnumerable<JobApplicationAssessmentReportResult> Results { get; set; }

        /// <summary>Gets or sets the additional information</summary>
        [DataMember(Name = "additionalInformation")]
        public string AdditionalInformation { get; set; }
    }
}
