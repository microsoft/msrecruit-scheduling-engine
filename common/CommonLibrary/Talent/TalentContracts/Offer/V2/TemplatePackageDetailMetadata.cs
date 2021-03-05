//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.OfferManagement.Contracts.Enums.V1;
    using Common.OfferManagement.Contracts.V2;

    /// <summary>
    /// Specifies the Data Contract for Template Package Detail
    /// </summary>
    [DataContract]
    public class TemplatePackageDetailMetadata
    {
        /// <summary>
        /// Gets or sets the collection of Template Package Details
        /// </summary>
        [DataMember(Name = "templatePackageDetails", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<TemplatePackageDetail> TemplatePackageDetails { get; set; }

        /// <summary>
        /// Gets or sets total template package count
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }
    }
}
