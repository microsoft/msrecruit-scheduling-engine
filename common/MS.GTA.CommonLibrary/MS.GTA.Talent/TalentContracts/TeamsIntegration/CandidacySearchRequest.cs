//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidacySearchRequest.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Talent.TalentContracts.TeamsIntegration
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Azure Search Request
    /// </summary>
    [DataContract]
    public class CandidacySearchRequest
    {
        /// <summary>
        /// Gets or sets QueryType :Full for Lucene and simple For Simple Query Language.
        /// </summary>
        [DataMember(Name = "RequisitionFilter", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> RequisitionFilter { get; set; }

        /// <summary>
        /// Gets or sets QueryType :Full for Lucene and simple For Simple Query Language
        /// </summary>
        [DataMember(Name = "StageFilter", IsRequired = false, EmitDefaultValue = false)]
        public CandidacyStage? StageFilter { get; set; }

        /// <summary>
        /// Gets or sets QueryType :Full for Lucene and simple For Simple Query Language
        /// </summary>
        [DataMember(Name = "SearchText", IsRequired = false, EmitDefaultValue = false)]
        public string SearchText { get; set; }

        /// <summary>
        /// Gets or sets skip
        /// </summary>
        [DataMember(Name = "Skip", IsRequired = false, EmitDefaultValue = false)]
        public int? Skip { get; set; }

        /// <summary>
        ///  Gets or sets Take
        /// </summary>
        [DataMember(Name = "Take", IsRequired = false, EmitDefaultValue = false)]
        public int? Take { get; set; }
    }
}
