//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOpeningStageActivityExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.XrmData.ViewModelExtensions;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class JobOpeningStageActivityExtensions
    {
        public static StageActivity ToViewModel(this JobOpeningStageActivity jobOpeningStageActivity) => jobOpeningStageActivity == null ? null : new StageActivity
        {
            ActivityType = jobOpeningStageActivity.ActivityType.GetValueOrDefault(),
            Audience = jobOpeningStageActivity.Audience.GetValueOrDefault(jobOpeningStageActivity.ActivityType, StageActivityExtensions.ExtractForCandidate(jobOpeningStageActivity?.Configuration)),
            Configuration = jobOpeningStageActivity.Configuration,
            Description = jobOpeningStageActivity.Description,
            DisplayName = jobOpeningStageActivity.DisplayLabel,
            Id = jobOpeningStageActivity.RecId.ToString(),
            IsEnableForCandidate = jobOpeningStageActivity.IsEnabledForCandidate.GetValueOrDefault(),
            Name = jobOpeningStageActivity.DisplayLabel,
            Ordinal = jobOpeningStageActivity.Ordinal.GetValueOrDefault(),
            Required = jobOpeningStageActivity.Required,
            SubOrdinal = jobOpeningStageActivity.SubOrdinal.GetValueOrDefault(),
        };
    }
}
