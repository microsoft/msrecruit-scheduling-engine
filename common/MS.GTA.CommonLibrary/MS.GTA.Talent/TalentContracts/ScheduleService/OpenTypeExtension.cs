// <copyright file="OpenTypeExtension.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

/*
 *  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
 *  See LICENSE in the source repository root for complete license information.
 */

namespace MS.GTA.ScheduleService.Contracts.V1
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
