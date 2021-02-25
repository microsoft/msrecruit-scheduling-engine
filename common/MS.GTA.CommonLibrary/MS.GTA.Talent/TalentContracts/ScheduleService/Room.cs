//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// Room contract
    /// </summary>
    [DataContract]
    public class Room
    {
        /// <summary>
        /// Gets or sets the room name
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the room address
        /// </summary>
        [DataMember(Name = "address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the response status
        /// </summary>
        [DataMember(Name = "status")]
        public InvitationResponseStatus Status { get; set; }
    }
}
