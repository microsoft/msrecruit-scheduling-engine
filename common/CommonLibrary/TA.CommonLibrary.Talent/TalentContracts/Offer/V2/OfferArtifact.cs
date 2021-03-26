//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.OfferManagement.Contracts.V2
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// The Offer Artifact data contract
    /// </summary>
    [DataContract]
    public class OfferArtifact
    {
        /// <summary>
        /// Gets or sets Offer Artifact ID of model
        /// </summary>
        [DataMember(Name = "offerArtifactID", IsRequired = true, EmitDefaultValue = false)]
        public string OfferArtifactID { get; set; }

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
        /// Gets or sets Artifact Type
        /// </summary>
        [DataMember(Name = "artifactType", IsRequired = false)]
        public OfferArtifactType ArtifactType { get; set; }

        /// <summary>
        /// Gets or sets attachment document type.
        /// </summary>
        [DataMember(Name = "documentType", IsRequired = false)]
        public OfferArtifactDocumentType DocumentType { get; set; }

        /// <summary>
        /// Gets or sets uploaded by.
        /// </summary>
        [DataMember(Name = "uploadedBy", IsRequired = false)]
        public OfferArtifactUploadedBy? UploadedBy { get; set; }

        /// <summary>
        /// Gets or sets internalResourceUri.
        /// </summary>
        [IgnoreDataMember]
        public string InternalResourceUri { get; set; }

        /// <summary>
        /// Gets or sets externalResourceUri.
        /// </summary>
        [IgnoreDataMember]
        public string ExternalResourceUri { get; set; }

        /// <summary>
        /// Gets or sets Created Date Time.
        /// </summary>
        [IgnoreDataMember]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets Contents.
        /// </summary>
        [IgnoreDataMember]
        public Stream Content { get; set; }
    }
}
