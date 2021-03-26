//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.V2;

    /// <summary>
    /// Specifies the Data Contract for Template Detail
    /// </summary>
    [DataContract]
    public class TemplateDetailMetadata
    {
        /// <summary>
        /// Gets or sets the collection of Template Details
        /// </summary>
        [DataMember(Name = "templateDetails", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<TemplateDetail> TemplateDetails { get; set; }

        /// <summary>
        /// Gets or sets total template count
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }
    }
}
