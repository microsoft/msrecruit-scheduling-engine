//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Talent pool candidate list request
    /// </summary>
    [DataContract]
    public class TalentPoolCandidateRequest
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
        /// Gets or sets search text
        /// </summary>
        [DataMember(Name = "searchText", IsRequired = false, EmitDefaultValue = false)]
        public string SearchText { get; set; }

    }
}
