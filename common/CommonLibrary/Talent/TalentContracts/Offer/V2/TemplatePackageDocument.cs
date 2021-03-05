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
    public class TemplatePackageDocument
    {
        /// <summary>
        /// Gets or sets Template Package ID
        /// </summary>
        [DataMember(Name = "templatepackageID", IsRequired = true)]
        public string TemplatePackageID { get; set; }

        /// <summary>
        /// Gets or sets Template Name.
        /// </summary>
        [DataMember(Name = "templateName", IsRequired = false)]
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets Template ID
        /// </summary>
        [DataMember(Name = "templateID", IsRequired = true)]
        public string TemplateID { get; set; }

        /// <summary>
        /// Gets or sets Template package document ID
        /// </summary>
        [DataMember(Name = "templatePackageDocumentID", IsRequired = true)]
        public string TemplatePackageDocumentID { get; set; }

        /// <summary>
        /// Gets or sets original template package ID
        /// </summary>
        [DataMember(Name = "originalTemplatePackageID", IsRequired = true)]
        public string OriginalTemplatePackageID { get; set; }

        /// <summary>
        /// Gets or sets Original Template ID
        /// </summary>
        [DataMember(Name = "originalTemplateID", IsRequired = true)]
        public string OriginalTemplateID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets IsTemplateRequired.
        /// </summary>
        [DataMember(Name = "isTemplateRequired", IsRequired = true)]
        public bool IsTemplateRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating ordinal
        /// </summary>
        [DataMember(Name = "ordinal", IsRequired = false)]
        public int Ordinal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether offer text is editable
        /// </summary>
        [DataMember(Name = "isOfferTextEditable", IsRequired = false)]
        public bool IsOfferTextEditable { get; set; }

        /// <summary>
        /// Gets or sets a list of tokens which are part of the template referring it
        /// </summary>
        [DataMember(Name = "tokens", IsRequired =false)]
        public IList<TokenValue> Tokens { get; set; }

        /// <summary>
        /// Gets or sets value of template created by
        /// </summary>
        [DataMember(Name = "templateCreatedBy", IsRequired = false)]
        public Worker TemplateCreatedBy { get; set; }

        /// <summary>
        /// Gets or sets value of template modified by
        /// </summary>
        [DataMember(Name = "templateModifiedBy", IsRequired = false)]
        public Worker TemplateModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets value of template created date
        /// </summary>
        [DataMember(Name = "templateCreatedDate", IsRequired = false)]
        public DateTime TemplateCreatedDate { get; set; }

        /// <summary>
        /// Gets or sets value of template modified date
        /// </summary>
        [DataMember(Name = "templateModifiedDate", IsRequired = false)]
        public DateTime TemplateModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets value of template status
        /// </summary>
        [DataMember(Name = "templateStatus", IsRequired = false)]
        public OfferTemplateStatus TemplateStatus { get; set; }

        /// <summary>
        /// Gets or sets value of template status reason
        /// </summary>
        [DataMember(Name = "templateStatusReason", IsRequired = false)]
        public OfferTemplateStatusReason TemplateStatusReason { get; set; }
    }
}
