//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for Template
    /// </summary>
    [DataContract]
    public class TemplateSaveResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether the service is successful.
        /// </summary>
        [DataMember(Name = "success", IsRequired = true)]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets New Template ID.
        /// </summary>
        [DataMember(Name = "newTemplateID", IsRequired = false)]
        public string NewTemplateID { get; set; }

        /// <summary>
        /// Gets or sets New Template Name.
        /// </summary>
        [DataMember(Name = "newTemplateName", IsRequired = false)]
        public string NewTemplateName { get; set; }
    }
}
