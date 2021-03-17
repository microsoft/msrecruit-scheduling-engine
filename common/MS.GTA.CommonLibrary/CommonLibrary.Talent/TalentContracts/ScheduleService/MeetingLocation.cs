//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The meeting location.
    /// </summary>
    [DataContract]
    public class MeetingLocation
    {
        /// <summary>
        /// Gets or sets the location display name.
        /// </summary>
        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the location address.
        /// </summary>
        [DataMember(Name = "address")]
        public MeetingAddress Address { get; set; }
    }
}
