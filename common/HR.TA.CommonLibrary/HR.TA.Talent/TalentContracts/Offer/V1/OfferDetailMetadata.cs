﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for offer metadata response
    /// </summary>
    [DataContract]
    public class OfferDetailMetadata
    {
        /// <summary>
        /// Gets or sets the Offer Metadata
        /// </summary>
        [DataMember(Name = "offerMetadata", IsRequired = false, EmitDefaultValue = false)]
        public OfferMetadata OfferMetadata { get; set; }

        /// <summary>
        /// Gets or sets total offer count
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }
    }
}
