//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V2
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using HR.TA.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// The Template Package Artifact data contract
    /// </summary>
    [DataContract]
    public class TemplatePackageArtifact
    {
        /// <summary>
        /// Gets or sets Template Package Artifact ID of model
        /// </summary>
        [DataMember(Name = "templatePackageArtifactID", IsRequired = true, EmitDefaultValue = false)]
        public string TemplatePackageArtifactID { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets file label
        /// </summary>
        [DataMember(Name = "fileLabel", IsRequired = false, EmitDefaultValue = false)]
        public string FileLabel { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets attachment document type.
        /// </summary>
        [DataMember(Name = "documentType", IsRequired = false, EmitDefaultValue = false)]
        public OfferArtifactDocumentType DocumentType { get; set; }
    }
}