//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.DocumentDB.Contracts;

    /// <summary>
    /// Send invitation lock
    /// </summary>
    [DataContract]
    public class SendInvitationLock : DocDbEntity
    {
        /// <summary>
        /// Gets or sets the scheduleId
        /// </summary>
        [DataMember(Name = "ScheduleId", IsRequired = false)]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets the LockedTime
        /// </summary>
        [DataMember(Name = "LockedTime", IsRequired = false)]
        public DateTime LockedTime { get; set; }
    }
}
