//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..Common.OfferManagement.Contracts.Enums.V1;
    using HR.TA..Common.OfferManagement.Contracts.V2;

    /// <summary>
    /// Specifies the Data Contract for Template Detail
    /// </summary>
    [DataContract]
    public class TemplateDetail
    {
        /// <summary>
        /// Gets or sets Template Name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Template ID.
        /// </summary>
        [DataMember(Name = "templateID", IsRequired = false)]
        public string TemplateID { get; set; }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        [DataMember(Name = "status", IsRequired = false)]
        public OfferTemplateStatus Status { get; set; }

        /// <summary>
        /// Gets or sets Status Reason.
        /// </summary>
        [DataMember(Name = "statusReason", IsRequired = false)]
        public OfferTemplateStatusReason StatusReason { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets Shared status.
        /// </summary>
        [DataMember(Name = "isShared", IsRequired = false)]
        public bool IsShared { get; set; }

        /// <summary>
        /// Gets or sets template shared Date Time.
        /// </summary>
        [DataMember(Name = "sharedDateTimeUTC", IsRequired = false)]
        public DateTime? SharedDateTimeUTC { get; set; }

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
        /// Gets or sets Sections
        /// </summary>
        [DataMember(Name = "sections", IsRequired = false)]
        public IList<TemplateSection> Sections { get; set; }

        /// <summary>
        /// Gets or sets list of tokens
        /// </summary>
        [DataMember(Name = "tokens", IsRequired = false)]
        public IList<string> Tokens { get; set; }

        /// <summary>
        /// Gets or sets Template Identifier(Numeric Id).
        /// </summary>
        [DataMember(Name = "templateIdentifier", IsRequired = false)]
        public string TemplateIdentifier { get; set; }

        /// <summary>
        /// Gets or sets Next template verson id
        /// </summary>
        [DataMember(Name = "nextVersionId", IsRequired = false)]
        public string NextVersionId { get; set; }

        /// <summary>
        /// Gets or sets Previous template verson id
        /// </summary>
        [DataMember(Name = "previousVersionId", IsRequired = false)]
        public string PreviousVersionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether offer text is editable
        /// </summary>
        [DataMember(Name = "isOfferTextEditable", IsRequired = false)]
        public bool IsOfferTextEditable { get; set; }

        /// <summary>
        /// Gets or sets Original template verson id
        /// </summary>
        [DataMember(Name = "originalTemplateId", IsRequired = false)]
        public string OriginalTemplateId { get; set; }

        /// <summary>
        /// Gets or sets template package ids where template is referred
        /// </summary>
        [DataMember(Name = "templatePackageIds", IsRequired = false)]
        public List<string> TemplatePackageIds { get; set; }

        /// <summary>
        /// Gets or sets created by
        /// </summary>
        [DataMember(Name = "createdBy", IsRequired = false)]
        public Worker CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets modified by
        /// </summary>
        [DataMember(Name = "modifiedBy", IsRequired = false)]
        public Worker ModifiedBy { get; set; }
    }
}
