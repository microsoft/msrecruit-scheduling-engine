//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobApplicationActivityAvailabilityExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class JobApplicationActivityAvailabilityExtensions
    {
        public static ScheduleAvailability ToViewModel(this JobApplicationActivityAvailability availability) => availability == null ? null : new ScheduleAvailability
        {
            StartDate = availability.Start.GetValueOrDefault(),
            EndDate = availability.End.GetValueOrDefault(),
            Id = availability.Autonumber,
            TimeZone = availability.TimeZone,
            IsCandidateAvailable = availability.IsCandidateAvailable.GetValueOrDefault(),
            IsHiringTeamAvailable = availability.IsHiringTeamAvailable.GetValueOrDefault(),
        };
    }
}
