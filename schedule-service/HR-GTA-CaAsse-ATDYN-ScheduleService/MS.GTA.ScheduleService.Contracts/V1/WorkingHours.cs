// <copyright file="WorkingHours.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.Graph;
    using Newtonsoft.Json;

    /// <summary>
    /// Working hours.
    /// </summary>
    public class WorkingHours
    {
        /// <summary>
        /// Gets or sets the days of the week on which the user works.
        /// </summary>
        [JsonProperty(PropertyName = "daysOfWeek")]
        public List<string> DaysOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the time of the day that the user starts working.
        /// </summary>
        [JsonProperty(PropertyName = "startTime")]
        public TimeOfDay StartTime { get; set; }

        /// <summary>
        /// Gets or sets the time of the day that the user ends working.
        /// </summary>
        [JsonProperty(PropertyName = "endTime")]
        public TimeOfDay EndTime { get; set; }

        /// <summary>
        /// Gets or sets the time zone to which the working hours apply.
        /// </summary>
        [JsonProperty(PropertyName = "timeZone")]
        public TimeZoneBase TimeZone { get; set; }
    }
}
