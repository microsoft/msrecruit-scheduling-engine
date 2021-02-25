//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System.Collections.Generic;
    using MS.GTA.TalentEntities.Enum;
    using MS.GTA.Common.OfferManagement.Contracts.V2;

    /// <summary>
    /// Job application contract.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Gets or sets the identifier for the job application.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the activities in the job application.
        /// </summary>
        public IList<ApplicationActivity> ApplicationActivities { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the job opening.
        /// </summary>
        public string JobOpeningId { get; set; }

        /// <summary>
        /// Gets or sets the title of the job opening.
        /// </summary>
        public string JobOpeningTitle { get; set; }

        /// <summary>
        /// Gets or sets the description of the job opening.
        /// </summary>
        public string JobOpeningDescription { get; set; }

        /// <summary>
        /// Gets or sets the location of the job opening.
        /// </summary>
        public string JobOpeningLocation { get; set; }

        /// <summary>
        /// Gets or sets the full name of the candidate for the job application.
        /// </summary>
        public string CandidateFullName { get; set; }

        /// <summary>
        /// Gets or sets the given name of the candidate for the job application.
        /// </summary>
        public string CandidateGivenName { get; set; }

        /// <summary>
        /// Gets or sets the surname of the candidate for the job application.
        /// </summary>
        public string CandidateSurname { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the candidate for the job application.
        /// </summary>
        public string CandidateId { get; set; }

        /// <summary>
        /// Gets or sets the object identifier of the candidate for the job application, if the candidate is internal.
        /// </summary>
        public string CandidateOId { get; set; }

        /// <summary>
        /// Gets or sets the email of the candidate for the job application.
        /// </summary>
        public string CandidateEmail { get; set; }

        /// <summary>
        /// Gets or sets candidate's current employer.
        /// </summary>
        public string CandidateCurrentEmployer { get; set; }

        /// <summary>
        /// Gets or sets the invitation Id of the job application.
        /// </summary>
        public string InvitationId { get; set; }

        /// <summary>
        /// Gets or sets the contact preference of the candidate for the job application.
        /// </summary>
        public string PreferredPhone { get; set; }

        /// <summary>
        /// Gets or sets the postal addresss of the candidate for the job application.
        /// </summary>
        public string ResidentialPostalAddress { get; set; }

        /// <summary>
        /// Gets or sets the alternate address of the candidate for the job application.
        /// </summary>
        public string OtherPostalAddress { get; set; }

        /// <summary>
        /// Gets or sets the citizenship of the candidate for the job application.
        /// </summary>
        public string CitizenshipPrimary { get; set; }

        /// <summary>
        /// Gets or sets the worker if the candidate is internal for the job application.
        /// </summary>
        public string InternalCandidate { get; set; }

        /// <summary>
        /// Gets or sets the LinkedIn ID of the candidate for the job application.
        /// </summary>
        public string LinkedInID { get; set; }

        /// <summary>
        /// Gets or sets the Hiring Manager Name for the Job Opening.
        /// </summary>
        public string HiringManagerName { get; set; }

        /// <summary>
        /// Gets or sets the Hiring Manager Email for the Job Opening.
        /// </summary>
        public string HiringManagerEmail { get; set; }

        /// <summary>
        /// Gets or sets the Hiring Manager Title for the Job Opening.
        /// </summary>
        public string HiringManagerTitle { get; set; }

        /// <summary>
        /// Gets or sets the Hiring Manager Department for the Job Opening.
        /// </summary>
        public string HiringManagerDepartment { get; set; }

        /// <summary>
        /// Gets or sets the Recruiter Name for the Job Opening.
        /// </summary>
        public string RecruiterName { get; set; }

        /// <summary>
        /// Gets or sets the Recruiter Title for the Job Opening.
        /// </summary>
        public string RecruiterTitle { get; set; }

        /// <summary>
        /// Gets or sets the Seniority Level for the Job Opening.
        /// </summary>
        public string SeniorityLevel { get; set; }

        /// <summary>
        /// Gets or sets Employment Type for the Job Opening.
        /// </summary>
        public string EmploymentType { get; set; }

        /// <summary>
        /// Gets or sets the Job Function for the Job Opening.
        /// </summary>
        public string JobFunction { get; set; }

        /// <summary>
        /// Gets or sets the Industry for the Job Opening.
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// Gets or sets the Currency Code for the Job Opening.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the Department for the Job Opening Position.
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the Minimum Remuneration for the Job Opening Position.
        /// </summary>
        public string MinimumRemuneration { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Remuneration for the Job Opening Position.
        /// </summary>
        public string MaximumRemuneration { get; set; }

        /// <summary>
        /// Gets or sets the Remuneration Period for the Job Opening Position.
        /// </summary>
        public string RemunerationPeriod { get; set; }

        /// <summary>
        /// Gets or sets the Job Grade for the Job Opening Position.
        /// </summary>
        public string JobGrade { get; set; }

        /// <summary>
        /// Gets or sets the Incentive Plan for the Job Opening Position.
        /// </summary>
        public string IncentivePlan { get; set; }

        /// <summary>
        /// Gets or sets the Role Type for the Job Opening Position.
        /// </summary>
        public string RoleType { get; set; }

        /// <summary>
        /// Gets or sets the Source Position Number for the Job Opening Position.
        /// </summary>
        public string SourcePositionNumber { get; set; }

        /// <summary>
        /// Gets or sets the Cost Center for the Job Opening Position.
        /// </summary>
        public string CostCenter { get; set; }

        /// <summary>
        /// Gets or sets the Career Level for the Job Opening Position.
        /// </summary>
        public string CareerLevel { get; set; }

        /// <summary>
        /// Gets or sets the Job Type for the Job Opening Position.
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// Gets or sets the Description for the Job Opening.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Position Title for the Job Opening.
        /// </summary>
        public string PositionTitle { get; set; }

        /// <summary>
        /// Gets or sets the Standard Title for the Job Opening.
        /// </summary>
        public string StandardTitle { get; set; }

        /// <summary>
        /// Gets or sets the custom attributes for application
        /// </summary>
        public IList<CustomAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the Reporting Manager Name for the Job application position.
        /// </summary>
        public string ReportingManagerName { get; set; }

        /// <summary>
        /// Gets or sets the Reporting Manager Email for the Job application position.
        /// </summary>
        public string ReportingManagerEmail { get; set; }

        /// <summary>
        /// Gets or sets the Offer Notes for the Job Application
        /// </summary>
        public IList<OfferNote> OfferNotes { get; set; }

        /// <summary>
        /// Gets or sets the Job Application Id From External Source.
        /// </summary>
        public string ExternalJobApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the Job Opening Id From External Source.
        /// </summary>
        public string ExternalJobOpeningID { get; set; }

        /// <summary>
        /// Gets or sets the Job Application Status
        /// </summary>
        public JobApplicationStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the Job Application Status Reason
        /// </summary>
        public JobApplicationStatusReason? StatusReason { get; set; }

        /// <summary>
        /// Gets or sets OpeningParticipant of job opening
        /// </summary>
        public IList<OpeningParticipant> OpeningParticipants { get; set; }
    }
}
