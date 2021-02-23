//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.FalconData.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Base.Utilities;
    using MS.GTA.Common.Contracts;
    using MS.GTA.Common.DocumentDB;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.Data.Utils;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Contracts.V1.Flights;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.Data.Models;
    using MS.GTA.ScheduleService.FalconData.Constants;
    using MS.GTA.ScheduleService.FalconData.ViewModelExtensions;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.EnumSetModel;
    using MS.GTA.Talent.FalconEntities.Attract;
    using MS.GTA.Talent.TalentContracts.InterviewService;
    using MS.GTA.Talent.TalentContracts.ScheduleService;
    using MS.GTA.TalentEntities.Enum;
    using Worker = Common.TalentEntities.Common.Worker;

    /// <summary>
    /// ScheduleQuery used for storing the schedule across a single request.
    /// </summary>
    public class ScheduleQuery : FalconQuery, IScheduleQuery
    {
        private const int DayOldFeedbackConstant = -1;
        private const int TwoWeekOldFeedbackConstant = -14;

        /// <summary>
        /// The instance for <see cref="ILogger{ScheduleQuery}"/>
        /// </summary>
        private readonly ILogger<ScheduleQuery> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleQuery" /> class
        /// </summary>
        /// <param name="falconQueryClient">falconQueryClient instance</param>
        /// <param name="logger">Logger</param>
        public ScheduleQuery(
            IFalconQueryClient falconQueryClient, ILogger<ScheduleQuery> logger)
            : base(falconQueryClient, TraceSourceMeta.LoggerFactory.CreateLogger(nameof(FalconQuery)) as ILogger<FalconQuery>)
        {
            this.FalconQueryClient = falconQueryClient;
            this.logger = logger;
        }

        /// <summary>
        /// Create Schedule
        /// </summary>
        /// <param name="schedule">Schedule entity</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>returns task</returns>
        public async Task<MeetingInfo> CreateSchedule(MeetingInfo schedule, string jobApplicationId)
        {
            this.logger.LogInformation($"Started {nameof(this.CreateSchedule)} method in {nameof(ScheduleQuery)}.");
            if (schedule == null)
            {
                throw new BadRequestStatusException("CreateSchedule: Input can not be null or empty");
            }

            this.logger.LogInformation("CreateSchedule started with schedule id as " + schedule.Id);

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);
            var scheduleObject = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == schedule.Id);

            if (scheduleObject == null || string.IsNullOrEmpty(scheduleObject.ScheduleID))
            {
                JobApplicationSchedule scheduleInfo = schedule.ToEntity();
                scheduleInfo.Type = "JobApplicationSchedule";
                scheduleInfo.ScheduleStatus = ScheduleStatus.Saved;
                scheduleInfo.JobApplicationID = jobApplicationId;

                try
                {
                    await client.Create<JobApplicationSchedule>(scheduleInfo);
                }
                catch (Exception ex)
                {
                    this.logger.LogError("CreateSchedule failed for schedule id: " + schedule.Id + " with error: " + ex.Message + " and stack trace:" + ex.StackTrace);
                    throw new OperationFailedException(ex.Message);
                }

                await this.UpdateScheduleHistoryAsync(scheduleInfo.ScheduleID, ScheduleStatus.Saved, jobApplicationId).ConfigureAwait(false);

                await this.InsertScheduleAuditLog(scheduleInfo, JobApplicationScheduleAction.Create).ConfigureAwait(false);

                this.logger.LogInformation("CreateSchedule completed with schedule id as " + schedule.Id);
            }
            else
            {
                this.logger.LogInformation("CreateSchedule failed with schedule id as " + schedule.Id + ". Because schedule is already exists.");
            }

            JobApplicationSchedule result = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == schedule.Id);

            this.logger.LogInformation($"Finished {nameof(this.CreateSchedule)} method in {nameof(ScheduleQuery)}.");
            return result?.ToContract();
        }

        /// <summary>
        /// Create Schedule
        /// </summary>
        /// <param name="schedule">Schedule entity</param>
        /// <returns>returns task</returns>
        public async Task<MeetingInfo> UpdateScheduleDetails(MeetingInfo schedule)
        {
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleDetails)} method in {nameof(ScheduleQuery)}.");
            if (schedule == null)
            {
                throw new BadRequestStatusException("Input request does not contain a valid schedule");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            this.logger.LogInformation("UpdateSchedule started for the schedule: " + schedule.Id);

            JobApplicationSchedule scheduleInfo = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == schedule.Id);

            scheduleInfo.BuildingAddress = schedule.MeetingDetails?[0].MeetingLocation?.RoomList?.Address;
            scheduleInfo.BuildingName = schedule.MeetingDetails?[0].MeetingLocation?.RoomList?.Name;
            scheduleInfo.BuildingResponseStatus = this.GetResponseStatus(schedule.MeetingDetails?[0].MeetingLocation?.RoomList?.Status);
            scheduleInfo.RoomAddress = schedule.MeetingDetails?[0].MeetingLocation?.Room?.Address;
            scheduleInfo.RoomName = schedule.MeetingDetails?[0].MeetingLocation?.Room?.Name;
            scheduleInfo.RoomResponseStatus = this.GetResponseStatus(schedule.MeetingDetails?[0].MeetingLocation?.Room?.Status);
            scheduleInfo.Subject = schedule.MeetingDetails?[0].Subject;
            scheduleInfo.Description = schedule.MeetingDetails?[0].Description;
            scheduleInfo.SkypeOnlineMeetingRequired = false;
            scheduleInfo.IsDirty = schedule.MeetingDetails?[0].IsDirty ?? false;
            scheduleInfo.IsPrivateMeeting = schedule.MeetingDetails?[0].IsPrivateMeeting ?? false;
            scheduleInfo.SkypeHtmlViewHref = schedule.MeetingDetails?[0].SkypeOnlineMeeting?.Links?.HtmlView?.Href;
            scheduleInfo.SkypeJoinUrl = schedule.MeetingDetails?[0].SkypeOnlineMeeting?.JoinUrl;
            scheduleInfo.SkypeOnlineMeetingId = schedule.MeetingDetails?[0].SkypeOnlineMeeting?.OnlineMeetingId;
            scheduleInfo.CalendarEventId = schedule.MeetingDetails?[0].CalendarEventId;
            scheduleInfo.MeetingDetailId = schedule.MeetingDetails?[0].Id;
            scheduleInfo.ScheduleRequester = schedule.Requester?.ToWorker();
            scheduleInfo.InterviewerTimeSlotID = schedule.InterviewerTimeSlotId;
            scheduleInfo.Location = schedule.MeetingDetails?[0].Location;
            scheduleInfo.IsInterviewerNameShared = schedule.MeetingDetails?[0].IsInterviewerNameShared;
            scheduleInfo.IsInterviewScheduleShared = schedule.MeetingDetails?[0].IsInterviewScheduleShared;
            scheduleInfo.ScheduleStatus = ScheduleStatus.Saved;
            scheduleInfo.ConferenceMeetingId = schedule.MeetingDetails?[0].Conference?.Id;
            scheduleInfo.ConferenceProviderType = schedule.MeetingDetails?[0].Conference?.Provider;
            scheduleInfo.OnlineMeetingRequired = schedule.MeetingDetails?[0].OnlineMeetingRequired ?? false;
            if (!string.IsNullOrWhiteSpace(schedule.MeetingDetails?[0].SchedulerAccountEmail))
            {
                scheduleInfo.ServiceAccountEmail = schedule.MeetingDetails?[0].SchedulerAccountEmail;
            }

            List<JobApplicationScheduleParticipant> jobApplicationScheduleParticipants = new List<JobApplicationScheduleParticipant>();
            if (schedule.MeetingDetails != null && schedule.MeetingDetails[0]?.Attendees != null)
            {
                foreach (var item in schedule.MeetingDetails[0]?.Attendees)
                {
                    if (item.User != null)
                    {
                        JobApplicationScheduleParticipant participant = new JobApplicationScheduleParticipant
                        {
                            OID = item.User.Id,
                            ParticipantMetadata = item.User.Email,
                            ParticipantComments = item.User.Comments
                        };
                        if (schedule.ScheduleStatus == ScheduleStatus.Saved && (item.ResponseStatus == InvitationResponseStatus.None
                            || item.ResponseStatus == InvitationResponseStatus.Sending))
                        {
                            participant.ParticipantStatus = InvitationResponseStatus.None;
                        }
                        else
                        {
                            if (scheduleInfo.StartDateTime != schedule.MeetingDetails?[0].UtcStart || scheduleInfo.EndDateTime != schedule.MeetingDetails?[0].UtcEnd)
                            {
                                participant.ParticipantStatus = InvitationResponseStatus.Pending;
                            }
                            else
                            {
                                participant.ParticipantStatus = item.ResponseStatus;
                            }
                        }

                        jobApplicationScheduleParticipants.Add(participant);
                    }
                }
            }

            scheduleInfo.StartDateTime = schedule.MeetingDetails?[0].UtcStart;
            scheduleInfo.EndDateTime = schedule.MeetingDetails?[0].UtcEnd;
            scheduleInfo.Participants = jobApplicationScheduleParticipants;

            try
            {
                await client.Update<JobApplicationSchedule>(scheduleInfo);
            }
            catch (Exception ex)
            {
                this.logger.LogError("UpdateScheduleDetails failed for schedule id: " + schedule.Id + " with error: " + ex.Message + " and stack trace:" + ex.StackTrace);

                throw new OperationFailedException(ex.Message);
            }

            await this.UpdateScheduleHistoryAsync(scheduleInfo.ScheduleID, ScheduleStatus.Saved, scheduleInfo?.JobApplicationID).ConfigureAwait(false);

            await this.InsertScheduleAuditLog(scheduleInfo, JobApplicationScheduleAction.Update).ConfigureAwait(false);

            var result = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == schedule.Id);

            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleDetails)} method in {nameof(ScheduleQuery)} to update schedule for the schedule {schedule.Id}.");
            return result?.ToContract();
        }

        /// <summary>
        /// Update Schedule for candidate sharing status.
        /// </summary>
        /// <param name="schedule">The instance of <see cref="CandidateScheduleCommunication"/>.</param>
        /// <returns>The instance of <see cref="Task{Boolean}"/> representing an asynchronous operation.</returns>
        public async Task<bool> UpdateScheduleWithSharingStatusAsync(CandidateScheduleCommunication schedule)
        {
            Contract.CheckValue(schedule, nameof(schedule));
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleWithSharingStatusAsync)} method in {nameof(ScheduleQuery)} to update schedulesharing status for the schedule {schedule.ScheduleId}.");
            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);
            JobApplicationSchedule scheduleInfo = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == schedule.ScheduleId);
            scheduleInfo.IsInterviewerNameShared = schedule.IsInterviewerNameShared;
            scheduleInfo.IsInterviewScheduleShared = schedule.IsInterviewScheduleShared;
            await client.Update<JobApplicationSchedule>(scheduleInfo);
            await this.InsertScheduleAuditLog(scheduleInfo, JobApplicationScheduleAction.SendToCandidate).ConfigureAwait(false);
            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleWithSharingStatusAsync)} method in {nameof(ScheduleQuery)} to update schedulesharing status for the schedule {schedule.ScheduleId}.");
            return true;
        }

        /// <summary>
        /// Get schedule By Job ID
        /// </summary>
        /// <param name="jobApplicationId">jobApplicationId</param>
        /// <returns>The task response</returns>
        public async Task<List<MeetingInfo>> GetSchedulesByJobApplicationId(string jobApplicationId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetSchedulesByJobApplicationId)} method in {nameof(ScheduleQuery)} for jobapplicationid: {jobApplicationId}.");
            if (string.IsNullOrEmpty(jobApplicationId))
            {
                throw new BadRequestStatusException("Input request does not contain a valid application id").EnsureLogged(this.logger);
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var result = await client.Get<JobApplicationSchedule>(a => a.JobApplicationID == jobApplicationId && a.ScheduleStatus != ScheduleStatus.Delete);
            var jobApplicationSchedules = result as JobApplicationSchedule[] ?? result.ToArray();

            List<MeetingInfo> meetingInfos = new List<MeetingInfo>();
            foreach (var item in jobApplicationSchedules?.OrderBy(a => a.StartDateTime))
            {
                meetingInfos.Add(item?.ToContract());
            }

            this.logger.LogInformation($"Finished {nameof(this.GetSchedulesByJobApplicationId)} method in {nameof(ScheduleQuery)} to get schedules by using jobapplicationid {jobApplicationId}.");
            return meetingInfos;
        }

        /// <summary>
        /// Get schedule By Schedule ID
        /// </summary>
        /// <param name="scheduleID">Schedule id</param>
        /// <returns>The task response</returns>
        public async Task<MeetingInfo> GetScheduleByScheduleId(string scheduleID)
        {
            this.logger.LogInformation($"Started {nameof(this.GetScheduleByScheduleId)} method in {nameof(ScheduleQuery)} for scheduleid: {scheduleID}.");
            if (string.IsNullOrWhiteSpace(scheduleID))
            {
                throw new BadRequestStatusException("Input request does not contain a valid schedule id").EnsureTraced();
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var result = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == scheduleID);

            this.logger.LogInformation($"Finished {nameof(this.GetScheduleByScheduleId)} method in {nameof(ScheduleQuery)} to get schedule by using scheduleid {scheduleID}.");
            return result?.ToContract();
        }

        /// <summary>
        /// Get schedule By Schedule ID
        /// </summary>
        /// <param name="scheduleIds">Schedule ids</param>
        /// <returns>The task response</returns>
        public async Task<List<MeetingInfo>> GetScheduleByScheduleIds(IList<string> scheduleIds)
        {
            this.logger.LogInformation($"Started {nameof(this.GetScheduleByScheduleIds)} method in {nameof(ScheduleQuery)}.");
            if (!scheduleIds?.Any() ?? true)
            {
                throw new BadRequestStatusException("Input request does not contain a valid schedules");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var result = await client.Get<JobApplicationSchedule>(a => scheduleIds.Contains(a.ScheduleID));

            List<MeetingInfo> meetingInfos = new List<MeetingInfo>();
            foreach (var item in result)
            {
                meetingInfos.Add(item?.ToContract());
            }

            this.logger.LogInformation($"Finished {nameof(this.GetScheduleByScheduleIds)} method in {nameof(ScheduleQuery)}.");
            return meetingInfos;
        }

        /// <summary>
        /// Update Schedule Status
        /// </summary>
        /// <param name="scheduleId">Schedule entity identifier</param>
        /// <param name="status">Schedule status</param>
        /// <returns>returns task</returns>
        public async Task<MeetingInfo> UpdateScheduleStatus(string scheduleId, ScheduleStatus status)
        {
            MeetingInfo meetingInfo;
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleStatus)} method in {nameof(ScheduleQuery)} for scheduleid:{scheduleId}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new BadRequestStatusException("Input request does not contain a valid schedule id");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var scheduleObject = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == scheduleId);

            if (scheduleObject != null && !string.IsNullOrEmpty(scheduleObject.ScheduleID) && scheduleObject.ScheduleStatus != status)
            {
                scheduleObject.ScheduleStatus = status;

                await client.Update(scheduleObject);

                await this.UpdateScheduleHistoryAsync(scheduleId, status, scheduleObject?.JobApplicationID).ConfigureAwait(false);

                await this.InsertScheduleAuditLog(scheduleObject, JobApplicationScheduleAction.Delete).ConfigureAwait(false);

                var result = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == scheduleId);

                meetingInfo = result?.ToContract();
            }
            else
            {
                meetingInfo = scheduleObject?.ToContract();
            }

            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleStatus)} method in {nameof(ScheduleQuery)} for update schedule status of the schedule {scheduleId}.");
            return meetingInfo;
        }

        /// <summary>
        /// Update Schedule Status
        /// </summary>
        /// <param name="scheduleId">Schedule entity identifier</param>
        /// <param name="status">Schedule status</param>
        /// <param name="emailTemplate">Email Template</param>
        /// <returns>returns task</returns>
        public async Task<MeetingInfo> UpdateScheduleEmailStatus(string scheduleId, ScheduleStatus status, EmailTemplate emailTemplate)
        {
            MeetingInfo meetingInfo;
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleEmailStatus)} method in {nameof(ScheduleQuery)} for the scheduleid {scheduleId}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new BadRequestStatusException("Input request does not contain a valid schedule");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);
            var scheduleObject = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == scheduleId);

            if (scheduleObject == null)
            {
                throw new BadRequestStatusException("We could not find the associated schedule, unable to send invitation");
            }
            else
            {
                if (scheduleObject?.EndDateTime < DateTime.UtcNow)
                {
                    throw new BadRequestStatusException("Associated schedule in past date, unable to send invitation");
                }
            }

            if (scheduleObject != null && !string.IsNullOrEmpty(scheduleObject.ScheduleID) && scheduleObject.ScheduleStatus != ScheduleStatus.Sent)
            {
                var commonClient = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);
                var jobApplicationObject = await commonClient.GetFirstOrDefault<JobApplication>(a => a.JobApplicationID == scheduleObject.JobApplicationID);

                if (scheduleObject.ScheduleStatus != status)
                {
                    this.logger.LogInformation("UpdateScheduleStatus started for the schedule: " + scheduleId);

                    scheduleObject.ScheduleStatus = status;

                    await client.Update(scheduleObject);

                    this.logger.LogInformation("UpdateScheduleStatus started for the schedule: " + scheduleId);
                }

                var scheduleMailDetails = await client.GetFirstOrDefault<JobApplicationScheduleMailDetails>(a => a.ScheduleID == scheduleId);
                if (scheduleMailDetails != null && !string.IsNullOrEmpty(scheduleMailDetails.ScheduleID))
                {
                    scheduleMailDetails.ScheduleMailSubject = emailTemplate?.Subject;
                    scheduleMailDetails.CCEmailAddressList = emailTemplate?.CcEmailAddressList;
                    scheduleMailDetails.BccEmailAddressList = emailTemplate?.BccEmailAddressList;
                    scheduleMailDetails.PrimaryEmailRecipients = emailTemplate?.PrimaryEmailRecipients;
                    scheduleMailDetails.ScheduleMailDescription = emailTemplate?.EmailContent;
                    scheduleMailDetails.MailSendDateTime = DateTime.UtcNow.ToString();

                    await client.Update(scheduleMailDetails);
                }
                else
                {
                    JobApplicationScheduleMailDetails jobApplicationScheduleMailDetails = new JobApplicationScheduleMailDetails
                    {
                        ScheduleID = scheduleId,
                        ScheduleMailSubject = emailTemplate?.Subject,
                        CCEmailAddressList = emailTemplate?.CcEmailAddressList,
                        BccEmailAddressList = emailTemplate?.BccEmailAddressList,
                        PrimaryEmailRecipients = emailTemplate?.PrimaryEmailRecipients,
                        ScheduleMailDescription = emailTemplate?.EmailContent,
                        MailSendDateTime = DateTime.UtcNow.ToString()
                    };

                    await client.Create(jobApplicationScheduleMailDetails);
                }

                this.logger.LogInformation($"UpdateScheduleEmailStatus: Update jobapplication with jobapplication activity as interview for schedule id: {0} and jobapplicationid: {1}", scheduleObject.ScheduleID, scheduleObject.JobApplicationID);
                ////Update jobapplication
                if (jobApplicationObject != null)
                {
                    List<JobApplicationActivity> activities = jobApplicationObject.JobApplicationActivities?.ToList();

                    var activityResult = activities?.Where(a => a.ActivityType == JobApplicationActivityType.Interview);

                    if (activityResult != null && !activityResult.Any())
                    {
                        JobApplicationActivity jobApplicationActivity = new JobApplicationActivity()
                        {
                            ActivityType = JobApplicationActivityType.Interview,
                            ActualStartDateTime = DateTime.UtcNow,
                        };

                        activities.Add(jobApplicationActivity);

                        jobApplicationObject.JobApplicationActivities = activities;

                        await commonClient.Update(jobApplicationObject);
                    }
                }

                await this.UpdateScheduleHistoryAsync(scheduleId, status, scheduleObject?.JobApplicationID).ConfigureAwait(false);

                await this.InsertScheduleAuditLog(scheduleObject, JobApplicationScheduleAction.SendToInterviewers).ConfigureAwait(false);

                var result = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == scheduleId);

                meetingInfo = result?.ToContract();
            }
            else
            {
                meetingInfo = scheduleObject?.ToContract();
            }

            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleEmailStatus)} method in {nameof(ScheduleQuery)}.");
            return meetingInfo;
        }

        /// <summary>
        /// Get schedule mail details
        /// </summary>
        /// <param name="scheduleId">schedule id</param>
        /// <returns>schedule mail details</returns>
        public async Task<JobApplicationScheduleMailDetails> GetScheduleMailDetails(string scheduleId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetScheduleMailDetails)} method in {nameof(ScheduleQuery)}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new BadRequestStatusException("Input request does not contain a valid schedule");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            this.logger.LogInformation($"Finished {nameof(this.GetScheduleMailDetails)} method in {nameof(ScheduleQuery)}.");
            return await client.GetFirstOrDefault<JobApplicationScheduleMailDetails>(a => a.ScheduleID == scheduleId);
        }

        /// <summary>
        /// Create Schedule
        /// </summary>
        /// <param name="scheduleIdEventIdMap">schedueId and EventId Map</param>
        /// <param name="status">Schedule status</param>
        /// <param name="serviceAccountName">service Account Name</param>
        /// <returns>returns task</returns>
        public async Task UpdateJobApplicationScheduleDetails(Dictionary<string, CalendarEvent> scheduleIdEventIdMap, ScheduleStatus status, string serviceAccountName)
        {
            this.logger.LogInformation($"Started {nameof(this.UpdateJobApplicationScheduleDetails)} method in {nameof(ScheduleQuery)}.");
            if (!scheduleIdEventIdMap?.Any() ?? true)
            {
                throw new OperationFailedException("UpdateJobApplicationScheduleDetails: Input can not be null or empty");
            }

            var scheduleIds = scheduleIdEventIdMap.Keys?.ToList();
            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var scheduleObjects = await client.Get<JobApplicationSchedule>(a => scheduleIds.Contains(a.ScheduleID));

            var updateSheduleObjectTasks = new List<Task>();
            var updateScheduleHistoryTasks = new List<Task>();
            var updateJobApplicationHistoryTasks = new List<Task>();
            foreach (var scheduleObject in scheduleObjects)
            {
                if (scheduleObject != null && !string.IsNullOrEmpty(scheduleObject.ScheduleID))
                {
                    scheduleObject.CalendarEventId = scheduleIdEventIdMap[scheduleObject.ScheduleID]?.Id;
                    scheduleObject.OnlineTeamMeeting = scheduleIdEventIdMap[scheduleObject.ScheduleID]?.OnlineMeeting;
                    scheduleObject.ScheduleStatus = status;
                    scheduleObject.ServiceAccountEmail = serviceAccountName;
                    scheduleObject.OnlineMeetingContent = this.GetOnlineMeetingContent(scheduleIdEventIdMap[scheduleObject.ScheduleID]?.Body?.Content);

                    List<JobApplicationScheduleParticipant> jobApplicationScheduleParticipants = new List<JobApplicationScheduleParticipant>();
                    if (scheduleObject.Participants != null)
                    {
                        foreach (var item in scheduleObject.Participants)
                        {
                            if (item.ParticipantStatus == InvitationResponseStatus.None || item.ParticipantStatus == InvitationResponseStatus.Sending)
                            {
                                item.ParticipantStatus = InvitationResponseStatus.Pending;
                            }
                        }
                    }

                    updateSheduleObjectTasks.Add(client.Update(scheduleObject));
                    updateScheduleHistoryTasks.Add(this.UpdateScheduleHistoryAsync(scheduleObject.ScheduleID, status, scheduleObject?.JobApplicationID));
                    updateJobApplicationHistoryTasks.Add(this.UpdateJobApplicationStatusHistoryAsync(scheduleObject.JobApplicationID, JobApplicationActionType.SendToInterviewers));
                }
            }

            await Task.WhenAll(updateSheduleObjectTasks).ConfigureAwait(false);
            await Task.WhenAll(updateScheduleHistoryTasks).ConfigureAwait(false);
            await Task.WhenAll(updateJobApplicationHistoryTasks).ConfigureAwait(false);
            this.logger.LogInformation($"Finished {nameof(this.UpdateJobApplicationScheduleDetails)} method in {nameof(ScheduleQuery)}.");
        }

        /// <summary>
        /// Override the schedule participant response manually.
        /// </summary>
        /// <param name="scheduleId">Schedule object</param>
        /// <param name="participantOid">participant office graph identifier</param>
        /// <param name="responseStatus">response</param>
        /// <returns>The instance of <see cref="bool"/></returns>
        public async Task<bool> UpdateScheduleParticipantResponseManualAsync(string scheduleId, string participantOid, InvitationResponseStatus responseStatus)
        {
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleParticipantResponseManualAsync)} method in {nameof(ScheduleQuery)}.");

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);
            var schedule = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == scheduleId);

            if (schedule == null)
            {
                throw new OperationFailedException("UpdateScheduleParticipantResponseManualAsync: Schedule with given id doesn't exist.");
            }

            bool success = false;
            foreach (var participant in schedule.Participants)
            {
                if (participant.OID == participantOid)
                {
                    participant.ParticipantStatus = responseStatus;
                    success = true;
                }
            }

            if (!success)
            {
                throw new OperationFailedException("UpdateScheduleParticipantResponseManualAsync: Participant with given id doesn't exist in the schedule.");
            }

            await client.Update(schedule).ConfigureAwait(false);

            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleParticipantResponseManualAsync)} method in {nameof(ScheduleQuery)}.");
            return true;
        }

        /// <summary>
        /// Update jobApplication schedule with interviewer response
        /// </summary>
        /// <param name="responseMessage">Response Message</param>
        /// <param name="responseSender">response send user details</param>
        /// <param name="responseFrom">response From user details</param>
        /// <returns>The instance of <see cref="InterviewerInviteResponseInfo"/>.</returns>
        public async Task<InterviewerInviteResponseInfo> UpdateScheduleWithResponse(Message responseMessage, Microsoft.Graph.User responseSender, Microsoft.Graph.User responseFrom)
        {
            MeetingTimeSpan proposedNewTime = null;
            string interviewerMessage = string.Empty;
            InterviewerInviteResponseInfo interviewerInviteResponseInfo = null;
            InterviewerResponseNotification interviewerResponseNotification = null;
            Task<JobApplicationParticipants> jobApplicationPaarticipantsTask = null;
            JobApplicationParticipants jobApplicationParticipants = null;
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleWithResponse)} method in {nameof(ScheduleQuery)}.");
            string calendarEventId = responseMessage?.Event?.Id;

            this.logger.LogInformation($"UpdateScheduleWithResponse: Recieved request to update schedule with calenderEventId: {calendarEventId} with status: {responseMessage?.MeetingMessageType}.");

            if (string.IsNullOrEmpty(calendarEventId))
            {
                throw new OperationFailedException("UpdateScheduleWithResponse: calendarEventId can not be null or empty").EnsureLogged(this.logger);
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var scheduleObject = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.CalendarEventId == calendarEventId);
            if (scheduleObject == null || string.IsNullOrEmpty(scheduleObject.ScheduleID))
            {
                throw new OperationFailedException($"UpdateScheduleWithResponse: jobApplication schedule doesn't for calederEventID {calendarEventId}").EnsureLogged(this.logger);
            }

            if (WebNotificationServiceFlight.IsEnabled)
            {
                jobApplicationPaarticipantsTask = this.GetJobApplicationWithParticipantsAsync(scheduleObject.JobApplicationID);
            }
            else
            {
                this.logger.LogWarning("The web notifications flight is not enabled.");
            }

            foreach (var item in scheduleObject.Participants)
            {
                /* Responses could be provided by the Interviewers themselves or by their On-Behalf users.
                 * In a direct response scenario - senderEmailAddress shall match the participant email address.
                 * In an On-behalf response scenario - fromEmailAddress shall match the participant email address.
                 */
                if (string.Equals(item.ParticipantMetadata, responseSender?.Mail, StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(item.ParticipantMetadata, responseSender?.UserPrincipalName, StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(item.ParticipantMetadata, responseFrom?.Mail, StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(item.ParticipantMetadata, responseFrom?.UserPrincipalName, StringComparison.InvariantCultureIgnoreCase))
                {
                    switch (responseMessage?.MeetingMessageType?.ToLower())
                    {
                        case GraphConstants.AcceptMessageType:
                            if (item.ParticipantStatus != InvitationResponseStatus.Accepted)
                            {
                                this.logger.LogInformation($"UpdateScheduleWithResponse: Updating schedule with response as : {responseMessage.MeetingMessageType}. for the schedule: {scheduleObject.ScheduleID}");

                                item.ParticipantStatus = InvitationResponseStatus.Accepted;
                                interviewerResponseNotification = new InterviewerResponseNotification()
                                {
                                    ScheduleId = scheduleObject.ScheduleID,
                                    JobApplicationId = scheduleObject.JobApplicationID,
                                    InterviewerOid = item.OID,
                                    ResponseStatus = InvitationResponseStatus.Accepted,
                                };
                            }

                            break;
                        case GraphConstants.TentativeMessageType:
                            if (item.ParticipantStatus != InvitationResponseStatus.TentativelyAccepted)
                            {
                                proposedNewTime = await this.GetParticipantProposedNewTime(responseMessage, item);
                                this.logger.LogInformation($"UpdateScheduleWithResponse: Updating schedule with response as : {responseMessage.MeetingMessageType}. for the schedule: {scheduleObject.ScheduleID}");
                                interviewerResponseNotification = new InterviewerResponseNotification()
                                {
                                    ScheduleId = scheduleObject.ScheduleID,
                                    JobApplicationId = scheduleObject.JobApplicationID,
                                    InterviewerOid = item.OID,
                                    ResponseStatus = InvitationResponseStatus.TentativelyAccepted,
                                    ProposedNewTime = proposedNewTime,
                                };

                                item.ParticipantStatus = InvitationResponseStatus.TentativelyAccepted;
                            }

                            break;
                        case GraphConstants.DeclineMessageType:
                            if (item.ParticipantStatus != InvitationResponseStatus.Declined)
                            {
                                item.ParticipantStatus = InvitationResponseStatus.Declined;
                                this.logger.LogInformation($"UpdateScheduleWithResponse: Updating schedule with response as : {responseMessage.MeetingMessageType}. for the schedule: {scheduleObject.ScheduleID}");
                                proposedNewTime = await this.GetParticipantProposedNewTime(responseMessage, item);
                                interviewerResponseNotification = new InterviewerResponseNotification()
                                {
                                    ScheduleId = scheduleObject.ScheduleID,
                                    JobApplicationId = scheduleObject.JobApplicationID,
                                    InterviewerOid = item.OID,
                                    ResponseStatus = InvitationResponseStatus.Declined,
                                    ProposedNewTime = proposedNewTime,
                                };
                            }

                            break;
                    }

                    item.ProposedNewTime = proposedNewTime;

                    ////Persisting comments
                    if (responseMessage.BodyPreview?.Length > 0)
                    {
                        interviewerMessage = responseMessage.BodyPreview;
                        item.ParticipantComments = responseMessage.BodyPreview;
                        item.ParticipantResponseDateTime = responseMessage.SentDateTime.UtcDateTime;
                    }
                }
            }

            await client.Update<JobApplicationSchedule>(scheduleObject);
            if (WebNotificationServiceFlight.IsEnabled)
            {
                try
                {
                    jobApplicationParticipants = await jobApplicationPaarticipantsTask.ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "The job application failed to load.");
                }
            }

            interviewerInviteResponseInfo = new InterviewerInviteResponseInfo
            {
                ResponseNotification = interviewerResponseNotification,
                ApplicationParticipants = jobApplicationParticipants,
                InterviewerMessage = interviewerMessage,
            };

            this.logger.LogInformation($"UpdateScheduleWithResponse: Sucessfully updated schedule with calenderEventId: {calendarEventId} with status: {responseMessage?.MeetingMessageType} for the schedule: {scheduleObject.ScheduleID}.");
            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleWithResponse)} method in {nameof(ScheduleQuery)}.");
            return interviewerInviteResponseInfo;
        }

        /// <inheritdoc />
        public async Task<InterviewerResponseNotification> UpdateScheduleWithCalendatEventResponse(CalendarEvent responseMessage, List<Microsoft.Graph.User> attendees)
        {
            ////attendees check is not mandatory here because, In case of attendees list null we should not stop updating user response. It should continue with scheduleObject participant metadata check
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleWithCalendatEventResponse)} method in {nameof(ScheduleQuery)}.");
            string calendarEventId = responseMessage?.Id;
            InterviewerResponseNotification interviewerResponseNotification = null;

            this.logger.LogInformation($"UpdateScheduleWithCalendatEventResponse: Receieved request to update schedule with calenderEventId: {calendarEventId}.");

            if (string.IsNullOrEmpty(calendarEventId))
            {
                throw new OperationFailedException("UpdateScheduleWithCalendatEventResponse: calendarEventId can not be null or empty").EnsureLogged(this.logger);
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var scheduleObject = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.CalendarEventId == calendarEventId);

            if (scheduleObject == null || string.IsNullOrEmpty(scheduleObject.ScheduleID))
            {
                throw new OperationFailedException($"UpdateScheduleWithCalendatEventResponse: jobApplication schedule doesn't exist for calederEventID {calendarEventId}").EnsureLogged(this.logger);
            }

            this.logger.LogInformation("UpdateScheduleWithCalendatEventResponse started for the schedule: " + scheduleObject.ScheduleID);

            foreach (var item in scheduleObject.Participants)
            {
                var graphUser = attendees?.Where(a => a.Mail.Equals(item.ParticipantMetadata, StringComparison.OrdinalIgnoreCase) || a.UserPrincipalName.Equals(item.ParticipantMetadata, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                var attendee = responseMessage?.Attendees?.Where(a => a.EmailAddress.Address.Equals(graphUser?.Mail, StringComparison.OrdinalIgnoreCase)
                || a.EmailAddress.Address.Equals(item.ParticipantMetadata, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (attendee != null)
                {
                    if (attendee.Status.Response.ToLower() == "accepted")
                    {
                        item.ParticipantStatus = InvitationResponseStatus.Accepted;
                    }
                    else if (attendee.Status.Response.ToLower() == "declined")
                    {
                        if (item.ParticipantStatus != InvitationResponseStatus.Declined)
                        {
                            interviewerResponseNotification = new InterviewerResponseNotification() { ScheduleId = scheduleObject.ScheduleID, JobApplicationId = scheduleObject.JobApplicationID, InterviewerOid = item.OID, ResponseStatus = InvitationResponseStatus.Declined };
                            item.ParticipantStatus = InvitationResponseStatus.Declined;
                        }
                    }
                    else if (attendee.Status.Response.ToLower() == "tentativelyaccepted")
                    {
                        if (item.ParticipantStatus != InvitationResponseStatus.TentativelyAccepted)
                        {
                            interviewerResponseNotification = new InterviewerResponseNotification() { ScheduleId = scheduleObject.ScheduleID, JobApplicationId = scheduleObject.JobApplicationID, InterviewerOid = item.OID, ResponseStatus = InvitationResponseStatus.TentativelyAccepted };
                            item.ParticipantStatus = InvitationResponseStatus.TentativelyAccepted;
                        }
                    }

                    ////Persisting comments
                    if (responseMessage.BodyPreview?.Length > 0)
                    {
                        item.ParticipantComments = responseMessage.BodyPreview;
                        item.ParticipantResponseDateTime = responseMessage.SentDateTime;
                    }
                }
            }

            await client.Update<JobApplicationSchedule>(scheduleObject);

            this.logger.LogInformation($"UpdateScheduleWithCalendatEventResponse: Sucessfully updated schedule with calenderEventId: {calendarEventId}. for the schedule: {scheduleObject.ScheduleID}");

            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleWithCalendatEventResponse)} method in {nameof(ScheduleQuery)}.");
            return interviewerResponseNotification;
        }

        /// <summary>
        /// Update schedule service account
        /// </summary>
        /// <param name="scheduleId">Schedule object</param>
        /// <param name="serviceAccountName">service Account Name</param>
        /// <returns>The task response</returns>
        public async Task<MeetingInfo> UpdateScheduleServiceAccount(string scheduleId, string serviceAccountName)
        {
            MeetingInfo meetingInfo;
            this.logger.LogInformation($"Started {nameof(this.UpdateScheduleServiceAccount)} method in {nameof(ScheduleQuery)} for scheduleid: {scheduleId}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new OperationFailedException("UpdateScheduleServiceAccount: Input can not be null or empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var scheduleObject = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == scheduleId);
            if (scheduleObject != null && !string.IsNullOrEmpty(scheduleObject.ScheduleID) &&
                (string.IsNullOrWhiteSpace(scheduleObject.ServiceAccountEmail) || !scheduleObject.ServiceAccountEmail.Equals(serviceAccountName, StringComparison.OrdinalIgnoreCase)))
            {
                scheduleObject.ServiceAccountEmail = serviceAccountName;

                await client.Update(scheduleObject);

                this.logger.LogInformation("UpdateScheduleServiceAccount finished for the schedule: " + scheduleId);

                var result = await client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == scheduleId);

                result.ScheduleRequester = await this.GetWorker(result.ScheduleRequester.OfficeGraphIdentifier);

                meetingInfo = result?.ToContract();
            }
            else
            {
                meetingInfo = scheduleObject?.ToContract();
            }

            this.logger.LogInformation($"Finished {nameof(this.UpdateScheduleServiceAccount)} method in {nameof(ScheduleQuery)}.");
            return meetingInfo;
        }

        /// <summary>
        /// Get schedules using freebusyschedule request
        /// </summary>
        /// <param name="requestFreeBusy">FreeBusy ScheduleRequest</param>
        /// <param name="jobApplicationId">jobApplicationId</param>
        /// <returns>The task response</returns>
        public async Task<List<MeetingInfo>> GetSchedules(FindFreeBusyScheduleRequest requestFreeBusy, string jobApplicationId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetSchedules)} method in {nameof(ScheduleQuery)}.");
            if (requestFreeBusy == null)
            {
                throw new OperationFailedException("GetSchedules: Input can not be null or empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var scheduleObjects = await client.Get<JobApplicationSchedule>(a => a.JobApplicationID == jobApplicationId &&
                    a.StartDateTime >= DateTime.Parse(requestFreeBusy.StartTime.DateTime) && a.EndDateTime <= DateTime.Parse(requestFreeBusy.EndTime.DateTime)
                    && a.ScheduleStatus != ScheduleStatus.Delete);

            List<MeetingInfo> schedules = new List<MeetingInfo>();

            var jobApplicationSchedules = scheduleObjects as JobApplicationSchedule[] ?? scheduleObjects.ToArray();
            foreach (var interviewer in requestFreeBusy.Schedules)
            {
                foreach (var item in jobApplicationSchedules)
                {
                    if (item.Participants?.Find(a => a.OID.ToLower().Equals(interviewer.ToLower())) != null)
                    {
                        schedules.Add(item?.ToContract());
                    }
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.GetSchedules)} method in {nameof(ScheduleQuery)}.");
            return schedules;
        }

        /// <summary>
        /// Get job application details
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>task response</returns>
        public async Task<JobApplication> GetJobApplication(string jobApplicationId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetJobApplication)} method in {nameof(ScheduleQuery)}.");
            if (jobApplicationId == null)
            {
                throw new BadRequestStatusException("The job application id can not be empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);
            var data = await client.GetFirstOrDefault<JobApplication>(a => a.JobApplicationID == jobApplicationId && a.Status == JobApplicationStatus.Active);
            this.logger.LogInformation($"Finished {nameof(this.GetJobApplication)} method in {nameof(ScheduleQuery)}.");
            return data;
        }

        /// <summary>
        /// Get job application details with all status
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>task response</returns>
        public async Task<JobApplication> GetJobApplicationWithStatus(string jobApplicationId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetJobApplicationWithStatus)} method in {nameof(ScheduleQuery)}.");
            if (jobApplicationId == null || string.IsNullOrWhiteSpace(jobApplicationId) || string.IsNullOrEmpty(jobApplicationId))
            {
                throw new BadRequestStatusException("The job application id can not be empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);
            var data = await client.GetFirstOrDefault<JobApplication>(a => a.JobApplicationID == jobApplicationId);
            this.logger.LogInformation($"Finished {nameof(this.GetJobApplicationWithStatus)} method in {nameof(ScheduleQuery)}.");
            return data;
        }

        /// <inheritdoc/>
        public async Task<JobApplicationParticipants> GetJobApplicationWithParticipantsAsync(string jobApplicationId)
        {
            IHcmDocumentClient client;
            IEnumerable<string> participantIds;
            SearchMetadataResponse searchMetadataResponse;
            Expression<Func<Worker, bool>> participantWorkerFilter;
            JobApplicationParticipants jobApplicationParticipants = null;
            this.logger.LogInformation($"Started {nameof(this.GetJobApplicationWithParticipantsAsync)} method in {nameof(ScheduleQuery)}.");
            if (string.IsNullOrWhiteSpace(jobApplicationId))
            {
                throw new OperationFailedException($"The job application Id is missing.{jobApplicationId}");
            }

            JobApplication jobApplication = await this.GetJobApplication(jobApplicationId).ConfigureAwait(false);
            if (jobApplication != null)
            {
                jobApplicationParticipants = new JobApplicationParticipants { Application = jobApplication };
                client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId).ConfigureAwait(false);
                participantIds = jobApplication.JobApplicationParticipants?.Select(jap => jap.OID).Distinct();
                participantWorkerFilter = worker => participantIds.Contains(worker.OfficeGraphIdentifier);
                searchMetadataResponse = await client.GetWithPagination(participantWorkerFilter, null, 0, 0).ConfigureAwait(false);
                foreach (var worker in searchMetadataResponse.Result.OfType<Worker>())
                {
                    jobApplicationParticipants.Participants.Add(new IVWorker
                    {
                        WorkerId = worker.WorkerId,
                        EmailPrimary = worker.EmailPrimary,
                        Name = worker.Name,
                        OfficeGraphIdentifier = worker.OfficeGraphIdentifier,
                    });
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.GetJobApplicationWithParticipantsAsync)} method in {nameof(ScheduleQuery)}.");
            return jobApplicationParticipants;
        }

        /// <summary>
        /// Get job application details
        /// </summary>
        /// <param name="scheduleId">schedule Id</param>
        /// <returns>task response</returns>
        public async Task<string> GetJobApplicationIdForSchedule(string scheduleId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetJobApplicationIdForSchedule)} method in {nameof(ScheduleQuery)}.");
            if (scheduleId == null)
            {
                throw new BadRequestStatusException("Input request does not contain a valid schedule id").EnsureLogged(this.logger);
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var data = client.GetFirstOrDefault<JobApplicationSchedule>(a => a.ScheduleID == scheduleId).Result?.JobApplicationID;
            this.logger.LogInformation($"Finished {nameof(this.GetJobApplicationIdForSchedule)} method in {nameof(ScheduleQuery)}.");
            return data;
        }

        /// <summary>
        /// Get Pending Schedules.
        /// </summary>
        /// <param name="forRecovery">Flag to decide the purpose of fetching pending schedules.</param>
        /// <returns>The list of pending Job Application Schedules.<see cref="JobApplicationSchedule"/></returns>
        public async Task<IList<JobApplicationSchedule>> GetPendingSchedules(bool forRecovery = false)
        {
            IList<JobApplicationSchedule> pendingSchedules = null;
            this.logger.LogInformation($"Started {nameof(this.GetPendingSchedules)} method in {nameof(ScheduleQuery)}.");
            DateTime currentDatetime = DateTime.UtcNow;

            var falconClient = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            this.logger.LogInformation($"Getting the schedules starting from {currentDatetime.AddHours(+48).ToString()} to {currentDatetime.AddHours(+72).ToString()}");
            var schedules = await falconClient.Get<JobApplicationSchedule>(a => a.StartDateTime > currentDatetime.AddHours(+48) && a.StartDateTime < currentDatetime.AddHours(+72) && a.ScheduleStatus == ScheduleStatus.Sent && a.Participants.Count() > 0);

            if (!forRecovery && (schedules != null || schedules.Count() > 0))
            {
                schedules = schedules.Where(s => s.Participants.All(p => p.ParticipantStatus == InvitationResponseStatus.Pending || p.ParticipantStatus == InvitationResponseStatus.Declined || p.ParticipantStatus == InvitationResponseStatus.TentativelyAccepted));

                pendingSchedules = new List<JobApplicationSchedule>();
                foreach (var schedule in schedules)
                {
                    var pendingSchedule = await falconClient.GetFirstOrDefault<ScheduleRemind>(s => s.ScheduleID == schedule.ScheduleID);
                    if (pendingSchedule == null)
                    {
                        ScheduleRemind scheduleremind = new ScheduleRemind()
                        {
                            ScheduleID = schedule?.ScheduleID,
                            DateTime = currentDatetime,
                            IsReminderSent = true
                        };
                        pendingSchedules.Add(schedule);
                        await falconClient.Create<ScheduleRemind>(scheduleremind);
                    }
                }
            }
            else
            {
                schedules = schedules.Where(s => s.Participants.Any(p => p.ParticipantStatus == InvitationResponseStatus.Pending));
                pendingSchedules = schedules?.ToList();
            }

            this.logger.LogInformation($"Finished {nameof(this.GetPendingSchedules)} method in {nameof(ScheduleQuery)}.");
            return pendingSchedules;
        }

        /// <summary>
        /// Get Position title of a Job Opening.
        /// </summary>
        /// <param name="jobOpeningId">Job Opening id</param>
        /// <returns>The Job Opening Position Title.<see cref="string"/></returns>
        public async Task<string> GetJobOpeningPositionTitle(string jobOpeningId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetJobOpeningPositionTitle)} method in {nameof(ScheduleQuery)}.");
            if (string.IsNullOrEmpty(jobOpeningId))
            {
                throw new BadRequestStatusException("Job Application Id can not be null or empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);

            var data = client.GetFirstOrDefault<JobOpening>(a => a.JobOpeningID == jobOpeningId).Result?.PositionTitle;
            this.logger.LogInformation($"Finished {nameof(this.GetJobOpeningPositionTitle)} method in {nameof(ScheduleQuery)}.");
            return data;
        }

        /// <summary>
        /// Update JobApplicationStatusHistory
        /// </summary>
        /// <param name="jobApplicationId">JobApplicationid</param>
        /// <param name="status">jobapplication actiob type</param>
        /// <returns>task response</returns>
        public async Task UpdateJobApplicationStatusHistoryAsync(string jobApplicationId, JobApplicationActionType status)
        {
            this.logger.LogInformation($"Started {nameof(this.UpdateJobApplicationStatusHistoryAsync)} method in {nameof(ScheduleQuery)}.");
            if (!string.IsNullOrEmpty(jobApplicationId))
            {
                this.logger.LogInformation("Updating JobApplicationStatusHistory for " + jobApplicationId);
                var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleHistoryContainerID);

                JobApplicationStatusHistory statusHistory = new JobApplicationStatusHistory()
                {
                    JobApplicationID = jobApplicationId,
                    CurrentJobApplicationStatus = status,
                    DateTime = DateTime.Now.ToString()
                };

                await client.Create<JobApplicationStatusHistory>(statusHistory);

                this.logger.LogInformation("Updated JobApplicationStatusHistory for " + jobApplicationId);
            }
            else
            {
                this.logger.LogWarning("Updating JobApplicationStatusHistory failed. Because jobApplicationId Input is null or empty.");
            }

            this.logger.LogInformation($"Finished {nameof(this.UpdateJobApplicationStatusHistoryAsync)} method in {nameof(ScheduleQuery)}.");
        }

        /// <summary>
        /// Get SendInvitationLock ItemsAsync based on schedule id
        /// </summary>
        /// <param name="scheduleIDs">The scheduleID.</param>
        /// <returns>List of Meeting infos.</returns>
        public async Task<List<SendInvitationLock>> GetScheduleLockItems(IList<string> scheduleIDs)
        {
            this.logger.LogInformation($"Started {nameof(this.GetScheduleLockItems)} method in {nameof(ScheduleQuery)}.");
            if (scheduleIDs == null || scheduleIDs.Count <= 0)
            {
                throw new BadRequestStatusException("scheduleID can not be null or empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            List<SendInvitationLock> sendInvitationLocks = client.Get<SendInvitationLock>(a => scheduleIDs.Contains(a.ScheduleId)).Result?.ToList();
            this.logger.LogInformation($"Finished {nameof(this.GetScheduleLockItems)} method in {nameof(ScheduleQuery)}.");
            return sendInvitationLocks;
        }

        /// <summary>
        /// Create SendInvitationLock ItemsAsync based on schedule id
        /// </summary>
        /// <param name="invitationLock">The scheduleID.</param>
        /// <returns>List of Meeting infos.</returns>
        public async Task CreateScheduleLockItem(SendInvitationLock invitationLock)
        {
            this.logger.LogInformation($"Started {nameof(this.CreateScheduleLockItem)} method in {nameof(ScheduleQuery)}.");
            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            await client.Create(invitationLock);

            this.logger.LogInformation($"Finished {nameof(this.CreateScheduleLockItem)} method in {nameof(ScheduleQuery)}.");
        }

        /// <summary>
        /// Delete SendInvitationLock items
        /// </summary>
        /// <param name="scheduleIDs">scheduleids</param>
        /// <returns>result</returns>
        public async Task DeleteScheduleLockItems(IList<string> scheduleIDs)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteScheduleLockItems)} method in {nameof(ScheduleQuery)}.");
            if (scheduleIDs == null)
            {
                throw new BadRequestStatusException("scheduleID can not be null or empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);
            IList<Task> deleteTasks = new List<Task>();
            foreach (var item in scheduleIDs)
            {
                var result = client.GetFirstOrDefault<SendInvitationLock>(a => a.ScheduleId == item).Result;

                if (result != null)
                {
                    deleteTasks.Add(client.Delete<SendInvitationLock>(result.Id));
                }
            }

            if (deleteTasks != null)
            {
                await Task.WhenAll(deleteTasks);
            }

            this.logger.LogInformation($"Finished {nameof(this.DeleteScheduleLockItems)} method in {nameof(ScheduleQuery)}.");
        }

        /// <summary>
        /// Delete SendInvitationLock item
        /// </summary>
        /// <param name="lockID">lockID</param>
        /// <returns>result</returns>
        public async Task DeleteScheduleLockItem(string lockID)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteScheduleLockItem)} method in {nameof(ScheduleQuery)}.");
            if (string.IsNullOrEmpty(lockID))
            {
                throw new BadRequestStatusException("lockID can not be null or empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var result = client.GetFirstOrDefault<SendInvitationLock>(a => a.Id == lockID).Result;

            if (result != null)
            {
                await client.Delete<SendInvitationLock>(result.Id);
            }

            this.logger.LogInformation($"Finished {nameof(this.DeleteScheduleLockItem)} method in {nameof(ScheduleQuery)}.");
        }

        /// <summary>
        /// CreateNotificationLockItem with NotificationMessageLock item
        /// </summary>
        /// <param name="notificationLock">notification lock</param>
        /// <returns>result</returns>
        public async Task CreateNotificationLockItem(NotificationMessageLock notificationLock)
        {
            this.logger.LogInformation($"Started {nameof(this.CreateNotificationLockItem)} method in {nameof(ScheduleQuery)}.");
            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            await client.Create(notificationLock);

            this.logger.LogInformation($"Finished {nameof(this.CreateNotificationLockItem)} method in {nameof(ScheduleQuery)}.");
        }

        /// <summary>
        /// Get notification lock with serviceBusMessageId
        /// </summary>
        /// <param name="serviceBusMessageId">serviceBusMessageId</param>
        /// <returns>result</returns>
        public async Task<NotificationMessageLock> GetNotificationLockItem(string serviceBusMessageId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetNotificationLockItem)} method in {nameof(ScheduleQuery)}.");

            if (string.IsNullOrEmpty(serviceBusMessageId))
            {
                throw new BadRequestStatusException("serviceBusMessageId can not be null or empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var result = await client.GetFirstOrDefault<NotificationMessageLock>(a => a.ServiceBusMessageId == serviceBusMessageId);
            this.logger.LogInformation($"Finished {nameof(this.GetNotificationLockItem)} method in {nameof(ScheduleQuery)}.");

            return result;
        }

        /// <summary>
        /// Delete NotificationLockItem with serviceBusMessageId
        /// </summary>
        /// <param name="serviceBusMessageId">serviceBusMessageId</param>
        /// <returns>result</returns>
        public async Task DeleteNotificationLockItem(string serviceBusMessageId)
        {
            this.logger.LogInformation($"Started {nameof(this.DeleteNotificationLockItem)} method in {nameof(ScheduleQuery)}.");

            if (string.IsNullOrEmpty(serviceBusMessageId))
            {
                throw new BadRequestStatusException("serviceBusMessageId can not be null or empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);

            var result = client.GetFirstOrDefault<NotificationMessageLock>(a => a.ServiceBusMessageId == serviceBusMessageId).Result;

            if (result != null)
            {
                await client.Delete<NotificationMessageLock>(result.Id);
            }

            this.logger.LogInformation($"Finished {nameof(this.DeleteNotificationLockItem)} method in {nameof(ScheduleQuery)}.");
        }

        /// <summary>
        /// Add or update timezone information for job application
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="timezone">timezone info</param>
        /// <returns>result</returns>
        public async Task AddOrUpdateTimezoneForJobApplication(string jobApplicationId, Timezone timezone)
        {
            this.logger.LogInformation($"Started {nameof(this.AddOrUpdateTimezoneForJobApplication)} method in {nameof(ScheduleQuery)}.");
            if (string.IsNullOrEmpty(jobApplicationId) || timezone == null)
            {
                this.logger.LogWarning("Updating AddOrUpdateTimezoneForJobApplication failed. Because jobApplicationId Input is null or empty.");
            }
            else
            {
                this.logger.LogInformation("Updating AddOrUpdateTimezoneForJobApplication for " + jobApplicationId);
                var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);
                var result = client.GetFirstOrDefault<TimezoneData>(a => a.JobApplicationId == jobApplicationId).Result;

                if (result == null)
                {
                    TimezoneData timezoneData = new TimezoneData
                    {
                        JobApplicationId = jobApplicationId,
                        TimezoneAbbr = timezone.TimezoneAbbr,
                        TimezoneName = timezone.TimezoneName,
                        UTCOffset = timezone.UTCOffset,
                        UTCOffsetInHours = timezone.UTCOffsetInHours,

                        Type = "TimezoneData"
                    };
                    await client.Create<TimezoneData>(timezoneData);

                    this.logger.LogInformation("Created TimezoneForJobApplication for " + jobApplicationId);
                }
                else
                {
                    result.TimezoneAbbr = timezone.TimezoneAbbr;
                    result.TimezoneName = timezone.TimezoneName;
                    result.UTCOffset = timezone.UTCOffset;
                    result.UTCOffsetInHours = timezone.UTCOffsetInHours;

                    await client.Update<TimezoneData>(result);

                    this.logger.LogInformation("Updated TimezoneForJobApplication for " + jobApplicationId);
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.AddOrUpdateTimezoneForJobApplication)} method in {nameof(ScheduleQuery)}.");
        }

        /// <summary>
        /// Get timezone information for job application
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>result</returns>
        public async Task<Timezone> GetTimezoneForJobApplication(string jobApplicationId)
        {
            Timezone timezone = null;
            this.logger.LogInformation($"Started {nameof(this.GetTimezoneForJobApplication)} method in {nameof(ScheduleQuery)}.");
            if (string.IsNullOrEmpty(jobApplicationId))
            {
                this.logger.LogWarning("GetTimezoneForJobApplication failed. Because jobApplicationId Input is null or empty.");
            }
            else
            {
                this.logger.LogInformation("GetTimezoneForJobApplication for " + jobApplicationId);
                var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);
                var result = client.GetFirstOrDefault<TimezoneData>(a => a.JobApplicationId == jobApplicationId).Result;
                if (result != null)
                {
                    timezone = new Timezone
                    {
                        TimezoneAbbr = result.TimezoneAbbr,
                        TimezoneName = result.TimezoneName,
                        UTCOffset = result.UTCOffset,
                        UTCOffsetInHours = result.UTCOffsetInHours
                    };
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.GetTimezoneForJobApplication)} method in {nameof(ScheduleQuery)}.");

            return timezone;
        }

        /// <summary>
        /// Updates the job applicaton.
        /// </summary>
        /// <param name="jobApplication">The instance of <see cref="JobApplication" />.</param>
        /// <returns>
        /// The instance of <see cref="Task{Boolean}" /> where <c>true</c> if job application is updated; otherwise <c>false</c>.
        /// </returns>
        public async Task<bool> UpdateJobApplicaton(JobApplication jobApplication)
        {
            Contract.CheckValue(jobApplication, nameof(jobApplication));
            var commonClient = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);
            await commonClient.Update<JobApplication>(jobApplication);
            return true;
        }

        /// <summary>
        /// Gets Job Application with candidate details by job application id
        /// </summary>
        /// <param name="jobApplicationId">job application id.</param>
        /// <returns>Job application.</returns>
        public async Task<JobApplication> GetJobApplicationWithCandidateDetails(string jobApplicationId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetJobApplication)} method in {nameof(ScheduleQuery)}.");

            if (string.IsNullOrWhiteSpace(jobApplicationId))
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid application id").EnsureLogged(this.logger);
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);

            this.logger.LogInformation("GetJobApplication JobApplicationQuery: Finished");
            var jobApplication = await client.GetFirstOrDefault<JobApplication>(ja => ja.JobApplicationID == jobApplicationId);

            // Get Candidate Name from Candidate
            var candidateTask = client.GetWithProjections<Candidate, Candidate>(
                c => c.CandidateID == jobApplication.Candidate.CandidateID,
                c => new Candidate()
                {
                    CandidateID = c.CandidateID,
                    FullName = c.FullName ?? null
                });

            // Get Position Title from Job Opening
            var jobOpeningTask = client.GetWithProjections<JobOpening, JobOpening>(
               jobOpening => jobOpening.JobOpeningID == jobApplication.JobOpening.JobOpeningID,
               jobOpening => new JobOpening()
               {
                   JobOpeningID = jobOpening.JobOpeningID,
                   PositionTitle = jobOpening.PositionTitle ?? null,
               });

            await Task.WhenAll(candidateTask, jobOpeningTask);

            jobApplication.Candidate.FullName = candidateTask?.Result?.FirstOrDefault()?.FullName;
            jobApplication.JobOpening.PositionTitle = jobOpeningTask?.Result?.FirstOrDefault()?.PositionTitle;

            this.logger.LogInformation($"Finished {nameof(this.GetJobApplication)} method in {nameof(ScheduleQuery)}.");
            return jobApplication;
        }

        /// <summary>
        /// Get job application details
        /// </summary>
        /// <param name="requisitionId">job application id</param>
        /// <returns>task response</returns>
        public async Task<List<JobApplication>> GetJobApplicationsByRequisitionId(string requisitionId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetJobApplicationsByRequisitionId)} method in {nameof(ScheduleQuery)}.");
            if (requisitionId == null)
            {
                throw new BadRequestStatusException("GetJobApplicationsByRequisitionId: Input can not be null or empty");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);

            this.logger.LogInformation($"Finished {nameof(this.GetJobApplicationsByRequisitionId)} method in {nameof(ScheduleQuery)}.");
            return (await client.Get<JobApplication>(a => a.JobOpening.ExternalJobOpeningID == requisitionId && a.Status == JobApplicationStatus.Active))?.ToList();
        }

        /// <summary>
        /// Get job application details
        /// </summary>
        /// <param name="jobApplicationIds">job application ids</param>
        /// <returns>task response</returns>
        public async Task<IList<JobApplication>> GetJobApplications(IList<string> jobApplicationIds)
        {
            this.logger.LogInformation($"Started {nameof(this.GetJobApplications)} method in {nameof(ScheduleQuery)}.");
            if (jobApplicationIds == null || jobApplicationIds.Count == 0)
            {
                throw new BadRequestStatusException("Input request does not contain a valid applications");
            }

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);

            var data = await client.Get<JobApplication>(ja => jobApplicationIds.Contains(ja.JobApplicationID));
            this.logger.LogInformation($"Finished {nameof(this.GetJobApplications)} method in {nameof(ScheduleQuery)}.");
            return data.ToList();
        }

        /// <summary>
        /// Gets all pending job schedules along with the job application.
        /// </summary>
        /// <param name="reminderOffsetHours">Hours passed after interview before sending feedback reminder.</param>
        /// <param name="reminderWindowMinutes">Window duration in minutes for which to send feedback reminder.</param>
        /// <returns>An instance of <see cref="Task{T}"/> where <c>T</c> being <see cref="IList{PendingFeedback}"/>.</returns>
        public async Task<IList<Data.Models.PendingFeedback>> GetAllPendingFeedbacksForReminderAsync(int reminderOffsetHours, int reminderWindowMinutes)
        {
            this.logger.LogInformation($"Started {nameof(this.GetAllPendingFeedbacksForReminderAsync)} method in {nameof(ScheduleQuery)}.");

            var pendingFeedbacksList = new List<Data.Models.PendingFeedback>();
            var currentUTCDateTime = DateTime.UtcNow;

            if (reminderOffsetHours < 0)
            {
                throw new OperationFailedException("Invalid feedback reminder offset duration.");
            }

            if (reminderWindowMinutes <= 0)
            {
                throw new OperationFailedException("Invalid reminder window duration.");
            }

            var windowStartDateTime = currentUTCDateTime.AddHours(-1 * reminderOffsetHours).AddMinutes(-1 * reminderWindowMinutes);
            var windowEndDateTime = currentUTCDateTime.AddHours(-1 * reminderOffsetHours);

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId).ConfigureAwait(false);
            var commonClient = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId).ConfigureAwait(false);

            var schedules = await client.Get<JobApplicationSchedule>(a => a.EndDateTime > windowStartDateTime
                && a.EndDateTime <= windowEndDateTime
                && a.ScheduleStatus == ScheduleStatus.Sent
                && a.Participants.Count() > 0).ConfigureAwait(false);
            var jobApplicationIds = schedules.Select(jas => jas.JobApplicationID).Distinct();

            var jobApplications = await commonClient.Get<JobApplication>(ja => jobApplicationIds.Contains(ja.JobApplicationID) && ja.Status == JobApplicationStatus.Active).ConfigureAwait(false);

            jobApplications?.ForEach(ja =>
            {
                ja.JobApplicationSchedules = schedules?.Where(jas => jas.JobApplicationID == ja.JobApplicationID).ToList();
            });

            pendingFeedbacksList = (await this.GetPendingFeedbacksForJobApplications(jobApplications?.ToList()).ConfigureAwait(false))?.ToList();

            this.logger.LogInformation($"Finished {nameof(this.GetAllPendingFeedbacksForReminderAsync)} method in {nameof(ScheduleQuery)}.");
            return pendingFeedbacksList;
        }

        /// <summary>
        /// Gets all pending job schedules for given job application.
        /// </summary>
        /// <param name="jobApplication">Job application.</param>
        /// <returns>An instance of <see cref="Task{T}"/> where <c>T</c> being <see cref="IList{PendingFeedback}"/>.</returns>
        public async Task<IList<Data.Models.PendingFeedback>> GetPendingSchedulesForJobApplication(JobApplication jobApplication)
        {
            this.logger.LogInformation($"Started {nameof(this.GetPendingSchedulesForJobApplication)} method in {nameof(ScheduleQuery)}.");

            if (jobApplication == null || string.IsNullOrWhiteSpace(jobApplication?.JobApplicationID))
            {
                throw new BadRequestStatusException($"{nameof(this.GetPendingSchedulesForJobApplication)}: JobApplication or job application id can not be null or empty");
            }

            var pendingFeedbacksList = new List<Data.Models.PendingFeedback>();
            DateTime currentDatetime = DateTime.UtcNow;

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId).ConfigureAwait(false);
            var schedules = await client.Get<JobApplicationSchedule>(jas => jas.JobApplicationID == jobApplication.JobApplicationID
                && jas.ScheduleStatus == ScheduleStatus.Sent
                && currentDatetime.AddDays(DayOldFeedbackConstant) > jas.StartDateTime
                && jas.StartDateTime > currentDatetime.AddDays(TwoWeekOldFeedbackConstant)).ConfigureAwait(false);

            jobApplication.JobApplicationSchedules = schedules?.ToList();

            pendingFeedbacksList = (await this.GetPendingFeedbacksForJobApplications(new List<JobApplication> { jobApplication }).ConfigureAwait(false))?.ToList();

            this.logger.LogInformation($"Finished {nameof(this.GetPendingSchedulesForJobApplication)} method in {nameof(ScheduleQuery)}.");
            return pendingFeedbacksList;
        }

        private async Task<IList<Data.Models.PendingFeedback>> GetPendingFeedbacksForJobApplications(IList<JobApplication> jobApplications)
        {
            this.logger.LogInformation($"Started {nameof(this.GetPendingFeedbacksForJobApplications)} method in {nameof(ScheduleQuery)}.");

            var workerIds = new List<string>();
            var pendingFeedbacksList = new List<Data.Models.PendingFeedback>();

            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId).ConfigureAwait(false);
            var commonClient = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId).ConfigureAwait(false);

            var jobOpeningIds = jobApplications?.Select(ja => ja.JobOpening.JobOpeningID).Distinct() ?? Enumerable.Empty<string>();
            var candidateIds = jobApplications?.Select(ja => ja.Candidate.CandidateID).Distinct() ?? Enumerable.Empty<string>();

            var jobOpeningTask = commonClient.GetWithProjections<JobOpening, JobOpening>(
               jobOpening => jobOpeningIds.Contains(jobOpening.JobOpeningID),
               jobOpening => new JobOpening()
               {
                   JobOpeningID = jobOpening.JobOpeningID,
                   PositionTitle = jobOpening.PositionTitle ?? null,
                   ExternalJobOpeningID = jobOpening.ExternalJobOpeningID ?? null
               });

            var candidateTask = commonClient.GetWithProjections<Candidate, Candidate>(
                c => candidateIds.Contains(c.CandidateID),
                c => new Candidate()
                {
                    CandidateID = c.CandidateID,
                    FullName = c.FullName ?? null
                });

            await Task.WhenAll(candidateTask, jobOpeningTask);

            jobApplications?.ForEach(ja =>
            {
                var hiringManagerOid = ja.JobApplicationParticipants?.FirstOrDefault(jop => jop.Role == JobParticipantRole.HiringManager)?.OID;
                var recruiterOid = ja.JobApplicationParticipants?.FirstOrDefault(jop => jop.Role == JobParticipantRole.Recruiter)?.OID;
                ja.Candidate = candidateTask?.Result?.FirstOrDefault(candidate => candidate.CandidateID == ja.Candidate.CandidateID);
                ja.JobOpening = jobOpeningTask?.Result?.FirstOrDefault(jobOpening => jobOpening.JobOpeningID == ja.JobOpening.JobOpeningID);

                ja?.JobApplicationSchedules?.ForEach(jas =>
                {
                    jas.Participants?.ForEach(participant =>
                    {
                        var isInterviewerWithPendingFeedback = ja.JobApplicationParticipants?.Where(jap => jap.OID == participant.OID && jap.Role == JobParticipantRole.Interviewer)?.Count() > 0
                                                                && !participant.IsAssessmentCompleted;

                        if (isInterviewerWithPendingFeedback)
                        {
                            var pendingFeedback = new Data.Models.PendingFeedback();
                            pendingFeedback.JobApplicationId = ja?.JobApplicationID;
                            pendingFeedback.PositionTitle = ja?.JobOpening?.PositionTitle;
                            pendingFeedback.CandidateName = ja?.Candidate?.FullName?.GivenName + " " + ja.Candidate?.FullName?.Surname;
                            pendingFeedback.ExternalJobOpeningId = ja?.JobOpening?.ExternalJobOpeningID;
                            pendingFeedback.Interviewer = new Common.TalentEntities.Common.Worker() { OfficeGraphIdentifier = participant.OID };
                            pendingFeedback.HiringManager = new Common.TalentEntities.Common.Worker() { OfficeGraphIdentifier = hiringManagerOid };
                            pendingFeedback.Recruiter = new Common.TalentEntities.Common.Worker() { OfficeGraphIdentifier = recruiterOid };
                            pendingFeedback.ScheduleRequester = new Common.TalentEntities.Common.Worker() { OfficeGraphIdentifier = jas?.ScheduleRequester?.OfficeGraphIdentifier };
                            pendingFeedbacksList.Add(pendingFeedback);

                            workerIds.Add(participant.OID);
                            workerIds.Add(jas.ScheduleRequester.OfficeGraphIdentifier);
                            workerIds.Add(hiringManagerOid);
                            workerIds.Add(recruiterOid);
                        }
                    });
                });
            });

            workerIds = workerIds?.Distinct()?.ToList();
            var workers = await commonClient.Get<Common.TalentEntities.Common.Worker>(w => workerIds.Contains(w.OfficeGraphIdentifier)).ConfigureAwait(false);

            pendingFeedbacksList?.ForEach(pf =>
            {
                var interviewer = workers?.FirstOrDefault(w => w.OfficeGraphIdentifier == pf.Interviewer?.OfficeGraphIdentifier);
                var hiringManager = workers?.FirstOrDefault(w => w.OfficeGraphIdentifier == pf.HiringManager?.OfficeGraphIdentifier);
                var recruiter = workers?.FirstOrDefault(w => w.OfficeGraphIdentifier == pf.Recruiter?.OfficeGraphIdentifier);
                var scheduleRequester = workers?.FirstOrDefault(w => w.OfficeGraphIdentifier == pf.ScheduleRequester?.OfficeGraphIdentifier);

                pf.Interviewer = interviewer;
                pf.HiringManager = hiringManager;
                pf.Recruiter = recruiter;
                pf.ScheduleRequester = scheduleRequester;
            });

            this.logger.LogInformation($"Finished {nameof(this.GetPendingFeedbacksForJobApplications)} method in {nameof(ScheduleQuery)}.");

            return pendingFeedbacksList;
        }

        /// <summary>
        /// get status
        /// </summary>
        /// <param name="status">status</param>
        /// <returns>meeting info contract</returns>
        private InvitationResponseStatus GetResponseStatus(InvitationResponseStatus? status)
        {
            if (status != null)
            {
                return status.ToContractEnum<InvitationResponseStatus>();
            }
            else
            {
                return InvitationResponseStatus.None;
            }
        }

        private async Task UpdateScheduleHistoryAsync(string scheduleId, ScheduleStatus status, string jobApplicationID)
        {
            this.logger.LogInformation("Updating ScheduleHistory for " + scheduleId);
            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleHistoryContainerID);
            var commonClient = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);

            if (jobApplicationID != null)
            {
                var jobApplication = await commonClient.GetFirstOrDefault<JobApplication>(ja => ja.JobApplicationID == jobApplicationID);

                if (jobApplication != null)
                {
                    if (status != ScheduleStatus.Sent)
                    {
                        jobApplication.IsScheduleSentToCandidate = false;
                    }

                    // For use cases like Candidate Tracker, we don't have an actual
                    // job application in place. Hence this update should be done inside the check.
                    await commonClient.Update<JobApplication>(jobApplication);
                }
            }

            ScheduleStatusHistory statusHistory = new ScheduleStatusHistory()
            {
                ScheduleID = scheduleId,
                CurrentScheduleStatus = status,
                DateTime = DateTime.Now.ToString()
            };

            await client.Create(statusHistory);

            this.logger.LogInformation("Updated ScheduleHistory for " + scheduleId);
        }

        private string GetOnlineMeetingContent(string content)
        {
            string onlineMeetingContent = string.Empty;

            if (!string.IsNullOrEmpty(content) && content.Contains("________________________________________________________________________________"))
            {
                string[] seperator = new string[1];
                seperator[0] = "________________________________________________________________________________";
                string[] test = content.Split(seperator, StringSplitOptions.None);

                onlineMeetingContent = "<div><span>________________________________________________________________________________" + test[1]
                    + "</div>________________________________________________________________________________</span>";
            }

            return onlineMeetingContent;
        }

        /// <summary>
        /// Gets the proposed new time for the participant.
        /// </summary>
        /// <param name="responseMessage">The response message.</param>
        /// <param name="item">The item.</param>
        /// <returns>The instance of <see cref="Task{MeetingTimeSpan}"/> representing an asynchronous operation.</returns>
        private async Task<MeetingTimeSpan> GetParticipantProposedNewTime(Message responseMessage, JobApplicationScheduleParticipant item)
        {
            MeetingTimeSpan proposedNewTime;
            var commonClient = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);
            var worker = await commonClient.GetFirstOrDefault<Worker>(wrk => wrk.WorkerId == item.OID || wrk.OfficeGraphIdentifier == item.OID);
            proposedNewTime = responseMessage?.Event?.Attendees?.Where(attendee => attendee.EmailAddress.Address.Equals(worker.EmailPrimary, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.ProposedNewTime;
            return proposedNewTime;
        }

        /// <summary>
        /// Inserts an audit log entry for the action performed on the job application schedule.
        /// </summary>
        /// <param name="jobApplicationSchedule">Job Application schedule on which the action has been performed.</param>
        /// <param name="action">Action performed - <see cref="JobApplicationScheduleAction"/>.</param>
        /// <returns>The instance of <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task InsertScheduleAuditLog(JobApplicationSchedule jobApplicationSchedule, JobApplicationScheduleAction action)
        {
            Contract.CheckValueOrNull(jobApplicationSchedule, "Schedule entity cannot be null");
            this.logger.LogInformation($"Inserting Audit log for Schedule ID:{0} and Action:{1}...", jobApplicationSchedule.ScheduleID, action);
            var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleHistoryContainerID);
            JobApplicationScheduleLog jobApplicationScheduleLogEntry = new JobApplicationScheduleLog()
            {
                ScheduleID = jobApplicationSchedule.ScheduleID,
                Action = action,
                BuildingName = jobApplicationSchedule.BuildingName,
                Description = jobApplicationSchedule.Description,
                EndDateTime = jobApplicationSchedule.EndDateTime,
                IsPrivateMeeting = jobApplicationSchedule.IsPrivateMeeting,
                JobApplicationID = jobApplicationSchedule.JobApplicationID,
                Location = jobApplicationSchedule.Location,
                OnlineMeetingRequired = jobApplicationSchedule.OnlineMeetingRequired,
                Participants = jobApplicationSchedule.Participants?.Select(p => new JobApplicationScheduleParticipant() { OID = p?.OID }).ToList(),
                RoomName = jobApplicationSchedule.RoomName,
                ScheduleRequester = new Worker() { OfficeGraphIdentifier = jobApplicationSchedule.ScheduleRequester?.OfficeGraphIdentifier, Id = jobApplicationSchedule.ScheduleRequester?.Id },
                ScheduleTimezoneAbbr = jobApplicationSchedule.ScheduleTimezoneAbbr,
                ScheduleTimezoneName = jobApplicationSchedule.ScheduleTimezoneName,
                ScheduleUTCOffset = jobApplicationSchedule.ScheduleUTCOffset,
                ScheduleUTCOffsetInHours = jobApplicationSchedule.ScheduleUTCOffsetInHours,
                StartDateTime = jobApplicationSchedule.StartDateTime,
                Subject = jobApplicationSchedule.Subject
            };

            await client.Create(jobApplicationScheduleLogEntry);
            this.logger.LogInformation($"Inserted Audit log for Schedule ID:{0} and Action:{1}.", jobApplicationSchedule.ScheduleID, action);
        }
    }
}
