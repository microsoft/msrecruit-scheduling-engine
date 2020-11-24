﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateSearchResponse.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Facet Details contract.
    /// </summary>
    [DataContract]
    public class CandidateSearchResponse
    {
        /// <summary>
        /// Canidates
        /// </summary>
        [DataMember(Name = "candidates", EmitDefaultValue = false, IsRequired = false)]
        public List<Applicant> Candidates { get; set; }

        /// <summary>
        /// Facet Response
        /// </summary>
        [DataMember(Name = "filters", EmitDefaultValue = false, IsRequired = false)]
        public List<FacetResponse> Filters { get; set; }

        /// <summary>
        /// Gets or sets total
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }
    }
}
