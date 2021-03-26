//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;


    /// <summary>
    /// Relevant Prospects
    /// </summary>
    [DataContract]
    public class RelevantProspects
    {
        /// <summary>
        /// Gets or sets the collection of prospects
        /// </summary>
        [DataMember(Name = "prospects", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Applicant> Prospects { get; set; }

        /// <summary>
        /// Gets or sets last updated by
        /// </summary>
        [DataMember(Name = "lastUpdatedBy", IsRequired = false, EmitDefaultValue = false)]
        public DateTime LastUpdatedBy { get; set; }
    }
}
