﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The outlook calendar event organizer
    /// </summary>
    [DataContract]
    public class Organizer
    {
        /// <summary>
        ///  Gets or sets the email address
        /// </summary>
        [DataMember(Name = "emailAddress")]
        public MeetingAttendeeEmailAddress EmailAddress { get; set; }
    }
}