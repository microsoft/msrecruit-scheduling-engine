//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Talent pool candidate list response
    /// </summary>
    [DataContract]
    public class TalentPoolCandidateResponse
    {
        /// <summary>
        /// Gets or sets the collection of candidates
        /// </summary>
        [DataMember(Name = "candidates", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Applicant> Candidates { get; set; }

        /// <summary>
        /// Gets or sets total candidate count
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }

    }
}
