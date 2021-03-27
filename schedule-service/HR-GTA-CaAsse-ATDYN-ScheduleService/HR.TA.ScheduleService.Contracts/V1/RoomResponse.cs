//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.ScheduleService.Contracts.V1;
    using Newtonsoft.Json;

    /// <summary>
    /// Room contract
    /// </summary>
    [DataContract]
    public class RoomResponse
    {
        /// <summary>
     /// Gets or sets the rooms
     /// </summary>
        [JsonProperty(PropertyName = "value")]
        public List<Room> Rooms { get; set; }

        /// <summary>
        /// Gets or sets the odata context
        /// </summary>
        [JsonProperty(PropertyName = "@odata.context")]
        public string Context { get; set; }
    }
}
