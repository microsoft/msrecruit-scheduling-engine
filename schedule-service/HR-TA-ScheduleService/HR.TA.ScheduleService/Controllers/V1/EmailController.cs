//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.Common.Common.Common.Web.Extensions;
    using HR.TA.Common.TalentAttract.Contract;
    using HR.TA.Common.Web;
    using HR.TA.ScheduleService.BusinessLibrary.Exceptions;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.Contracts;
    using HR.TA.ScheduleService.Contracts.V1;
    using HR.TA.ServicePlatform.AspNetCore.Mvc.Filters;
    using HR.TA.ServicePlatform.Exceptions;
    using HR.TA.Talent.TalentContracts.InterviewService;
    using HR.TA.Talent.TalentContracts.ScheduleService;
    using HR.TA.TalentEntities.Enum;

    /// <summary>
    /// The Email Controller.
    /// </summary>
    [Route("v1/email")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EmailController : HCMWebApiAuthenticatedController
    {
        private readonly IEmailManager emailManager;

        private readonly IRoleManager roleManager;

        /// <summary>
        /// The instance for <see cref="ILogger{EmailController}"/>.
        /// </summary>
        private readonly ILogger<EmailController> logger;

        /// <summary>
        /// holds servicecontext
        /// </summary>
        private readonly IHCMServiceContext hcmServiceContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor instance.</param>
        /// <param name="serviceContext">service context</param>
        /// <param name="emailManager">email manager instance</param>
        /// <param name="roleMananger">role manager instance</param>
        /// <param name="logger">The instance for <see cref="ILogger{EmailController}"/>.</param>
        public EmailController(
            IHttpContextAccessor httpContextAccessor,
            IHCMServiceContext serviceContext,
            IEmailManager emailManager,
            IRoleManager roleMananger,
            ILogger<EmailController> logger)
                : base(httpContextAccessor)
        {
            this.hcmServiceContext = serviceContext;
            this.emailManager = emailManager;
            this.logger = logger;
            this.roleManager = roleMananger;
        }

        /// <summary>
        /// Sends a reminder to interviewer to complete the feedback.
        /// </summary>
        /// <param name="pendingFeedbackRequest">Pending Feedback information.</param>
        /// <returns>Reminder Success response.</returns>
        [HttpPost("remind/interviewer")]
        [MonitorWith("GTAV1SendFeedbackReminder")]
        public async Task<bool> SendFeedbackReminder([FromBody]PendingFeedback pendingFeedbackRequest)
        {
            this.logger.LogInformation($"Started {nameof(this.SendFeedbackReminder)} method in {nameof(EmailController)}.");
            if (pendingFeedbackRequest == null || string.IsNullOrEmpty(pendingFeedbackRequest?.JobApplicationId) || string.IsNullOrEmpty(pendingFeedbackRequest?.InterviewerOID))
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid application or interviewer").EnsureLogged(this.logger);
            }

            var hcmContextUserId = this.hcmServiceContext.UserId;
            if (await this.roleManager.IsUserInJobApplicationParticipants(hcmContextUserId, pendingFeedbackRequest?.JobApplicationId, string.Empty).ConfigureAwait(false))
            {
                this.logger.LogInformation($"Finished {nameof(this.SendFeedbackReminder)} method in {nameof(EmailController)}.");
                return await this.emailManager.SendFeedbackReminder(pendingFeedbackRequest);
            }
            else
            {
                throw new UnauthorizedStatusException("Pending Feedback request failed as user is not part of Job Application Participants list.").EnsureLogged(this.logger);
            }
        }

        /// <summary>
        /// Sends a reminder to scheduler to remind about the interviwer's response.
        /// </summary>
        /// <returns>Reminder Success response.</returns>
        [HttpPost("remind/scheduler")]
        [MonitorWith("GTAV1SendReminderToScheduler")]
        public async Task<bool> SendSchedulerReminder()
        {
            bool isReminderSent = false;
            this.logger.LogInformation($"Started {nameof(this.SendSchedulerReminder)} method in {nameof(EmailController)}.");
            isReminderSent = await this.emailManager.SendSchedulerReminder();
            this.logger.LogInformation($"Finished {nameof(this.SendSchedulerReminder)} method in {nameof(EmailController)}.");
            return isReminderSent;
        }

        /// <summary>
        /// Sends a reminder to scheduler on sending invitations failed.
        /// </summary>
        /// <param name="scheduleId">schedule Id</param>
        /// <returns>Reminder failure invitation.</returns>
        [HttpPost("invitefailremind/scheduler")]
        [MonitorWith("GTAV1SendInviteFailRemind")]
        public async Task<bool> SendInviteFailReminder(string scheduleId)
        {
            bool isInviteReminderSent = false;
            this.logger.LogInformation($"Started {nameof(this.SendInviteFailReminder)} method in {nameof(EmailController)}.");
            if (string.IsNullOrEmpty(scheduleId))
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid schedule id").EnsureLogged(this.logger);
            }
            //TBD: Need enhancements here.
            isInviteReminderSent = await this.emailManager.SendInviteFailReminder(scheduleId);
            this.logger.LogInformation($"Finished {nameof(this.SendInviteFailReminder)} method in {nameof(EmailController)}.");
            return isInviteReminderSent;
        }

        /// <summary>
        /// Sends a reminder to scheduler on sending invitations failed.
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="schedulerOid">scheduler oid</param>
        /// <param name="candidateName">candidate name</param>
        /// <returns>Send email to scheduler for new assignment.</returns>
        [HttpPost("schedulerassignment/scheduler")]
        [MonitorWith("GTAV1SendAssignmentToScheduler")]
        public async Task<bool> SendAssignmentEmailToScheduler(string jobApplicationId, string schedulerOid, string candidateName)
        {
            bool isAssignmentEmailToScheduler = false;
            this.logger.LogInformation($"Started {nameof(this.SendAssignmentEmailToScheduler)} method in {nameof(EmailController)}.");
            if (string.IsNullOrEmpty(jobApplicationId))
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid application id").EnsureLogged(this.logger);
            }

            isAssignmentEmailToScheduler = await this.emailManager.SendAssignmentEmailToScheduler(jobApplicationId, schedulerOid, candidateName);
            this.logger.LogInformation($"Finished {nameof(this.SendAssignmentEmailToScheduler)} method in {nameof(EmailController)}.");
            return isAssignmentEmailToScheduler;
        }

        /// <summary>
        /// Sends email to the specified interviewers of the job application.
        /// </summary>
        /// <param name="notificationRequest">The instance of <see cref="EmailNotificationRequest"/>.</param>
        /// <returns>The instance of <see cref="Task{IActionResult}"/> representing the asynchronous task.</returns>
        /// <response code="200">Returns a flag indicating success or failure..</response>
        /// <response code="400">If the job application id is missing or interviewers list is empty or null.</response>
        [HttpPost("requestfeedback")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [MonitorWith("GTAV1SendFeedbackOnlyEmail")]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public async Task<IActionResult> SendEmailToInterviewers([FromBody]EmailNotificationRequest notificationRequest)
        {
            IActionResult actionResult = null;
            bool mailSent = false;
            var hcmContextUserId = this.hcmServiceContext.UserId;
            this.logger.LogInformation($"Started {nameof(this.SendEmailToInterviewers)} method in {nameof(EmailController)}.");

            // var principal = ServiceContext.Principal.TryGetCurrent<Common.Base.Security.HCMApplicationPrincipal>();
            if (await this.roleManager.IsUserHMorRecOrContributor(hcmContextUserId, notificationRequest?.JobApplicationId, string.Empty))
            {
                bool isWobUser = this.roleManager.IsUserWobAuthenticated(this.hcmServiceContext.isWobAuthenticated);
                mailSent = await this.emailManager.SendFeedbackEmailAsync(notificationRequest, hcmContextUserId, isWobUser).ConfigureAwait(false);
                actionResult = this.Ok(mailSent);
                this.logger.LogInformation($"Finished {nameof(this.SendEmailToInterviewers)} method in {nameof(EmailController)}.");
            }
            else
            {
                throw new UnauthorizedStatusException("Request Feedback request failed as user is not an interviewer or AA.").EnsureLogged(this.logger);
            }

            return actionResult;
        }

        /// <summary>
        /// Send email notification about tool switching to IV.
        /// </summary>
        /// <param name="requisitionId">job application id</param>
        /// <returns>Send email to scheduler for new assignment.</returns>
        [HttpPost("pilot/notification/{requisitionId}")]
        [MonitorWith("GTAV1SendPilotInvitation")]
        public async Task<bool> SendPilotNotification(string requisitionId)
        {
            bool isPilotEmail = false;
            this.logger.LogInformation($"Started {nameof(this.SendPilotNotification)} method in {nameof(EmailController)}.");
            if (string.IsNullOrEmpty(requisitionId))
            {
                throw new InvalidRequestDataValidationException("SendPilotNotification: requisition id cannot be null").EnsureLogged(this.logger);
            }

            isPilotEmail = await this.emailManager.SendPilotNotification(requisitionId);
            this.logger.LogInformation($"Finished {nameof(this.SendPilotNotification)} method in {nameof(EmailController)}.");
            return isPilotEmail;
        }

        /// <summary>
        /// Sends a mail to inform AA on sharing feedback.
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="aaOid">AA OID</param>
        /// <returns>Send email to scheduler for new assignment.</returns>
        [HttpPost("schedulerassignment/sharefeedback")]
        [MonitorWith("GTAV1SendShareFeedbackEmailToAA")]
        public async Task<bool> SendShareFeedbackEmailToAA(string jobApplicationId, [FromBody]string[] aaOid)
        {
            bool isShareFeedbackEmailToAA = false;
            this.logger.LogInformation($"Started {nameof(this.SendShareFeedbackEmailToAA)} method in {nameof(EmailController)}.");
            if (string.IsNullOrWhiteSpace(jobApplicationId) || aaOid == null || aaOid.Length == 0)
            {
                throw new InvalidRequestDataValidationException("SendShareFeedbackEmailToAA: input paramenter cannot be null or empty").EnsureLogged(this.logger);
            }

            if (await this.roleManager.IsUserContributor(this.hcmServiceContext.UserId, jobApplicationId))
            {
                isShareFeedbackEmailToAA = await this.emailManager.SendShareFeedbackEmailToAA(jobApplicationId, aaOid);
                this.logger.LogInformation($"Finished {nameof(this.SendShareFeedbackEmailToAA)} method in {nameof(EmailController)}.");
            }
            else
            {
                throw new UnauthorizedStatusException("Share Feedback failed as user is not a contributor.").EnsureLogged(this.logger);
            }

            return isShareFeedbackEmailToAA;
        }

        /// <summary>
        /// Send email notification
        /// </summary>
        /// <param name="jobApplicationId">JobApplication Id</param>
        /// <param name="emailNotification">email notification</param>
        /// <returns>Http Status Code</returns>
        [HttpPost]
        [Route("{jobApplicationId}/sendemail")]
        [MonitorWith("GTAV1SendEmail")]
        public async Task<HttpStatusCode> SendEmailNotification(string jobApplicationId, [FromBody] ScheduleInvitationDetails emailNotification)
        {
            this.logger.LogInformation($"Started {nameof(this.SendEmailNotification)} method in {nameof(EmailController)}.");
            if (jobApplicationId == null)
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid application id").EnsureLogged(this.logger);
            }

            if (emailNotification == null)
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid email address").EnsureLogged(this.logger);
            }

            var hcmContextUserId = this.hcmServiceContext.UserId;
            if (await this.roleManager.IsUserInJobApplicationParticipants(hcmContextUserId, jobApplicationId, string.Empty))
            {
                bool isWobUser = this.roleManager.IsUserWobAuthenticated(this.hcmServiceContext.isWobAuthenticated);
                await this.emailManager.SendEmailNotification(jobApplicationId, emailNotification, this.hcmServiceContext.Email, hcmContextUserId, isWobUser);
            }
            else
            {
                throw new UnauthorizedStatusException("Send Summary Email Notification failed as user is not part of Job Application Participants list.").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.SendEmailNotification)} method in {nameof(EmailController)}.");
            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Send email notification
        /// </summary>
        /// <param name="jobApplicationId">JobApplication Id</param>
        /// <param name="scheduleInvitationDetailsV2">email notification</param>
        /// <returns>Http Status Code</returns>
        [HttpPost]
        [Route("{jobApplicationId}/sendemailV2")]
        [MonitorWith("GTAV1SendEmailV2")]
        [Consumes("multipart/form-data")]
        public async Task<HttpStatusCode> SendEmailNotificationV2(string jobApplicationId, [FromForm] ScheduleInvitationDetailsV2 scheduleInvitationDetailsV2)
        {
            this.logger.LogInformation($"Started {nameof(this.SendEmailNotificationV2)} method in {nameof(EmailController)}.");
            if (jobApplicationId == null)
            {
                throw new BusinessRuleViolationException("Input request does not contain a valid application id").EnsureLogged(this.logger);
            }

            var hcmContextUserId = this.hcmServiceContext.UserId;
            if (await this.roleManager.IsUserInJobApplicationParticipants(hcmContextUserId, jobApplicationId, string.Empty))
            {
                bool isWobUser = this.roleManager.IsUserWobAuthenticated(this.hcmServiceContext.isWobAuthenticated);

                ScheduleInvitationDetails emailNotification = this.CreateScheduleInvitationDetails(scheduleInvitationDetailsV2);
                await this.emailManager.SendEmailNotification(jobApplicationId, emailNotification, this.hcmServiceContext.Email, hcmContextUserId, isWobUser);
            }
            else
            {
                throw new UnauthorizedStatusException("Send Summary Email Notification failed as user is not part of Job Application Participants list.").EnsureLogged(this.logger);
            }

            this.logger.LogInformation($"Finished {nameof(this.SendEmailNotificationV2)} method in {nameof(EmailController)}.");
            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Sends email reminders to interviewers to complete the pending feedbacks.
        /// </summary>
        /// <returns>An instance of <see cref="Task{Boolean}"/>.</returns>
        [HttpPost("remind/participants")]
        [MonitorWith("GTAV1SendFeedbackReminderToAllAsync")]
        public async Task<bool> SendFeedbackReminderToAllAsync()
        {
            bool areRemindersSent = false;
            this.logger.LogInformation($"Started {nameof(this.SendFeedbackReminderToAllAsync)} method in {nameof(EmailController)}.");
            areRemindersSent = await this.emailManager.SendFeedbackReminderToAllAsync();
            this.logger.LogInformation($"Finished {nameof(this.SendFeedbackReminderToAllAsync)} method in {nameof(EmailController)}.");
            return areRemindersSent;
        }

        /// <summary>
        /// Sends email to schedulers for delegation assignment.
        /// </summary>
        /// <param name="delegationRequestCollection">Delegation request collection</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        [HttpPost("delegationassignmenttoscheduler")]
        [MonitorWith("GTAV1DelegationAssigned")]
        public async Task SendDelegationAssignmentToSchedulerAsync([FromBody] IEnumerable<DelegationRequest> delegationRequestCollection)
        {
            this.logger.LogInformation($"Started {nameof(this.SendDelegationAssignmentToSchedulerAsync)} method in {nameof(EmailController)}.");
            if (delegationRequestCollection == null || !delegationRequestCollection.Any())
            {
                this.logger.LogInformation($"No delegation present for sending notifications.");
            }
            else
            {
                await this.emailManager.SendDelegationAssignmentToSchedulerAsync(delegationRequestCollection).ConfigureAwait(false);
            }

            this.logger.LogInformation($"Finished {nameof(this.SendDelegationAssignmentToSchedulerAsync)} method in {nameof(EmailController)}.");
        }

        /// <summary>
        /// Creates a scheduleInvitationDetails object from scheduleInvitationDetailsV2 object.
        /// </summary>
        /// <param name="data">scheduleInvitationDetailsV2 object</param>
        /// <returns>An instance of <see cref="ScheduleInvitationDetails"/>.</returns>
        private ScheduleInvitationDetails CreateScheduleInvitationDetails(ScheduleInvitationDetailsV2 data)
        {
            if (data == null)
            {
                throw new InvalidRequestDataValidationException("Input request does not contain a valid data object").EnsureLogged(this.logger);
            }

            return new ScheduleInvitationDetails
            {
                IsInterviewTitleShared = data.IsInterviewTitleShared,
                ScheduleId = data.ScheduleId,
                InterviewTitle = data.InterviewTitle,
                SkypeMeetingUrl = data.SkypeMeetingUrl,
                Location = data.Location,
                EndTime = data.EndTime,
                StartTime = data.StartTime,
                InterviewDate = data.InterviewDate,
                EmailAttachments = new FileAttachmentRequest { Files = data.EmailAttachmentFiles, FileLabels = data.EmailAttachmentFileLabels },
                EmailContent = data.EmailContent,
                Subject = data.Subject,
                BccEmailAddressList = data.BccEmailAddressList,
                CcEmailAddressList = data.CcEmailAddressList,
                PrimaryEmailRecipients = data.PrimaryEmailRecipients,
                Requester = new GraphPerson
                {
                    Comments = data.RequesterComments,
                    Email = data.RequesterEmail,
                    GivenName = data.RequesterGivenName,
                    Id = data.RequesterId,
                    MobilePhone = data.RequesterMobilePhone,
                    Name = data.RequesterName,
                    OfficeLocation = data.RequesterOfficeLocation,
                    PreferredLanguage = data.RequesterPreferredLanguage,
                    Surname = data.RequesterSurname,
                    Title = data.RequesterTitle,
                    UserPrincipalName = data.RequesterUserPrincipalName,
                },
                IsInterviewScheduleShared = data.IsInterviewScheduleShared,
                IsInterviewerNameShared = data.IsInterviewerNameShared,
            };
        }
    }
}
