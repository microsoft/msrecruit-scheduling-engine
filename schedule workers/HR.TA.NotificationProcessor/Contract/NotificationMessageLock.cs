//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.NotificationProcessor.Contract
{
    using System;
    using System.Runtime.Serialization;

    public class NotificationMessageLock
    {
        [DataMember(Name = "serviceBusMessageId", EmitDefaultValue = true, IsRequired = true)]
        public string ServiceBusMessageId { get; set; }

        [DataMember(Name = "lockedTime", EmitDefaultValue = true, IsRequired = true)]
        public DateTime LockedTime { get; set; }
    }
}