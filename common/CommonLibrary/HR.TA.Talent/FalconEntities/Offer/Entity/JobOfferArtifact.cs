//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>

namespace HR.TA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferArtifact
    {
        [DataMember(Name = "JobOfferArtifactID")]
        public string JobOfferArtifactID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "FileLabel")]
        public string FileLabel { get; set; }

        [DataMember(Name = "UploadedBy")]
        public JobOfferArtifactUploadedBy UploadedBy { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "ArtifactType")]
        public JobOfferArtifactType ArtifactType { get; set; }

        [DataMember(Name = "DocumentType")]
        public JobOfferArtifactDocumentType DocumentType { get; set; }

        [DataMember(Name = "InternalResourceUri")]
        public string InternalResourceUri { get; set; }
    }
}
