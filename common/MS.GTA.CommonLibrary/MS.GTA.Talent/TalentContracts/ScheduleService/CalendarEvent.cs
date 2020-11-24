// <copyright file="CalendarEvent.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// An outlook calendar event
    /// </summary>
    [DataContract]
    public class CalendarEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEvent"/> class.
        /// </summary>
        public CalendarEvent()
        {
        }

        /// <summary>
        /// Gets or sets the open data protocol entity tag
        /// </summary>
        [DataMember(Name = "oDataEtag", IsRequired = false, EmitDefaultValue = false)]
        public string ODataEtag { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the transaction Id
        /// </summary>
        [DataMember(Name = "TransactionId", IsRequired = false, EmitDefaultValue = false)]
        public Guid? TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the created date time
        /// </summary>
        [DataMember(Name = "createdDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the last modified date time
        /// </summary>
        [DataMember(Name = "lastModifiedDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? LastModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the change key
        /// </summary>
        [DataMember(Name = "changeKey", IsRequired = false, EmitDefaultValue = false)]
        public string ChangeKey { get; set; }

        /// <summary>
        /// Gets or sets the array of categories objects
        /// </summary>
        [DataMember(Name = "categories", IsRequired = false, EmitDefaultValue = false)]
        public List<string> Categories { get; set; }

        /// <summary>
        /// Gets or sets the original start time zone
        /// </summary>
        [DataMember(Name = "originalStartTimeZone", IsRequired = false, EmitDefaultValue = false)]
        public string OriginalStartTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the original end time zone
        /// </summary>
        [DataMember(Name = "originalEndTimeZone", IsRequired = false, EmitDefaultValue = false)]
        public string OriginalEndTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the response status
        /// </summary>
        [DataMember(Name = "responseStatus", IsRequired = false, EmitDefaultValue = false)]
        public ResponseStatus ResponseStatus { get; set; }

        /// <summary>
        /// Gets or sets the ICal unique id
        /// </summary>
        [DataMember(Name = "iCalUId", IsRequired = false, EmitDefaultValue = false)]
        public string ICalUId { get; set; }

        /// <summary>
        /// Gets or sets the reminder minutes before start
        /// </summary>
        [DataMember(Name = "reminderMinutesBeforeStart", IsRequired = false, EmitDefaultValue = false)]
        public int ReminderMinutesBeforeStart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the reminder is on
        /// </summary>
        [DataMember(Name = "isReminderOn", IsRequired = false, EmitDefaultValue = false)]
        public bool IsReminderOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event has attachments
        /// </summary>
        [DataMember(Name = "hasAttachments", IsRequired = false, EmitDefaultValue = false)]
        public bool HasAttachments { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        [DataMember(Name = "subject", IsRequired = false, EmitDefaultValue = false)]
        public object Subject { get; set; }

        /// <summary>
        /// Gets or sets the body
        /// </summary>
        [DataMember(Name = "body", IsRequired = false, EmitDefaultValue = false)]
        public CalendarBody Body { get; set; }

        /// <summary>
        /// Gets or sets the body preview
        /// </summary>
        [DataMember(Name = "bodyPreview", IsRequired = false, EmitDefaultValue = false)]
        public string BodyPreview { get; set; }

        /// <summary>
        /// Gets or sets the importance
        /// </summary>
        [DataMember(Name = "importance", IsRequired = false, EmitDefaultValue = false)]
        public string Importance { get; set; }

        /// <summary>
        /// Gets or sets the sensitivity
        /// </summary>
        [DataMember(Name = "sensitivity", IsRequired = false, EmitDefaultValue = false)]
        public string Sensitivity { get; set; }

        /// <summary>
        /// Gets or sets the start
        /// </summary>
        [DataMember(Name = "start", IsRequired = false, EmitDefaultValue = false)]
        public MeetingDateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end
        /// </summary>
        [DataMember(Name = "end", IsRequired = false, EmitDefaultValue = false)]
        public MeetingDateTime End { get; set; }

        /// <summary>
        /// Gets or sets the location
        /// </summary>
        [DataMember(Name = "location", IsRequired = false, EmitDefaultValue = false)]
        public MeetingLocation Location { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is all day
        /// </summary>
        [DataMember(Name = "isAllDay", IsRequired = false, EmitDefaultValue = false)]
        public bool IsAllDay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is cancelled
        /// </summary>
        [DataMember(Name = "isCancelled", IsRequired = false, EmitDefaultValue = false)]
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attendee the is organizer
        /// </summary>
        [DataMember(Name = "isOrganizer", IsRequired = false, EmitDefaultValue = false)]
        public bool IsOrganizer { get; set; }

        /// <summary>
        /// Gets or sets the recurrence
        /// </summary>
        [DataMember(Name = "recurrence", IsRequired = false, EmitDefaultValue = false)]
        public object Recurrence { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a response is requested
        /// </summary>
        [DataMember(Name = "responseRequested", IsRequired = false, EmitDefaultValue = false)]
        public bool ResponseRequested { get; set; }

        /// <summary>
        /// Gets or sets the series master id
        /// </summary>
        [DataMember(Name = "seriesMasterId", IsRequired = false, EmitDefaultValue = false)]
        public object SeriesMasterId { get; set; }

        /// <summary>
        /// Gets or sets the show as
        /// </summary>
        [DataMember(Name = "showAs", IsRequired = false, EmitDefaultValue = false)]
        public string ShowAs { get; set; }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        [DataMember(Name = "type", IsRequired = false, EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the attendees
        /// </summary>
        [DataMember(Name = "attendees", IsRequired = false, EmitDefaultValue = false)]
        public List<MeetingAttendee> Attendees { get; set; }

        /// <summary>
        /// Gets or sets the organizer
        /// </summary>
        [DataMember(Name = "organizer", IsRequired = false, EmitDefaultValue = false)]
        public Organizer Organizer { get; set; }

        /// <summary>
        /// Gets or sets the web link
        /// </summary>
        [DataMember(Name = "webLink", IsRequired = false, EmitDefaultValue = false)]
        public string WebLink { get; set; }

        /// <summary>
        /// Gets or sets the online meeting
        /// </summary>
        [DataMember(Name = "onlineMeeting", IsRequired = false, EmitDefaultValue = false)]
        public OnlineMeeting OnlineMeeting { get; set; }

        /// <summary>
        /// Gets or sets the SingleValueLegacyExtendedProperty
        /// </summary>
        [DataMember(Name = "singleValueExtendedProperties", IsRequired = false, EmitDefaultValue = false)]
        public List<SingleValueLegacyExtendedProperty> SingleValueExtendedProperties { get; set; }

        /// <summary>
        /// Gets or sets the open type extension list
        /// </summary>
        [DataMember(Name = "extensions", IsRequired = false, EmitDefaultValue = false)]
        public List<OpenTypeExtension> Extensions { get; set; }

        /// <summary>
        /// Gets or sets the sent date time
        /// </summary>
        [DataMember(Name = "sentDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? SentDateTime { get; set; }

        /// <summary>
        /// Gets or sets the onlinemeeting presence
        /// </summary>
        [DataMember(Name = "isOnlineMeeting", IsRequired = false, EmitDefaultValue = false)]
        public bool IsOnlineMeeting { get; set; }

        /// <summary>
        /// Gets or sets the online meeting URL
        /// </summary>
        [DataMember(Name = "onlineMeetingProvider", IsRequired = false, EmitDefaultValue = false)]
        public string OnlineMeetingProvider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow new time proposals for the event.
        /// </summary>
        /// <value>
        ///   <c>true</c> if new time proposals are allowed for the event; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "allowNewTimeProposals", IsRequired = false, EmitDefaultValue = false)]
        public bool AllowNewTimeProposals { get; set; }
    }

    /// <summary>
    /// A Single Value Legacy Extended Property
    /// </summary>
    [DataContract]
    public class SingleValueLegacyExtendedProperty
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [DataMember(Name = "value", IsRequired = false, EmitDefaultValue = false)]
        public string Value { get; set; }
    }
}
