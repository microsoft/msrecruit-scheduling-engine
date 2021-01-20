// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="SearchMetadataResponse.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SearchMetadataResponse
    {
        [DataMember(Name = "result", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<object> Result { get; set; }

        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }

        [DataMember(Name = "continuationToken", IsRequired = false, EmitDefaultValue = false)]
        public string ContinuationToken { get; set; }
    }
}
