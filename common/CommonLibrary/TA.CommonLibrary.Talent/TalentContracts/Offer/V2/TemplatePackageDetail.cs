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
    /// Specifies the Data Contract for Template Package Detail
    /// </summary>
    [DataContract]
    public class TemplatePackageDetail
    {
        /// <summary>
        /// Gets or sets Template Package Name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Template Package ID.
        /// </summary>
        [DataMember(Name = "templatePackageID", IsRequired = false)]
        public string TemplatePackageID { get; set; }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        [DataMember(Name = "status", IsRequired = false)]
        public OfferTemplatePackageStatus Status { get; set; }

        /// <summary>
        /// Gets or sets Status Reason.
        /// </summary>
        [DataMember(Name = "statusReason", IsRequired = false)]
        public OfferTemplatePackageStatusReason StatusReason { get; set; }

        /// <summary>
        /// Gets or sets Created Date Time.
        /// </summary>
        [DataMember(Name = "createdDateTimeUTC", IsRequired = false)]
        public DateTime CreatedDateTimeUTC { get; set; }

        /// <summary>
        /// Gets or sets Modified Date Time.
        /// </summary>
        [DataMember(Name = "modifiedDateTimeUTC", IsRequired = false)]
        public DateTime ModifiedDateTimeUTC { get; set; }

        /// <summary>
        /// Gets or sets Next template package version id
        /// </summary>
        public string NextVersionId { get; set; }

        /// <summary>
        /// Gets or sets Previous template package version id
        /// </summary>
        [DataMember(Name = "previousVersionId", IsRequired = false)]
        public string PreviousVersionId { get; set; }

        /// <summary>
        /// Gets or sets Original template package id
        /// </summary>
        [DataMember(Name = "originalTemplatePackageId", IsRequired = false)]
        public string OriginalTemplatePackageId { get; set; }

        /// <summary>
        /// Gets or sets Template Package template details.
        /// </summary>
        [DataMember(Name = "templatePackageDocuments", IsRequired = false)]
        public IList<TemplatePackageDocument> TemplatePackageDocuments { get; set; }

        /// <summary>
        /// Gets or sets the Optional tokens in the Package
        /// </summary>
        [DataMember(Name = "optionalTokens", IsRequired = false)]
        public IList<string> OptionalTokens { get; set; }

        /// <summary>
        /// Gets or sets the Template Package artifacts in the Package
        /// </summary>
        [DataMember(Name = "templatePackageArtifacts", IsRequired = false, EmitDefaultValue = false)]
        public IList<TemplatePackageArtifact> TemplatePackageArtifacts { get; set; }

        /// <summary>
        /// Gets or sets worker details who last modified.
        /// </summary>
        [DataMember(Name = "modifiedBy", IsRequired = false)]
        public Worker ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets worker details who created.
        /// </summary>
        [DataMember(Name = "createdBy", IsRequired = false)]
        public Worker CreatedBy { get; set; }
    }
}
