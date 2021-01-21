//----------------------------------------------------------------------------
// <copyright file="IScheduleManager.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.Talent.EnumSetModel;
    using MS.GTA.Talent.TalentContracts.ScheduleService;

    /// <summary>
    /// IScheduleManager interface
    /// </summary>
    public interface IScheduleManager
    {
        /// <summary>
        /// Gets a list of free busy schedule .
        /// </summary>
        /// <param name="freeBusyRequest">The free time request information.</param>
        /// <returns>The free meeting time response.</returns>
        Task<List<MeetingInfo>> GetFreeBusySchedule(FreeBusyRequest freeBusyRequest);

        /// <summary>
        /// Gets a list of free busy schedule .
        /// </summary>
        /// <param name="freeBusyRequest">The free time request information.</param>
        /// <param name="jobApplicationId">job applicationid</param>
        /// <returns>The free meeting time response.</returns>
        Task<List<MeetingInfo>> GetSchedulesByFreeBusyRequest(FreeBusyRequest freeBusyRequest, string jobApplicationId);

        /// <summary>
        /// Send Calendar Event
        /// </summary>
        /// <param name="scheduleIds">schedule id to be processed</param>
        /// <param name="serviceAccountName">Service Account Name</param>
        /// <returns>Calendar Event reponse</returns>
        Task<IList<string>> SendCalendarEvent(IList<string> scheduleIds, string serviceAccountName);

        /// <summary>
        /// Create schedule
        /// </summary>
        /// <param name="schedule">Schedule object</param>
        /// <param name="jobApplicationId">job applicationid</param>
        /// <returns>The task response</returns>
        Task<MeetingInfo> CreateSchedule(MeetingInfo schedule, string jobApplicationId);

        /// <summary>
        /// Update schedule
        /// </summary>
        /// <param name="schedule">Schedule object</param>
        /// <returns>The task response</returns>
        Task<MeetingInfo> UpdateSchedule(MeetingInfo schedule);

        /// <summary>
        /// Get schedule By Job ID
        /// </summary>
        /// <param name="jobApplicationId">jobApplicationId</param>
        /// <returns>The task response</returns>
        Task<List<MeetingInfo>> GetSchedulesByJobApplicationId(string jobApplicationId);

        /// <summary>
        /// Get schedule By Schedule ID
        /// </summary>
        /// <param name="scheduleID">Schedule id</param>
        /// <returns>The task response</returns>
        Task<MeetingInfo> GetScheduleByScheduleId(string scheduleID);

        /// <summary>
        /// Queue schedule
        /// </summary>
        /// <param name="scheduleID">Schedule object</param>
        /// <param name="status">Schedule status</param>
        /// <param name="emailTemplate">Email Template</param>
        /// <param name="rootActivityID">root ActivityID</param>
        /// <param name="contextUserId">Current Context User Office Graph Identifier</param>
        /// <param name="requesterEmail">Email Id of the requester</param>
        /// <param name="isWobUser">Status of WOB Authentication the current user</param>
        /// <returns>The task response</returns>
        Task<MeetingInfo> QueueSchedule(string scheduleID, ScheduleStatus status, EmailTemplate emailTemplate, string rootActivityID, string contextUserId = null, string requesterEmail = null, bool isWobUser = false);

        /// <summary>
        /// Resend schedule
        /// </summary>
        /// <param name="scheduleId">schedule id</param>
        /// <param name="serviceAccountName">service account</param>
        /// <returns>result</returns>
        Task<bool> ReSendSchedule(string scheduleId, string serviceAccountName = null);

        /// <summary>
        /// Update schedule status
        /// </summary>
        /// <param name="scheduleID">Schedule object</param>
        /// <param name="status">Schedule status</param>
        /// <returns>The task response</returns>
        Task<MeetingInfo> UpdateScheduleStatus(string scheduleID, ScheduleStatus status);

        /// <summary>
        /// Update schedule service account
        /// </summary>
        /// <param name="scheduleID">Schedule object</param>
        /// <param name="serviceAccountName">service Account Name</param>
        /// <returns>The task response</returns>
        Task<MeetingInfo> UpdateScheduleServiceAccountAsync(string scheduleID, string serviceAccountName);

        /// <summary>
        /// Delete schedule
        /// </summary>
        /// <param name="scheduleID">Schedule object</param>
        /// <param name="rootActivityID">root ActivityID</param>
        /// <returns>The task response</returns>
        Task DeleteSchedule(string scheduleID, string rootActivityID);

        /// <summary>
        /// Sends the interview schedule to applicant.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <param name="scheduleInvitationRequest">The instance of <see cref="ScheduleInvitationRequest"/>.</param>
        /// <param name="requesterEmail">The requester email address.</param> /// <param name="requesterOid">The requestor Office Graph Identifier</param>
        /// <param name="isWobUser"> Flag to get the status of the Wob Authentication</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        Task SendInterviewScheduleToApplicantAsync(string jobApplicationId, ScheduleInvitationRequest scheduleInvitationRequest, string requesterEmail, string requesterOid = null, bool isWobUser = false);

        /// <summary>
        /// Validate the application before the any action
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        Task<bool> ValidateApplicationByApplicationId(string jobApplicationId);

        /// <summary>
        /// Validate the application before the any action
        /// </summary>
        /// <param name="scheduleID">The job schedule ID identifier.</param>
        /// <returns>The instance of <see cref="Task"/> representing an asynchronous operation.</returns>
        Task<bool> ValidateApplicationByScheduleId(string scheduleID);

        /// <summary>
        /// Create or update timezone for job application
        /// </summary>
        /// <param name="jobApplicationId">jobapplicationid</param>
        /// <param name="timezone">timezone information</param>
        /// <returns>task result</returns>
        Task UpdateTimezoneForJobApplication(string jobApplicationId, Timezone timezone);

        /// <summary>
        /// Get timezone using job application id
        /// </summary>
        /// <param name="jobApplicationId">jobapplicationid</param>
        /// <returns>task result</returns>
        Task<Timezone> GetTimezoneByJobApplicationId(string jobApplicationId);

        /// <summary>
        /// Get Meeting Suggestions
        /// </summary>
        /// <param name="suggestMeetingsRequest">meeting suggestion request object</param>
        /// <returns>task result</returns>
        Task<IList<MeetingInfo>> GetMeetingSuggestions(SuggestMeetingsRequest suggestMeetingsRequest);
    }
}
