//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

// Note: This namespace needs to stay the same since the docdb collection name depends on it
namespace CommonLibrary.TalentEngagementService.Data.Candidates.DocumentDB
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.TalentAttract.Contract;
    using CommonLibrary.TalentEntities.Enum;

    /// <summary> Job Application Invitation </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [DataContract]
    public class JobApplicationInvitation
    {
        /// <summary>Gets or sets the id.</summary>
        [DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary>Gets or sets the invitation token.</summary>
        [DataMember(Name = nameof(InvitationToken))]
        public InvitationToken InvitationToken { get; set; }

        /// <summary>Gets or sets the object id.</summary>
        [DataMember(Name = nameof(ObjectID))]
        public string ObjectID { get; set; }

        /// <summary>Gets or sets the identity provider.</summary>
        [DataMember(Name = nameof(IdentityProvider))]
        public string IdentityProvider { get; set; }

        /// <summary>Gets or sets the identity provider.</summary>
        [DataMember(Name = nameof(IdentityProviderUserName))]
        public string IdentityProviderUserName { get; set; }

        /// <summary>Gets or sets the application data.</summary>
        [DataMember(Name = nameof(ApplicationData))]
        public List<ApplicationData> ApplicationData { get; set; }
    }

    /// <summary>Application Data</summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [DataContract]
    public class ApplicationData
    {
        /// <summary>Gets or sets the id.</summary>
        [DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary>Gets or sets the tenant id.</summary>
        [DataMember(Name = nameof(TenantID))]
        public string TenantID { get; set; }

        /// <summary>Gets or sets the environment id.</summary>
        [DataMember(Name = nameof(EnvironmentID))]
        public string EnvironmentID { get; set; }

        /// <summary>Gets or sets the job application id.</summary>
        [DataMember(Name = nameof(JobApplicationID))]
        public string JobApplicationID { get; set; }

        /// <summary>Gets or sets the job opening id.</summary>
        [DataMember(Name = nameof(JobOpeningID), EmitDefaultValue = false)]
        public string JobOpeningID { get; set; }

        /// <summary>Gets or sets the application name.</summary>
        [DataMember(Name = nameof(ApplicationName))]
        public string ApplicationName { get; set; }

        /// <summary>Gets or sets the display data.</summary>
        [DataMember(Name = nameof(DisplayData))]
        public DisplayData DisplayData { get; set; }
    }

    /// <summary>Display Data</summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [DataContract]
    public class DisplayData
    {
        /// <summary>Gets or sets the id.</summary>
        [DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary>Gets or sets the job title.</summary>
        [DataMember(Name = nameof(JobTitle))]
        public string JobTitle { get; set; }

        /// <summary>Gets or sets the job location.</summary>
        [DataMember(Name = nameof(JobLocation))]
        public string JobLocation { get; set; }

        /// <summary>Gets or sets the company Name.</summary>
        [DataMember(Name = nameof(CompanyName))]
        public string CompanyName { get; set; }

        /// <summary>Gets or sets the job application status.</summary>
        [DataMember(Name = nameof(JobApplicationStatus))]
        public string JobApplicationStatus { get; set; }

        /// <summary>Gets or sets the job application days since.</summary>
        [DataMember(Name = nameof(JobApplicationDate))]
        public DateTime JobApplicationDate { get; set; }

        /// <summary>Gets or sets the job description.</summary>
        [DataMember(IsRequired = false, Name = nameof(JobDescription))]
        public string JobDescription { get; set; }

        /// <summary>Gets or sets the job description.</summary>
        [DataMember(IsRequired = false, Name = nameof(JobPostLink))]
        public IList<string> JobPostLink { get; set; }

        /// <summary>Gets or sets the current job stage.</summary>
        [DataMember(IsRequired = false, Name = nameof(CurrentJobStage))]
        public JobStage CurrentJobStage { get; set; }

        /// <summary>Gets or sets current application stage.</summary>
        [DataMember(IsRequired = false, Name = nameof(CurrentApplicationStage))]
        public ApplicationStage CurrentApplicationStage { get; set; }
    }

    /// <summary>Display Data</summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [DataContract]
    public class CompanyDisplayData
    {
        /// <summary>Gets or sets the tenant id.</summary>
        [DataMember(Name = "tenantId")]
        public string TenantId { get; set; }

        /// <summary>Gets or sets the company Name.</summary>
        [DataMember(Name = nameof(CompanyName))]
        public string CompanyName { get; set; }

        /// <summary>Gets or sets the company Name.</summary>
        [DataMember(IsRequired = false, Name = nameof(CompanyAlias))]
        public string CompanyAlias { get; set; }

        /// <summary>Gets or sets the job application status.</summary>
        [DataMember(Name = nameof(CompanyLocation))]
        public string CompanyLocation { get; set; }

        [DataMember(IsRequired = false, Name = "ImageUrls")]
        public IList<string> ImageUrls { get; set; }

        [DataMember(IsRequired = false, Name = nameof(AutoNumbers))]
        public IList<string> AutoNumbers { get; set; }

        [DataMember(IsRequired = false, Name = nameof(Environments))]
        public List<EnvironmentMetadata> Environments { get; set; }
    }

    /// <summary>The invitation token.</summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [DataContract]
    public class InvitationToken
    {
        /// <summary> Gets or sets the id. </summary>
        [DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary> Gets or sets the value. </summary>
        [DataMember(Name = nameof(Value))]
        public string Value { get; set; }

        /// <summary> Gets or sets the expiry date time. </summary>
        [DataMember(Name = nameof(ExpiryDateTime))]
        public DateTime ExpiryDateTime { get; set; }
    }
}
