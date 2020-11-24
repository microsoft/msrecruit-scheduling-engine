//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOpeningStageExtensions.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public static class JobOpeningStageExtensions
    {
        public static ApplicationStage ToViewModel(this JobOpeningStage jobOpeningStage, bool isActiveStage = false) => jobOpeningStage == null ? null : new ApplicationStage
        {
            ////Stage = jobOpeningStage.JobStage.GetValueOrDefault(),
            Order = jobOpeningStage.Ordinal.GetValueOrDefault(),
            DisplayName = jobOpeningStage.DisplayName,
            Description = jobOpeningStage.Description,
            IsActiveStage = isActiveStage,
            StageActivities = jobOpeningStage.JobOpeningStageActivities?.Where(r => r != null).Select(r => r.ToViewModel()).OrderBy(r => r.Ordinal).ToList(),
            CustomFields = jobOpeningStage.GetCustomFields()
        };
    }
}
