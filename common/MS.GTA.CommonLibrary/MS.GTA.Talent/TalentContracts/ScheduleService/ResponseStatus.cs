//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The outlook calendar event response status
    /// </summary>
    [DataContract]
    public class ResponseStatus
    {
        /// <summary>
        /// Gets or sets the event response
        /// </summary>
        [DataMember(Name = "response")]
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the event time
        /// </summary>
        [DataMember(Name = "time")]
        public DateTime Time { get; set; }
    }
}
