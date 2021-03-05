//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Talent.TalentContracts.ScheduleService
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Schedule invitation details data contract
    /// </summary>
    [DataContract]
    public class ScheduleInvitationDetailsV2
    {
        /// <summary>
        /// Gets or sets the requesterName
        /// </summary>
        [DataMember(Name = "requesterName", IsRequired = false)]
        public string RequesterName { get; set; }

        /// <summary>
        /// Gets or sets the requesterId
        /// </summary>
        [DataMember(Name = "requesterId", IsRequired = false)]
        public string RequesterId { get; set; }

        /// <summary>
        /// Gets or sets the requesterTitle
        /// </summary>
        [DataMember(Name = "requesterTitle", IsRequired = false)]
        public string RequesterTitle { get; set; }

        /// <summary>
        /// Gets or sets the requesterEmail
        /// </summary>
        [DataMember(Name = "requesterEmail", IsRequired = false)]
        public string RequesterEmail { get; set; }

        /// <summary>
        /// Gets or sets the requesterGivenName
        /// </summary>
        [DataMember(Name = "requesterGivenName", IsRequired = false)]
        public string RequesterGivenName { get; set; }

        /// <summary>
        /// Gets or sets the requesteMobilePhoner
        /// </summary>
        [DataMember(Name = "requesteMobilePhoner", IsRequired = false)]
        public string RequesterMobilePhone { get; set; }

        /// <summary>
        /// Gets or sets the requesterOfficeLocation
        /// </summary>
        [DataMember(Name = "requesterOfficeLocation", IsRequired = false)]
        public string RequesterOfficeLocation { get; set; }

        /// <summary>
        /// Gets or sets the requesterPreferredLanguage
        /// </summary>
        [DataMember(Name = "requesterPreferredLanguage", IsRequired = false)]
        public string RequesterPreferredLanguage { get; set; }

        /// <summary>
        /// Gets or sets the requesterSurname
        /// </summary>
        [DataMember(Name = "requesterSurname", IsRequired = false)]
        public string RequesterSurname { get; set; }

        /// <summary>
        /// Gets or sets the requesterUserPrincipalName
        /// </summary>
        [DataMember(Name = "requesterUserPrincipalName", IsRequired = false)]
        public string RequesterUserPrincipalName { get; set; }

        /// <summary>
        /// Gets or sets the requesterInvitationResponseStatus
        /// </summary>
        [DataMember(Name = "requesterInvitationResponseStatus", IsRequired = false)]
        public string RequesterInvitationResponseStatus { get; set; }

        /// <summary>
        /// Gets or sets the requesterComments
        /// </summary>
        [DataMember(Name = "requesterComments", IsRequired = false)]
        public string RequesterComments { get; set; }

        /// <summary>
        /// Gets or sets primary email recipients - in the 'to' line
        /// </summary>
        [DataMember(Name = "primaryEmailRecipients")]
        public List<string> PrimaryEmailRecipients { get; set; }

        /// <summary>
        /// Gets or sets Cc actual email address List- client will fill this for scheduling service.
        /// </summary>
        [DataMember(Name = "ccEmailAddressList", IsRequired = false)]
        public List<string> CcEmailAddressList { get; set; }

        /// <summary>
        /// Gets or sets Bcc actual email address List- client will fill this for scheduling service.
        /// </summary>
        [DataMember(Name = "bccEmailAddressList", IsRequired = false)]
        public List<string> BccEmailAddressList { get; set; }

        /// <summary>
        /// Gets or sets primary email recipients - in the 'to' line
        /// </summary>
        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets  Email Content
        /// </summary>
        [DataMember(Name = "emailContent")]
        public string EmailContent { get; set; }

        /// <summary>
        /// Gets or sets attachment files of the email
        /// </summary>
        [DataMember(Name = "emailAttachmentFiles", IsRequired = false)]
        public IFormFileCollection EmailAttachmentFiles { get; set; }

        /// <summary>
        /// Gets or sets attachment fileLabels of the email
        /// </summary>
        [DataMember(Name = "emailAttachmentFileLabels", IsRequired = false)]
        public IEnumerable<string> EmailAttachmentFileLabels { get; set; }

        /// <summary>
        /// Gets or sets Interview Date
        /// </summary>
        [DataMember(Name = "interviewDate", IsRequired = false)]
        public string InterviewDate { get; set; }

        /// <summary>
        /// Gets or sets Interview Start time
        /// </summary>
        [DataMember(Name = "startTime", IsRequired = false)]
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets Interview End Time
        /// </summary>
        [DataMember(Name = "endTime", IsRequired = false)]
        public string EndTime { get; set; }

        /// <summary>
        /// Gets or sets Interview Location
        /// </summary>
        [DataMember(Name = "location", IsRequired = false)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets Skype Meeting Url to join interview
        /// </summary>
        [DataMember(Name = "skypeMeetingUrl", IsRequired = false)]
        public string SkypeMeetingUrl { get; set; }

        /// <summary>
        /// Gets or sets Interview Title
        /// </summary>
        [DataMember(Name = "interviewTitle", IsRequired = false)]
        public string InterviewTitle { get; set; }

        /// <summary>
        /// Gets or sets SchedulId
        /// </summary>
        [DataMember(Name = "ScheduleId", IsRequired = false)]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets IsInterviewTitleShared
        /// </summary>
        [DataMember(Name = "IsInterviewTitleShared", IsRequired = false)]
        public bool IsInterviewTitleShared { get; set; }

        /// <summary>
        /// Gets or sets IsInterviewScheduleShared
        /// </summary>
        [DataMember(Name = "IsInterviewScheduleShared", IsRequired = false)]
        public bool IsInterviewScheduleShared { get; set; }

        /// <summary>
        /// Gets or sets IsInterviewerNameShared
        /// </summary>
        [DataMember(Name = "IsInterviewerNameShared", IsRequired = false)]
        public bool IsInterviewerNameShared { get; set; }
    }
}
