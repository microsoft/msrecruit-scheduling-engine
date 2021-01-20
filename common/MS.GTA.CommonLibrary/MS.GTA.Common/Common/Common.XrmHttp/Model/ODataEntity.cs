//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [DataContract]
    public class ODataEntity
    {
        [DataMember(Name = "@odata.context")]
        public string ODataContext { get; set; }

        [DataMember(Name = "@odata.etag")]
        public string ODataEtag { get; set; }

        [JsonIgnore]
        public int? ODataBatchContentIdReference { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> ODataUnmappedFields { get; set; }
    }
}