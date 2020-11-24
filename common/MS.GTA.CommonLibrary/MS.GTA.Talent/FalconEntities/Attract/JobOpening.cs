//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.DocumentDB.Contracts;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.Talent.FalconEntities;
    using MS.GTA.Talent.FalconEntities.Attract;
    using MS.GTA.TalentEntities.Enum;

    [DataContract]
    public class JobOpening : DocDbEntity
    {
        [DataMember(Name = "JobOpeningId", EmitDefaultValue = false, IsRequired = false)]
        public string JobOpeningID { get; set; }

        [DataMember(Name = "PositionTitle", EmitDefaultValue = false, IsRequired = false)]
        public string PositionTitle { get; set; }

        [DataMember(Name = "Description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "NumberOfOpenings", EmitDefaultValue = false, IsRequired = false)]
        public long? NumberOfOpenings { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningStatus? Status { get; set; }

        [DataMember(Name = "StatusReason", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningStatusReason? StatusReason { get; set; }

        [DataMember(Name = "PositionLocation", EmitDefaultValue = false, IsRequired = false)]
        public string PositionLocation { get; set; }

        [DataMember(Name = "PostalAddress", EmitDefaultValue = false, IsRequired = false)]
        public Address PostalAddress { get; set; }

        [DataMember(Name = "ApplyURI", EmitDefaultValue = false, IsRequired = false)]
        public string ApplyURI { get; set; }

        [DataMember(Name = "SeniorityLevel", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningSeniorityLevel? SeniorityLevel { get; set; }

        [DataMember(Name = "TravelPercentage", EmitDefaultValue = false, IsRequired = false)]
        public long? TravelPercentage { get; set; }

        [DataMember(Name = "CompanyIndustry", EmitDefaultValue = false, IsRequired = false)]
        public string CompanyIndustry { get; set; }

        [DataMember(Name = "JobSkills", EmitDefaultValue = false, IsRequired = false)]
        public string JobSkills { get; set; }

        [DataMember(Name = "JobSeniorityLevel", EmitDefaultValue = false, IsRequired = false)]
        public string JobSeniorityLevel { get; set; }

        [DataMember(Name = "EmploymentType", EmitDefaultValue = false, IsRequired = false)]
        public string EmploymentType { get; set; }

        [DataMember(Name = "JobFunction", EmitDefaultValue = false, IsRequired = false)]
        public string JobFunction { get; set; }

        [DataMember(Name = "Responsibilities", EmitDefaultValue = false, IsRequired = false)]
        public string Responsibilities { get; set; }

        [DataMember(Name = "ExternalJobOpeningSource", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningExternalSource? ExternalJobOpeningSource { get; set; }

        [DataMember(Name = "ExternalJobOpeningID", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalJobOpeningID { get; set; }

        [DataMember(Name = "JobOpeningVisibility", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningVisibility? JobOpeningVisibility { get; set; }

        [DataMember(Name = "ExternalStatus", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalStatus { get; set; }

        [DataMember(Name = "PrimaryPositionID", EmitDefaultValue = false, IsRequired = false)]
        public string PrimaryPositionID { get; set; }

        [DataMember(Name = "QualificationSummary", EmitDefaultValue = false, IsRequired = false)]
        public string QualificationSummary { get; set; }

        [DataMember(Name = "PositionStartDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? PositionStartDate { get; set; }

        [DataMember(Name = "PositionEndDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? PositionEndDate { get; set; }

        [DataMember(Name = "ApplicationStartDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ApplicationStartDate { get; set; }

        [DataMember(Name = "ApplicationCloseDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ApplicationCloseDate { get; set; }

        [DataMember(Name = "Comment", EmitDefaultValue = false, IsRequired = false)]
        public string Comment { get; set; }

        [DataMember(Name = "IsTemplate", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsTemplate { get; set; }

        [DataMember(Name = "JobOpeningParticipants", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobOpeningParticipant> JobOpeningParticipants { get; set; }

        [DataMember(Name = "JobApplications", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplication> JobApplications { get; set; }

        [DataMember(Name = "JobOpeningPositions", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobOpeningPosition> JobOpeningPositions { get; set; }

        [DataMember(Name = "JobOpeningExtendedAttributes", EmitDefaultValue = false, IsRequired = false)]
        public IList<CustomAttributes> JobOpeningExtendedAttributes { get; set; }

        [DataMember(Name = "IsSyncedWithLinkedIn", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsSyncedWithLinkedIn { get; set; }

        [DataMember(Name = "IsHiringTeamSyncedWithLinkedIn", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsHiringTeamSyncedWithLinkedIn { get; set; }

        [DataMember(Name = "Configuration", EmitDefaultValue = false, IsRequired = false)]
        public string Configuration { get; set; }

        [DataMember(Name = "JobActivationDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? JobActivationDate { get; set; }

        [DataMember(Name = "Priority", EmitDefaultValue = false, IsRequired = false)]
        public string Priority { get; set; }

    }
}
