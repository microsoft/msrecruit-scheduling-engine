//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.ScheduleService.Contracts.V1;

    /// <summary>
    /// The meeting location constraints.
    /// </summary>
    [DataContract]
    public class LocationConstraintModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the location is required for the meeting.
        /// </summary>
        [DataMember(Name = "isRequired")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a location should be suggested.
        /// </summary>
        [DataMember(Name = "suggestLocation")]
        public bool SuggestLocation { get; set; }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        [DataMember(Name = "locations")]
        public List<MeetingLocation> Locations { get; set; }
    }
}
