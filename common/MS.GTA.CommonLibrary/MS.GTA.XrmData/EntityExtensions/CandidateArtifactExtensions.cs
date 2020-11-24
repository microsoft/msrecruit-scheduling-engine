//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateArtifactExtensions.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Data.Utils;
    using MS.GTA.TalentEntities.Enum;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class CandidateArtifactExtensions
    {
        public static ApplicantAttachment ToViewModel(this CandidateArtifact artifact) => artifact == null ? null : new ApplicantAttachment()
        {
            Id = artifact.Autonumber,
            Type = artifact.ArtifactPurpose.GetValueOrDefault(),
            DocumentType = ContentTypeToExtension(artifact.ContentType),
            Name = artifact.ArtifactName,
            Description = artifact.Description,
            Reference = artifact.BlobReference,
            UploadedBy = artifact.XrmCreatedBy.ToViewModel(),
            UploadedDateTime = artifact.XrmCreatedOn,
        };

        public static ApplicantAttachment ToTalentViewModel(this CandidateArtifact artifact) => artifact == null ? null : new ApplicantAttachment()
        {
            Id = artifact.Autonumber,
            Type = artifact.ArtifactPurpose.GetValueOrDefault(),
            DocumentType = ContentTypeToExtension(artifact.ContentType),
            Name = artifact.ArtifactName,
            Description = artifact.Description,
            Reference = artifact.BlobReference,
            UploadedBy = artifact.XrmCreatedBy.ToViewModel(),
            UploadedDateTime = artifact.XrmCreatedOn,
        };

        private static CandidateAttachmentDocumentType ContentTypeToExtension(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return CandidateAttachmentDocumentType.PDF;
            }

            return contentType.ToExtension();
        }
    }
}
