//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.DocumentDB.Contracts;

    /// <summary>
    /// Notification Content
    /// </summary>
    [DataContract]
    public class NotificationContent
    {
        /// <summary>
        /// Gets or sets notification list from graph.
        /// </summary>
        [DataMember(Name = "value")]
        public List<Notification> Value { get; set; }
    }
}
