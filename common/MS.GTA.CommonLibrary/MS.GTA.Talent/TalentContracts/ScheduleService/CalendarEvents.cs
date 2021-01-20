﻿// <copyright file="CalendarEvents.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Outlook calendar events
    /// </summary>
    [DataContract]
    public class CalendarEvents
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarEvents"/> class.
        /// </summary>
        public CalendarEvents()
        {
        }

        /// <summary>
        /// Gets or sets the odata context
        /// </summary>
        [DataMember(Name = "oDataContext", IsRequired = false, EmitDefaultValue = false)]
        public string ODataContext { get; set; }

        /// <summary>
        /// Gets or sets the calendar events
        /// </summary>
        [DataMember(Name = "value")]
        public List<CalendarEvent> Value { get; set; }
    }
}
