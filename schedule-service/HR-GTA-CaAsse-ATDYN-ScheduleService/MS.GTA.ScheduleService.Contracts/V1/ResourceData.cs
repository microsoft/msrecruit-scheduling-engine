//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.Contracts.V1
{
    using Newtonsoft.Json;

    /// <summary>
    /// The resource data
    /// </summary>
    public class ResourceData
    {
        /// <summary>
        /// Gets or sets the id of the resource
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the OData e-tag property.
        /// </summary>
        [JsonProperty(PropertyName = "@odata.etag")]
        public string ODataEtag { get; set; }

        /// <summary>
        /// Gets or sets the OData ID of the resource. This is the same value as the resource property.
        /// </summary>
        [JsonProperty(PropertyName = "@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// Gets or sets the OData type of the resource: "#Microsoft.Graph.Message", "#Microsoft.Graph.Event", or "#Microsoft.Graph.Contact".
        /// </summary>
        [JsonProperty(PropertyName = "@odata.type")]
        public string ODataType { get; set; }
    }
}
