// <copyright file="MeetingAttendeeEmailAddress.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using Microsoft.Graph;
    using System.Runtime.Serialization;

    /// <summary>
    /// The email address.
    /// </summary>
    [DataContract]
    public class OnlineMeeting
    {
        /// <summary>
        /// Gets or sets the joinUrl.
        /// </summary>
        [DataMember(Name = "joinUrl")]
        public string JoinUrl { get; set; }

        /// <summary>
        /// Gets or sets the conferenceId
        /// </summary>
        [DataMember(Name = "conferenceId")]
        public string ConferenceId { get; set; }

        /// <summary>
        /// Gets or sets the tollNumber
        /// </summary>
        [DataMember(Name = "tollNumber")]
        public string TollNumber { get; set; }

        /// <summary>
        /// Gets or sets the tollFreeNumbers
        /// </summary>
        [DataMember(Name = "tollFreeNumbers")]
        public string TollFreeNumbers { get; set; }

        /// <summary>
        /// Gets or sets the phones
        /// </summary>
        [DataMember(Name = "phones")]
        public string Phones { get; set; }

        /// <summary>
        /// Gets or sets the quickDial
        /// </summary>
        [DataMember(Name = "quickDial")]
        public string QuickDial { get; set; }

    }
}
