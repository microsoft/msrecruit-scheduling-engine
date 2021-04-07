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
        [DataMember(Name = "ServiceBusMessageId", EmitDefaultValue = true, IsRequired = true)]
        public string ServiceBusMessageId { get; set; }

        [DataMember(Name = "LockedTime", EmitDefaultValue = true, IsRequired = true)]
        public DateTime LockedTime { get; set; }
    }
}