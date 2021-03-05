//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Talent.TalentContracts.ScheduleService.Conferencing
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using Microsoft.Graph;
    using Talent.EnumSetModel.SchedulingService;
    using Talent.FalconEntities.Attract.Conference;
    using Newtonsoft.Json;

    /// <summary>
    /// The <see cref="ConferenceInfo"/> class holds the online conference information.
    /// </summary>
    [DataContract]
    public class ConferenceInfo
    {
        /// <summary>
        /// Gets or sets the conference meeting identifier.
        /// </summary>
        /// <value>
        /// The conference meeting identifier.
        /// </value>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the conference meeting subject.
        /// </summary>
        /// <value>
        /// The conference meeting subject.
        /// </value>
        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the conference start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the conference end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the conference meeting join URL.
        /// </summary>
        /// <value>
        /// The conference meeting join URL.
        /// </value>
        [DataMember(Name = "joinUrl")]
        public string JoinUrl { get; set; }

        /// <summary>
        /// Gets or sets the conference audio information.
        /// </summary>
        /// <value>
        /// The instance of <see cref="AudioInfo"/>.
        /// </value>
        [DataMember(Name = "audio")]
        public AudioInfo Audio { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the participants are admitted automatically.
        /// </summary>
        /// <value>
        /// The admit users.
        /// </value>
        [DataMember(Name = "admitUsers")]
        public string AdmitUsers { get; set; }

        /// <summary>
        /// Gets or sets the confermence meeting join information.
        /// </summary>
        /// <value>
        /// The conference meeting join information.
        /// </value>
        [DataMember(Name = "joinInfo")]
        public string JoinInfo { get; set; }

        /// <summary>
        /// Gets or sets the conference provider.
        /// </summary>
        /// <value>
        /// The conference provider.
        /// </value>
        [DataMember(Name = "provider")]
        public ConferenceProvider? Provider { get; set; }

        /// <summary>
        /// Gets or sets the job application identifier (non-serializable property).
        /// </summary>
        /// <value>
        /// The job application identifier (non-serializable property).
        /// </value>
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the schedule identifier (non-serializable property).
        /// </summary>
        /// <value>
        /// The schedule identifier ( (non-serializable property).
        /// </value>
        public string ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets the organizer OId (non-serializable property).
        /// </summary>
        /// <value>
        /// The organizer OId (non-serializable property).
        /// </value>
        public string OrganizerOid { get; set; }

        /// <summary>
        /// Performs an explicit conversion from <see cref="OnlineMeeting"/> to <see cref="ConferenceInfo"/>.
        /// </summary>
        /// <param name="onlineMeeting">The instance of <see cref="OnlineMeeting"/>.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator ConferenceInfo(OnlineMeeting onlineMeeting)
        {
            ConferenceInfo conferenceInfo = null;
            string serializedMeeting;
            ConferencePrivate conferencePrivateInfo;
            string html;
            if (onlineMeeting != null)
            {
                serializedMeeting = JsonConvert.SerializeObject(onlineMeeting);
                conferencePrivateInfo = JsonConvert.DeserializeObject<ConferencePrivate>(serializedMeeting);
                html = conferencePrivateInfo.JoinInformation?.Content?.Split(',')[1];
                if (!string.IsNullOrWhiteSpace(html))
                {
                    html = WebUtility.UrlDecode(html);
                }

                conferenceInfo = new ConferenceInfo
                {
                    JoinUrl = onlineMeeting.JoinWebUrl,
                    Id = onlineMeeting.Id,
                    Audio = onlineMeeting.AudioConferencing,
                    AdmitUsers = conferencePrivateInfo.AutoAdmittedUsers,
                    JoinInfo = html,
                    Subject = onlineMeeting.Subject,
                    StartTime = onlineMeeting.StartDateTime?.DateTime,
                    EndTime = onlineMeeting.EndDateTime?.DateTime,
                    Provider = ConferenceProvider.MicrosoftTeams,
                    OrganizerOid = onlineMeeting.Participants?.Organizer?.Identity?.User?.Id
                };
            }

            return conferenceInfo;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ConferenceMeeting"/> to <see cref="ConferenceInfo"/>.
        /// </summary>
        /// <param name="conferenceMeeting">The instance of <see cref="ConferenceMeeting"/>.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ConferenceInfo(ConferenceMeeting conferenceMeeting)
        {
            ConferenceInfo conferenceInfo = null;
            if (conferenceMeeting != null)
            {
                conferenceInfo = new ConferenceInfo
                {
                    JoinUrl = conferenceMeeting.JoinUrl,
                    Id = conferenceMeeting.Id,
                    Audio = conferenceMeeting.Audio,
                    AdmitUsers = conferenceMeeting.AutoAdmitUsers,
                    JoinInfo = conferenceMeeting.JoinInformation,
                    Subject = conferenceMeeting.Subject,
                    StartTime = conferenceMeeting.StartTime,
                    EndTime = conferenceMeeting.StartTime,
                    Provider = conferenceMeeting.Provider,
                    JobApplicationId = conferenceMeeting.JobApplicationId,
                    OrganizerOid = conferenceMeeting.OrganizerOid,
                    ScheduleId = conferenceMeeting.ScheduleId
                };
            }

            return conferenceInfo;
        }

        /// <summary>
        /// The <see cref="ConferencePrivate"/> aids to retrieve join information of the conference.
        /// </summary>
        private class ConferencePrivate
        {
            /// <summary>
            /// Gets or sets whether the users are automatically admitted in conference.
            /// </summary>
            /// <value>
            /// The automatic admitted users.
            /// </value>
            public string AutoAdmittedUsers { get; set; }

            /// <summary>
            /// Gets or sets the conference join information.
            /// </summary>
            /// <value>
            /// The conference join information.
            /// </value>
            public JoinInformationInternal JoinInformation { get; set; }
        }

        /// <summary>
        /// The <see cref="JoinInformationInternal"/> class stores the jin information content details.
        /// </summary>
        private class JoinInformationInternal
        {
            /// <summary>
            /// Gets or sets the content.
            /// </summary>
            /// <value>
            /// The content.
            /// </value>
            public string Content { get; set; }

            /// <summary>
            /// Gets or sets the type of the content.
            /// </summary>
            /// <value>
            /// The type of the content.
            /// </value>
            public string ContentType { get; set; }
        }
    }
}
