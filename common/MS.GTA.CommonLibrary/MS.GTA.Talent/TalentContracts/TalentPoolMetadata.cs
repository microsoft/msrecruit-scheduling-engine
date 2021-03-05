//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Talent pool Metadata
    /// </summary>
    [DataContract]
    public class TalentPoolMetadata
    {
        /// <summary>
        /// Gets or sets the collection of talent pools
        /// </summary>
        [DataMember(Name = "talentPools", IsRequired = false, EmitDefaultValue = false)]
        public List<TalentPool> TalentPools { get; set; }

        /// <summary>
        /// Gets or sets total talent pools count
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }
    }
}
