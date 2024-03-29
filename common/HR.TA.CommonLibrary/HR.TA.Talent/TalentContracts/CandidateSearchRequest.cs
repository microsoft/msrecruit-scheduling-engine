﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Facet Details contract.
    /// </summary>
    [DataContract]
    public class CandidateSearchRequest
    {
        /// <summary>
        /// Gets or sets skip
        /// </summary>
        [DataMember(Name = "skip", IsRequired = false, EmitDefaultValue = false)]
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets skip
        /// </summary>
        [DataMember(Name = "take", IsRequired = false, EmitDefaultValue = false)]
        public int Take { get; set; }

        /// <summary>
        /// Facet Search Request
        /// </summary>
        [DataMember(Name = "facetSearchRequest", EmitDefaultValue = false, IsRequired = false)]
        public List<FacetSearchRequest> FacetSearchRequest { get; set; }

    }
}