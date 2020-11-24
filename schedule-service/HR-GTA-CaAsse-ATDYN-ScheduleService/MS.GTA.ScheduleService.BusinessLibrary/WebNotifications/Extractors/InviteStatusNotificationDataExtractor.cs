// <copyright file="InviteStatusNotificationDataExtractor.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Extractors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Base.Security;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.Common.WebNotifications;
    using MS.GTA.Common.WebNotifications.Exceptions;
    using MS.GTA.Common.WebNotifications.Interfaces;
    using MS.GTA.Common.WebNotifications.Models;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Configurations;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.Data.Models;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.Talent.TalentContracts.InterviewService;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The <see cref="InviteStatusNotificationDataExtractor"/> class helps to extract the invite status update event's web notification properties.
    /// </summary>
    /// <seealso cref="IWebNotificationDataExtractor" />
    internal class InviteStatusNotificationDataExtractor : IWebNotificationDataExtractor
    {
        /// <summary>
        /// The title data constant.
        /// </summary>
        private const string Title = "Invite Status Update";

        /// <summary>
        /// The app notification type constant.
        /// </summary>
        private const string AppNotificationType = "InviteStatusUpdate";

        /// <summary>
        /// The lock object
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// The status verb map.
        /// </summary>
        private static readonly Dictionary<InvitationResponseStatus, string> StatusVerbMap = new Dictionary<InvitationResponseStatus, string>
        {
            { InvitationResponseStatus.Accepted, "accepted" },
            { InvitationResponseStatus.Declined, "declined" },
            { InvitationResponseStatus.TentativelyAccepted, "tentatively accepted" },
        };

        /// <summary>
        /// The processed interviewer map
        /// </summary>
        private static readonly Dictionary<string, DateTime> ProcessedInterviewerMap = new Dictionary<string, DateTime>();

        /// <summary>
        /// The interviewer invite response infos
        /// </summary>
        private readonly IEnumerable<InterviewerInviteResponseInfo> interviewerInviteResponseInfos;

        /// <summary>
        /// The deep links templates.
        /// </summary>
        private readonly DeepLinksConfiguration deepLinksTemplates;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<InviteStatusNotificationDataExtractor> logger;

        /// <summary>
        /// The scheduleQuery.
        /// </summary>
        private readonly IScheduleQuery scheduleQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="InviteStatusNotificationDataExtractor"/> class.
        /// </summary>
        /// <param name="interviewerInviteResponseInfos">The instance for <see cref="InterviewerInviteResponseInfo"/>.</param>
        /// <param name="deepLinksConfiguration">The instance of <see cref="DeepLinksConfiguration"/>.</param>
        /// <param name="scheduleQuery">The instance of <see cref="IScheduleQuery"/>.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">
        /// interviewerResponseJobApplicationObjects
        /// or
        /// logger
        /// </exception>
        public InviteStatusNotificationDataExtractor(IEnumerable<InterviewerInviteResponseInfo> interviewerInviteResponseInfos, DeepLinksConfiguration deepLinksConfiguration, IScheduleQuery scheduleQuery, ILogger<InviteStatusNotificationDataExtractor> logger)
        {
            this.interviewerInviteResponseInfos = interviewerInviteResponseInfos ?? throw new ArgumentNullException(nameof(interviewerInviteResponseInfos));
            this.deepLinksTemplates = deepLinksConfiguration ?? throw new ArgumentNullException(nameof(deepLinksConfiguration));
            this.scheduleQuery = scheduleQuery ?? throw new ArgumentNullException(nameof(scheduleQuery));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Dictionary<string, string>>> Extract()
        {
            this.logger.LogInformation($"Started {nameof(this.Extract)} method in {nameof(InviteStatusNotificationDataExtractor)}");
            Task<IEnumerable<Dictionary<string, string>>> notificationsDataTask = this.ExtractNotificationsProperties();
            this.logger.LogInformation($"Finished {nameof(this.Extract)} method in {nameof(InviteStatusNotificationDataExtractor)}");
            return notificationsDataTask;
        }

        /// <summary>
        /// Extracts the relevant properties for web notifications.
        /// </summary>
        /// <returns>The instance of <see cref="IEnumerable{T}"/> where <c>T</c> being <see cref="Dictionary{String, String}" />.</returns>
        private Task<IEnumerable<Dictionary<string, string>>> ExtractNotificationsProperties()
        {
            List<Dictionary<string, string>> notificationsData = new List<Dictionary<string, string>>();
            JobApplication jobApplication;
            List<ParticipantData> schedulerData = new List<ParticipantData>();
            List<ParticipantData> recruiterData = new List<ParticipantData>();
            foreach (var interviewerInviteResponseInfo in this.interviewerInviteResponseInfos)
            {
                if (interviewerInviteResponseInfo.ApplicationParticipants?.Application != null &&
                    interviewerInviteResponseInfo.ResponseNotification?.InterviewerOid != null &&
                    !this.CheckDuplicateProcessing(interviewerInviteResponseInfo))
                {
                    try
                    {
                        jobApplication = interviewerInviteResponseInfo.ApplicationParticipants.Application;

                        schedulerData.AddRange(this.ExtractParticipantsData(interviewerInviteResponseInfo.ApplicationParticipants, JobParticipantRole.Contributor));
                        recruiterData.AddRange(this.ExtractParticipantsData(interviewerInviteResponseInfo.ApplicationParticipants, JobParticipantRole.Recruiter));

                        notificationsData.AddRange(schedulerData.Select(sch => this.ExtractNotificationProperties(interviewerInviteResponseInfo, sch, false)));
                        notificationsData.AddRange(recruiterData.Select(rec => this.ExtractNotificationProperties(interviewerInviteResponseInfo, rec, false)));
                        List<string> participantsOids = new List<string>();
                        participantsOids.AddRange(recruiterData.Select(rd => rd.ObjectIdentifier));
                        participantsOids.AddRange(schedulerData.Select(sd => sd.ObjectIdentifier));
                        List<ParticipantData> wobUsersParticipants = this.ExtractWobUserDataAsync(participantsOids).Result;
                        notificationsData.AddRange(wobUsersParticipants.Select(wp => this.ExtractNotificationProperties(interviewerInviteResponseInfo, wp, true)));
                    }
                    catch (Exception ex)
                    {
                        // Do not rethrow to ensure execution progress to next item after logging.
                        this.logger.LogError(ex, $"Invite status update data extraction failed for interviewer with identifier '{interviewerInviteResponseInfo.ResponseNotification.InterviewerOid}'. Skipping to next...");
                    }
                }
                else
                {
                    this.logger.LogWarning("The interviewer response info is either missing job application(/ participants) or response notification(/interviewer Oid). Skipping to next...");
                }
            }

            return Task.FromResult(notificationsData.AsEnumerable());
        }

        /// <summary>
        /// Checks whether the processing is duplicate.
        /// </summary>
        /// <param name="interviewerInviteResponseInfo">The interviewer invite response information.</param>
        /// <returns><c>true</c> if this is a duplicate processing; otherwise <c>false</c>.</returns>
        private bool CheckDuplicateProcessing(InterviewerInviteResponseInfo interviewerInviteResponseInfo)
        {
            bool isDuplicate = false;
            DateTime timeStamp;
            string processedInterviewerScheduleId;
            string interviewerOid = interviewerInviteResponseInfo.ResponseNotification?.InterviewerOid;
            string scheduleId = interviewerInviteResponseInfo.ResponseNotification?.ScheduleId;
            string checkKey = (interviewerOid ?? string.Empty) + "_" + (scheduleId ?? string.Empty);
            lock (LockObject)
            {
                this.logger.LogInformation("Clearing expired entries before duplicate processing check...");
                for (int i = ProcessedInterviewerMap.Keys.Count - 1; i > -1; i--)
                {
                    processedInterviewerScheduleId = ProcessedInterviewerMap.Keys.ElementAt(i);
                    timeStamp = ProcessedInterviewerMap[processedInterviewerScheduleId];
                    if ((DateTime.UtcNow - timeStamp).TotalSeconds >= 4)
                    {
                        ProcessedInterviewerMap.Remove(processedInterviewerScheduleId);
                    }
                }

                if (!string.IsNullOrWhiteSpace(interviewerOid))
                {
                    isDuplicate = ProcessedInterviewerMap.ContainsKey(checkKey);
                    if (!isDuplicate)
                    {
                        ProcessedInterviewerMap[checkKey] = DateTime.UtcNow;
                    }
                    else
                    {
                        this.logger.LogInformation($"Duplicate Key '{checkKey}'");
                    }
                }
            }

            return isDuplicate;
        }

        /// <summary>
        /// Extracts the relevant properties for web notification.
        /// </summary>
        /// <param name="interviewerInviteResponseInfo"> Interviewer invite response info</param>
        /// <param name="participantData"> Participant Data</param>
        /// <param name="isForWobContext"> is the request for wob data</param>
        /// <returns>The instance of <see cref="IEnumerable{T}"/> where <c>T</c> being <see cref="Dictionary{String, String}" />.</returns>
        private Dictionary<string, string> ExtractNotificationProperties(InterviewerInviteResponseInfo interviewerInviteResponseInfo, ParticipantData participantData, bool isForWobContext)
        {
            Dictionary<string, string> notificationData = new Dictionary<string, string>();
            JobApplication jobApplication;
            ParticipantData interviewerData;
            jobApplication = interviewerInviteResponseInfo.ApplicationParticipants.Application;
            InterviewerResponseNotification interviewerResponseNotification = interviewerInviteResponseInfo.ResponseNotification;
            interviewerData = this.ExtractInterviewerData(interviewerInviteResponseInfo);
            if (jobApplication != null && interviewerResponseNotification != null && interviewerInviteResponseInfo.ApplicationParticipants != null)
            {
                notificationData = new Dictionary<string, string>
                    {
                        { NotificationConstants.Title, Title },
                        { NotificationConstants.JobTitle, jobApplication.JobOpening?.PositionTitle ?? NotificationConstants.Unknown },
                        { NotificationConstants.DeepLink, this.deepLinksTemplates.ScheduleSummaryUrl.Replace("{JobApplicationId}", jobApplication.JobApplicationID) },
                        { WebNotificationConstants.InterviewerName, interviewerData.Name },
                        { WebNotificationConstants.StatusVerb, StatusVerbMap[interviewerResponseNotification.ResponseStatus] },
                        { NotificationConstants.SenderObjectId, interviewerData.ObjectIdentifier },
                        { NotificationConstants.SenderName, interviewerData.Name },
                        { NotificationConstants.SenderEmail, interviewerData.Email },
                        { NotificationConstants.RecipientObjectId, participantData.ObjectIdentifier },
                        { NotificationConstants.RecipientName, participantData.Name },
                        { NotificationConstants.RecipientEmail, participantData.Email },
                        { NotificationConstants.AppNotificationType, AppNotificationType },
                        { NotificationConstants.IsWobContext, isForWobContext.ToString() },
                    };

                if (!string.IsNullOrWhiteSpace(interviewerInviteResponseInfo.InterviewerMessage))
                {
                    notificationData.Add(WebNotificationConstants.MessageResponse, interviewerInviteResponseInfo.InterviewerMessage);
                }

                if (interviewerInviteResponseInfo.ResponseNotification.ProposedNewTime != null)
                {
                    // Add timezone designatior Z to make parseable in local time zone.
                    notificationData.Add(WebNotificationConstants.ProposedStartTime, interviewerInviteResponseInfo.ResponseNotification.ProposedNewTime.Start.DateTime + "Z");
                    notificationData.Add(WebNotificationConstants.ProposedEndTime, interviewerInviteResponseInfo.ResponseNotification.ProposedNewTime.End.DateTime + "Z");
                }
            }

            return notificationData;
        }

        /// <summary>
        /// Extracts the interviewer data.
        /// </summary>
        /// <param name="interviewerInviteResponseInfo">The instance of <see cref="InterviewerInviteResponseInfo"/>.</param>
        /// <returns>The instance of <see cref="ParticipantData"/>.</returns>
        /// <exception cref="WebNotificationException">The user, providing feedback, with OID '{feedback.FeedbackProvider.ObjectId}' is not a job application participant.</exception>
        private ParticipantData ExtractInterviewerData(InterviewerInviteResponseInfo interviewerInviteResponseInfo)
        {
            ParticipantData interviewerData;
            InterviewerResponseNotification responseNotification = interviewerInviteResponseInfo.ResponseNotification;
            interviewerData = this.GetParticipantWithId(responseNotification.InterviewerOid, interviewerInviteResponseInfo.ApplicationParticipants.Participants);
            if (interviewerData == null)
            {
                throw new OperationFailedException($"The user, acting on invite, with OID '{responseNotification.InterviewerOid}' is not a job application participant.");
            }

            return interviewerData;
        }

        /// <summary>
        /// Extracts the participants data.
        /// </summary>
        /// <param name="jobApplicationParticipants">The instance for <see cref="JobApplicationParticipant"/>.</param>
        /// <param name="jobParticipantRole">Role of the participant</param>
        /// <returns>The instance of <see cref="ParticipantData"/>.</returns>
        private List<ParticipantData> ExtractParticipantsData(JobApplicationParticipants jobApplicationParticipants, JobParticipantRole jobParticipantRole)
        {
            ParticipantData parData = null;
            List<ParticipantData> participantsData = new List<ParticipantData>();
            JobApplication jobApplication = jobApplicationParticipants.Application;
            List<JobApplicationParticipant> participants = jobApplication.JobApplicationParticipants
                    .Where(jap => jap.Role == jobParticipantRole)
                    .ToList();

            if (!participants.Any())
            {
                this.logger.LogWarning($"No {jobParticipantRole}s are found for job application with identifier '{jobApplication.JobApplicationID}'.");
                return participantsData;
            }

            participants.ForEach(par =>
            {
                parData = this.GetParticipantWithId(par.OID, jobApplicationParticipants.Participants);
                if (parData != null)
                {
                    participantsData.Add(parData);
                }
            });

            if (!participantsData.Any(par => par != null))
            {
                this.logger.LogWarning($"For some {jobParticipantRole}s information is missing for job application with identifier '{jobApplication.JobApplicationID}'.");
                participantsData = participantsData.Where(par => par != null)?.ToList();
            }

            return participantsData;
        }

        /// <summary>
        /// Gets the participant with identifier.
        /// </summary>
        /// <param name="objectIdentifer">The participant object identifer.</param>
        /// <param name="participants">The instance for <see cref="IEnumerable{IVWorker}"/>.</param>
        /// <returns>The instance of <see cref="ParticipantData"/>.</returns>
        private ParticipantData GetParticipantWithId(string objectIdentifer, IEnumerable<IVWorker> participants)
        {
            string fullName;
            ParticipantData participantData = null;
            IVWorker worker = participants.Where(wrk => wrk != null && ((!string.IsNullOrWhiteSpace(wrk.WorkerId) && wrk.WorkerId.Equals(objectIdentifer, StringComparison.OrdinalIgnoreCase)) || (!string.IsNullOrWhiteSpace(wrk.OfficeGraphIdentifier) && wrk.OfficeGraphIdentifier.Equals(objectIdentifer, StringComparison.OrdinalIgnoreCase)))).FirstOrDefault();
            if (worker != null)
            {
                fullName = worker.FullName;
                if (string.IsNullOrWhiteSpace(worker.FullName))
                {
                    fullName = (worker.Name.GivenName ?? string.Empty) + " " + (worker.Name.Surname ?? string.Empty);
                    if (string.IsNullOrWhiteSpace(fullName))
                    {
                        fullName = NotificationConstants.Unknown;
                    }
                }

                participantData = new ParticipantData
                {
                    Name = fullName.Trim(),
                    ObjectIdentifier = objectIdentifer,
                    Email = worker.EmailPrimary ?? NotificationConstants.UnknownEmail
                };
            }

            return participantData;
        }

        /// <summary>
        /// This method is to extract wob users data for each of participant
        /// </summary>
        /// <param name="oids"> list of recruiters</param>
        /// <returns>list of wobusers</returns>
        private async Task<List<ParticipantData>> ExtractWobUserDataAsync(List<string> oids)
        {
            List<ParticipantData> wobUsersData = new List<ParticipantData>();
            if (oids.Any())
            {
                var wobUsers = await this.scheduleQuery.GetWobUsersDelegationAsync(oids).ConfigureAwait(false);
                if (wobUsers != null && wobUsers.Any())
                {
                    wobUsers.ForEach(wu =>
                    {
                        ParticipantData participantData = new ParticipantData();
                        participantData.Email = wu.To.EmailPrimary;
                        participantData.Name = wu.To.Name.GivenName;
                        participantData.ObjectIdentifier = wu.To.OfficeGraphIdentifier;
                        wobUsersData.Add(participantData);
                    });
                }
            }

            return wobUsersData;
        }
    }
}
