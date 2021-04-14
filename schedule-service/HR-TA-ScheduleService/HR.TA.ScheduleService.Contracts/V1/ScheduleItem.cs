//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.ScheduleService.Contracts.V1;

    /// <summary>
    /// An outlook calendar event
    /// </summary>
    [DataContract]
    public class ScheduleItem
    {
        /// <summary>
        /// Gets or sets the location where the corresponding event is held or attended from
        /// </summary>
        [DataMember(Name = "location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the availability status of the user or resource during the corresponding event. The possible values are: free, tentative, busy, oof, workingElsewhere, unknown
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the sensitivity of the corresponding event. True if the event is marked private, false otherwise.
        /// </summary>
        [DataMember(Name = "isPrivate")]
        public string IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the corresponding event's subject line
        /// </summary>
        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the date, time, and time zone that the corresponding event starts.
        /// </summary>
        [DataMember(Name = "start")]
        public MeetingDateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the date, time, and time zone that the corresponding event ends.
        /// </summary>
        [DataMember(Name = "end")]
        public MeetingDateTime End { get; set; }
    }
}
