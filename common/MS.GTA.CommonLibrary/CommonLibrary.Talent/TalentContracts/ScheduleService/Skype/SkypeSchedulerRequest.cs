//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;

    /// <summary>
    /// Skype scheduler request
    /// </summary>
    public class SkypeSchedulerRequest
    {
        /// <summary>
        /// Gets or sets the automatic leader assignment
        /// </summary>
        public string AutomaticLeaderAssignment { get; set; }

        /// <summary>
        /// Gets or sets whether the entry exit announcement is "Enabled" or "Disabled"
        /// </summary>
        public string EntryExitAnnouncement { get; set; }

        /// <summary>
        /// Gets or sets the leaders list in "sip:user@domain.com" format
        /// </summary>
        public List<string> Leaders { get; set; }

        /// <summary>
        /// Gets or sets the list of attendees in "sip:user@domain.com" format
        /// </summary>
        public List<string> Attendees { get; set; }

        /// <summary>
        /// Gets or sets whether the lobby bypass for phone users is "Enabled" or "Disabled"
        /// </summary>
        public string LobbyBypassForPhoneUsers { get; set; }

        /// <summary>
        /// Gets or sets the subject of the meeting
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets whether the phone user admission is "Enabled" or "Disabled"
        /// </summary>
        public string PhoneUserAdmission { get; set; }

        /// <summary>
        /// Gets or sets the expiration time
        /// </summary>
        public string ExpirationTime { get; set; }
    }
}
