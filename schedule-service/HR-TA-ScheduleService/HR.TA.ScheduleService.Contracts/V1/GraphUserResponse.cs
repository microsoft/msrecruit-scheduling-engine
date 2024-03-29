//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// User Graph Response
    /// </summary>
    public class GraphUserResponse
    {
        /// <summary>
        /// Gets or sets the OData context property.
        /// </summary>
        [JsonProperty(PropertyName = "@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// Gets or sets the User details from graph.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public List<Microsoft.Graph.User> Users { get; set; }
    }
}
