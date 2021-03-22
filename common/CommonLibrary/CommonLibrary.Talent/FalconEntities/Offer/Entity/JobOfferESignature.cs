//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;
    using CommonLibrary.Common.DocumentDB.Contracts;

    [DataContract]
    public class JobOfferESignature : DocDbEntity
    {
        [DataMember(Name = "JobOfferESignatureId")]
        public string JobOfferESignatureId { get; set; }

        [DataMember(Name = "JobOfferId")]
        public string JobOfferId { get; set; }

        [DataMember(Name = "ExternalDocumentReference")]
        public string ExternalDocumentReference { get; set; }

        [DataMember(Name = "EsignatureType")]
        public ESignatureType? EsignatureType { get; set; }

        [DataMember(Name = "SigningCandidateName")]
        public string SigningCandidateName { get; set; }

        [DataMember(Name = "RecruiterOId")]
        public string RecruiterOId { get; set; }

        [DataMember(Name = "ArtifactType")]
        public string ArtifactType { get; set; }
    }
}