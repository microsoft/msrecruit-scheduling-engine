//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;
    using HR.TA.Common.DocumentDB.Contracts;

    /// <summary>
    /// Send invitation lock
    /// </summary>
    [DataContract]
    public class NotificationMessageLock : DocDbEntity
    {
        /// <summary>
        /// Gets or sets the ServiceBusMessageId
        /// </summary>
        [DataMember(Name = "ServiceBusMessageId", EmitDefaultValue = true, IsRequired = true)]
        public string ServiceBusMessageId { get; set; }

        /// <summary>
        /// Gets or sets the LockedTime
        /// </summary>
        [DataMember(Name = "LockedTime", EmitDefaultValue = true, IsRequired = true)]
        public DateTime LockedTime { get; set; }
    }
}
