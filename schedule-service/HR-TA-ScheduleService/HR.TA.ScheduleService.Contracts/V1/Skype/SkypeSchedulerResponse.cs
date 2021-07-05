//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Skype for business scheduler response
    /// </summary>
    [DataContract]
    public class SkypeSchedulerResponse
    {
        /// <summary>
        /// Gets or sets the Join URL
        /// </summary>
        [DataMember(Name = "joinUrl")]
        public string JoinUrl { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting Id
        /// </summary>
        [DataMember(Name = "onlineMeetingId")]
        public string OnlineMeetingId { get; set; }

        /// <summary>
        /// Gets or sets the Expiration time
        /// </summary>
        [DataMember(Name = "expirationTime")]
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets the Meeting subject
        /// </summary>
        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting leaders
        /// </summary>
        [DataMember(Name = "leaders")]
        public string[] Leaders { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting attendees
        /// </summary>
        [DataMember(Name = "attendees")]
        public string[] Attendees { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting access level
        /// </summary>
        [DataMember(Name = "accessLevel")]
        public string AccessLevel { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting entry exit announcement
        /// </summary>
        [DataMember(Name = "entryExitAnnouncement")]
        public string EntryExitAnnouncement { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting automatic leader assignment
        /// </summary>
        [DataMember(Name = "automaticLeaderAssignment")]
        public string AutomaticLeaderAssignment { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting URI
        /// </summary>
        [DataMember(Name = "onlineMeetingUri")]
        public string OnlineMeetingUri { get; set; }

        /// <summary>
        /// Gets or sets the Organizer URI
        /// </summary>
        [DataMember(Name = "organizerUri")]
        public string OrganizerUri { get; set; }

        /// <summary>
        /// Gets or sets the Phone user admission
        /// </summary>
        [DataMember(Name = "phoneUserAdmission")]
        public string PhoneUserAdmission { get; set; }

        /// <summary>
        /// Gets or sets the Lobby bypass for phone users
        /// </summary>
        [DataMember(Name = "lobbyBypassForPhoneUsers")]
        public string LobbyBypassForPhoneUsers { get; set; }

        /// <summary>
        /// Gets or sets the Entity tag
        /// </summary>
        [DataMember(Name = "etag")]
        public string Etag { get; set; }

        /// <summary>
        /// Gets or sets the Meeting links
        /// </summary>
        [DataMember(Name = "_links")]
        public SkypeLinks Links { get; set; }

        /// <summary>
        /// Gets or sets the Online meeting embedded extensions
        /// </summary>
        [DataMember(Name = "_embedded")]
        public SkypeEmbedded Embedded { get; set; }
    }
}
