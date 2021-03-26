//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Facet Details contract.
    /// </summary>
    [DataContract]
    public class FacetSearchRequest
    {
        /// <summary>
        /// Facet Type
        /// </summary>
        [DataMember(Name = "facet", EmitDefaultValue = false, IsRequired = false)]
        public FacetType Facet { get; set; }

        /// <summary> 
        /// search text
        /// </summary>
        [DataMember(Name = "searchText", EmitDefaultValue = false, IsRequired = false)]
        public string SearchText { get; set; }

        /// <summary>
        /// filter check.
        /// </summary>
        [DataMember(Name = "isFilter", EmitDefaultValue = false, IsRequired = false)]
        public bool IsFilter { get; set; }
    }
}

