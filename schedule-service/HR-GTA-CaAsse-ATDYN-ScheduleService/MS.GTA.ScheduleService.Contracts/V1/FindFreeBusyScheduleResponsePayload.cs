// <copyright file="FindFreeBusyScheduleResponsePayload.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.Graph;
    using Newtonsoft.Json;

    /// <summary>
    /// The find meeting time request.
    /// </summary>
    public class FindFreeBusyScheduleResponsePayload
    {
        /// <summary>
        /// Gets or sets gets or sets odata context.
        /// </summary>
        [JsonProperty(PropertyName = "context")]
        public string OdataContext { get; set; }

        /// <summary>
        /// Gets or sets list of free busy responses.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public List<FindFreeBusyScheduleResponse> Value { get; set; }
    }
}