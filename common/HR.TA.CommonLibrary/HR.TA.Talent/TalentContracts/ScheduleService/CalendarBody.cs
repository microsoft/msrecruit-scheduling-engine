//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Outlook calendar event body
    /// </summary>
    [DataContract]
    public class CalendarBody
    {
        /// <summary>
        /// Gets or sets the content type
        /// </summary>
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        [DataMember(Name = "content")]
        public string Content { get; set; }
    }
}
