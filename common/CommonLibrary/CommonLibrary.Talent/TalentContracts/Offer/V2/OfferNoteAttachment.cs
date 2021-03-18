//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.V2
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The Offer note attachment contract.
    /// </summary>
    [DataContract]
    public class OfferNoteAttachment
    {
        /// <summary>Gets or sets AttachmentID</summary>
        [DataMember(Name = "AttachmentID", EmitDefaultValue = false, IsRequired = false)]
        public string AttachmentID { get; set; }

        /// <summary>Gets or sets CandidateNoteID</summary>
        [DataMember(Name = "CandidateNoteID", EmitDefaultValue = false, IsRequired = false)]
        public string CandidateNoteID { get; set; }

        /// <summary>Gets or sets FileName</summary>
        [DataMember(Name = "FileName", EmitDefaultValue = false, IsRequired = false)]
        public string FileName { get; set; }

        /// <summary>Gets or sets ArtifactType</summary>
        [DataMember(Name = "ArtifactType", EmitDefaultValue = false, IsRequired = false)]
        public string ArtifactType { get; set; }

        /// <summary>Gets or sets NoteUri</summary>
        [DataMember(Name = "NoteUri", EmitDefaultValue = false, IsRequired = false)]
        public string NoteUri { get; set; }
    }
}
