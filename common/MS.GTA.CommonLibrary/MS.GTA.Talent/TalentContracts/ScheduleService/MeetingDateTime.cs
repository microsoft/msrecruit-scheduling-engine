// <copyright file="MeetingDateTime.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The meeting date and time.
    /// </summary>
    [DataContract]
    public class MeetingDateTime
    {
        /// <summary>
        /// Gets or sets the meeting date and time.
        /// </summary>
        [DataMember(Name = "dateTime")]
        public string DateTime { get; set; }

        /// <summary>
        /// Gets or sets the meeting time zone.
        /// </summary>
        [DataMember(Name = "timeZone")]
        public string TimeZone { get; set; }
    }
}
