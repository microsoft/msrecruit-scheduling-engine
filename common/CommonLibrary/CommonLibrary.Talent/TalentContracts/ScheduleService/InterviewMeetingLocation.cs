//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Location contract
    /// </summary>
    [DataContract]
    public class InterviewMeetingLocation
    {
        /// <summary>
        /// Gets or sets the room list
        /// </summary>
        [DataMember(Name = "roomList")]
        public Room RoomList { get; set; }

        /// <summary>
        /// Gets or sets the room
        /// </summary>
        [DataMember(Name = "room")]
        public Room Room { get; set; }
    }
}
