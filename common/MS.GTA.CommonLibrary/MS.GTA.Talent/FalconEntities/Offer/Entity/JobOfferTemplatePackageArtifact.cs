//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;
    using MS.GTA.Common.DocumentDB.Contracts;

    [DataContract]
    public class JobOfferTemplatePackageArtifact : DocDbEntity
    {
        [DataMember(Name = "JobOfferTemplatePackageArtifactID")]
        public string JobOfferTemplatePackageArtifactID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "DocumentType")]
        public JobOfferArtifactDocumentType? DocumentType { get; set; }

        [DataMember(Name = "FileLabel")]
        public string FileLabel { get; set; }

        [DataMember(Name = "InternalResourceUri")]
        public string InternalResourceUri { get; set; }
    }
}
