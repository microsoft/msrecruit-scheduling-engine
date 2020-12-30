//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ScheduleInvitationDetails.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using MS.GTA.Talent.TalentContracts.InterviewService;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Schedule invitation details data contract
    /// </summary>
    [DataContract]
    public class ScheduleInvitationDetails
    {
        /// <summary>
        /// Gets or sets the requester
        /// </summary>
        [DataMember(Name = "requester", IsRequired = false)]
        public GraphPerson Requester { get; set; }

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
        /// Gets or sets attachments of the email
        /// </summary>
        [DataMember(Name = "emailAttachments", IsRequired = false)]
        public FileAttachmentRequest EmailAttachments { get; set; }

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

        [DataMember(Name = "IsInterviewTitleShared", IsRequired = false)]
        public bool IsInterviewTitleShared { get; set; }

        [DataMember(Name = "IsInterviewScheduleShared", IsRequired = false)]
        public bool IsInterviewScheduleShared { get; set; }

        [DataMember(Name = "IsInterviewerNameShared", IsRequired = false)]
        public bool IsInterviewerNameShared { get; set; }
    }
}
