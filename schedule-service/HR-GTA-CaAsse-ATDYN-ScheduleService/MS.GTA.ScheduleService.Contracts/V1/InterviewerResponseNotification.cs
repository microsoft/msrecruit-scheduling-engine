//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// Interviewer Response Notification
    /// </summary>
    [DataContract]
    public class InterviewerResponseNotification
    {
        /// <summary>
        /// Gets or sets schedule id
        /// </summary>
        [DataMember(Name = "scheduleId")]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets jobapplication id
        /// </summary>
        [DataMember(Name = "jobApplicationId")]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets interviewer oid
        /// </summary>
        [DataMember(Name = "interviewerOid")]
        public string InterviewerOid { get; set; }

        /// <summary>
        /// Gets or sets response status
        /// </summary>
        [DataMember(Name = "responseStatus")]
        public InvitationResponseStatus ResponseStatus { get; set; }

        /// <summary>
        /// Gets or sets the proposed new time for the meeting.
        /// </summary>
        /// <value>
        /// The instance of <see cref="MeetingTimeSpan"/>.
        /// </value>
        public MeetingTimeSpan ProposedNewTime { get; set; }
    }
}
