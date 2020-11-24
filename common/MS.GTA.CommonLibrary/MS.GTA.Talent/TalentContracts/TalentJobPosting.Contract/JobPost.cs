//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobPost.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.TalentJobPosting.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The job post data contract.
    /// </summary>
    [DataContract]
    public class JobPost
    {
        /// <summary>Gets or sets id.</summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>Gets or sets autoNumber.</summary>
        [DataMember(Name = "autoNumber", IsRequired = false)]
        public string AutoNumber { get; set; }

        /// <summary>Gets or sets version.</summary>
        [DataMember(Name = "version", IsRequired = false)]
        public int Version { get; set; }

        /// <summary> Gets or sets external job opening Id. </summary>
        [DataMember(Name = "externalId", IsRequired = false)]
        public string ExternalId { get; set; }

        /// <summary>Gets or sets the tenant id.</summary>
        [DataMember(Name = "tenantId", IsRequired = false)]
        public string TenantId { get; set; }

        /// <summary>Gets or sets the environment id.</summary>
        [DataMember(Name = "environmentId", IsRequired = false)]
        public string EnvironmentId { get; set; }

        /// <summary>Gets or sets title.</summary>
        [DataMember(Name = "title", IsRequired = true)]
        public string Title { get; set; }
        // TODO
        /*
        /// <summary>Gets or sets the company details.</summary>
        [DataMember(Name = "companyDetails", IsRequired = false)]
        public CompanyDetails CompanyDetails { get; set; }*/

        /// <summary> Gets or sets job description. </summary>
        [DataMember(Name = "description", IsRequired = false)]
        public string Description { get; set; }
        // TODO
        
        /// <summary>Gets or sets the job post visibility.</summary>
        [DataMember(Name = "jobPostVisibility", IsRequired = false)]
        public JobPostVisibility JobPostVisibility { get; set; }

        /// <summary> Gets or sets the apply JobPostURI. </summary>
        [DataMember(Name = "applyURI", IsRequired = false)]
        public string ApplyURI { get; set; }

        /// <summary>Gets or sets a value indicating whether is apply hosted on talent.</summary>
        [DataMember(Name = "isApplyHostedOnTalent", IsRequired = false)]
        public bool IsApplyHostedOnTalent { get; set; }

        /// <summary> Gets or sets number of openings for job.</summary>
        [DataMember(Name = "numberOfOpenings", IsRequired = false)]
        public long? NumberOfOpenings { get; set; }

        /// <summary> Gets or sets job status. </summary>
        [DataMember(Name = "status", IsRequired = false)]
        public JobOpeningStatus Status { get; set; }

        /// <summary> Gets or sets job status reason. </summary>
        [DataMember(Name = "statusReason", IsRequired = false)]
        public JobOpeningStatusReason StatusReason { get; set; }

        /// <summary> Gets or sets job location. </summary>
        [DataMember(Name = "location", IsRequired = false)]
        public string Location { get; set; }

        /// <summary> Gets or sets external job opening source. </summary>
        [DataMember(Name = "source", IsRequired = false)]
        public string Source { get; set; }

        /// <summary> Gets or sets start date. </summary>
        [DataMember(Name = "postingDate", IsRequired = false)]
        public DateTime? PostingDate { get; set; }

        /// <summary> Gets or sets job hiring team. </summary>
        [DataMember(Name = "hiringTeam", IsRequired = false)]
        public IList<HiringTeamMember> HiringTeam { get; set; }

        /// <summary>Gets or sets the department.</summary>
        [DataMember(Name = "department", IsRequired = false)]
        public string Department { get; set; }

        /// <summary>Gets or sets the employment type.</summary>
        [DataMember(Name = "employmentType", IsRequired = false)]
        public string EmploymentType { get; set; }

        /// <summary>Gets or sets the seniority level.</summary>
        [DataMember(Name = "seniorityLevel", IsRequired = false)]
        public string SeniorityLevel { get; set; }

        /// <summary>Gets or sets the skills.</summary>
        [DataMember(Name = "skills", IsRequired = false)]
        public IList<string> Skills { get; set; }

        /// <summary>Gets or sets the job functions.</summary>
        [DataMember(Name = "jobFunctions", IsRequired = false)]
        public IList<string> JobFunctions { get; set; }

        /// <summary>Gets or sets the company industry.</summary>
        [DataMember(Name = "companyIndustry", IsRequired = false)]
        public IList<string> CompanyIndustry { get; set; }

        /// <summary> Gets or sets Postal Address. </summary>
        [DataMember(Name = "postalAddress", IsRequired = false)]
        public Address PostalAddress { get; set; }

        /// <summary> Gets or sets the last time this JobPost was modified (epoch seconds UTC) </summary>
        [DataMember(Name = "lastModifiedDateTime", IsRequired = false)]
        public long LastModifiedDateTime { get; set; }

        /// <summary> Gets or sets create date. </summary>
        [DataMember(Name = "createDate", IsRequired = false)]
        public DateTime? CreateDate { get; set; }
               
        /// <summary>Gets or sets whether resume is required for an applicant to apply for a job.</summary>
        [DataMember(Name = "enforceApplicantForResume", IsRequired = false)]
        public bool EnforceApplicantForResume { get; set; }
    }
}