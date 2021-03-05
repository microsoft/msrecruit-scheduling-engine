//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace ScheduleService.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;
    using Talent.EnumSetModel.SchedulingService;
    using TalentEntities.Enum;

    /// <summary>
    /// Meeting attendee
    /// </summary>
    [DataContract]
    public class Attendee
    {
        /// <summary>
        /// Gets or sets the list of users
        /// </summary>
        [DataMember(Name = "user")]
        public GraphPerson User { get; set; }

        /// <summary>
        /// Gets or sets the response status
        /// </summary>
        [DataMember(Name = "responseStatus", IsRequired = false, EmitDefaultValue = true)]
        public InvitationResponseStatus ResponseStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the flag indicates if the response status is invalid
        /// </summary>
        [DataMember(Name = "isResponseStatusInvalid", IsRequired = false, EmitDefaultValue = true)]
        public bool IsResponseStatusInvalid { get; set; }

        /// <summary>
        /// Gets or sets the response comment
        /// </summary>
        [DataMember(Name = "responseComment", IsRequired = false, EmitDefaultValue = false)]
        public string ResponseComment { get; set; }

        /// <summary>
        /// Gets or sets the response date time
        /// </summary>
        [DataMember(Name = "responseDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime ResponseDateTime { get; set; }

        /// <summary>
        /// Gets or sets the FreeBusyStatus
        /// </summary>
        [DataMember(Name = "freeBusyStatus", IsRequired = false, EmitDefaultValue = true)]
        public FreeBusyScheduleStatus FreeBusyStatus { get; set; }
    }
}
