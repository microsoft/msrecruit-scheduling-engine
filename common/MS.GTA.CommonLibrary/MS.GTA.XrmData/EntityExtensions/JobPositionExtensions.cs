//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobPositionExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.Util;
    using MS.GTA.Data.Utils;
    using MS.GTA.TalentEntities.Enum;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    /// <summary>The entity extensions.</summary>
    public static class JobPositionExtensions
    {
        private const string CareerLevelKey = "CareerLevel";
        private const string CostCenterKey = "CostCenter";
        private const string CurrencyCodeKey = "CurrencyCode";
        private const string IncentivePlanKey = "IncentivePlan";
        private const string JobGradeKey = "JobGrade";
        private const string MaximumRemunerationKey = "MaximumRemuneration";
        private const string MinimumRemunerationKey = "MinimumRemuneration";
        private const string RemunerationPeriodKey = "RemunerationPeriod";
        private const string RoleTypeKey = "RoleType";

        /// <summary>Convert a job position to a view model.</summary>
        /// <param name="jobPosition">The job post.</param>
        /// <returns>The <see cref="JobOpeningPosition"/>.</returns>
        public static JobOpeningPosition ToViewModel(this JobPosition jobPosition)
        {
            if (jobPosition == null)
            {
                return null;
            }

            var additionalMetadata = jobPosition.AdditionalMetadata == null ?
                new System.Collections.Generic.Dictionary<string, string>() :
                AdditionalMetadataExtensions.DeserializeStringOnlyAdditionalMetadata(jobPosition.AdditionalMetadata);

            var currencyCode = additionalMetadata.ContainsKey(CurrencyCodeKey) ? additionalMetadata[CurrencyCodeKey].TryParseAsEnum<TalentEntities.Enum.Common.CurrencyCode>() : null;
            long.TryParse(additionalMetadata.ContainsKey(MaximumRemunerationKey) ? additionalMetadata[MaximumRemunerationKey] : null, out long maximumRemuneration);
            long.TryParse(additionalMetadata.ContainsKey(MinimumRemunerationKey) ? additionalMetadata[MinimumRemunerationKey] : null, out long minimumRemuneration);
            var remunerationPeriod = additionalMetadata.ContainsKey(RemunerationPeriodKey) ? additionalMetadata[RemunerationPeriodKey].TryParseAsEnum<RenumerationPeriod>() : null;

            return new JobOpeningPosition
            {
                JobId = jobPosition.Job?.Autonumber,
                JobOpeningPositionId = jobPosition.Autonumber,
                SourcePositionNumber = jobPosition.Autonumber,
                JobFunction = jobPosition.Job?.JobFunction?.Name,
                JobType = jobPosition.Job?.JobType?.Description,
                PositionType = jobPosition.PositionType?.Description?.TryParseAsEnum<JobOpeningPositionType>(),
                Title = jobPosition.Description,
                UserAction = UserAction.None,
                JobIds = jobPosition.JobOpenings?.Select(jo => jo.Autonumber)?.ToList(),
                ReferenceJobOpeningIds = jobPosition.JobOpenings?.Select(jo => jo.Autonumber)?.ToList(),
                ReferenceApplicationIds = jobPosition.JobApplications?.Select(ja => ja.Autonumber)?.ToList(),
                Department = jobPosition.Department?.Name,
                Status = jobPosition.Status.ToStatus(),
                StatusReason = jobPosition.StatusReason.ToStatusReason(),
                ExtendedAttributes = additionalMetadata,
                CareerLevel = additionalMetadata.ContainsKey(CareerLevelKey) ? additionalMetadata[CareerLevelKey] : null,
                CostCenter = additionalMetadata.ContainsKey(CostCenterKey) ? additionalMetadata[CostCenterKey] : null,
                CurrencyCode = currencyCode,
                IncentivePlan = additionalMetadata.ContainsKey(IncentivePlanKey) ? additionalMetadata[IncentivePlanKey] : null,
                JobGrade = additionalMetadata.ContainsKey(JobGradeKey) ? additionalMetadata[JobGradeKey] : null,
                MaximumRemuneration = maximumRemuneration,
                MinimumRemuneration = minimumRemuneration,
                RemunerationPeriod = remunerationPeriod,
                RoleType = additionalMetadata.ContainsKey(RoleTypeKey) ? additionalMetadata[RoleTypeKey] : null,
                CustomFields = jobPosition.GetCustomFields(),
            };
        }

        public static JobPositionStatus ToStatus(this Status source)
        {
            switch (source)
            {
                case Status.Inactive: return JobPositionStatus.Inactive;
                default:
                    return JobPositionStatus.Active;
            }
        }

        public static JobPositionStatusReason ToStatusReason(this StatusReason source)
        {
            switch (source)
            {
                case StatusReason.Inactive: return JobPositionStatusReason.Closed;
                default:
                    return JobPositionStatusReason.Open;
            }
        }

    }
}
