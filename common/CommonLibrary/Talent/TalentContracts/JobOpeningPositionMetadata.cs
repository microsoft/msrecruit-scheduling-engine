//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Job opening position metadata
    /// </summary>
    [DataContract]
    public class JobOpeningPositionMetadata
    {
        /// <summary>
        /// Gets or sets the collection of job opening positions
        /// </summary>
        [DataMember(Name = "jobOpeningPositions", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<JobOpeningPosition> JobOpeningPositions { get; set; }

        /// <summary>
        /// Gets or sets total job opening position count
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }


        /// <summary>
        /// Gets or sets search text
        /// </summary>
        [DataMember(Name = "continuationToken", IsRequired = false, EmitDefaultValue = false)]
        public string ContinuationToken { get; set; }

    }
}
