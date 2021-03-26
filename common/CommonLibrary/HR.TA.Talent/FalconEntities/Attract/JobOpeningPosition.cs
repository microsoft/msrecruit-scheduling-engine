//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Attract
{
    using System.Runtime.Serialization;
    using HR.TA..Common.DocumentDB.Contracts;
    using System.Collections.Generic;
    using HR.TA..TalentEntities.Enum;
    using HR.TA..Common.TalentEntities.Enum.Common;

    [DataContract]
    public class JobOpeningPosition : DocDbEntity
    {
        [DataMember(Name = "JobOpeningPositionId", EmitDefaultValue = false, IsRequired = false)]
        public string JobOpeningPositionId { get; set; }

        [DataMember(Name = "Title", EmitDefaultValue = false, IsRequired = false)]
        public string Title { get; set; }

        [DataMember(Name = "PositionType", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningPositionType? PositionType { get; set; }

        [DataMember(Name = "Department", EmitDefaultValue = false, IsRequired = false)]
        public string Department { get; set; }

        [DataMember(Name = "CurrencyCode", EmitDefaultValue = false, IsRequired = false)]
        public CurrencyCode? CurrencyCode { get; set; }

        [DataMember(Name = "MinimumRemuneration", EmitDefaultValue = false, IsRequired = false)]
        public long? MinimumRemuneration { get; set; }

        [DataMember(Name = "MaximumRemuneration", EmitDefaultValue = false, IsRequired = false)]
        public long? MaximumRemuneration { get; set; }

        [DataMember(Name = "RemunerationPeriod", EmitDefaultValue = false, IsRequired = false)]
        public RenumerationPeriod? RemunerationPeriod { get; set; }

        [DataMember(Name = "JobGrade", EmitDefaultValue = false, IsRequired = false)]
        public string JobGrade { get; set; }

        [DataMember(Name = "IncentivePlan", EmitDefaultValue = false, IsRequired = false)]
        public string IncentivePlan { get; set; }

        [DataMember(Name = "RoleType", EmitDefaultValue = false, IsRequired = false)]
        public string RoleType { get; set; }

        [DataMember(Name = "SourcePositionNumber", EmitDefaultValue = false, IsRequired = false)]
        public string SourcePositionNumber { get; set; }

        [DataMember(Name = "CostCenter", EmitDefaultValue = false, IsRequired = false)]
        public string CostCenter { get; set; }

        [DataMember(Name = "CareerLevel", EmitDefaultValue = false, IsRequired = false)]
        public string CareerLevel { get; set; }

        [DataMember(Name = "JobType", EmitDefaultValue = false, IsRequired = false)]
        public string JobType { get; set; }

        [DataMember(Name = "JobFunction", EmitDefaultValue = false, IsRequired = false)]
        public string JobFunction { get; set; }

        [DataMember(Name = "JobOpeningPositionExtendedAttributes", EmitDefaultValue = false, IsRequired = false)]
        public IList<CustomAttributes> JobOpeningPositionExtendedAttributes { get; set; }

        [DataMember(Name = "JobApplications", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplication> JobApplications { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public JobPositionStatus Status { get; set; }

        [DataMember(Name = "StatusReason", EmitDefaultValue = false, IsRequired = false)]
        public JobPositionStatusReason StatusReason { get; set; }
    }
}
