//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..Common.Contracts;
    using HR.TA..Common.TalentEntities.Common;
    using HR.TA..TalentEntities.Enum;

    /// <summary>
    /// The job data contract.
    /// </summary>
    [DataContract]
    public class Job : TalentBaseContract
    {
        /// <summary>Gets or sets id.</summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>Gets or sets title.</summary>
        [DataMember(Name = "title", IsRequired = false)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets job description.
        /// </summary>
        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets number of openings for job.
        /// </summary>
        [DataMember(Name = "numberOfOpenings", IsRequired = false, EmitDefaultValue = false)]
        public long? NumberOfOpenings { get; set; }

        /// <summary>
        /// Gets or sets number of offers for job.
        /// </summary>
        [DataMember(Name = "numberOfOffers", IsRequired = false, EmitDefaultValue = false)]
        public long? NumberOfOffers { get; set; }

        /// <summary>
        /// Gets or sets job status.
        /// </summary>
        [DataMember(Name = "status", IsRequired = false)]
        public JobOpeningStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets job status reason.
        /// </summary>
        [DataMember(Name = "statusReason", IsRequired = false)]
        public JobOpeningStatusReason? StatusReason { get; set; }

        /// <summary>
        /// Gets or sets job location.
        /// </summary>
        [DataMember(Name = "location", IsRequired = false, EmitDefaultValue = false)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets external job opening source.
        /// </summary>
        [DataMember(Name = "source", IsRequired = false)]
        public JobOpeningExternalSource Source { get; set; }

        /// <summary>
        /// Gets or sets external job opening Id.
        /// </summary>
        [DataMember(Name = "externalId", IsRequired = false, EmitDefaultValue = false)]
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets job external status.
        /// </summary>
        [DataMember(Name = "externalStatus", IsRequired = false)]
        public string ExternalStatus { get; set; }

        /// <summary>
        /// Gets or sets start date.
        /// </summary>
        [DataMember(Name = "createdDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets job comment.
        /// </summary>
        [DataMember(Name = "comment", IsRequired = false, EmitDefaultValue = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets job position Id
        /// </summary>
        [DataMember(Name = "primaryPositionID", IsRequired = false, EmitDefaultValue = false)]
        public string PrimaryPositionID { get; set; }

        /// <summary>
        /// Gets or sets job position uri
        /// </summary>
        [DataMember(Name = "positionURI", IsRequired = false, EmitDefaultValue = false)]
        public string PositionURI { get; set; }

        /// <summary>
        /// Gets or sets job position uri
        /// </summary>
        [DataMember(Name = "applyURI", IsRequired = false, EmitDefaultValue = false)]
        public string ApplyURI { get; set; }

        /// <summary>
        /// Gets or sets job skills
        /// </summary>
        [DataMember(Name = "skills", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> Skills { get; set; }

        /// <summary>
        /// Gets or sets job seniority level
        /// </summary>
        [DataMember(Name = "seniorityLevel", IsRequired = false, EmitDefaultValue = false)]
        public string SeniorityLevel { get; set; }

        /// <summary>
        /// Gets or sets job seniority level
        /// </summary>
        [DataMember(Name = "seniorityLevelValue", IsRequired = false, EmitDefaultValue = false)]
        public OptionSetValue SeniorityLevelValue { get; set; }

        /// <summary>
        /// Gets or sets job employment type
        /// </summary>
        [DataMember(Name = "employmentType", IsRequired = false, EmitDefaultValue = false)]
        public string EmploymentType { get; set; }

        /// <summary>
        /// Gets or sets job employment type
        /// </summary>
        [DataMember(Name = "employmentTypeValue", IsRequired = false, EmitDefaultValue = false)]
        public OptionSetValue EmploymentTypeValue { get; set; }

        /// <summary>
        /// Gets or sets job functions
        /// </summary>
        [DataMember(Name = "jobFunctions", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> JobFunctions { get; set; }

        /// <summary>
        /// Gets or sets job company industries
        /// </summary>
        [DataMember(Name = "companyIndustries", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> CompanyIndustries { get; set; }

        /// <summary>
        /// Gets or sets job position uri
        /// </summary>
        [DataMember(Name = "isTemplate", IsRequired = false, EmitDefaultValue = false)]
        public bool IsTemplate { get; set; }

        /// <summary>
        /// Gets or sets job position uri
        /// </summary>
        [DataMember(Name = "jobTemplate", IsRequired = false, EmitDefaultValue = false)]
        public JobTemplate JobTemplate { get; set; }

        /// <summary>
        /// Gets or sets job stages.
        /// </summary>
        [DataMember(Name = "stages", IsRequired = false, EmitDefaultValue = false)]
        public IList<ApplicationStage> Stages { get; set; }

        /// <summary>
        /// Gets or sets job hiring team.
        /// </summary>
        [DataMember(Name = "hiringTeam", IsRequired = false, EmitDefaultValue = false)]
        public IList<HiringTeamMember> HiringTeam { get; set; }

        /// <summary>
        /// Gets or sets job applications.
        /// </summary>
        [DataMember(Name = "applications", IsRequired = false, EmitDefaultValue = false)]
        public IList<Application> Applications { get; set; }

        /// <summary>
        /// Gets or sets job external post.
        /// </summary>
        [DataMember(Name = "externalJobPost", IsRequired = false, EmitDefaultValue = false)]
        public IList<ExternalJobPost> ExternalJobPosts { get; set; }

        /// <summary>Gets or sets participants.</summary>
        [DataMember(Name = "templateParticipants", IsRequired = false)]
        public IList<JobOpeningTemplateParticipant> TemplateParticipants { get; set; }

        /// <summary>
        /// Gets or sets job external post.
        /// </summary>
        [DataMember(Name = "jobOpeningVisibility", IsRequired = false, EmitDefaultValue = false)]
        public JobOpeningVisibility? JobOpeningVisibility { get; set; }

        /// <summary>Gets or sets job opening positions.</summary>
        [DataMember(Name = "jobOpeningPositions", IsRequired = false, EmitDefaultValue = false)]
        public IList<JobOpeningPosition> JobOpeningPositions { get; set; }

        /// <summary>
        /// Gets or sets Position Start Date.
        /// </summary>
        [DataMember(Name = "positionStartDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? PositionStartDate { get; set; }

        /// <summary>
        /// Gets or sets Position End Date.
        /// </summary>
        [DataMember(Name = "positionEndDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? PositionEndDate { get; set; }

        /// <summary>
        /// Gets or sets Application Start Date.
        /// </summary>
        [DataMember(Name = "applicationStartDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? ApplicationStartDate { get; set; }

        /// <summary>
        /// Gets or sets Application Close Date.
        /// </summary>
        [DataMember(Name = "applicationCloseDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? ApplicationCloseDate { get; set; }

        /// <summary>
        /// Gets or sets Postal Address.
        /// </summary>
        [DataMember(Name = "postalAddress", IsRequired = false, EmitDefaultValue = false)]
        public Address PostalAddress { get; set; }

        /// <summary>
        /// Gets or sets ExtendedAttributes.
        /// </summary>
        [DataMember(Name = "extendedAttributes", IsRequired = false, EmitDefaultValue = false)]
        public Dictionary<string, string> ExtendedAttributes { get; set; }

        /// <summary>
        /// Gets or sets configuration.
        /// </summary>
        [DataMember(Name = "configuration", IsRequired = false, EmitDefaultValue = false)]
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets job approval participants.
        /// </summary>
        [DataMember(Name = "approvalParticipants", IsRequired = false, EmitDefaultValue = false)]
        public IList<JobApprovalParticipant> ApprovalParticipants { get; set; }

        /// <summary>
        /// Gets or sets delegates from the hiring team
        /// </summary>
        [DataMember(Name = "delegates", IsRequired = false, EmitDefaultValue = false)]
        public IList<Delegate> Delegates { get; set; }

        /// <summary>
        /// Gets or sets the list of permissions to the job for the calling user.
        /// </summary>
        [DataMember(Name = "userPermissions", IsRequired = false, EmitDefaultValue = false)]
        public IList<JobPermission> UserPermissions { get; set; }

        /// <summary>
        /// Gets or sets the priority for the job like internal/external/both.
        /// </summary>
        [DataMember(Name = "priority", IsRequired = false, EmitDefaultValue = false)]
        public string Priority { get; set; }
    }
}
