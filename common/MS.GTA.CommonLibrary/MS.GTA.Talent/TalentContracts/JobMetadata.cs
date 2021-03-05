//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Job Metadata
    /// </summary>
    [DataContract]
    public class JobMetadata
    {
        /// <summary>
        /// Gets or sets the collection of jobs
        /// </summary>
        [DataMember(Name = "jobs", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Job> Jobs { get; set; }

        /// <summary>
        /// Gets or sets total job count
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets continuation token for skip
        /// </summary>
        [DataMember(Name = "continuationToken", IsRequired = false, EmitDefaultValue = false)]
        public string ContinuationToken { get; set; }
    }
}
