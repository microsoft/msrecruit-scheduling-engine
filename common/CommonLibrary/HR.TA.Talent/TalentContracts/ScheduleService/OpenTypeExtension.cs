//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Open type extension for Microsoft Graph
    /// </summary>
    [DataContract]
    public class OpenTypeExtension
    {
        /// <summary>
        /// Gets or sets the open data type
        /// </summary>
        [DataMember(Name = "@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// Gets or sets extension name
        /// </summary>
        [DataMember(Name = "extensionName")]
        public string ExtensionName { get; set; }

        /// <summary>
        /// Gets or sets the schedule event id
        /// </summary>
        [DataMember(Name = "scheduleEventId", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleEventId { get; set; }

        /// <summary>
        /// Gets or sets the attendee object Id
        /// </summary>
        [DataMember(Name = "attendeeObjectId", IsRequired = false, EmitDefaultValue = false)]
        public string AttendeeObjectId { get; set; }
    }
}
