//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;
    using HR.TA.ScheduleService.Contracts.V1;

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
