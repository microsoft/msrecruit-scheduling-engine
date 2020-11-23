// <copyright file="CandidateCommunicator.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.NotifyCandidate
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Base.Utilities;
    using MS.GTA.Common.Common.Common.Email.Contracts;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.ScheduleService.BusinessLibrary.Strings;
    using MS.GTA.ScheduleService.BusinessLibrary.Utils;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Utils;
    using MS.GTA.Talent.TalentContracts.ScheduleService;

    /// <summary>
    /// The <see cref="CandidateCommunicator"/> implements the mechanism to communicate with the candidate.
    /// </summary>
    /// <seealso cref="ICandidateCommunicator" />
    internal class CandidateCommunicator : ICandidateCommunicator
    {
        private const string TeamsToken = "<p><strong>[Insert MS Teams Link if One or More Interviews via Teams]</strong></p>";

        /// <summary>
        /// The instance for <see cref="CandidateCommunicatorMakers"/>.
        /// </summary>
        private readonly CandidateCommunicatorMakers communicatorMakers;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CandidateCommunicator> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandidateCommunicator"/> class.
        /// </summary>
        /// <param name="communicatorMakers">The instance of <see cref="CandidateCommunicatorMakers"/></param>
        /// <param name="logger">The instance for <see cref="ILogger{CandidateCommunicator}"/>.</param>
        public CandidateCommunicator(CandidateCommunicatorMakers communicatorMakers, ILogger<CandidateCommunicator> logger)
        {
            Contract.CheckValue(communicatorMakers, nameof(communicatorMakers));
            this.communicatorMakers = communicatorMakers;
            this.logger = logger;
        }

        /// <summary>
        /// Sends the invitation asynchronously.
        /// </summary>
        /// <param name="jobApplication">The instance of <see cref="JobApplication"/>.</param>
        /// <param name="scheduleInvitationRequest">The instance of <see cref="ScheduleInvitationRequest" />.</param>
        /// <param name="schedules">The instance of <see cref="List{MeetingInfo}"/>.</param>
        /// <returns>
        /// The instance of <see cref="Task{Boolean}" /> with <c>true</c> if invitation is sent; otherwise <c>false</c>.
        /// </returns>
        public async Task<bool> SendInvitationAsync(JobApplication jobApplication, ScheduleInvitationRequest scheduleInvitationRequest, List<MeetingInfo> schedules)
        {
            Contract.CheckValue(jobApplication, nameof(jobApplication));
            Contract.CheckValue(scheduleInvitationRequest, nameof(scheduleInvitationRequest));
            Contract.CheckValue(schedules, nameof(schedules));
            return await this.SendInvitation(jobApplication, scheduleInvitationRequest, schedules).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends the invitation.
        /// </summary>
        /// <param name="jobApplication">The job application.</param>
        /// <param name="scheduleInvitationRequest">The instance of <see cref="ScheduleInvitationRequest"/>.</param>
        /// <param name="schedules">The instance of <see cref="List{MeetingInfo}"/>.</param>
        /// <returns>The instance of <see cref="Task{Boolean}"/> where <c>true</c> denotes the success;otherwise <c>false</c>.</returns>
        private async Task<bool> SendInvitation(JobApplication jobApplication, ScheduleInvitationRequest scheduleInvitationRequest, List<MeetingInfo> schedules)
        {
            string scheduleSummary = string.Empty;
            List<NotificationAttachment> attachments;
            var timezoneTask = this.communicatorMakers.ScheduleQuery.GetTimezoneForJobApplication(jobApplication.JobApplicationID);

            // The below statement filters only shared schedules for which there are interviewers who have not declined the meeting.
            var sharedSchedules = scheduleInvitationRequest.SharedSchedules.SelectMany(sch =>
                    schedules.Where(s => scheduleInvitationRequest.IsInterviewScheduleShared && sch.IsInterviewScheduleShared && s.Id.Equals(sch.ScheduleId, StringComparison.OrdinalIgnoreCase)
                    && s.MeetingDetails?[0].UtcStart > DateTime.UtcNow)).ToList();
            var timezone = await timezoneTask.ConfigureAwait(false);

            if (sharedSchedules.Any())
            {
                attachments = this.PrepareNotificationAttachments(scheduleInvitationRequest, sharedSchedules, timezone);
                scheduleSummary = await this.communicatorMakers.EmailHelper.GetScheduleSummaryAsync(sharedSchedules, scheduleInvitationRequest, timezone).ConfigureAwait(false);
            }
            else
            {
                attachments = new List<NotificationAttachment>();
            }

            var notificationItem = new NotificationItem()
            {
                To = scheduleInvitationRequest.PrimaryEmailRecipient,
                CC = scheduleInvitationRequest.CcEmailAddressList != null ? string.Join(";", scheduleInvitationRequest.CcEmailAddressList) : string.Empty,
                Subject = scheduleInvitationRequest.Subject,
                Body = this.GetMainContent(scheduleInvitationRequest.EmailContent) + scheduleSummary,
                Attachments = attachments,
                ReplyTo = this.communicatorMakers.RequesterEmail ?? string.Empty
            };

            var notificationItems = new List<NotificationItem>() { notificationItem };
            bool responseStatus = await this.communicatorMakers.NotificationClient.SendEmail(notificationItems);
            return responseStatus;
        }

        private string GetMainContent(string emailContent)
        {
            ////Checking emailContent has content or not
            if (!string.IsNullOrWhiteSpace(emailContent))
            {
                return emailContent.Replace(TeamsToken, string.Empty);
            }

            return emailContent;
        }

        /// <summary>
        /// Prepares the notification attachments.
        /// </summary>
        /// <param name="scheduleInvitationRequest">The instance of <see cref="ScheduleInvitationRequest"/>.</param>
        /// <param name="schedules">The instance of <see cref="List{MeetingInfo}"/>.</param>
        /// <param name="timezone">time zone</param>
        /// <returns>The instance of <see cref="NotificationAttachment"/>.</returns>
        private List<NotificationAttachment> PrepareNotificationAttachments(ScheduleInvitationRequest scheduleInvitationRequest, List<MeetingInfo> schedules, Timezone timezone)
        {
            var emailStyleTemplate = this.LoadResource(BusinessConstants.EmailTemplateWithoutButton);
            List<NotificationAttachment> attachments = new List<NotificationAttachment>();
            string icsTemplate = this.LoadResource(BusinessConstants.ICSTemplate);
            foreach (var item in schedules)
            {
                if (item.MeetingDetails?[0].Attendees.Where(a => a.ResponseStatus != TalentEntities.Enum.InvitationResponseStatus.None
                && a.ResponseStatus != TalentEntities.Enum.InvitationResponseStatus.Declined).Any() ?? false)
                {
                    StringBuilder sb = new StringBuilder();
                    string dateFormat = "yyyyMMddTHHmmssZ";
                    string now = DateTime.Now.ToUniversalTime().ToString(dateFormat);
                    string ics = icsTemplate.Replace("[StartDate]", item.MeetingDetails?[0].UtcStart.ToString(dateFormat));
                    ics = ics.Replace("[EndDate]", item.MeetingDetails?[0].UtcEnd.ToString(dateFormat));
                    ics = ics.Replace("[GUID]", Guid.NewGuid().ToString());
                    ics = ics.Replace("[MailSubject]", scheduleInvitationRequest.Subject);
                    ics = ics.Replace("[Location]", EmailUtils.GetMeetingLocation(item.MeetingDetails?[0].MeetingLocation));
                    ////populate meeting info
                    string summaryContent = string.Empty;
                    if (item.MeetingDetails[0].OnlineMeetingRequired)
                    {
                        summaryContent = EmailUtils.GetTeamMeetingInfo(item.MeetingDetails?[0].Conference);
                    }

                    string emailContent = this.PopulateEmailContentWithTeamsMeetingInfo(scheduleInvitationRequest.EmailContent, summaryContent);

                    string htmlBody = Regex.Replace(this.GetEmailContent(emailStyleTemplate, emailContent) ?? string.Empty, @"\t|\n|\r", string.Empty);
                    ics = ics.Replace("[HTMLBody]", htmlBody);
                    NotificationAttachment attachment = new NotificationAttachment()
                    {
                        FileName = scheduleInvitationRequest.Subject + ".ics",
                        FileBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(ics)),
                    };

                    attachments.Add(attachment);
                }
            }

            if (scheduleInvitationRequest.EmailAttachments != null && scheduleInvitationRequest.EmailAttachments.Files != null && scheduleInvitationRequest.EmailAttachments.Files.Count > 0)
            {
                attachments.AddRange(EmailUtils.PrepareEmailAttachments(scheduleInvitationRequest.EmailAttachments));
            }

            return attachments;
        }

        private string PopulateEmailContentWithTeamsMeetingInfo(string emailContent, string meetingContent)
        {
            if (!string.IsNullOrWhiteSpace(emailContent) && !string.IsNullOrWhiteSpace(meetingContent))
            {
                if (!emailContent.EndsWith("</span>", StringComparison.OrdinalIgnoreCase))
                {
                    emailContent = emailContent + "</span>";
                }

                if (emailContent.Contains(TeamsToken))
                {
                    emailContent = emailContent.Replace(TeamsToken, meetingContent);
                }
                else
                {
                    emailContent = emailContent + meetingContent;
                }
            }

            return emailContent;
        }

        /// <summary>
        /// Loads the resource from resource file.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>The resource value.</returns>
        private string LoadResource(string resourceKey)
        {
            string resourceValue = string.Empty;
            ResourceManager resourceManager;
            Type serviceType = typeof(ScheduleServiceEmailTemplate);
            resourceManager = new ResourceManager(serviceType.Namespace + ".ScheduleServiceEmailTemplate", serviceType.Assembly);
            resourceValue = resourceManager.GetString(resourceKey);
            return resourceValue;
        }

        /// <summary>Parses the subject and body templates.</summary>
        /// <param name="template">The message Template.</param>
        /// <param name="templateContent">The message content.</param>
        /// <returns>The parsed template.</returns>
        private string GetEmailContent(string template, string templateContent)
        {
            var templateParams = new Dictionary<string, string>
                {
                    { "EmailHeaderUrl", BusinessConstants.MicrosoftLogoUrl },
                    { "EmailBodyContent", templateContent },
                    { "CompanyInfoFooter", BusinessConstants.MicrosoftInfoFooter },
                    { "Company_Name", "Microsoft" },
                    { "Privacy_Policy_Link", BusinessConstants.PrivacyPolicyUrl },
                    { "Terms_And_Conditions_Link", BusinessConstants.TermsAndConditionsUrl },
                };

            foreach (var key in templateParams.Keys)
            {
                var value = templateParams[key] ?? string.Empty;
                template = template.Replace($"{{{key}}}", value, StringComparison.InvariantCultureIgnoreCase);
            }

            return template;
        }
    }
}
