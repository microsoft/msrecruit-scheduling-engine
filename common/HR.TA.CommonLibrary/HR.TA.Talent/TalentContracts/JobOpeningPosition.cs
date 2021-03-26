//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.Contracts;
    using HR.TA.TalentEntities.Common;
    using HR.TA.TalentEntities.Enum;
    using HR.TA.TalentEntities.Enum.Common;

    /// <summary>
    /// The job position data contract.
    /// </summary>
    [DataContract]
    public class JobOpeningPosition : TalentBaseContract
    {
        /// <summary>
        /// Gets or sets JobOpeningPositionId.
        /// </summary>
        [DataMember(Name = "jobOpeningPositionId", IsRequired = false, EmitDefaultValue = false)]
        public string JobOpeningPositionId { get; set; }

        /// <summary>
        /// Gets or sets JobId.
        /// </summary>
        [DataMember(Name = "jobId", IsRequired = false, EmitDefaultValue = false)]
        public string JobId { get; set; }

        /// <summary>
        /// Gets or sets a list of JobId.
        /// </summary>
        [DataMember(Name = "jobIds", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> JobIds { get; set; }

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        [DataMember(Name = "title", IsRequired = false, EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets User action.
        /// </summary>
        [DataMember(Name = "userAction", IsRequired = false, EmitDefaultValue = false)]
        public UserAction UserAction { get; set; }

        /// <summary>
        /// Gets or sets CareerLevel.
        /// </summary>
        [DataMember(Name = "careerLevel", IsRequired = false, EmitDefaultValue = false)]
        public string CareerLevel { get; set; }

        /// <summary>
        /// Gets or sets CostCenter.
        /// </summary>
        [DataMember(Name = "costCenter", IsRequired = false, EmitDefaultValue = false)]
        public string CostCenter { get; set; }

        /// <summary>
        /// Gets or sets SourcePositionNumber.
        /// </summary>
        [DataMember(Name = "sourcePositionNumber", IsRequired = false, EmitDefaultValue = false)]
        public string SourcePositionNumber { get; set; }

        /// <summary>
        /// Gets or sets RoleType.
        /// </summary>
        [DataMember(Name = "roleType", IsRequired = false, EmitDefaultValue = false)]
        public string RoleType { get; set; }

        /// <summary>
        /// Gets or sets IncentivePlan.
        /// </summary>
        [DataMember(Name = "incentivePlan", IsRequired = false, EmitDefaultValue = false)]
        public string IncentivePlan { get; set; }

        /// <summary>
        /// Gets or sets JobGrade.
        /// </summary>
        [DataMember(Name = "jobGrade", IsRequired = false, EmitDefaultValue = false)]
        public string JobGrade { get; set; }

        /// <summary>
        /// Gets or sets RemunerationPeriod.
        /// </summary>
        [DataMember(Name = "remunerationPeriod", IsRequired = false, EmitDefaultValue = false)]
        public RenumerationPeriod? RemunerationPeriod { get; set; }

        /// <summary>
        /// Gets or sets MaximumRemuneration.
        /// </summary>
        [DataMember(Name = "maximumRemuneration", IsRequired = false, EmitDefaultValue = false)]
        public long? MaximumRemuneration { get; set; }

        /// <summary>
        /// Gets or sets MinimumRemuneration.
        /// </summary>
        [DataMember(Name = "minimumRemuneration", IsRequired = false, EmitDefaultValue = false)]
        public long? MinimumRemuneration { get; set; }

        /// <summary>
        /// Gets or sets CurrencyCode.
        /// </summary>
        [DataMember(Name = "currencyCode", IsRequired = false, EmitDefaultValue = false)]
        public CurrencyCode? CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets Department.
        /// </summary>
        [DataMember(Name = "department", IsRequired = false, EmitDefaultValue = false)]
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets PositionType.
        /// </summary>
        [DataMember(Name = "positionType", IsRequired = false, EmitDefaultValue = false)]
        public JobOpeningPositionType? PositionType { get; set; }

        /// <summary>
        /// Gets or sets JobType.
        /// </summary>
        [DataMember(Name = "jobType", IsRequired = false, EmitDefaultValue = false)]
        public string JobType { get; set; }

        /// <summary>
        /// Gets or sets JobFunction.
        /// </summary>
        [DataMember(Name = "jobFunction", IsRequired = false, EmitDefaultValue = false)]
        public string JobFunction { get; set; }

        /// <summary>
        /// Gets or sets job Id collection, which has reference to this job position
        /// </summary>
        [DataMember(Name = "referenceJobOpeningIds", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> ReferenceJobOpeningIds { get; set; }

        /// <summary>
        /// Gets or sets application Id collection, which has reference to this job position
        /// </summary>
        [DataMember(Name = "referenceApplicationIds", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> ReferenceApplicationIds { get; set; }

        /// <summary>
        /// Gets or sets ExtendedAttributes.
        /// </summary>
        [DataMember(Name = "extendedAttributes", IsRequired = false, EmitDefaultValue = false)]
        public Dictionary<string, string> ExtendedAttributes { get; set; }

        /// <summary>
        /// Gets or sets ReportsTo.
        /// </summary>
        [DataMember(Name = "reportsTo", IsRequired = false, EmitDefaultValue = false)]
        public Worker ReportsTo { get; set; }

        /// <summary>
        /// Gets or sets status of the job position.
        /// </summary>
        [DataMember(Name = "status", IsRequired = false, EmitDefaultValue = false)]
        public JobPositionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets status reason of the job position.
        /// </summary>
        [DataMember(Name = "statusReason", IsRequired = false, EmitDefaultValue = false)]
        public JobPositionStatusReason StatusReason { get; set; }
    }
}
