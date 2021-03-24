//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Data.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CommonLibrary.Common.Provisioning.Entities.FalconEntities.Attract;
    using CommonLibrary.Common.TalentEntities.Common;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.Models;
    using MS.GTA.Talent.EnumSetModel;
    using MS.GTA.Talent.FalconEntities.Attract;
    using MS.GTA.Talent.FalconEntities.Attract.Conference;
    using MS.GTA.Talent.TalentContracts.ScheduleService;
    using MS.GTA.TalentEntities.Enum;
    using CommonContracts = CommonLibrary.Common.Email.Contracts;

    /// <summary>
    /// Schedule query definition
    /// </summary>
    public interface IScheduleQuery
    {
        /// <summary>
        /// Create Schedule
        /// </summary>
        /// <param name="schedule">Schedule entity</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>returns task</returns>
        Task<MeetingInfo> CreateSchedule(MeetingInfo schedule, string jobApplicationId);

        /// <summary>
        /// Create Schedule
        /// </summary>
        /// <param name="schedule">Schedule entity</param>
        /// <returns>returns task</returns>
        Task<MeetingInfo> UpdateScheduleDetails(MeetingInfo schedule);

        /// <summary>
        /// Update Schedule for candidate sharing status.
        /// </summary>
        /// <param name="schedule">The instance of <see cref="CandidateScheduleCommunication"/>.</param>
        /// <returns>The instance of <see cref="Task{Boolean}"/> representing an asynchronous operation.</returns>
        Task<bool> UpdateScheduleWithSharingStatusAsync(CandidateScheduleCommunication schedule);

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
        /// Get schedule By Schedule ID
        /// </summary>
        /// <param name="scheduleIds">Schedule ids</param>
        /// <returns>The task response</returns>
        Task<List<MeetingInfo>> GetScheduleByScheduleIds(IList<string> scheduleIds);

        /// <summary>
        /// Create Schedule
        /// </summary>
        /// <param name="scheduleId">Schedule entity identifier</param>
        /// <param name="status">Schedule status</param>
        /// <returns>returns task</returns>
        Task<MeetingInfo> UpdateScheduleStatus(string scheduleId, ScheduleStatus status);

        /// <summary>
        /// Update Schedule
        /// </summary>
        /// <param name="scheduleId">Schedule entity identifier</param>
        /// <param name="status">Schedule status</param>
        /// <param name="emailTemplate">Email Template</param>
        /// <returns>returns task</returns>
        Task<MeetingInfo> UpdateScheduleEmailStatus(string scheduleId, ScheduleStatus status, EmailTemplate emailTemplate);

        /// <summary>
        /// Get schedule mail details
        /// </summary>
        /// <param name="scheduleId">schedule id</param>
        /// <returns>schedule mail details</returns>
        Task<JobApplicationScheduleMailDetails> GetScheduleMailDetails(string scheduleId);

        /// <summary>
        /// Create Schedule
        /// </summary>
        /// <param name="scheduleIdEventIdMap">scheduleId and EventId Map</param>
        /// <param name="status">Schedule status</param>
        /// <param name="serviceAccountName">service Account Name</param>
        /// <returns>returns task</returns>
        Task UpdateJobApplicationScheduleDetails(Dictionary<string, CalendarEvent> scheduleIdEventIdMap, ScheduleStatus status, string serviceAccountName);

        /// <summary>
        /// Override the schedule participant response manually.
        /// </summary>
        /// <param name="scheduleId">Schedule object</param>
        /// <param name="participantOid">participant office graph identifier</param>
        /// <param name="responseStatus">response</param>
        /// <returns>The instance of <see cref="bool"/></returns>
        Task<bool> UpdateScheduleParticipantResponseManualAsync(string scheduleId, string participantOid, InvitationResponseStatus responseStatus);

        /// <summary>
        /// Update jobApplication schedule with invite response
        /// </summary>
        /// <param name="message">response message</param>
        /// <param name="responseSender">response send user details</param>
        /// <param name="responseFrom">response From user details</param>
        /// <returns>The instance of <see cref="InterviewerInviteResponseInfo"/>.</returns>
        Task<InterviewerInviteResponseInfo> UpdateScheduleWithResponse(Message message, Microsoft.Graph.User responseSender, Microsoft.Graph.User responseFrom);

        /// <summary>
        /// Update jobApplication schedule with interviewers responses
        /// </summary>
        /// <param name="responseMessage">CalendarEvent object</param>
        /// <param name="attendees">attendees</param>
        /// <returns>Notification required information</returns>
        Task<InterviewerResponseNotification> UpdateScheduleWithCalendatEventResponse(CalendarEvent responseMessage, List<Microsoft.Graph.User> attendees);

        /// <summary>
        /// Update schedule service account
        /// </summary>
        /// <param name="scheduleId">Schedule object</param>
        /// <param name="serviceAccountName">service Account Name</param>
        /// <returns>The task response</returns>
        Task<MeetingInfo> UpdateScheduleServiceAccount(string scheduleId, string serviceAccountName);

        /// <summary>
        /// Get schedules using freebusyschedule request
        /// </summary>
        /// <param name="requestFreeBusy">FreeBusy ScheduleRequest</param>
        /// <param name="jobApplicationId">jobApplicationId</param>
        /// <returns>The task response</returns>
        Task<List<MeetingInfo>> GetSchedules(FindFreeBusyScheduleRequest requestFreeBusy, string jobApplicationId);

        /// <summary>
        /// Get Workers with offer grpah identifier ids
        /// </summary>
        /// <param name="ids">oids</param>
        /// <returns>worker</returns>
        Task<List<Worker>> GetWorkers(List<string> ids);

        /// <summary>
        /// Gets a list of Job Applications.
        /// </summary>
        /// <param name="jobApplicationIds">Job Application Ids</param>
        /// <returns>Job Applications</returns>
        Task<IList<JobApplication>> GetJobApplications(IList<string> jobApplicationIds);

        /// <summary>
        /// Get Worker with offer grpah identifier id
        /// </summary>
        /// <param name="id">oids</param>
        /// <returns>worker</returns>
        Task<Worker> GetWorker(string id);

        /// <summary>
        /// Get job application details
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>task response</returns>
        Task<JobApplication> GetJobApplication(string jobApplicationId);

        /// <summary>
        /// Get job application details
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>task response</returns>
        Task<JobApplication> GetJobApplicationWithStatus(string jobApplicationId);

        /// <summary>
        /// Gets the job application with participants asynchronously.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <returns>The instance of <see cref="JobApplicationParticipants"/>.</returns>
        Task<JobApplicationParticipants> GetJobApplicationWithParticipantsAsync(string jobApplicationId);

        /// <summary>
        /// Get jobpplicaton id for schedule
        /// </summary>
        /// <param name="scheduleID">schedule id</param>
        /// <returns>rask response</returns>
        Task<string> GetJobApplicationIdForSchedule(string scheduleID);

        /// <summary>
        /// Get Email template
        /// </summary>
        /// <param name="templateId">email template id</param>
        /// <returns>Email Template</returns>
        Task<CommonContracts.EmailTemplate> GetTemplate(string templateId);

        /// <summary>
        /// Get Pending Schedules
        /// </summary>
        /// <param name="forRecovery">Flag to decide the purpose of fetching pending schedules.</param>
        /// <returns> List of Pending Schedules</returns>
        Task<IList<JobApplicationSchedule>> GetPendingSchedules(bool forRecovery = false);

        /// <summary>
        /// Update JobApplicationStatusHistory
        /// </summary>
        /// <param name="jobApplicationId">JobApplicationid</param>
        /// <param name="status">jobapplication actiob type</param>
        /// <returns>task response</returns>
        Task UpdateJobApplicationStatusHistoryAsync(string jobApplicationId, JobApplicationActionType status);

        /// <summary>
        /// Get Position title of a Job Opening.
        /// </summary>
        /// <param name="jobOpeningId">Job Opening id</param>
        /// <returns>The Job Opening Position Title.<see cref="string"/></returns>
        Task<string> GetJobOpeningPositionTitle(string jobOpeningId);

        /// <summary>
        /// GetItemsAsync based on schedule id
        /// </summary>
        /// <param name="scheduleID">The scheduleID.</param>
        /// <returns>List of Meeting infos.</returns>
        Task<List<SendInvitationLock>> GetScheduleLockItems(IList<string> scheduleID);

        /// <summary>
        /// Create ItemsAsync based on schedule id
        /// </summary>
        /// <param name="scheduleID">The scheduleID.</param>
        /// <returns>List of Meeting infos.</returns>
        Task CreateScheduleLockItem(SendInvitationLock scheduleID);

        /// <summary>
        /// Delete SendInvitationLock items
        /// </summary>
        /// <param name="scheduleIDs">scheduleids</param>
        /// <returns>result</returns>
        Task DeleteScheduleLockItems(IList<string> scheduleIDs);

        /// <summary>
        /// Delete SendInvitationLock item
        /// </summary>
        /// <param name="lockID">lockID</param>
        /// <returns>result</returns>
        Task DeleteScheduleLockItem(string lockID);

        /// <summary>
        /// CreateNotificationLockItem with NotificationMessageLock item
        /// </summary>
        /// <param name="notificationLock">notification lock</param>
        /// <returns>result</returns>
        Task CreateNotificationLockItem(NotificationMessageLock notificationLock);

        /// <summary>
        /// Get notification lock item with servicebus messageid
        /// </summary>
        /// <param name="serviceBusMessageId">serviceBusMessageId</param>
        /// <returns>result</returns>
        Task<NotificationMessageLock> GetNotificationLockItem(string serviceBusMessageId);

        /// <summary>
        /// Delete NotificationLockItem with servicebus messageid
        /// </summary>
        /// <param name="serviceBusMessageId">serviceBusMessageId</param>
        /// <returns>result</returns>
        Task DeleteNotificationLockItem(string serviceBusMessageId);

        /// <summary>
        /// Add or update timezone information for job application
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="timezone">timezone info</param>
        /// <returns>result</returns>
        Task AddOrUpdateTimezoneForJobApplication(string jobApplicationId, Timezone timezone);

        /// <summary>
        /// Get timezone information for job application
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>result</returns>
        Task<Timezone> GetTimezoneForJobApplication(string jobApplicationId);

        /// <summary>
        /// Updates the job applicaton.
        /// </summary>
        /// <param name="jobApplication">The instance of <see cref="JobApplication"/>.</param>
        /// <returns>The instance of <see cref="Task{Boolean}"/> where <c>true</c> if job application is updated; otherwise <c>false</c>. </returns>
        Task<bool> UpdateJobApplicaton(JobApplication jobApplication);

        /// <summary>
        /// Gets Job Application with candidate details by job application id
        /// </summary>
        /// <param name="jobApplicationId">job application id.</param>
        /// <returns>Job application.</returns>
        Task<JobApplication> GetJobApplicationWithCandidateDetails(string jobApplicationId);

        /// <summary>
        /// Get job application details
        /// </summary>
        /// <param name="requisitionId">job application id</param>
        /// <returns>task response</returns>
        Task<List<JobApplication>> GetJobApplicationsByRequisitionId(string requisitionId);

        /// <summary>
        /// Gets all pending feedbacks along with the job application and schedule information.
        /// </summary>
        /// <param name="reminderOffsetHours">Hours passed after interview before sending feedback reminder.</param>
        /// <param name="reminderWindowMinutes">Window duration in minutes for which to send feedback reminder.</param>
        /// <returns>An instance of <see cref="Task{T}"/> where <c>T</c> being <see cref="IList{PendingFeedback}"/>.</returns>
        Task<IList<Data.Models.PendingFeedback>> GetAllPendingFeedbacksForReminderAsync(int reminderOffsetHours, int reminderWindowMinutes);

        /// <summary>
        /// Gets all pending job schedules for given job application.
        /// </summary>
        /// <param name="jobApplication">Job application.</param>
        /// <returns>An instance of <see cref="Task{T}"/> where <c>T</c> being <see cref="IList{PendingFeedback}"/>.</returns>
        Task<IList<Data.Models.PendingFeedback>> GetPendingSchedulesForJobApplication(JobApplication jobApplication);

        /// <summary>
        /// Get Wob Delegates with User Office Graph Identifiers
        /// </summary>
        /// <param name="userId"> User <see cref="Delegation"/> Office Graph Identifier</param>
        /// <returns>A list of <see cref="Delegation"/></returns>
        Task<List<Delegation>> GetWobUsersDelegation(string userId);

        /// <summary>
        /// Get Wob Delegates with User Office Graph Identifiers
        /// </summary>
        /// <param name="userId"> User <see cref="Delegation"/> Office Graph Identifier</param>
        /// <returns>A list of <see cref="Delegation"/></returns>
        Task<List<Delegation>> GetWobUsersDelegationAsync(List<string> userId);
    }
}
