//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA..Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using HR.TA..Common.Contracts;
    using HR.TA..TalentEntities.Enum;

    /// <summary>The job application details.</summary>
    [DataContract]
    public class JobApplicationDetails : TalentBaseContract
    {
        /// <summary>Gets or sets the application id.</summary>
        [DataMember(Name = nameof(ApplicationId))]
        public string ApplicationId { get; set; }

        /// <summary>Gets or sets the tenant id.</summary>
        [DataMember(Name = nameof(TenantId))]
        public string TenantId { get; set; }

        /// <summary>Gets or sets the company name.</summary>
        [DataMember(Name = nameof(CompanyName))]
        public string CompanyName { get; set; }

        /// <summary>Gets or sets the company location.</summary>
        [DataMember(Name = nameof(PositionLocation))]
        public string PositionLocation { get; set; }

        /// <summary>Gets or sets the position title.</summary>
        [DataMember(Name = nameof(PositionTitle))]
        public string PositionTitle { get; set; }

        /// <summary>Gets or sets the job description.</summary>
        [DataMember(Name = nameof(JobDescription))]
        public string JobDescription { get; set; }

        /// <summary>Gets or sets the job apply link.</summary>
        [DataMember(Name = nameof(JobPostLink), IsRequired = false)]
        public string JobPostLink { get; set; }

        /// <summary>Gets or sets the date applied.</summary>
        [DataMember(Name = nameof(DateApplied))]
        public DateTime DateApplied { get; set; }

        /// <summary>Gets or sets the status.</summary>
        [DataMember(Name = nameof(Status))]
        public JobApplicationStatus Status { get; set; }

        /// <summary>Gets or sets job external status.</summary>
        [DataMember(Name = nameof(ExternalStatus), IsRequired = false)]
        public string ExternalStatus { get; set; }

        /// <summary>Gets or sets the external source.</summary>
        [DataMember(Name = nameof(ExternalSource), IsRequired = false, EmitDefaultValue = false)]
        public JobApplicationExternalSource? ExternalSource { get; set; }

        /// <summary>Gets or sets the status reason</summary>
        [DataMember(Name = nameof(StatusReason), IsRequired = false)]
        public JobApplicationStatusReason StatusReason { get; set; }

        /// <summary>Gets or sets the rejection reason</summary>
        [DataMember(Name = nameof(RejectionReason), IsRequired = false)]
        public OptionSetValue RejectionReason { get; set; }

        /// <summary>Gets or sets the current job stage.</summary>
        [DataMember(Name = nameof(CurrentJobStage), IsRequired = false)]
        public JobStage CurrentJobStage { get; set; }

        /// <summary>
        /// Gets or sets current application stage.
        /// </summary>
        [DataMember(Name = nameof(CurrentApplicationStage), IsRequired = false)]
        public ApplicationStage CurrentApplicationStage { get; set; }

        /// <summary>Gets or sets the interviews.</summary>
        [DataMember(Name = nameof(Interviews))]
        public IList<JobApplicationInterview> Interviews { get; set; }

        /// <summary>
        /// Gets or sets the application schedules.
        /// </summary>
        [DataMember(Name = nameof(ApplicationSchedules), IsRequired = false, EmitDefaultValue = false)]
        public IList<ApplicationSchedule> ApplicationSchedules { get; set; }

        /// <summary>
        /// Gets or sets the applicant attachments. 
        /// </summary>
        [DataMember(Name = nameof(ApplicantAttachments), IsRequired = false, EmitDefaultValue = false)]
        public IList<ApplicantAttachment> ApplicantAttachments { get; set; }

        /// <summary>Gets or sets the applicant assessments.</summary>
        [DataMember(Name = nameof(ApplicantAssessments), IsRequired = false, EmitDefaultValue = false)]
        public IList<ApplicantAssessmentReport> ApplicantAssessments { get; set; }

        /// <summary>
        /// Gets or sets application stages
        /// </summary>
        [DataMember(Name = nameof(ApplicationStages), IsRequired = false, EmitDefaultValue = false)]
        public IList<ApplicationStage> ApplicationStages { get; set; }

        /// <summary>
        /// Gets or sets candidate personal details for this application
        /// </summary>
        [DataMember(Name = "candidatePersonalDetails", IsRequired = false, EmitDefaultValue = false)]
        public IList<CandidatePersonalDetails> CandidatePersonalDetails { get; set; }
    }

    /// <summary>The job application interview.</summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    [DataContract]
    public class ApplicantAssessmentReport
    {
        /// <summary>Gets or sets the title.</summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>Gets or sets the assessment url.</summary>
        [DataMember(Name = "assessmentURL", IsRequired = false, EmitDefaultValue = false)]
        public string AssessmentURL { get; set; }

        /// <summary>
        /// Gets or sets the external assessment report ID.
        /// </summary>
        [DataMember(Name = "externalAssessmentReportID", IsRequired = false, EmitDefaultValue = false)]
        public string ExternalAssessmentReportID { get; set; }

        /// <summary>
        /// Gets or sets the provider key.
        /// </summary>
        [DataMember(Name = "providerKey", IsRequired = false, EmitDefaultValue = false)]
        public string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the assessment status.
        /// </summary>
        [DataMember(Name = "assessmentStatus", IsRequired = false, EmitDefaultValue = false)]
        public AssessmentStatus AssessmentStatus { get; set; }

        /// <summary>
        /// Gets or sets the date ordered.
        /// </summary>
        [DataMember(Name = "dateOrdered", IsRequired = false, EmitDefaultValue = false)]
        public DateTime DateOrdered { get; set; }

        /// <summary>
        /// Gets or sets the date completed.
        /// </summary>
        [DataMember(Name = "dateCompleted", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? DateCompleted { get; set; }

        /// <summary>
        /// Gets or sets the Link Url for assessment.
        /// </summary>
        [DataMember(Name = "stageOrdinal", IsRequired = false, EmitDefaultValue = false)]
        public long? StageOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the Link Url for assessment.
        /// </summary>
        [DataMember(Name = "activityOrdinal", IsRequired = false, EmitDefaultValue = false)]
        public long? ActivityOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the Link Url for assessment.
        /// </summary>
        [DataMember(Name = "activitySubOrdinal", IsRequired = false, EmitDefaultValue = false)]
        public long? ActivitySubOrdinal { get; set; }
    }

    /// <summary>The job application interview.</summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    [DataContract]
    public class JobApplicationInterview
    {
        /// <summary>Gets or sets the interviewer name.</summary>
        [DataMember(Name = nameof(InterviewerName))]
        public string InterviewerName { get; set; }

        /// <summary>Gets or sets the linked in identity.</summary>
        [DataMember(Name = nameof(LinkedinIdentity))]
        public string LinkedinIdentity { get; set; }

        /// <summary>Gets or sets the start date.</summary>
        [DataMember(Name = nameof(StartDate))]
        public DateTime StartDate { get; set; }

        /// <summary>Gets or sets the end date.</summary>
        [DataMember(Name = nameof(EndDate))]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the stage ordinal.
        /// </summary>
        [DataMember(Name = "stageOrdinal", IsRequired = false, EmitDefaultValue = false)]
        public long? StageOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the activity ordinal.
        /// </summary>
        [DataMember(Name = "activityOrdinal", IsRequired = false, EmitDefaultValue = false)]
        public long? ActivityOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the activity sub ordinal.
        /// </summary>
        [DataMember(Name = "activitySubOrdinal", IsRequired = false, EmitDefaultValue = false)]
        public long? ActivitySubOrdinal { get; set; }
    }
}