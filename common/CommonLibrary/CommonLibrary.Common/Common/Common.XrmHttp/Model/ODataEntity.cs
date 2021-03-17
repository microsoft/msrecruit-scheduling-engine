//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.XrmHttp
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
