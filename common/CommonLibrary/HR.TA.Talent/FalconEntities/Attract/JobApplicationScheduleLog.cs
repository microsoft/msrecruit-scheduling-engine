//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Talent.FalconEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..Common.DocumentDB.Contracts;
    using HR.TA..Common.Provisioning.Entities.FalconEntities.Attract;
    using HR.TA..Common.TalentEntities.Common;
    using HR.TA..TalentEntities.Enum;

    /// <summary>
    /// The <see cref="JobApplicationScheduleLog"/> class represents the audit log entry for every action taken on job application schedule.
    /// </summary>
    /// <seealso cref="DocDbEntity" />
    [DataContract]
    public class JobApplicationScheduleLog : DocDbEntity
    {
        /// <summary>
        /// Gets or sets the job application identifier.
        /// </summary>
        /// <value>
        /// The job application identifier.
        /// </value>
        [DataMember(Name = "JobApplicationID", EmitDefaultValue = false, IsRequired = true)]
        public string JobApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the schedule identifier.
        /// </summary>
        /// <value>
        /// The schedule identifier.
        /// </value>
        [DataMember(Name = "ScheduleID", EmitDefaultValue = false, IsRequired = true)]
        public string ScheduleID { get; set; }

        /// <summary>
        /// Gets or sets the start date time.
        /// </summary>
        /// <value>
        /// The start date time or <c>null</c>.
        /// </value>
        [DataMember(Name = "StartDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the end date time.
        /// </summary>
        /// <value>
        /// The end date time or <c>null</c>.
        /// </value>
        [DataMember(Name = "EndDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the interview schedule participants.
        /// </summary>
        /// <value>
        /// The interview schedule participants.
        /// </value>
        [DataMember(Name = "JobApplicationScheduleParticipants", EmitDefaultValue = false, IsRequired = false)]
        public List<JobApplicationScheduleParticipant> Participants { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [DataMember(Name = "Subject", IsRequired = false, EmitDefaultValue = false)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember(Name = "Description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the calendar event identifier.
        /// </summary>
        /// <value>
        /// The calendar event identifier.
        /// </value>
        [DataMember(Name = "CalendarEventId", IsRequired = false, EmitDefaultValue = false)]
        public string CalendarEventId { get; set; }

        /// <summary>
        /// Gets or sets the schedule requester.
        /// </summary>
        /// <value>
        /// The schedule requester as instance of <see cref="Worker"/>.
        /// </value>
        [DataMember(Name = "ScheduleRequester", EmitDefaultValue = false, IsRequired = false)]
        public Worker ScheduleRequester { get; set; }

        /// <summary>
        /// Gets or sets the name of the schedule timezone.
        /// </summary>
        /// <value>
        /// The name of the schedule timezone.
        /// </value>
        [DataMember(Name = "ScheduleTimezoneName", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleTimezoneName { get; set; }

        /// <summary>
        /// Gets or sets the schedule UTC offset.
        /// </summary>
        /// <value>
        /// The schedule UTC offset.
        /// </value>
        [DataMember(Name = "ScheduleUTCOffset", IsRequired = false, EmitDefaultValue = false)]
        public int ScheduleUTCOffset { get; set; }

        /// <summary>
        /// Gets or sets the schedule UTC offset in hours.
        /// </summary>
        /// <value>
        /// The schedule UTC offset in hours.
        /// </value>
        [DataMember(Name = "ScheduleUTCOffsetInHours", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleUTCOffsetInHours { get; set; }

        /// <summary>
        /// Gets or sets the schedule timezone abbrevation.
        /// </summary>
        /// <value>
        /// The schedule timezone abbrevation.
        /// </value>
        [DataMember(Name = "ScheduleTimezoneAbbr", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleTimezoneAbbr { get; set; }

        /// <summary>
        /// Gets or sets the name of the building.
        /// </summary>
        /// <value>
        /// The name of the building.
        /// </value>
        [DataMember(Name = "BuildingName", EmitDefaultValue = false, IsRequired = false)]
        public string BuildingName { get; set; }

        /// <summary>
        /// Gets or sets the name of the room.
        /// </summary>
        /// <value>
        /// The name of the room.
        /// </value>
        [DataMember(Name = "RoomName", EmitDefaultValue = false, IsRequired = false)]
        public string RoomName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is private meeting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is private meeting; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "IsPrivateMeeting", EmitDefaultValue = false, IsRequired = false)]
        public bool IsPrivateMeeting { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [DataMember(Name = "Location", IsRequired = false, EmitDefaultValue = false)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the onlinemeeting required or not
        /// </summary>
        [DataMember(Name = "OnlineMeetingRequired", IsRequired = false, EmitDefaultValue = false)]
        public bool OnlineMeetingRequired { get; set; }

        /// <summary>
        /// Gets or sets the action carried on the schedule.
        /// </summary>
        [DataMember(Name = "Action", IsRequired = false, EmitDefaultValue = true)]
        public JobApplicationScheduleAction Action { get; set; }
    }
}
