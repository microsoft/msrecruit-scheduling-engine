//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Provisioning.Entities.FalconEntities.Attract
{
    using System;
    using System.Runtime.Serialization;
    using CommonLibrary.TalentEntities.Enum;

    [DataContract]
    public class CandidateAttachment
    {
        [DataMember(Name = "DocumentType", EmitDefaultValue = false, IsRequired = false)]
        public CandidateAttachmentDocumentType? DocumentType { get; set; }

        [DataMember(Name = "Type", EmitDefaultValue = false, IsRequired = false)]
        public CandidateAttachmentType? Type { get; set; }

        [DataMember(Name = "Name", EmitDefaultValue = false, IsRequired = false)]
        public string Name { get; set; }

        [DataMember(Name = "Description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "BlobResourceUri", EmitDefaultValue = false, IsRequired = false)]
        public string BlobResourceUri { get; set; }

        [DataMember(Name = "UploadedDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime UploadedDateTime { get; set; }
    }
}
