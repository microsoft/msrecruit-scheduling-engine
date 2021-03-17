//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Talent.TalentContracts.InterviewService
{
    using CommonLibrary.Common.TalentAttract.Contract;
    using CommonLibrary.Talent.FalconEntities.Attract;
    using CommonLibrary.TalentEntities.Enum;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Feedback data contract.
    /// </summary>

    [DataContract]
    public class FeedbackSummary
    {
        /// <summary>Gets or sets Feedback Status.</summary>
        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationAssessmentStatus? Status { get; set; }

        /// <summary>Gets or sets Recommendation.</summary>
        [DataMember(Name = "IsRecommendedToContinue", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsRecommendedToContinue { get; set; }

        /// <summary>Gets or sets the SubmittedDate.</summary>
        [DataMember(Name = "SubmittedDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? SubmittedDateTime { get; set; }

        /// <summary>Gets or sets the Interviewer Name.</summary>
        [DataMember(Name = "InterviewerName", EmitDefaultValue = false, IsRequired = false)]
        public string InterviewerName { get; set; }

        /// <summary> Gets or sets the Interviewer Oid/// </summary>
        [DataMember(Name = "OID", EmitDefaultValue = false, IsRequired = false)]
        public string OID { get; set; }

        /// <summary> Gets or sets the Reminder Status/// </summary>
        [DataMember(Name = "RemindApplicable", EmitDefaultValue = false, IsRequired = false)]
        public bool RemindApplicable { get; set; }

        /// <summary> Gets or sets the Schedule Available Status/// </summary>
        [DataMember(Name = "IsScheduleAvailable", EmitDefaultValue = false, IsRequired = false)]
        public bool IsScheduleAvailable { get; set; } 

        /// <summary> Gets or sets the Overall Comment/// </summary>
        [DataMember(Name = "OverallComment", EmitDefaultValue = false, IsRequired = false)]
        public string OverallComment { get; set; }

        /// <summary> Gets or sets the FileAttachment Details/// </summary>
        [DataMember(Name = "FileAttachments", EmitDefaultValue = false, IsRequired = false)]
        public IList<FileAttachmentInfo> FileAttachments { get; set; }

        /// <summary> Gets or sets the Submitted by oid/// </summary>
        [DataMember(Name = "SubmittedByOID", EmitDefaultValue = false, IsRequired = false)]
        public string SubmittedByOID { get; set; }

        /// <summary> Gets or sets the Submitted by name/// </summary>
        [DataMember(Name = "SubmittedByName", EmitDefaultValue = false, IsRequired = false)]
        public string SubmittedByName { get; set; }

        /// <summary> Gets or sets the Notes provided by Submitter</summary>
        [DataMember(Name = "Notes", EmitDefaultValue = false, IsRequired = false)]
        public string Notes { get; set; }

        /// <summary> Gets or sets the Submitted by oid for the Notes </summary>
        [DataMember(Name = "NotesSubmittedByOID", EmitDefaultValue = false, IsRequired = false)]
        public string NotesSubmittedByOID { get; set; }

        /// <summary> Gets or sets the Submitted by name for the notes.</summary>
        [DataMember(Name = "NotesSubmittedByName", EmitDefaultValue = false, IsRequired = false)]
        public string NotesSubmittedByName { get; set; }

        /// <summary>Gets or sets the SubmittedDate for the notes.</summary>
        [DataMember(Name = "NotesSubmittedDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? NotesSubmittedDateTime { get; set; }

    }
}
