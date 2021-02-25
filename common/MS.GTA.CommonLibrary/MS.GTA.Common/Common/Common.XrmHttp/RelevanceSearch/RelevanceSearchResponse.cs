//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.RelevanceSearch
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class RelevanceSearchResponse
    {
        [DataMember(Name = "value", IsRequired = false, EmitDefaultValue = false)]
        public RelevanceSearchMatch[] Matches { get; set; }

        [DataMember(Name = "facets", IsRequired = false, EmitDefaultValue = false)]
        public Dictionary<string, JToken> Facets { get; set; }

        [DataMember(Name = "warnings", IsRequired = false, EmitDefaultValue = false)]
        public string[] Warnings { get; set; }

        [DataMember(Name = "errors", IsRequired = false, EmitDefaultValue = false)]
        public string[] Errors { get; set; }
    }
}
