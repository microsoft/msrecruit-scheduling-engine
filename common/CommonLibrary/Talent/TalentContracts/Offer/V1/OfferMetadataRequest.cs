//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Get the offer metadata request
    /// </summary>
    [DataContract]
    public class OfferMetadataRequest
    {
        /// <summary>
        /// Gets or sets filter by status
        /// </summary>
        [DataMember(Name = "filterBy", IsRequired = false, EmitDefaultValue = false)]
        public OfferStatus? FilterBy { get; set; }

        /// <summary>
        /// Gets or sets skip
        /// </summary>
        [DataMember(Name = "skip", IsRequired = false, EmitDefaultValue = false)]
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets Take
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
