//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="FileAttachmentInfo.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
using System.Runtime.Serialization;

namespace MS.GTA.Talent.TalentContracts.InterviewService
{
    [DataContract]
    public class FileAttachmentInfo
    {
        [DataMember(Name = "fileId", EmitDefaultValue = false, IsRequired = true)]
        public string FileId { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false, IsRequired = true)]
        public string FileName { get; set; }

        [DataMember(Name = "blobURI", EmitDefaultValue = false, IsRequired = true)]
        public string BlobURI { get; set; }

        [DataMember(Name = "fileLabel", IsRequired = false, EmitDefaultValue = false)]
        public string FileLabel { get; set; }

        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        [DataMember(Name = "documentType", IsRequired = false)]
        public string DocumentType { get; set; }

        [DataMember(Name = "fileSize", IsRequired = false)]
        public long FileSize { get; set; }

    }
}
