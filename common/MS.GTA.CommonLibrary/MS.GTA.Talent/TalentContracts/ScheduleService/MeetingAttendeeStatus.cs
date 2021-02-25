//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The attendee status
    /// </summary>
    [DataContract]
    public class MeetingAttendeeStatus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingAttendeeStatus"/> class.
        /// </summary>
        public MeetingAttendeeStatus()
        {
            this.Response = "none";

            this.Time = "0001-01-01T00:00:00Z";
        }

        /// <summary>
        /// Gets or sets the attendee response as a string of none, accepted, tentativelyAccepted, or declined
        /// </summary>
        [DataMember(Name = "response")]
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the attended response time
        /// </summary>
        [DataMember(Name = "time")]
        public string Time { get; set; }
    }
}
