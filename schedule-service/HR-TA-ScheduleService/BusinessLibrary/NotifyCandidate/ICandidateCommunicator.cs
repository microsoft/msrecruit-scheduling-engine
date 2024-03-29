//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.BusinessLibrary.NotifyCandidate
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HR.TA.Common.Provisioning.Entities.FalconEntities.Attract;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.Talent.TalentContracts.ScheduleService;

    /// <summary>
    /// The <see cref="ICandidateCommunicator"/> provides mechanism to communicate with candidate.
    /// </summary>
    public interface ICandidateCommunicator
    {
        /// <summary>
        /// Sends the invitation asynchronously.
        /// </summary>
        /// <param name="jobApplication">The instance of <see cref="JobApplication"/>.</param>
        /// <param name="scheduleInvitationRequest">The instance of <see cref="ScheduleInvitationRequest"/>.</param>
        /// <param name="schedules">The instance of <see cref="List{MeetingInfo}"/>.</param>
        /// <returns>The instance of <see cref="Task{Boolean}"/> with <c>true</c> if invitation is sent; otherwise <c>false</c>.</returns>
        Task<bool> SendInvitationAsync(JobApplication jobApplication, ScheduleInvitationRequest scheduleInvitationRequest, List<MeetingInfo> schedules);
    }
}
