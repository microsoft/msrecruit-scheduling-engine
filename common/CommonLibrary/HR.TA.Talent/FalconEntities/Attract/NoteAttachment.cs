//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Attract
{
    using System.Runtime.Serialization;
    using HR.TA..TalentEntities.Enum;

    [DataContract]
    public class NoteAttachment
    {
        [DataMember(Name = "AttachmentID", EmitDefaultValue = false, IsRequired = false)]
        public string AttachmentID { get; set; }

        [DataMember(Name = "CandidateNoteID", EmitDefaultValue = false, IsRequired = false)]
        public string CandidateNoteID { get; set; }

        [DataMember(Name = "FileName", EmitDefaultValue = false, IsRequired = false)]
        public string FileName { get; set; }

        [DataMember(Name = "ArtifactType", EmitDefaultValue = false, IsRequired = false)]
        public ArtifactType ArtifactType { get; set; }

        [DataMember(Name = "NoteUri", EmitDefaultValue = false, IsRequired = false)]
        public string NoteUri { get; set; }
    }
}
