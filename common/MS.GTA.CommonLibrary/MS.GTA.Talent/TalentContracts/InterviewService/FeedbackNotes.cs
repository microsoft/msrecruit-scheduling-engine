// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="FeedbackNotes.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace MS.GTA.Talent.TalentContracts.InterviewService
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>The feedback notes.</summary>
    [DataContract]
    public class FeedbackNotes
    {
        /// <summary>Gets or sets the office identifier of submitter of the notes.</summary>
        [DataMember(Name = "submittedByOID")]
        public string SubmittedByOID { get; set; }

        /// <summary>Gets or sets the name of submitter of the notes.</summary>
        [DataMember(Name = "submittedByName")]
        public string SubmittedByName { get; set; }

        /// <summary>Gets or sets the office identifier of interviewer associated to the notes.</summary>
        [DataMember(Name = "participantOID")]
        public string ParticipantOID { get; set; }

        /// <summary>Gets or sets the name of interviewer associated to the notes.</summary>
        [DataMember(Name = "participantName")]
        public string ParticipantName { get; set; }

        /// <summary>Gets or sets the date when notes was submitted.</summary>
        [DataMember(Name = "submittedDateTime")]
        public DateTime? SubmittedDateTime { get; set; }

        /// <summary>Gets or sets the notes.</summary>
        [DataMember(Name = "notes")]
        public string Notes { get; set; }
    }
}
