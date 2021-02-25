//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Talent.FalconEntities.Attract
{
    using MS.GTA.Common.DocumentDB.Contracts;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.TalentEntities.Enum;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract]
    public class JobApplicationAssessment : DocDbEntity
    {
        [DataMember(Name = "JobApplicationAssessmentID", EmitDefaultValue = false, IsRequired = false)]
        public string JobApplicationAssessmentID { get; set; }

        [DataMember(Name = "StrengthComment", EmitDefaultValue = false, IsRequired = false)]
        public string StrengthComment { get; set; }

        [DataMember(Name = "WeaknessComment", EmitDefaultValue = false, IsRequired = false)]
        public string WeaknessComment { get; set; }

        [DataMember(Name = "OverallComment", EmitDefaultValue = false, IsRequired = false)]
        public string OverallComment { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationAssessmentStatus? Status { get; set; }

        [DataMember(Name = "StatusReason", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationAssessmentStatusReason? StatusReason { get; set; }

        [DataMember(Name = "IsRecommendedToContinue", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsRecommendedToContinue { get; set; }

        [DataMember(Name = "JobApplication", EmitDefaultValue = false, IsRequired = false)]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "JobApplicationParticipant", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationParticipant JobApplicationParticipant { get; set; }

        [DataMember(Name = "SubmittedDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? SubmittedDateTime { get; set; }

        [DataMember(Name = "FeedbackProvidedBy", EmitDefaultValue = false, IsRequired = false)]
        public string FeedbackProvidedBy { get; set; }

        [DataMember(Name = "FileAttachments", EmitDefaultValue = false, IsRequired = false)]
        public IList<FileAttachmentInfoEntity> FileAttachments { get; set; }

        [DataMember(Name = "Notes", EmitDefaultValue = false, IsRequired = false)]
        public string Notes { get; set; }

        [DataMember(Name = "NotesSubmittedBy", EmitDefaultValue = false, IsRequired = false)]
        public string NotesSubmittedBy { get; set; }

        [DataMember(Name = "NotesSubmittedDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? NotesSubmittedDateTime { get; set; }

    }
}
