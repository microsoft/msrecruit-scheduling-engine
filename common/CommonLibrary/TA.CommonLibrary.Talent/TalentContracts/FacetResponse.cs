//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Facet Details contract.
    /// </summary>
    [DataContract]
    public class FacetResponse
    {
        /// <summary>
        /// Facet Type
        /// </summary>
        [DataMember(Name = "filter", EmitDefaultValue = false, IsRequired = false)]
        public FacetType Filter { get; set; }

        /// <summary> 
        /// Facet Details.
        /// </summary>
        [DataMember(Name = "facetDetails", EmitDefaultValue = false, IsRequired = false)]
        public List<FacetDetail> FacetDetails { get; set; }
    }
}
