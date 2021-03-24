//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Business.V1
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using CommonLibrary.Common.Base.Security;
    using CommonLibrary.Common.Base.Utilities;
    using CommonLibrary.Common.Common.Common.Email.Contracts;
    using CommonLibrary.Common.Email.Contracts;
    using CommonLibrary.Common.TalentAttract.Contract;
    using CommonLibrary.Common.TalentEntities.Common;
    using CommonLibrary.CommonDataService.Common.Internal;
    using MS.GTA.ScheduleService.BusinessLibrary.Configurations;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Notification;
    using MS.GTA.ScheduleService.BusinessLibrary.Utils;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Contracts.V1.Flights;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ScheduleService.Utils;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Talent.FalconEntities.Attract;
    using MS.GTA.Talent.TalentContracts.InterviewService;
    using MS.GTA.Talent.TalentContracts.ScheduleService;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// EmailManager implementation
    /// </summary>
    public class EmailManager : IEmailManager
    {
        /// <summary>The notification client.</summary>
        private readonly INotificationClient notificationClient;

        /// <summary>Falcon query client.</summary>
        private readonly FalconQuery falconQuery;

        /// <summary>configuaration manager.</summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>trace source </summary>
        private readonly ITraceSource traceSource;

        /// <summary>schedule query</summary>
        private readonly IScheduleQuery scheduleQuery;

        /// <summary>
        /// The internals provider
        /// </summary>
        private readonly IInternalsProvider internalsProvider;

        /// <summary>
        /// Email Hepler
        /// </summary>
        private readonly IEmailHelper emailHelper;

        /// <summary>
        /// The instance for <see cref="ILogger{EmailManager}"/>.
        /// </summary>
        private readonly ILogger<EmailManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailManager"/> class
        /// </summary>
        /// <param name="notificationClient">The notification client</param>
        /// <param name="falconQuery">The falcon query client</param>
        /// <param name="configurationManager">The configuration manager</param>
        /// <param name="traceSource">The trace source</param>
        /// <param name="scheduleQuery">The schedule query</param>
        /// /// <param name="email">email helper instance</param>
        /// <param name="internalsProvider">The instance for <see cref="IInternalsProvider"/>.</param>
        /// <param name="logger">The instance for <see cref="ILogger{EmailManager}"/>.</param>
        public EmailManager(
            INotificationClient notificationClient,
            FalconQuery falconQuery,
            IConfigurationManager configurationManager,
            ITraceSource traceSource,
            IScheduleQuery scheduleQuery,
            IEmailHelper email,
            IInternalsProvider internalsProvider,
            ILogger<EmailManager> logger)
        {
            Contract.AssertValue(notificationClient, nameof(notificationClient));

            this.notificationClient = notificationClient;
            this.falconQuery = falconQuery;
            this.configurationManager = configurationManager;
            this.traceSource = traceSource;
            this.scheduleQuery = scheduleQuery;
            this.internalsProvider = internalsProvider;
            this.emailHelper = email;
            this.logger = logger;
        }

        /// <summary>
        /// Sends a reminder to interviewer to complete the feedback.
        /// </summary>
        /// <param name="pendingFeedbackRequest">Pending Feedback information.</param>
        /// <returns>Reminder Success response.</returns>
        public async Task<bool> SendFeedbackReminder(PendingFeedback pendingFeedbackRequest)
        {
            bool isFeedbackReminderSent = false;
            this.logger.LogInformation($"Started {nameof(this.SendFeedbackReminder)} method in {nameof(EmailManager)}.");
            if (pendingFeedbackRequest == null || string.IsNullOrEmpty(pendingFeedbackRequest?.JobApplicationId) || string.IsNullOrEmpty(pendingFeedbackRequest?.InterviewerOID))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid application or interviewer").EnsureLogged(this.logger);
            }

            var emailTemplateConfiguaration = this.configurationManager.Get<EmailTemplateConfiguaration>();

            this.logger.LogInformation($"SendFeedbackReminder: get email template {emailTemplateConfiguaration.FeedbackReminderEmailTemplate}");
            var feedbackReminderEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.FeedbackReminderEmailTemplate);
            var jobApplication = await this.scheduleQuery.GetJobApplicationWithCandidateDetails(pendingFeedbackRequest.JobApplicationId);

            var pendingSchedules = await this.scheduleQuery.GetPendingSchedulesForJobApplication(jobApplication);

            if (pendingSchedules?.Where(ps => ps.Interviewer?.OfficeGraphIdentifier == pendingFeedbackRequest?.InterviewerOID)?.Count() == 0)
            {
                this.logger.LogInformation($"{nameof(this.SendFeedbackReminder)}: No pending feedback exists for interviewer {pendingFeedbackRequest.InterviewerOID} and job application {pendingFeedbackRequest.JobApplicationId} .");
                return isFeedbackReminderSent;
            }

            var universityReq = jobApplication?.JobOpening?.JobOpeningExtendedAttributes?.Any(x => x.EntityType == EntityType.JobOpening && x.Name.ToLower() == BusinessConstants.JobOpeningHireType && x.Value.ToLower() == BusinessConstants.UniversityHireType);

            if (universityReq != null && (bool)universityReq)
            {
                feedbackReminderEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.InterviewerFeedbackReminderForUniversityReqs);
            }

            if (feedbackReminderEmailTemplate == null)
            {
                this.logger.LogError($"SendFeedbackReminder: email template {(universityReq != null && (bool)universityReq ? emailTemplateConfiguaration.InterviewerFeedbackReminderForUniversityReqs : emailTemplateConfiguaration.FeedbackReminderEmailTemplate)} does not exist");
                isFeedbackReminderSent = false;
            }
            else
            {
                if (jobApplication == null)
                {
                    this.logger.LogInformation("SendFeedbackReminder EmailManager: JobApplication not found");
                    isFeedbackReminderSent = false;
                }
                else
                {
                    IList<string> workerIds = new List<string> { pendingFeedbackRequest.InterviewerOID, jobApplication?.JobApplicationParticipants?.FirstOrDefault(jop => jop.Role == TalentEntities.Enum.JobParticipantRole.HiringManager)?.OID, jobApplication?.JobApplicationParticipants?.FirstOrDefault(jop => jop.Role == TalentEntities.Enum.JobParticipantRole.Recruiter)?.OID, jobApplication?.JobApplicationParticipants?.FirstOrDefault(jop => jop.Role == TalentEntities.Enum.JobParticipantRole.Contributor)?.OID };
                    var workers = await this.falconQuery.GetWorkers(workerIds?.Distinct()?.Where(w => w != null).ToList());

                    var interviewer = workers?.FirstOrDefault(w => w.OfficeGraphIdentifier == pendingFeedbackRequest.InterviewerOID);
                    var hiringManager = workers?.FirstOrDefault(w => w.OfficeGraphIdentifier == jobApplication?.JobApplicationParticipants?.FirstOrDefault(jop => jop.Role == TalentEntities.Enum.JobParticipantRole.HiringManager)?.OID);
                    var recruiter = workers?.FirstOrDefault(w => w.OfficeGraphIdentifier == jobApplication?.JobApplicationParticipants?.FirstOrDefault(jop => jop.Role == TalentEntities.Enum.JobParticipantRole.Recruiter)?.OID);
                    var contributor = workers?.FirstOrDefault(w => w.OfficeGraphIdentifier == jobApplication?.JobApplicationParticipants?.FirstOrDefault(jop => jop.Role == TalentEntities.Enum.JobParticipantRole.Contributor)?.OID);

                    if (interviewer == null || interviewer?.EmailPrimary == null)
                    {
                        throw new InvalidRequestDataValidationException("Could not find interviewer or email address of interviewer");
                    }

                    if (hiringManager == null || hiringManager?.EmailPrimary == null || hiringManager?.FullName == null)
                    {
                        this.logger.LogInformation("SendFeedbackReminder EmailManager: Hiring Manager or its email or name not found");
                    }

                    if (recruiter == null || recruiter?.EmailPrimary == null || recruiter?.FullName == null)
                    {
                        this.logger.LogInformation("SendFeedbackReminder EmailManager: Recruiter or its email or name not found");
                    }

                    if (contributor == null || contributor?.EmailPrimary == null || contributor?.FullName == null)
                    {
                        this.logger.LogInformation("SendFeedbackReminder EmailManager: Contributor or its email or name not found");
                    }

                    var principal = ServiceContext.Principal.TryGetCurrent<HCMApplicationPrincipal>();
                    var clientUrl = this.configurationManager.Get<IVClientConfiguration>().RecruitingHubClientUrl;

                    var messageParams = new Dictionary<string, string>
                    {
                        { EmailTemplateTokens.Job_Title.ToString(), pendingFeedbackRequest?.PositionTitle ?? jobApplication?.JobOpening?.PositionTitle ?? string.Empty },
                        { EmailTemplateTokens.Candidate_Name.ToString(), pendingFeedbackRequest?.CandidateName ?? jobApplication?.Candidate?.FullName?.GivenName ?? string.Empty },
                        { EmailTemplateTokens.Requester_Name.ToString(), principal?.GivenName + " " + principal?.FamilyName ?? string.Empty },
                        { EmailTemplateTokens.Requester_Email.ToString(), principal?.EmailAddress ?? string.Empty },
                        { EmailTemplateTokens.External_Job_Id.ToString(), jobApplication?.JobOpening?.ExternalJobOpeningID ?? string.Empty },
                        { EmailTemplateTokens.Interviewer_FirstName.ToString(), interviewer?.Name?.GivenName ?? string.Empty },
                        { EmailTemplateTokens.HiringManager_Name.ToString(), hiringManager?.FullName ?? string.Empty },
                        { EmailTemplateTokens.Call_To_Action_Link.ToString(), clientUrl + $"iv/feedback/provide/{pendingFeedbackRequest.JobApplicationId}/{pendingFeedbackRequest.InterviewerOID}" },
                    };

                    var notification = new NotificationItem
                    {
                        To = this.PopulateToOrCCListInEmail(feedbackReminderEmailTemplate.To, interviewer, hiringManager, recruiter, contributor),
                        CC = this.PopulateToOrCCListInEmail(feedbackReminderEmailTemplate.Cc, interviewer, hiringManager, recruiter, contributor),
                        Subject = this.ParseTemplate(feedbackReminderEmailTemplate.Subject, messageParams),
                        Body = this.ParseTemplate(feedbackReminderEmailTemplate.Body, messageParams)
                    };

                    isFeedbackReminderSent = await this.notificationClient.SendEmail(new List<NotificationItem> { notification }, pendingFeedbackRequest.JobApplicationId);
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.SendFeedbackReminder)} method in {nameof(EmailManager)}.");
            return isFeedbackReminderSent;
        }

        /// <summary>
        /// Notify scheduler when interviewer decline or tentatively accepted the invitation
        /// </summary>
        /// <param name="interviewerResponseNotification">interviewer response for notification</param>
        /// <param name="messageResponse">The message response if any.</param>
        /// <returns>task</returns>
        public async Task SendDeclineEmailToScheduler(InterviewerResponseNotification interviewerResponseNotification, string messageResponse = null)
        {
            Common.Email.Contracts.EmailTemplate declineEmailTemplate;
            Timezone timezone = null;
            DateTime startDateTime;
            DateTime endDateTime;
            this.logger.LogInformation($"Started {nameof(this.SendDeclineEmailToScheduler)} method in {nameof(EmailManager)}.");
            bool proposedTime = interviewerResponseNotification.ProposedNewTime != null;
            try
            {
                var emailTemplateConfiguaration = this.configurationManager.Get<EmailTemplateConfiguaration>();

                this.logger.LogInformation($"{nameof(this.SendDeclineEmailToScheduler)}: get email template {emailTemplateConfiguaration.InterviewerDeclineEmailTemplate}");
                declineEmailTemplate = await this.ChooseNonAcceptedEmailTemplate(interviewerResponseNotification, messageResponse, emailTemplateConfiguaration);

                if (declineEmailTemplate == null)
                {
                    this.logger.LogError($"{nameof(this.SendDeclineEmailToScheduler)}: email template {emailTemplateConfiguaration.InterviewerDeclineEmailTemplate} does not exist");
                }
                else
                {
                    this.logger.LogInformation($"{nameof(this.SendDeclineEmailToScheduler)}: get schedule {interviewerResponseNotification.ScheduleId}");
                    var schedule = await this.scheduleQuery.GetScheduleByScheduleId(interviewerResponseNotification.ScheduleId);

                    if (schedule == null)
                    {
                        this.logger.LogError($"{nameof(this.SendDeclineEmailToScheduler)}: schedule {schedule} does not exist");
                    }
                    else
                    {
                        var jobApplication = await this.scheduleQuery.GetJobApplication(interviewerResponseNotification.JobApplicationId);
                        var title = await this.scheduleQuery.GetJobOpeningPositionTitle(jobApplication?.JobOpening?.JobOpeningID);
                        var interviewerDetails = await this.scheduleQuery.GetWorker(interviewerResponseNotification.InterviewerOid);
                        var clientUrl = this.configurationManager.Get<IVClientConfiguration>().RecruitingHubClientUrl;
                        var candidateName = $"{jobApplication?.Candidate?.FullName?.GivenName} {jobApplication?.Candidate?.FullName?.Surname}";
                        Dictionary<string, string> templateParams = new Dictionary<string, string>
                        {
                            { "First_Interviewer_Name", interviewerDetails?.Name?.GivenName },
                            { "Job_Title", title?.ToString() },
                            { "ResponseStatus", this.ToLowercaseNamingConvention(interviewerResponseNotification.ResponseStatus.ToString(), true) },
                            { "Requester_Name", schedule.Requester.Name },
                            { "External_Job_Id", jobApplication?.JobOpening?.ExternalJobOpeningID },
                            { "Call_To_Action_Link", clientUrl + $"iv/candidate-view/interview/{interviewerResponseNotification.JobApplicationId}" },
                            { "Candidate_Name", candidateName }
                        };

                        if (proposedTime)
                        {
                            timezone = await this.scheduleQuery.GetTimezoneForJobApplication(interviewerResponseNotification.JobApplicationId);
                            if (timezone == null)
                            {
                                throw new BusinessRuleViolationException($"{nameof(this.SendDeclineEmailToScheduler)}: Timezone for job application Id '{interviewerResponseNotification.JobApplicationId}' is not found.");
                            }

                            startDateTime = this.GetTimezoneDateTime(interviewerResponseNotification.ProposedNewTime.Start.DateTime, timezone);
                            endDateTime = this.GetTimezoneDateTime(interviewerResponseNotification.ProposedNewTime.End.DateTime, timezone);
                            templateParams.Add("ProposedTime", $"{startDateTime} - {endDateTime} {timezone.TimezoneAbbr}");
                        }

                        if (!string.IsNullOrWhiteSpace(messageResponse))
                        {
                            templateParams.Add("ResponseText", messageResponse);
                        }

                        var notification = new NotificationItem
                        {
                            To = schedule.Requester.Email,
                            Subject = this.ParseTemplate(declineEmailTemplate.Subject, templateParams),
                            Body = this.ParseTemplate(declineEmailTemplate.Body, templateParams)
                        };

                        if (WobUserFeatureFlight.IsEnabled)
                        {
                            this.logger.LogInformation($"Status of the WOB Flight : true. Adding WOB Participants in CC.");
                            notification = await this.AddWobUsersToNotificationRecepientAsync(notification, schedule.Requester.Id).ConfigureAwait(false);
                        }

                        this.logger.LogInformation($"{nameof(this.SendDeclineEmailToScheduler)}: send email to scheduler for schedule {interviewerResponseNotification?.ScheduleId}");
                        await this.notificationClient.SendEmail(new List<NotificationItem> { notification }, interviewerResponseNotification?.ScheduleId);
                    }
                }
            }
            catch (Exception ex)
            {
                //// Log error and don't fail the main flow as user response is processed before
                this.logger.LogError($"{nameof(this.SendDeclineEmailToScheduler)}: decline email failed to send to requester with error {ex.Message} and stack trace {ex.StackTrace}");
            }

            this.logger.LogInformation($"Finished {nameof(this.SendDeclineEmailToScheduler)} method in {nameof(EmailManager)}.");
        }

        /// <summary>
        /// Sends a reminder to scheduler to remind about the interviwer's response.
        /// </summary>
        /// <returns>Reminder Success response.</returns>
        public async Task<bool> SendSchedulerReminder()
        {
            bool isSchedulerReminderSent = false;
            HashSet<string> uniqueJobAppIds = new HashSet<string>();
            this.logger.LogInformation($"Started {nameof(this.SendSchedulerReminder)} method in {nameof(EmailManager)}.");
            var emailTemplateConfiguaration = this.configurationManager.Get<EmailTemplateConfiguaration>();

            this.logger.LogInformation($"SendSchedulerReminder: get email template {emailTemplateConfiguaration.FeedbackReminderEmailTemplate}");
            var clientUrl = this.configurationManager.Get<IVClientConfiguration>().RecruitingHubClientUrl;

            this.logger.LogInformation($"SendSchedulerReminder: get schedules where interviewers are pending");
            var schedules = await this.scheduleQuery.GetPendingSchedules();

            if (schedules == null || schedules.Count() == 0)
            {
                this.logger.LogInformation($"SendSchedulerReminder: No pending schedules");
                isSchedulerReminderSent = false;
            }
            else
            {
                HashSet<string> uniqueParticipantIDs = new HashSet<string>();
                HashSet<string> contributorOids = new HashSet<string>();

                schedules.ForEach(s =>
                {
                    uniqueJobAppIds.Add(s?.JobApplicationID);
                    s?.Participants?.ForEach(sp => uniqueParticipantIDs.Add(sp?.OID));
                });

                var scheduleParticipants = await this.scheduleQuery.GetWorkers(uniqueParticipantIDs.ToList());

                var jobApplications = await this.scheduleQuery.GetJobApplications(uniqueJobAppIds.ToList());

                jobApplications.ToList().ForEach(ja =>
                {
                    var contributors = ja?.JobApplicationParticipants?.ToList().FindAll(jap => jap.Role == JobParticipantRole.Contributor);
                    contributors.ForEach(co => contributorOids.Add(co.OID));
                });

                var contributorWorkers = await this.scheduleQuery.GetWorkers(contributorOids.ToList());

                foreach (var jobId in uniqueJobAppIds)
                {
                    HashSet<string> contributorEmails = new HashSet<string>();

                    // Gets the schedules for a job.
                    var schedulesforJob = schedules.ToList().FindAll(s => s.JobApplicationID == jobId);

                    var jobApplication = jobApplications.ToList().Find(ja => ja.JobApplicationID == jobId);
                    var title = await this.scheduleQuery.GetJobOpeningPositionTitle(jobApplication?.JobOpening?.JobOpeningID);

                    // Gets the contributors email ids.
                    var contributors = jobApplication?.JobApplicationParticipants?.ToList().FindAll(jap => jap.Role == JobParticipantRole.Contributor);

                    contributors.ForEach(coj =>
                    {
                        var contributorWorker = contributorWorkers.First(c => c.OfficeGraphIdentifier == coj.OID);
                        contributorEmails.Add(contributorWorker?.EmailPrimary);
                    });

                    var contributorToString = string.Join(";", contributorEmails.ToList());

                    // Gets the summary to be attached at the end of the Body.
                    var summary = this.GetReminderSummary(schedulesforJob.OrderBy(s => s.StartDateTime).ToList(), scheduleParticipants);
                    var candidateName = jobApplication?.Candidate?.FullName?.GivenName + " " + jobApplication?.Candidate?.FullName?.Surname;

                    Dictionary<string, string> templateParams = new Dictionary<string, string>
                    {
                        { "External_Job_id", jobApplication?.JobOpening?.ExternalJobOpeningID?.ToString() },
                        { "Job_Title", title?.ToString() },
                        { "Candidate_Name", candidateName },
                        { "Company_Name", "Microsoft" },
                        { "Header_Image_Url", BusinessConstants.MicrosoftLogoUrl },
                        { "Privacy_Policy_Link", BusinessConstants.PrivacyPolicyUrl },
                        { "Terms_And_Conditions_Link", BusinessConstants.TermsAndConditionsUrl },
                        { "Call_To_Action_Link", clientUrl + $"iv/candidate-view/interview/{jobApplication?.JobApplicationID?.ToString()}" }
                    };

                    var notification = new NotificationItem
                    {
                        To = contributorToString
                    };
                    Common.Email.Contracts.EmailTemplate reminderEmailTemplate = null;

                    if (WobUserFeatureFlight.IsEnabled)
                    {
                        this.logger.LogInformation($"Status of the WOB Flight : true. Adding WOB Participants in CC.");
                        foreach (string schedulerOid in contributors?.Select(sch => sch.OID))
                        {
                            notification = await this.AddWobUsersToNotificationRecepientAsync(notification, schedulerOid).ConfigureAwait(false);
                        }

                        if (!string.IsNullOrEmpty(notification.CC))
                        {
                            reminderEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.SchedulerReminderEmailTemplateWithNotes);
                        }
                        else
                        {
                            reminderEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.SchedulerReminderEmailTemplate);
                        }
                    }
                    else
                    {
                        reminderEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.SchedulerReminderEmailTemplate);
                    }

                    if (reminderEmailTemplate == null)
                    {
                        this.logger.LogError($"SendSchedulerReminder: email template {emailTemplateConfiguaration.SchedulerReminderEmailTemplate} does not exist");
                        isSchedulerReminderSent = false;
                    }

                    notification.Subject = this.ParseTemplate(reminderEmailTemplate.Subject, templateParams);
                    notification.Body = this.ParseTemplate(reminderEmailTemplate.Body, templateParams) + summary;

                    this.logger.LogInformation($"SendSchedulerReminder: send email to scheduler for job applicaion {jobId}");
                    await this.notificationClient.SendEmail(new List<NotificationItem> { notification }, jobId);
                }

                isSchedulerReminderSent = true;
            }

            this.logger.LogInformation($"Finished {nameof(this.SendSchedulerReminder)} method in {nameof(EmailManager)}.");
            return isSchedulerReminderSent;
        }

        /// <summary>
        /// sends a reminider to scheduler on invites failure
        /// </summary>
        /// <param name="scheduleID">scheduleid</param>
        /// <returns>Reminder Success response</returns>
        public async Task<bool> SendInviteFailReminder(string scheduleID)
        {
            bool isInviteFailReminderSent = false;
            this.logger.LogInformation($"Started {nameof(this.SendInviteFailReminder)} method in {nameof(EmailManager)}.");
            var emailTemplateConfiguaration = this.configurationManager.Get<EmailTemplateConfiguaration>();
            try
            {
                this.logger.LogInformation($"SendInviteFailReminder: get email template {emailTemplateConfiguaration.InterviewInvitationFailedEmailTemplate}");
                var inviteFailEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.InterviewInvitationFailedEmailTemplate);

                if (inviteFailEmailTemplate == null)
                {
                    this.logger.LogError($"SendInviteFailReminder: email template {emailTemplateConfiguaration.InterviewInvitationFailedEmailTemplate} does not exist");
                    isInviteFailReminderSent = false;
                }
                else
                {
                    this.logger.LogInformation($"SendInviteFailReminder: get schedule {scheduleID}");
                    var schedule = await this.scheduleQuery.GetScheduleByScheduleId(scheduleID);

                    if (schedule == null)
                    {
                        this.logger.LogError($"SendInviteFailReminder: schedule {schedule} does not exist");
                        isInviteFailReminderSent = false;
                    }
                    else
                    {
                        var jobId = await this.scheduleQuery.GetJobApplicationIdForSchedule(scheduleID);

                        var jobApplication = await this.scheduleQuery.GetJobApplication(jobId);
                        var title = await this.scheduleQuery.GetJobOpeningPositionTitle(jobApplication?.JobOpening?.JobOpeningID);
                        var interviewerDetails = await this.scheduleQuery.GetWorker(schedule.Requester.Id);
                        var clientUrl = this.configurationManager.Get<IVClientConfiguration>().RecruitingHubClientUrl;

                        Dictionary<string, string> templateParams = new Dictionary<string, string>
                        {
                            { "Job_Title", title?.ToString() },
                            { "Requester_Name", schedule.Requester?.Name },
                            { "External_Job_Id", jobApplication?.ExternalJobApplicationID },
                            { "Call_To_Action_Link", clientUrl + $"iv/candidate-view/interview/{jobId}" },
                            { "Company_Name", "Microsoft" },
                            { "Privacy_Policy_Link", BusinessConstants.PrivacyPolicyUrl },
                            { "Terms_And_Conditions_Link", BusinessConstants.TermsAndConditionsUrl }
                        };

                        var notification = new NotificationItem
                        {
                            To = schedule.Requester?.Email,
                            Subject = this.ParseTemplate(inviteFailEmailTemplate.Subject, templateParams),
                            Body = this.ParseTemplate(inviteFailEmailTemplate.Body, templateParams)
                        };

                        if (WobUserFeatureFlight.IsEnabled)
                        {
                            this.logger.LogInformation($"Status of the WOB Flight : true. Adding WOB Participants in CC.");
                            notification = await this.AddWobUsersToNotificationRecepientAsync(notification, schedule.Requester?.Id).ConfigureAwait(false);
                        }

                        this.logger.LogInformation($"SendInviteFailReminder: send email to scheduler for schedule {scheduleID}");
                        await this.notificationClient.SendEmail(new List<NotificationItem> { notification }, scheduleID);
                        isInviteFailReminderSent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //// Log error and don't fail the main flow as user response is processed before
                this.logger.LogError($"SendInviteFailReminder: send email failed to send to requester with error {ex.Message} and stack trace {ex.StackTrace}");
                isInviteFailReminderSent = false;
            }

            this.logger.LogInformation($"Finished {nameof(this.SendInviteFailReminder)} method in {nameof(EmailManager)}.");
            return isInviteFailReminderSent;
        }

        /// <summary>
        /// send new assignment email to scheduler
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="schedulerOid">scheduler oid</param>
        /// <param name="candidateName">candidate name</param>
        /// <returns>True/false</returns>
        public async Task<bool> SendAssignmentEmailToScheduler(string jobApplicationId, string schedulerOid, string candidateName)
        {
            bool isAssignmentEmailToSchedulerSent = false;
            this.logger.LogInformation($"Started {nameof(this.SendAssignmentEmailToScheduler)} method in {nameof(EmailManager)}.");
            var emailTemplateConfiguaration = this.configurationManager.Get<EmailTemplateConfiguaration>();
            try
            {
                this.logger.LogInformation($"SendAssignmentEmailToScheduler: get email template {emailTemplateConfiguaration.ScheduleAssignmentEmailTemplate}");
                this.logger.LogInformation($"SendAssignmentEmailToScheduler: get job application id {jobApplicationId}");
                var jobApplication = await this.scheduleQuery.GetJobApplication(jobApplicationId);
                var schedulerWorker = await this.falconQuery.GetWorker(schedulerOid);
                var recruiterOid = jobApplication?.JobApplicationParticipants.Where(a => a.Role == TalentEntities.Enum.JobParticipantRole.Recruiter).FirstOrDefault()?.OID;
                var recruiterWorker = await this.falconQuery.GetWorker(recruiterOid);
                var hiringManagerOid = jobApplication?.JobApplicationParticipants.Where(a => a.Role == TalentEntities.Enum.JobParticipantRole.HiringManager).FirstOrDefault()?.OID;
                var hiringManagerWorker = await this.falconQuery.GetWorker(hiringManagerOid);

                if (jobApplication == null)
                {
                    this.logger.LogError($"SendAssignmentEmailToScheduler: job application {jobApplication} does not exist");
                    isAssignmentEmailToSchedulerSent = false;
                }
                else
                {
                    var title = await this.scheduleQuery.GetJobOpeningPositionTitle(jobApplication?.JobOpening?.JobOpeningID);
                    var clientUrl = this.configurationManager.Get<IVClientConfiguration>().RecruitingHubClientUrl;

                    Dictionary<string, string> templateParams = new Dictionary<string, string>
                    {
                        { "Job_Title", title?.ToString() },
                        { "Candidate_Name", candidateName },
                        { "Job_Id", jobApplication?.JobOpening?.ExternalJobOpeningID ?? string.Empty },
                        { "Call_To_Action_Link", clientUrl + $"iv/candidate-view/interview/{jobApplicationId}" },
                        { "Scheduler_First_Name", schedulerWorker.Name.GivenName },
                        { "Recruiter_Name", recruiterWorker?.FullName },
                        { "HiringManager_Name", hiringManagerWorker?.FullName },
                        { "Requester_Email", recruiterWorker?.EmailPrimary },
                        { "Requester_Name", recruiterWorker?.FullName },
                        { "Company_Name", "Microsoft" },
                        { "Privacy_Policy_Link", BusinessConstants.PrivacyPolicyUrl },
                        { "Terms_And_Conditions_Link", BusinessConstants.TermsAndConditionsUrl }
                    };

                    var notification = new NotificationItem
                    {
                        To = schedulerWorker.EmailPrimary
                    };
                    Common.Email.Contracts.EmailTemplate inviteFailEmailTemplate = null;

                    if (WobUserFeatureFlight.IsEnabled)
                    {
                        this.logger.LogInformation($"Status of the WOB Flight : true. Adding WOB Participants in CC.");
                        notification = await this.AddWobUsersToNotificationRecepientAsync(notification, schedulerOid).ConfigureAwait(false);

                        if (!string.IsNullOrEmpty(notification.CC))
                        {
                            inviteFailEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.ScheduleAssignmentEmailTemplateWithNotes);
                        }
                        else
                        {
                            inviteFailEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.ScheduleAssignmentEmailTemplate);
                        }
                    }
                    else
                    {
                        inviteFailEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.ScheduleAssignmentEmailTemplate);
                    }

                    if (inviteFailEmailTemplate == null)
                    {
                        this.logger.LogError($"SendAssignmentEmailToScheduler: email template {emailTemplateConfiguaration.ScheduleAssignmentEmailTemplate} does not exist");
                        isAssignmentEmailToSchedulerSent = false;
                    }

                    notification.Subject = this.ParseTemplate(inviteFailEmailTemplate.Subject, templateParams);
                    notification.Body = this.ParseTemplate(inviteFailEmailTemplate.Body, templateParams);
                    this.logger.LogInformation($"SendAssignmentEmailToScheduler: send email to scheduler for job application id {jobApplicationId}");
                    await this.notificationClient.SendEmail(new List<NotificationItem> { notification }, jobApplicationId);
                    isAssignmentEmailToSchedulerSent = true;
                }
            }
            catch (Exception ex)
            {
                //// Log error and don't fail the main flow as user response is processed before
                this.logger.LogError($"SendAssignmentEmailToScheduler: send email failed to send to requester with error {ex.StackTrace}");
                isAssignmentEmailToSchedulerSent = false;
            }

            this.logger.LogInformation($"Finished {nameof(this.SendAssignmentEmailToScheduler)} method in {nameof(EmailManager)}.");
            return isAssignmentEmailToSchedulerSent;
        }

        /// <summary>
        /// Sends the feedback email.
        /// </summary>
        /// <param name="notificationRequest">The instance of <see cref="EmailNotificationRequest" />.</param>
        /// <param name="userOid">Requester Office Graph Identifier</param>
        /// <param name="isWobUser">Checks if the user is a wob user</param>
        /// <returns>
        /// The instance of <see cref="Task{Boolean}" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="OperationNotAllowedException">
        /// The job application Id is not specified.
        /// or
        /// The interviewers are not specified.
        /// </exception>
        public async Task<bool> SendFeedbackEmailAsync(EmailNotificationRequest notificationRequest, string userOid = null, bool isWobUser = false)
        {
            bool mailSent = false;
            NotificationItem notification;
            this.logger.LogInformation($"Started {nameof(this.SendFeedbackEmailAsync)} method in {nameof(EmailManager)}.");
            Contract.CheckValue(notificationRequest, nameof(notificationRequest));

            this.ValidateNotificationRequest(notificationRequest);
            notification = this.PrepareNotification(notificationRequest);

            if (isWobUser)
            {
                this.logger.LogInformation($"Status of the WOB Flight :{isWobUser}. Adding WOB Participants in CC.");

                var wobUsers = await this.scheduleQuery.GetWobUsersDelegation(userOid).ConfigureAwait(false);
                notification.CC = string.Format(notification.CC + ";" + string.Join(";", wobUsers.Select(wob => wob.To.EmailPrimary)));
                this.logger.LogInformation($"Added {wobUsers.Count} WOB Participants in CC.");
            }

            this.logger.LogInformation($"{nameof(this.SendFeedbackEmailAsync)}: send feedback email to added WOB participant {userOid}");
            mailSent = await this.notificationClient.SendEmail(new List<NotificationItem> { notification }, userOid);

            this.logger.LogInformation($"Finished {nameof(this.SendFeedbackEmailAsync)} method in {nameof(EmailManager)}.");
            return mailSent;
        }

        /// <summary>
        /// Send email notification about tool switching to IV.
        /// </summary>
        /// <param name="requisitionId">requisition id</param>
        /// <returns>email Success response</returns>
        public async Task<bool> SendPilotNotification(string requisitionId)
        {
            bool isPilotEmailSent = false;
            this.logger.LogInformation($"Started {nameof(this.SendPilotNotification)} method in {nameof(EmailManager)}.");
            var emailTemplateConfiguaration = this.configurationManager.Get<EmailTemplateConfiguaration>();
            try
            {
                this.logger.LogInformation($"SendPilotNotification: get email template {emailTemplateConfiguaration.PilotEmailTemplate}");
                var inviteFailEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.PilotEmailTemplate);
                if (inviteFailEmailTemplate == null)
                {
                    this.logger.LogError($"SendPilotNotification: email template {emailTemplateConfiguaration.PilotEmailTemplate} does not exist");
                    isPilotEmailSent = false;
                }
                else
                {
                    this.logger.LogInformation($"SendPilotNotification: get job application id {requisitionId}");
                    var jobApplications = await this.scheduleQuery.GetJobApplicationsByRequisitionId(requisitionId);
                    if (jobApplications == null)
                    {
                        this.logger.LogError($"SendPilotNotification: job application {requisitionId} does not exist");
                        isPilotEmailSent = false;
                    }
                    else
                    {
                        List<string> participantsOid = new List<string>();
                        foreach (var jobApplication in jobApplications)
                        {
                            participantsOid.AddRange(jobApplication.JobApplicationParticipants.Where(a => a.Role == JobParticipantRole.HiringManager || a.Role == JobParticipantRole.Recruiter || a.Role == JobParticipantRole.Contributor).Select(a => a.OID));
                        }

                        var participantsWorker = await this.falconQuery.GetWorkers(participantsOid);
                        var clientUrl = this.configurationManager.Get<IVClientConfiguration>().RecruitingHubClientUrl;

                        Dictionary<string, string> templateParams = new Dictionary<string, string>
                        {
                            { "requisition_id", requisitionId },
                            { "dashboardlink", clientUrl }
                        };

                        var notification = new NotificationItem
                        {
                            To = string.Join(";", participantsWorker.Select(a => a.EmailPrimary).ToArray()),
                            Subject = this.ParseTemplate(inviteFailEmailTemplate.Subject, templateParams),
                            Body = this.ParseTemplate(inviteFailEmailTemplate.Body, templateParams)
                        };
                        this.logger.LogInformation($"SendPilotNotification: send email to scheduler for job application id {requisitionId}");
                        await this.notificationClient.SendEmail(new List<NotificationItem> { notification }, requisitionId);
                        isPilotEmailSent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //// Log error and don't fail the main flow as user response is processed before
                this.logger.LogError($"SendPilotNotification: send email failed to send to requester with error {ex.StackTrace}");
                isPilotEmailSent = false;
            }

            this.logger.LogInformation($"Finished {nameof(this.SendPilotNotification)} method in {nameof(EmailManager)}.");
            return isPilotEmailSent;
        }

        /// <summary>
        /// send new assignment email to scheduler
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="aaOid">AA oid</param>
        /// <returns>True/false</returns>
        public async Task<bool> SendShareFeedbackEmailToAA(string jobApplicationId, string[] aaOid)
        {
            bool isShareFeedbackEmailToAASent = false;
            this.logger.LogInformation($"Started {nameof(this.SendShareFeedbackEmailToAA)} method in {nameof(EmailManager)}.");
            var emailTemplateConfiguaration = this.configurationManager.Get<EmailTemplateConfiguaration>();
            try
            {
                if (string.IsNullOrWhiteSpace(jobApplicationId) || aaOid == null || aaOid.Length == 0)
                {
                    throw new BusinessRuleViolationException("Input request does not contain a valid application or team member");
                }

                this.logger.LogInformation($"SendShareFeedbackEmailToAA: get job application {jobApplicationId}");
                var jobApplication = await this.scheduleQuery.GetJobApplication(jobApplicationId);
                this.logger.LogInformation($"SendShareFeedbackEmailToAA: get email template {emailTemplateConfiguaration.ShareFeedbackEmailTemplate}");
                var shareFeedbackEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.ShareFeedbackEmailTemplate);

                if (shareFeedbackEmailTemplate == null)
                {
                    this.logger.LogError($"SendShareFeedbackEmailToAA: email template {emailTemplateConfiguaration.ShareFeedbackEmailTemplate} does not exist");
                    isShareFeedbackEmailToAASent = false;
                }
                else
                {
                    foreach (string aaUserID in aaOid)
                    {
                        var aaworker = await this.falconQuery.GetWorker(aaUserID);
                        var clientUrl = this.configurationManager.Get<IVClientConfiguration>().RecruitingHubClientUrl;

                        Dictionary<string, string> templateParams = new Dictionary<string, string>();

                        if (jobApplication.Candidate != null && jobApplication.Candidate.FullName != null)
                        {
                            templateParams.Add("CANDIDATE", jobApplication.Candidate.FullName.GivenName);
                        }
                        else
                        {
                            templateParams.Add("CANDIDATE", string.Empty);
                        }

                        templateParams.Add("Call_To_Action_Link", clientUrl + $"iv/candidate-view/feedback/{jobApplicationId}");
                        templateParams.Add("Company_Name", "Microsoft");
                        templateParams.Add("Privacy_Policy_Link", BusinessConstants.PrivacyPolicyUrl);
                        templateParams.Add("Terms_And_Conditions_Link", BusinessConstants.TermsAndConditionsUrl);

                        var notification = new NotificationItem
                        {
                            To = aaworker.EmailPrimary,
                            Subject = this.ParseTemplate(shareFeedbackEmailTemplate.Subject, templateParams),
                            Body = this.ParseTemplate(shareFeedbackEmailTemplate.Body, templateParams)
                        };

                        this.logger.LogInformation($"SendShareFeedbackEmailToAA: send email to scheduler for job application id {jobApplicationId} and aaOid {aaUserID}");
                        await this.notificationClient.SendEmail(new List<NotificationItem> { notification }, jobApplicationId);
                        isShareFeedbackEmailToAASent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //// Log error and don't fail the main flow as user response is processed before
                this.logger.LogError($"SendShareFeedbackEmailToAA: send email failed to send to requester with error {ex.StackTrace}");
                isShareFeedbackEmailToAASent = false;
                throw;
            }

            this.logger.LogInformation($"Finished {nameof(this.SendShareFeedbackEmailToAA)} method in {nameof(EmailManager)}.");
            return isShareFeedbackEmailToAASent;
        }

        /// <summary>
        /// Send email notification.
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="emailDetails">email details</param>
        /// <param name="userMailAddress">user mail address</param>
        /// <param name="requesterOid"> The Requester Office Graph Identifier</param>
        /// <param name="isWobUser"> Flag for checking the user wob Authentication</param>
        /// <returns>email Success response</returns>
        public async Task<bool> SendEmailNotification(string jobApplicationId, ScheduleInvitationDetails emailDetails, string userMailAddress, string requesterOid = null, bool isWobUser = false)
        {
            if (emailDetails == null)
            {
                this.logger.LogError($"{nameof(this.SendEmailNotification)} method in {nameof(EmailManager)}: Send email notification failed as the details are null.");
                throw new InvalidRequestDataValidationException($"{nameof(this.SendEmailNotification)} method in {nameof(EmailManager)}: Send email notification failed as the details are null..").EnsureLogged(this.logger);
            }

            if (emailDetails.IsInterviewScheduleShared)
            {
                var schedules = await this.scheduleQuery.GetSchedulesByJobApplicationId(jobApplicationId);
                var timezone = await this.scheduleQuery.GetTimezoneForJobApplication(jobApplicationId);
                ////Ignoring past schedules
                var sharedSchedules = schedules.Where(s => s.MeetingDetails?[0].UtcStart > DateTime.UtcNow).ToList();

                var interviewSummary = await this.emailHelper.GetScheduleSummary(sharedSchedules, emailDetails, timezone);
                emailDetails.EmailContent += interviewSummary;
            }

            bool mailSent = false;

            this.logger.LogInformation($"Started {nameof(this.SendEmailNotification)} method in {nameof(EmailManager)}.");
            var notification = this.PrepareEmailNotification(emailDetails);
            if (!string.IsNullOrWhiteSpace(userMailAddress))
            {
                notification.ReplyTo = userMailAddress;
            }

            if (isWobUser)
            {
                bool isUserPresent = notification.CC.IndexOf(userMailAddress, StringComparison.OrdinalIgnoreCase) >= 0 || notification.CC.IndexOf(userMailAddress, StringComparison.OrdinalIgnoreCase) >= 0;
                if (isUserPresent)
                {
                    this.logger.LogInformation($"The WOB parent user is present in the recipient list. Adding WOB Participants in CC.");

                    var wobUsers = await this.scheduleQuery.GetWobUsersDelegation(requesterOid).ConfigureAwait(false);
                    notification.CC = string.Format(notification.CC + ";" + string.Join(";", wobUsers.Select(wob => wob.To.EmailPrimary)));
                    this.logger.LogInformation($"Added {wobUsers.Count} WOB Participants in CC.");
                }
            }

            this.logger.LogInformation($"{nameof(this.SendEmailNotification)}: send email  for job application id {jobApplicationId}.");
            mailSent = await this.notificationClient.SendEmail(new List<NotificationItem> { notification }, jobApplicationId);
            this.logger.LogInformation($"Finished {nameof(this.SendEmailNotification)} method in {nameof(EmailManager)}.");
            return mailSent;
        }

        /// <summary>
        /// Sends email reminders to interviewers to complete the pending feedbacks if due for more than 24 hours.
        /// </summary>
        /// <returns>An instance of <see cref="Task{Boolean}"/>.</returns>
        public async Task<bool> SendFeedbackReminderToAllAsync()
        {
            var allMailsSent = true;
            this.logger.LogInformation($"Started {nameof(this.SendFeedbackReminderToAllAsync)} method in {nameof(EmailManager)}.");

            var emailTemplateConfiguaration = this.configurationManager.Get<EmailTemplateConfiguaration>();

            this.logger.LogInformation($"{nameof(this.SendFeedbackReminderToAllAsync)} method in {nameof(EmailManager)}: get email template {emailTemplateConfiguaration.FeedbackReminderEmailTemplate}");

            int notificationReminderOffsetHours = this.configurationManager.Get<AutomatedFeedbackReminderConfiguration>().FeedbackReminderOffsetDurationHours;
            int notificationReminderWindowMinutes = this.configurationManager.Get<AutomatedFeedbackReminderConfiguration>().FeedbackReminderWindowMinutes;

            var pendingFeedbacks = await this.scheduleQuery.GetAllPendingFeedbacksForReminderAsync(notificationReminderOffsetHours, notificationReminderWindowMinutes);
            var notificationItems = new List<NotificationItem>();

            var feedbackReminderEmailTemplateForNonUVReqs = this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.AutomatedFeedbackReminderEmailTemplate).Result;
            var feedbackReminderEmailTemplateForUVReqs = this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.AutomatedInterviewerFeedbackReminderForUniversityReqs).Result;

            pendingFeedbacks?.ForEach(pendingFeedback =>
            {
                var feedbackReminderEmailTemplate = feedbackReminderEmailTemplateForNonUVReqs;
                var jobApplication = this.scheduleQuery.GetJobApplicationWithCandidateDetails(pendingFeedback.JobApplicationId).Result;
                var universityReq = jobApplication?.JobOpening?.JobOpeningExtendedAttributes?.Any(x => x.EntityType == EntityType.JobOpening && x.Name.ToLower() == BusinessConstants.JobOpeningHireType && x.Value.ToLower() == BusinessConstants.UniversityHireType);

                if (universityReq != null && (bool)universityReq)
                {
                    feedbackReminderEmailTemplate = feedbackReminderEmailTemplateForUVReqs;
                }

                if (feedbackReminderEmailTemplate == null)
                {
                    allMailsSent = false;
                    this.logger.LogError($"{nameof(this.SendFeedbackReminderToAllAsync)} method in {nameof(EmailManager)}: email template {(universityReq != null && (bool)universityReq ? emailTemplateConfiguaration.InterviewerFeedbackReminderForUniversityReqs : emailTemplateConfiguaration.FeedbackReminderEmailTemplate)} does not exist");
                }
                else
                {
                    var hiringManager = pendingFeedback.HiringManager;
                    var recruiter = pendingFeedback.Recruiter;
                    var scheduleRequester = pendingFeedback.ScheduleRequester;
                    var interviewer = pendingFeedback.Interviewer;

                    if (hiringManager == null || string.IsNullOrWhiteSpace(hiringManager?.EmailPrimary) || string.IsNullOrWhiteSpace(hiringManager?.FullName))
                    {
                        this.logger.LogInformation($"{nameof(this.SendFeedbackReminderToAllAsync)} method in {nameof(EmailManager)}: Hiring Manager or its email or name not found");
                    }

                    if (recruiter == null || string.IsNullOrWhiteSpace(recruiter?.EmailPrimary) || string.IsNullOrWhiteSpace(recruiter?.FullName))
                    {
                        this.logger.LogInformation($"{nameof(this.SendFeedbackReminderToAllAsync)} method in {nameof(EmailManager)}: Recruiter or its email or name not found");
                    }

                    if (scheduleRequester == null || string.IsNullOrWhiteSpace(scheduleRequester?.FullName) || string.IsNullOrWhiteSpace(scheduleRequester?.EmailPrimary))
                    {
                        this.logger.LogInformation($"{nameof(this.SendFeedbackReminderToAllAsync)} method in {nameof(EmailManager)}: No full name or email found for schedule requester with oid {scheduleRequester?.OfficeGraphIdentifier}");
                    }

                    if (interviewer == null || string.IsNullOrWhiteSpace(interviewer?.EmailPrimary) || string.IsNullOrWhiteSpace(interviewer?.Name?.GivenName))
                    {
                        this.logger.LogInformation($"{nameof(this.SendFeedbackReminderToAllAsync)} method in {nameof(EmailManager)}: No given name or email found for schedule requester with oid {interviewer?.OfficeGraphIdentifier}");
                        allMailsSent = false;
                    }
                    else
                    {
                        var clientUrl = this.configurationManager.Get<IVClientConfiguration>().RecruitingHubClientUrl;

                        var messageParams = new Dictionary<string, string>
                    {
                        { EmailTemplateTokens.Job_Title.ToString(), pendingFeedback?.PositionTitle ?? string.Empty },
                        { EmailTemplateTokens.Candidate_Name.ToString(), pendingFeedback?.CandidateName ?? string.Empty },
                        { EmailTemplateTokens.External_Job_Id.ToString(), pendingFeedback?.ExternalJobOpeningId ?? string.Empty },
                        { EmailTemplateTokens.Interviewer_FirstName.ToString(), interviewer?.Name?.GivenName ?? string.Empty },
                        { EmailTemplateTokens.HiringManager_Name.ToString(), hiringManager?.FullName ?? string.Empty },
                        { EmailTemplateTokens.Call_To_Action_Link.ToString(), clientUrl + $"iv/feedback/provide/{pendingFeedback.JobApplicationId}/{interviewer.OfficeGraphIdentifier}?ref=email" },
                    };

                        var notification = new NotificationItem
                        {
                            To = this.PopulateToOrCCListInEmail(feedbackReminderEmailTemplate.To, interviewer, hiringManager, recruiter, scheduleRequester),
                            CC = this.PopulateToOrCCListInEmail(feedbackReminderEmailTemplate.Cc, interviewer, hiringManager, recruiter, scheduleRequester),
                            Subject = this.ParseTemplate(feedbackReminderEmailTemplate.Subject, messageParams),
                            Body = this.ParseTemplate(feedbackReminderEmailTemplate.Body, messageParams),
                            TrackingId = pendingFeedback.JobApplicationId
                        };

                        notificationItems.Add(notification);
                    }
                }
            });

            if (notificationItems.Count > 0)
            {
                this.logger.LogInformation($"{nameof(this.SendFeedbackReminderToAllAsync)}: sending feedback remainder email.");
                ////added trackingID at notification item level
                allMailsSent &= await this.notificationClient.SendEmail(notificationItems, string.Empty);
            }

            this.logger.LogInformation($"Finished {nameof(this.SendFeedbackReminderToAllAsync)} method in {nameof(EmailManager)}.");
            return allMailsSent;
        }

        /// <inheritdoc/>
        public async Task SendDelegationAssignmentToSchedulerAsync(IEnumerable<DelegationRequest> delegationRequestCollection)
        {
            string mailSentForDelegations = null;
            this.logger.LogInformation($"Started {nameof(this.SendDelegationAssignmentToSchedulerAsync)} method in {nameof(EmailManager)}.");

            if (delegationRequestCollection == null || !delegationRequestCollection.Any())
            {
                this.logger.LogInformation($"{nameof(this.SendDelegationAssignmentToSchedulerAsync)} No delegation present for sending notifications.");
            }
            else
            {
                this.logger.LogInformation($"Delegation ids to process for sending email {string.Join("; ", delegationRequestCollection.Select(ids => ids.DelegationId))}");
                var emailTemplateConfiguaration = this.configurationManager.Get<EmailTemplateConfiguaration>();
                this.logger.LogInformation($"Email template for delegation assignment: {emailTemplateConfiguaration.DelegationAssignmentTemplate}");
                var delegationAssignmentEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.DelegationAssignmentTemplate).ConfigureAwait(false);

                if (delegationAssignmentEmailTemplate == null)
                {
                    this.logger.LogError($"{nameof(this.SendDelegationAssignmentToSchedulerAsync)} Email template {emailTemplateConfiguaration.DelegationAssignmentTemplate} does not exist.");
                }
                else
                {
                    foreach (var delegationRequest in delegationRequestCollection)
                    {
                        bool isValid = true;

                        if (string.IsNullOrWhiteSpace(delegationRequest?.From?.ObjectId)
                            || string.IsNullOrWhiteSpace(delegationRequest.To?.ObjectId)
                            || delegationRequest.From.ObjectId.Equals(delegationRequest.To.ObjectId, StringComparison.OrdinalIgnoreCase)
                            || delegationRequest.DelegationStatus != DelegationStatus.Active)
                        {
                            this.logger.LogInformation($"{nameof(this.SendDelegationAssignmentToSchedulerAsync)} Delegation {delegationRequest.DelegationId} not eligible for notifications.");
                            isValid = false;
                        }

                        if (string.IsNullOrWhiteSpace(delegationRequest.From.Email) || string.IsNullOrWhiteSpace(delegationRequest.From.FullName))
                        {
                            this.logger.LogInformation($"{nameof(this.SendDelegationAssignmentToSchedulerAsync)}: Delegation From person email or full name not found.");
                            isValid = false;
                        }

                        if (string.IsNullOrWhiteSpace(delegationRequest.To.Email) || string.IsNullOrWhiteSpace(delegationRequest.To.FullName))
                        {
                            this.logger.LogInformation($"{nameof(this.SendDelegationAssignmentToSchedulerAsync)}: Delegation To person email or full name not found.");
                            isValid = false;
                        }

                        if (isValid)
                        {
                            var clientUrl = this.configurationManager.Get<IVClientConfiguration>().RecruitingHubClientUrl;
                            var messageParams = new Dictionary<string, string>
                            {
                                { EmailTemplateTokens.Delegation_From_Name.ToString(), delegationRequest.From.FullName },
                                { EmailTemplateTokens.Delegation_To_Name.ToString(), delegationRequest.To.FullName },
                                { EmailTemplateTokens.Delegation_From_Date.ToString(), delegationRequest.FromDate.GetValueOrDefault().Date.ToString("MM/dd/yyyy") },
                                { EmailTemplateTokens.Delegation_To_Date.ToString(), delegationRequest.ToDate.GetValueOrDefault().Date.ToString("MM/dd/yyyy") },
                            };

                            string copyList = delegationRequest.From.Email;
                            if (delegationRequest.From.Email != delegationRequest.RequestedBy.Email)
                            {
                                copyList = delegationRequest.From.Email + ";" + delegationRequest.RequestedBy.Email;
                            }

                            var notification = new NotificationItem
                            {
                                To = delegationRequest.To.Email,
                                CC = copyList,
                                Subject = this.ParseTemplate(delegationAssignmentEmailTemplate.Subject, messageParams),
                                Body = this.ParseTemplate(delegationAssignmentEmailTemplate.Body, messageParams)
                            };

                            this.logger.LogInformation($"{nameof(this.SendDelegationAssignmentToSchedulerAsync)}: Sending email for delegation request {delegationRequest.DelegationId}.");

                            if (await this.notificationClient.SendEmail(new List<NotificationItem> { notification }, delegationRequest.DelegationId).ConfigureAwait(false))
                            {
                                if (string.IsNullOrWhiteSpace(mailSentForDelegations))
                                {
                                    mailSentForDelegations = delegationRequest.DelegationId;
                                }
                                else
                                {
                                    mailSentForDelegations = mailSentForDelegations + ", " + delegationRequest.DelegationId;
                                }
                            }
                        }
                    }
                }
            }

            this.logger.LogInformation($"Finished {nameof(this.SendDelegationAssignmentToSchedulerAsync)} method in {nameof(EmailManager)}.", mailSentForDelegations);
            return;
        }

        /// <summary>
        /// Gets the timezone specific date time.
        /// </summary>
        /// <param name="dateTimeinUTC">The date time in UTC.</param>
        /// <param name="timezone">The instance of <see cref="Timezone"/>.</param>
        /// <returns>The timezone specific date time.</returns>
        private DateTime GetTimezoneDateTime(string dateTimeinUTC, Timezone timezone)
        {
            DateTime dateTime = DateTime.Parse(dateTimeinUTC);
            dateTime = dateTime.AddMinutes(timezone.UTCOffset);
            return dateTime;
        }

        /// <summary>
        /// Chooses the non accepted email template.
        /// </summary>
        /// <param name="interviewerResponseNotification">The instance of <see cref="InterviewerResponseNotification"/>.</param>
        /// <param name="messageResponse">The message response.</param>
        /// <param name="emailTemplateConfiguaration">The instance of <see cref="EmailTemplateConfiguaration"/>.</param>
        /// <returns>The instance of <see cref="Common.Email.Contracts.EmailTemplate"/>.</returns>
        private async Task<Common.Email.Contracts.EmailTemplate> ChooseNonAcceptedEmailTemplate(InterviewerResponseNotification interviewerResponseNotification, string messageResponse, EmailTemplateConfiguaration emailTemplateConfiguaration)
        {
            Common.Email.Contracts.EmailTemplate unacceptedEmailTemplate;
            bool proposedTime = interviewerResponseNotification.ProposedNewTime != null;
            bool respondedWithProposedTime = proposedTime && !string.IsNullOrWhiteSpace(messageResponse);
            if (respondedWithProposedTime)
            {
                unacceptedEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.InterviewerDeclineProposedTimeMessageEmailTemplate);
            }
            else if (proposedTime)
            {
                unacceptedEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.InterviewerDeclineProposedTimeEmailTemplate);
            }
            else if (!string.IsNullOrWhiteSpace(messageResponse))
            {
                unacceptedEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.InterviewerDeclineWithMessageEmailTemplate);
            }
            else
            {
                unacceptedEmailTemplate = await this.scheduleQuery.GetTemplate(emailTemplateConfiguaration.InterviewerDeclineEmailTemplate);
            }

            return unacceptedEmailTemplate;
        }

        /// <summary>
        /// Prepares the notification.
        /// </summary>
        /// <param name="notificationRequest">The instance of <see cref="EmailNotificationRequest"/>.</param>
        /// <returns>The instance of <see cref="NotificationItem"/>.</returns>
        private NotificationItem PrepareNotification(
            EmailNotificationRequest notificationRequest)
        {
            string to = string.Join(";", notificationRequest.MailTo.Select(interviewer => interviewer.Email).Distinct());
            string cc = notificationRequest.MailCC?.Count > 0 ? string.Join(";", notificationRequest.MailCC.Select(recipient => recipient.Email).Distinct()) : string.Empty;
            var notification = new NotificationItem
            {
                To = to,
                CC = cc,
                Subject = notificationRequest.Subject ?? string.Empty,
                Body = notificationRequest.EmailBody ?? string.Empty
            };

            return notification;
        }

        /// <summary>
        /// Prepares the email notification.
        /// </summary>
        /// <param name="emailNotification"> Email content.</param>
        /// <returns>The instance of <see cref="NotificationItem"/>.</returns>
        private NotificationItem PrepareEmailNotification(ScheduleInvitationDetails emailNotification)
        {
            string subject = emailNotification.Subject != null ? emailNotification.Subject : string.Empty;
            string to = emailNotification.PrimaryEmailRecipients != null ? string.Join(";", emailNotification.PrimaryEmailRecipients.Distinct()) : string.Empty;
            string cc = emailNotification.CcEmailAddressList != null ? string.Join(";", emailNotification.CcEmailAddressList.Distinct()) : string.Empty;
            IList<NotificationAttachment> attachments = new List<NotificationAttachment>();
            if (emailNotification.EmailAttachments != null && emailNotification.EmailAttachments.Files != null && emailNotification.EmailAttachments.Files.Count > 0)
            {
                attachments = EmailUtils.PrepareEmailAttachments(emailNotification.EmailAttachments);
            }

            var notification = new NotificationItem
            {
                To = to,
                CC = cc,
                Subject = subject,
                Body = emailNotification.EmailContent,
                Attachments = attachments,
            };

            return notification;
        }

        /// <summary>Parses the subject and body templates.</summary>
        /// <param name="templateContent">The message Subject Template.</param>
        /// <param name="templateParams">The message Body Template.</param>
        /// <returns>returns parsed template</returns>
        private string ParseTemplate(string templateContent, Dictionary<string, string> templateParams)
        {
            foreach (var key in templateParams.Keys)
            {
                var value = templateParams[key] ?? string.Empty;
                templateContent = templateContent.Replace($"{{{key}}}", value, StringComparison.InvariantCultureIgnoreCase);
            }

            return templateContent;
        }

        /// <summary>
        /// Convert CamelCase string to camel case.
        /// </summary>
        /// <param name="camelCase">String to be converted</param>
        /// <param name="toLowercase">Convert to lowercase as well.</param>
        /// <returns>saperated string based on capital letters</returns>
        private string ToLowercaseNamingConvention(string camelCase, bool toLowercase)
        {
            var r = new Regex(
                    @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])",
                    RegexOptions.IgnorePatternWhitespace);

            var convertedString = r.Replace(camelCase, " ");
            if (toLowercase)
            {
                return convertedString.ToLower();
            }

            return convertedString;
        }

        /// <summary>
        /// Form Reminder Schedule Summary
        /// </summary>
        /// <param name="jobApplicationSchedules">Schedules</param>
        /// <param name="scheduleParticipants">Wokers</param>
        /// <returns>Table of schedule summary</returns>
        private string GetReminderSummary(List<JobApplicationSchedule> jobApplicationSchedules, List<Worker> scheduleParticipants)
        {
            string scheduleSummary = string.Empty;
            if (jobApplicationSchedules == null || jobApplicationSchedules.Count <= 0)
            {
                return string.Empty;
            }

            bool shouldContainResponseMessageColumn = false;
            foreach (var schedule in jobApplicationSchedules)
            {
                schedule?.Participants?.ForEach(sp => shouldContainResponseMessageColumn |= !string.IsNullOrWhiteSpace(sp.ParticipantComments));

                if (shouldContainResponseMessageColumn)
                {
                    break;
                }
            }

            scheduleSummary = "<style>.smryTable{font-family : arial, sans-serif;border-collapse: collapse;width: 100%;} .smryTr{border-bottom: 2px solid #dddddd;} .smryTd{text-align: center; padding-top: 6px; padding-bottom: 6px; font-size: 12px; }";
            scheduleSummary = scheduleSummary + ".smryTh{border: 2px solid #ffffff; background-color: #dddddd; text-align: center; padding: 6px; font-size: 13px; } .status{font-size: 10px; color:#0d0d0d} </style><table>";
            scheduleSummary = scheduleSummary + "<tr class = \"smryTr\"><th class = \"smryTh\">Interview Details</th><th class = \"smryTh\">Interviewers</th><th class = \"smryTh\">Location</th>";
            if (shouldContainResponseMessageColumn)
            {
                scheduleSummary = scheduleSummary + "<th class = \"smryTh\">Response Message</th></tr>";
            }
            else
            {
                scheduleSummary = scheduleSummary + "</tr>";
            }

            foreach (var schedule in jobApplicationSchedules)
            {
                var timeZoneInfo = this.scheduleQuery.GetTimezoneForJobApplication(schedule?.JobApplicationID).Result;
                scheduleSummary = scheduleSummary + "<tr class = \"smryTr\">";
                scheduleSummary = scheduleSummary + "<td class = \"smryTd\">" + this.ParseTimebyTimeZoneOffset(schedule?.StartDateTime.ToString(), schedule?.EndDateTime.ToString(), timeZoneInfo?.UTCOffset, timeZoneInfo.TimezoneAbbr) + "<br>" + schedule?.Subject?.ToString() + "</td>";
                scheduleSummary = scheduleSummary + "<td class = \"smryTd\">";
                schedule?.Participants?.ForEach(sp =>
                {
                    var participantDetails = scheduleParticipants.First(w => w.OfficeGraphIdentifier == sp.OID);
                    scheduleSummary = scheduleSummary + participantDetails?.FullName?.ToString() + "<br><span class = \"status\"> (" + sp.ParticipantStatus.ToString() + ")</span>" + "<br>";
                });
                scheduleSummary = scheduleSummary + "</td>";
                if (schedule.BuildingName != null && schedule.BuildingName.Length > 0)
                {
                    scheduleSummary = scheduleSummary + "<td class = \"smryTd\">" + schedule?.BuildingName.ToString();

                    if (schedule.RoomName != null && schedule.RoomName.Length > 0)
                    {
                        scheduleSummary = scheduleSummary + " - " + schedule?.RoomName.ToString();
                    }

                    if (schedule.OnlineMeetingRequired)
                    {
                        scheduleSummary = scheduleSummary + "<br>Via Teams";
                    }

                    scheduleSummary = scheduleSummary + "</td>";
                }
                else if (schedule.RoomName != null && schedule.RoomName.Length > 0)
                {
                    scheduleSummary = scheduleSummary + "<td class = \"smryTd\">" + schedule?.RoomName.ToString();

                    if (schedule.OnlineMeetingRequired)
                    {
                        scheduleSummary = scheduleSummary + "<br>Via Teams";
                    }

                    scheduleSummary = scheduleSummary + "</td>";
                }
                else if (schedule.OnlineMeetingRequired)
                {
                    scheduleSummary = scheduleSummary + "<td class = \"smryTd\"> Via Teams </td>";
                }
                else
                {
                    scheduleSummary = scheduleSummary + "<td class = \"smryTd\"> - </td>";
                }

                if (shouldContainResponseMessageColumn)
                {
                    scheduleSummary = scheduleSummary + "<td class = \"smryTd\">";
                    schedule?.Participants?.ForEach(sp =>
                    {
                        if (string.IsNullOrWhiteSpace(sp?.ParticipantComments))
                        {
                            scheduleSummary = scheduleSummary + " - ";
                        }
                        else
                        {
                            scheduleSummary = scheduleSummary + sp.ParticipantComments;
                        }
                    });
                    scheduleSummary = scheduleSummary + "</td>";
                }

                scheduleSummary = scheduleSummary + "</tr>";
            }

            scheduleSummary = scheduleSummary + "</table>";
            return scheduleSummary;
        }

        /// <summary>
        /// Parse Time by TimeZone Offset
        /// </summary>
        /// <param name="startDateTime">Start Date Time</param>
        /// <param name="endDateTime">End Date Time</param>
        /// <param name="timeZoneOffset">Time Zone Offset</param>
        /// <param name="timeZoneAbbr">Time Zone Abbrievation</param>
        /// <returns>time</returns>
        private string ParseTimebyTimeZoneOffset(string startDateTime, string endDateTime, int? timeZoneOffset, string timeZoneAbbr)
        {
            int offsetValue = 0;
            if (timeZoneOffset.HasValue)
            {
                offsetValue = timeZoneOffset.Value;
            }

            var convStartDateTime = DateTime.Parse(startDateTime).AddMinutes(offsetValue);
            var convEndtDateTime = DateTime.Parse(endDateTime).AddMinutes(offsetValue);

            var interviewDateTime = convStartDateTime.ToString("MMMM d yyyy <br> h:mm tt - ") + convEndtDateTime.ToString("h:mm tt") + " (" + timeZoneAbbr + ")";
            return interviewDateTime;
        }

        /// <summary>
        /// Validates the feedback only email notification request.
        /// </summary>
        /// <param name="notificationRequest">The instance of <see cref="EmailNotificationRequest"/>.</param>
        /// <exception cref="OperationNotAllowedException">
        /// The job application Id is not specified.
        /// or
        /// The 'To' members are not specified.
        /// or
        /// The 'CC' members are not specified.
        /// </exception>
        private void ValidateNotificationRequest(EmailNotificationRequest notificationRequest)
        {
            if (string.IsNullOrWhiteSpace(notificationRequest.JobApplicationId))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid application id").EnsureLogged(this.logger);
            }

            if (notificationRequest.MailTo == null || notificationRequest.MailTo.Count == 0)
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid 'To' members in email.").EnsureLogged(this.logger);
            }

            if (string.IsNullOrWhiteSpace(notificationRequest.EmailBody))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid email body.").EnsureLogged(this.logger);
            }

            if (string.IsNullOrWhiteSpace(notificationRequest.Subject))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid subject text.").EnsureLogged(this.logger);
            }
        }

        /// <summary>
        /// Adds the Wob Users to CC in case the Wob Flight is switched on
        /// </summary>
        /// <param name="notification"> An instance of <see cref="NotificationItem"/></param>
        /// <param name="schedulerOid">The scheduler object id</param>
        /// <returns>An instance of <see cref="Task{NotificationItem}"/> representing the asynchronous operation.</returns>
        private async Task<NotificationItem> AddWobUsersToNotificationRecepientAsync(NotificationItem notification, string schedulerOid)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (string.IsNullOrWhiteSpace(schedulerOid))
            {
                throw new ArgumentException($"'{nameof(schedulerOid)}' cannot be null or empty", nameof(schedulerOid));
            }

            var wobUsers = await this.scheduleQuery.GetWobUsersDelegation(schedulerOid).ConfigureAwait(false);
            notification.CC = string.Join(";", wobUsers.Select(wob => wob.To.EmailPrimary));
            this.logger.LogInformation($"Added {wobUsers.Count} WOB Participants in CC.");
            return notification;
        }

        private string PopulateToOrCCListInEmail(List<string> rolesToBeAdded, Worker interviewer, Worker hiringManager, Worker recruiter, Worker contributor)
        {
            StringBuilder recipientList = new StringBuilder();

            if (rolesToBeAdded?.Count > 0)
            {
                foreach (var role in rolesToBeAdded)
                {
                    switch (role.ToLower())
                    {
                        case "interviewer":
                            recipientList.Append(interviewer?.EmailPrimary + ";");
                            break;

                        case "hiringmanager":
                            recipientList.Append(hiringManager?.EmailPrimary + ";");
                            break;

                        case "recruiter":
                            recipientList.Append(recruiter?.EmailPrimary + ";");
                            break;

                        case "contributor":
                            recipientList.Append(contributor?.EmailPrimary + ";");
                            break;

                        default:
                            break;
                    }
                }
            }

            return recipientList.ToString();
        }
    }
}
