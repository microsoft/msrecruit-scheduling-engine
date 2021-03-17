//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Talent.TalentContracts.InterviewService
{
    using System.Runtime.Serialization;
    using CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// The interviewer data contract.
    /// </summary>
    [DataContract]
    public class Interviewer
    {
        /// <summary>Gets or sets the candidate email.</summary>
        [DataMember(Name = "PrimaryEmail", EmitDefaultValue = false, IsRequired = false)]
        public string PrimaryEmail { get; set; }

        /// <summary>Gets or sets Given Name.</summary>
        [DataMember(Name = "Name", EmitDefaultValue = false, IsRequired = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the office graph identifier.
        /// </summary>
        [DataMember(Name = "OfficeGraphIdentifier", EmitDefaultValue = false, IsRequired = false)]
        public string OfficeGraphIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the profession of interviewer.
        /// </summary>
        [DataMember(Name = "Profession", EmitDefaultValue = false, IsRequired = false)]
        public string Profession { get; set; }

        /// <summary>
        /// Gets or sets the role of the participant.
        /// </summary>
        [DataMember(Name = "Role", EmitDefaultValue = false, IsRequired = false)]
        public JobParticipantRole? Role { get; set; }

        /// <summary>
        /// Gets or sets the meeting status.
        /// </summary>
        [DataMember(Name = "InterviewerResponseStatus", EmitDefaultValue = false, IsRequired = false)]
        public InvitationResponseStatus? InterviewerResponseStatus { get; set; }

        /// <summary>
        /// Gets or sets the interviewer comments.
        /// </summary>
        [DataMember(Name = "InterviewerComments", EmitDefaultValue = false, IsRequired = false)]
        public string InterviewerComments { get; set; }
    }
}
