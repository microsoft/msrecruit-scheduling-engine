//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="RelevanceSearchRequest.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.RelevanceSearch
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class RelevanceSearchRequest
    {
        [DataMember(Name = "searchterms", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<string> SearchTerms { get; set; }

        [DataMember(Name = "entitytypes", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Type> EntityTypes { get; set; }

        [DataMember(Name = "facets", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<string> Facets { get; set; }

        [DataMember(Name = "filters", IsRequired = false, EmitDefaultValue = false)]
        public Dictionary<string, Dictionary<string, List<string>>> Filters { get; set; }

        [DataMember(Name = "skip", IsRequired = false, EmitDefaultValue = false)]
        public int Skip { get; set; }

        [DataMember(Name = "take", IsRequired = false, EmitDefaultValue = false)]
        public int Take { get; set; }
    }
}
