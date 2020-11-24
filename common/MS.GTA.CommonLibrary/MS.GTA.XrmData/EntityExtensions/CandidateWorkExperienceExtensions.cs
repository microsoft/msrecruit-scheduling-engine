//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateWorkExperienceExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.Util;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class CandidateWorkExperienceExtensions
    {
        public static WorkExperience ToViewModel(this CandidateWorkExperience experience) => experience == null ? null : new WorkExperience()
        {
            WorkExperienceId = experience.CandidateWorkExperienceID,
            Title = experience.Title,
            Organization = experience.Organization,
            Location = experience.Location,
            Description = experience.Description,
            IsCurrentPosition = experience.IsCurrentPosition,
            FromMonthYear = experience.FromMonthYear,
            ToMonthYear = experience.ToMonthYear,
            CustomFields = experience.GetCustomFields(),
        };
    }
}
