//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..TalentEntities.Enum;

    /// <summary>
    /// Talent pool Metadata Request
    /// </summary>
    [DataContract]
    public class TalentPoolMetadaRequest
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

        /// <summary>
        /// Talent pool role access filter
        /// </summary>
        [DataMember(Name = "talentPoolRoles", IsRequired = false, EmitDefaultValue = false)]
        public List<TalentPoolParticipantRole> TalentPoolRoles { get; set; }
    }
}
