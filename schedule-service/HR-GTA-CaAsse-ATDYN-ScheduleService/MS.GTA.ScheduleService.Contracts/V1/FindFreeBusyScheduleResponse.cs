//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.Graph;
    using Newtonsoft.Json;

    /// <summary>
    /// The find meeting time request.
    /// </summary>
    public class FindFreeBusyScheduleResponse
    {
        /// <summary>
        /// Gets or sets an SMTP address of the user, distribution list, or resource, identifying an instance of scheduleInformation.
        /// </summary>
        [JsonProperty(PropertyName = "scheduleId")]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets represents a merged view of availability of all the items in scheduleItems. The view consists of time slots. Availability during each time slot is indicated with: 0= free, 1= tentative, 2= busy, 3= out of office, 4= working elsewhere.
        /// </summary>
        [JsonProperty(PropertyName = "availabilityView")]
        public string AvailabilityView { get; set; }

        /// <summary>
        /// Gets or sets error information from attempting to get the availability of the user, distribution list, or resource.
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public FreeBusyError Error { get; set; }

        /// <summary>
        /// Gets or sets contains the items that describe the availability of the user or resource.
        /// </summary>
        [JsonProperty(PropertyName = "scheduleItems")]
        public List<ScheduleItem> ScheduleItems { get; set; }

        /// <summary>
        /// Gets or sets the days of the week and hours in a specific time zone that the user works. These are set as part of the user's mailboxSettings.
        /// </summary>
        [JsonProperty(PropertyName = "workingHours")]
        public WorkingHours WorkingHours { get; set; }
    }
}
