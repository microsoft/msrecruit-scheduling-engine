//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;
    using HR.TA.TalentEntities.Enum;

    /// <summary>
    /// The ScheduleAttendee contract.
    /// </summary>
    [DataContract]
    public class ScheduleAttendee
    {
        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        [DataMember(Name = "userId", IsRequired = false, EmitDefaultValue = false)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        [DataMember(Name = "userName", IsRequired = false, EmitDefaultValue = false)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets ScheduleEventId
        /// </summary>
        [DataMember(Name = "scheduleEventId", IsRequired = false, EmitDefaultValue = false)]
        public string ScheduleEventId { get; set; }

        /// <summary>
        /// Gets or sets ResponseStatus.
        /// </summary>
        [DataMember(Name = "responseStatus", IsRequired = false, EmitDefaultValue = false)]
        public InvitationResponseStatus ResponseStatus { get; set; }

        /// <summary>
        /// Gets or sets start time.
        /// </summary>
        [DataMember(Name = "startTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime StartTime { get; set; }
    }
}
