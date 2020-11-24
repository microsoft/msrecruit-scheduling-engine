// <copyright file="MeetingTimeSlot.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;
    using MS.GTA.ScheduleService.Contracts.V1;

    /// <summary>
    /// The meeting timeslot.
    /// </summary>
    [DataContract]
    public class MeetingTimeSlot
    {
        /// <summary>
        /// Gets or sets the meeting start date and time.
        /// </summary>
        [DataMember(Name = "start")]
        public MeetingDateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the meeting end date and time.
        /// </summary>
        [DataMember(Name = "end")]
        public MeetingDateTime End { get; set; }
    }
}
