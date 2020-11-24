//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TemplateDetailMetadata.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.OfferManagement.Contracts.Enums.V1;
    using MS.GTA.Common.OfferManagement.Contracts.V2;

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
