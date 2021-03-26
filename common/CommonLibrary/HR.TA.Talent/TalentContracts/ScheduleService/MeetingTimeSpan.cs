//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="MeetingTimeSpan"/> class holds the start and end meeting time.
    /// </summary>
    [DataContract]
    public class MeetingTimeSpan
    {
        /// <summary>
        /// Gets or sets the start date and time of the meeting.
        /// </summary>
        /// <value>
        /// The instance of <see cref="MeetingDateTime"/>.
        /// </value>
        [DataMember(Name = "start", IsRequired = true)]
        public MeetingDateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end date and time of the meeting.
        /// </summary>
        /// <value>
        /// The instance of <see cref="MeetingDateTime"/>.
        /// </value>
        [DataMember(Name = "end", IsRequired = true)]
        public MeetingDateTime End { get; set; }
    }
}
