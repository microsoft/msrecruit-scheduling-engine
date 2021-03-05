//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp.RelevanceSearch
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [DataContract]
    public class RelevanceSearchMatch
    {
        [DataMember(Name = "@search.score")]
        public float SearchScore { get; set; }

        [DataMember(Name = "@search.highlights")]
        public Dictionary<string, JArray> SearchHighlights { get; set; }

        [DataMember(Name = "@search.entityname")]
        public string EntityName { get; set; }

        [DataMember(Name = "@search.objectid")]
        public string ObjectId { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> RelevanceMatchUnmappedFields { get; set; }

        public List<RelevanceSearchHighlight> GetSearchHighlights()
        {
            var highlights = new List<RelevanceSearchHighlight>();

            foreach (var highlightKeyValuePair in SearchHighlights)
            {
                var hightlight = new RelevanceSearchHighlight
                {
                    EntityType = EntityName,
                    Field = highlightKeyValuePair.Key,
                    ValueWithHighlight = (string)highlightKeyValuePair.Value[0],
                    Id = new Guid(ObjectId),
                };

                highlights.Add(hightlight);
            }

            return highlights;
        }

        public string Id { get; set; }
    }
}
