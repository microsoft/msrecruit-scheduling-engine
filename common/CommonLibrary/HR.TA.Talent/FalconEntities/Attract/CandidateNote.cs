//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Attract
{
    using System;
    using System.Runtime.Serialization;
    using HR.TA..Common.TalentEntities.Common;
    using HR.TA..TalentEntities.Enum;

    [DataContract]
    public class CandidateNote
    {

        [DataMember(Name = "CandidateNoteID", EmitDefaultValue = false, IsRequired = false)]
        public string CandidateNoteID { get; set; }

        [DataMember(Name = "OID", EmitDefaultValue = false, IsRequired = false)]
        public string OID { get; set; }

        [DataMember(Name = "Text", EmitDefaultValue = false, IsRequired = false)]
        public string Text { get; set; }

        [DataMember(Name = "Attachment", EmitDefaultValue = false, IsRequired = false)]
        public NoteAttachment Attachment { get; set; }

        [DataMember(Name = "CreatedDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime CreatedDateTime { get; set; }

        [DataMember(Name = "Visibility", EmitDefaultValue = false, IsRequired = false)]
        public CandidateNoteVisibility? Visibility { get; set; }

        [DataMember(Name = "Candidate", EmitDefaultValue = false, IsRequired = false)]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "Owner", EmitDefaultValue = false, IsRequired = false)]
        public Worker Owner { get; set; }

        [DataMember(Name = "JobApplication", EmitDefaultValue = false, IsRequired = false)]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "IsSyncedWithLinkedIn", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsSyncedWithLinkedIn { get; set; }

        [DataMember(Name = "NoteType", EmitDefaultValue = false, IsRequired = false)]
        public CandidateNoteType? NoteType { get; set; }
    }
}
