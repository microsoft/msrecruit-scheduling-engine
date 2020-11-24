//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ApplicationNote.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;
    using MS.GTA.Common.Contracts;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The Application note data contract.
    /// </summary>
    [DataContract]
    public class ApplicationNote : TalentBaseContract
    {
        /// <summary>Gets or sets id.</summary>
        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [DataMember(Name = "text", IsRequired = false, EmitDefaultValue = false)]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        [DataMember(Name = "visibility", IsRequired = false, EmitDefaultValue = false)]
        public CandidateNoteVisibility Visibility { get; set; }

        /// <summary>Gets or sets owner object id.</summary>
        [DataMember(Name = "ownerObjectId", IsRequired = false, EmitDefaultValue = false)]
        public string OwnerObjectId { get; set; }

        /// <summary>Gets or sets owner name.</summary>
        [DataMember(Name = "ownerName", IsRequired = false, EmitDefaultValue = false)]
        public string OwnerName { get; set; }

        /// <summary>Gets or sets owner email.</summary>
        [DataMember(Name = "ownerEmail", IsRequired = false, EmitDefaultValue = false)]
        public string OwnerEmail { get; set; }

        /// <summary>Gets or sets id.</summary>
        [DataMember(Name = "createdDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? CreatedDate { get; set; }

        [DataMember(Name = "noteType", EmitDefaultValue = false, IsRequired = false)]
        public CandidateNoteType? NoteType { get; set; }
    }
}
