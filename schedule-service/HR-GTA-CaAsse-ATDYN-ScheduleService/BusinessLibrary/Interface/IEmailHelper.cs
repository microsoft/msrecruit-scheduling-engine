//----------------------------------------------------------------------------
// <copyright file="IEmailHelper.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.Talent.TalentContracts.ScheduleService;

    /// <summary>
    /// Email helper definition
    /// </summary>
    public interface IEmailHelper
    {
        /// <summary>
        /// Sends a reminder to interviewer to complete the feedback.
        /// </summary>
        /// <param name="schedules">schedules information.</param>
        /// <param name="scheduleInvitationDetails">schedule invitation details</param>
        /// <param name="timezone">timezone</param>
        /// <returns>Reminder Success response.</returns>
        Task<string> GetScheduleSummary(List<MeetingInfo> schedules, ScheduleInvitationDetails scheduleInvitationDetails, Timezone timezone);

        /// <summary>
        /// Gets the schedule summary.
        /// </summary>
        /// <param name="schedules">The schedules.</param>
        /// <param name="scheduleInvitationRequest">The sinstance of <see cref="ScheduleInvitationRequest"/>.</param>
        /// <param name="timezone">The timezone.</param>
        /// <returns>The schedule summary table.</returns>
        Task<string> GetScheduleSummaryAsync(List<MeetingInfo> schedules, ScheduleInvitationRequest scheduleInvitationRequest, Timezone timezone);
    }
}
