//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.FalconEntities.Attract.Conference
{
    using System;
    using System.Runtime.Serialization;
    using HR.TA.Common.DocumentDB.Contracts;
    using HR.TA.Talent.EnumSetModel.SchedulingService;
    using HR.TA.Talent.TalentContracts.ScheduleService.Conferencing;

    /// <summary>
    /// The <see cref="ConferenceMeeting"/> class represents the conference meeting entity.
    /// </summary>
    /// <seealso cref="DocDbEntity" />
    [DataContract]
    public class ConferenceMeeting : DocDbEntity
    {
        /// <summary>
        /// Gets or sets the conference join information.
        /// </summary>
        /// <value>
        /// The conference join information.
        /// </value>
        [DataMember(Name = "joinInformation", EmitDefaultValue = false, IsRequired = false)]
        public string JoinInformation { get; set; }

        /// <summary>
        /// Gets or sets the conference meeting subject.
        /// </summary>
        /// <value>
        /// The conference meeting subject.
        /// </value>
        [DataMember(Name = "subject", EmitDefaultValue = false, IsRequired = false)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the conference organizer oid.
        /// </summary>
        /// <value>
        /// The conference organizer oid.
        /// </value>
        [DataMember(Name = "organizerOid")]
        public string OrganizerOid { get; set; }

        /// <summary>
        /// Gets or sets the conference UTC start time.
        /// </summary>
        /// <value>
        /// The conference UTC start time.
        /// </value>
        [DataMember(Name = "startTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the conference UTC end time.
        /// </summary>
        /// <value>
        /// The conference UTC end time.
        /// </value>
        [DataMember(Name = "endTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the conference meeting provider.
        /// </summary>
        /// <value>
        /// The conference meeting provider.
        /// </value>
        [DataMember(Name = "provider")]
        public ConferenceProvider? Provider { get; set; }

        /// <summary>
        /// Gets or sets the conference meeting join URL.
        /// </summary>
        /// <value>
        /// The conference meeting join URL.
        /// </value>
        [DataMember(Name = "joinUrl", EmitDefaultValue = false, IsRequired = false)]
        public string JoinUrl { get; set; }

        /// <summary>
        /// Gets or sets the conference meeting audio information.
        /// </summary>
        /// <value>
        /// The conference meeting audio information.
        /// </value>
        [DataMember(Name = "audio", EmitDefaultValue = false, IsRequired = false)]
        public AudioInfo Audio { get; set; }

        /// <summary>
        /// Gets or sets whether to automatically admit users.
        /// </summary>
        /// <value>
        /// The automatic admit users.
        /// </value>
        [DataMember(Name = "autoAdmitUsers", EmitDefaultValue = false, IsRequired = false)]
        public string AutoAdmitUsers { get; set; }

        /// <summary>
        /// Gets or sets the job application identifier.
        /// </summary>
        /// <value>
        /// The job application identifier.
        /// </value>
        [DataMember(Name = "jobApplicationId", EmitDefaultValue = false)]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the schedule identifier.
        /// </summary>
        /// <value>
        /// The schedule identifier.
        /// </value>
        [DataMember(Name = "scheduleId", EmitDefaultValue = false)]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ConferenceInfo"/> to <see cref="ConferenceMeeting"/>.
        /// </summary>
        /// <param name="conferenceInfo">The instance of <see cref="ConferenceInfo"/>.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ConferenceMeeting(ConferenceInfo conferenceInfo)
        {
            ConferenceMeeting conferenceMeeting = null;
            if (conferenceInfo != null)
            {
                conferenceMeeting = new ConferenceMeeting
                {
                    AutoAdmitUsers = conferenceInfo.AdmitUsers,
                    Audio = conferenceInfo.Audio,
                    EndTime = conferenceInfo.EndTime,
                    StartTime = conferenceInfo.StartTime,
                    Id = conferenceInfo.Id,
                    JoinInformation = conferenceInfo.JoinInfo,
                    JoinUrl = conferenceInfo.JoinUrl,
                    Provider = conferenceInfo.Provider,
                    Subject = conferenceInfo.Subject,
                    JobApplicationId = conferenceInfo.JobApplicationId,
                    ScheduleId = conferenceInfo.ScheduleId,
                    OrganizerOid = conferenceInfo.OrganizerOid
                };
            }

            return conferenceMeeting;
        }
    }
}
