//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The email address.
    /// </summary>
    [DataContract]
    public class MeetingAttendeeEmailAddress
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [DataMember(Name = "address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the person's object id.
        /// </summary>
        [DataMember(Name = "objectId", IsRequired = false, EmitDefaultValue = false)]
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the person's object id.
        /// </summary>
        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}
