//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Contracts
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SearchMetadataRequest
    {
        [DataMember(Name = "skip", IsRequired = false, EmitDefaultValue = false)]
        public int Skip { get; set; }

        [DataMember(Name = "take", IsRequired = false, EmitDefaultValue = false)]
        public int Take { get; set; }

        [DataMember(Name = "searchText", IsRequired = false, EmitDefaultValue = false)]
        public string SearchText { get; set; }

        [DataMember(Name = "searchFields", IsRequired = false, EmitDefaultValue = false)]
        public string[] SearchFields { get; set; }

        [DataMember(Name = "continuationToken", IsRequired = false, EmitDefaultValue = false)]
        public string ContinuationToken { get; set; }
    }
}
