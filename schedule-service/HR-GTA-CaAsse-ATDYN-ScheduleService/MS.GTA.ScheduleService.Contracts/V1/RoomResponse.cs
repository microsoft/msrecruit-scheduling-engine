// <copyright file="RoomResponse.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.ScheduleService.Contracts.V1;
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
