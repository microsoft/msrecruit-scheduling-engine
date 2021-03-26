//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Talent.TalentContracts.InterviewService
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Application Note contract.
    /// </summary>
    [DataContract]
    public class IVApplicationNote
    {
        /// <summary>
        /// Gets or sets note id.
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false, IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets note text.
        /// </summary>
        [DataMember(Name = "text", EmitDefaultValue = false, IsRequired = false)]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets owner's oid.
        /// </summary>
        [DataMember(Name = "ownerObjectId", EmitDefaultValue = false, IsRequired = false)]
        public string OwnerObjectId { get; set; }

        /// <summary>
        /// Gets or sets the created date time for the note.
        /// </summary>
        [DataMember(Name = "createdDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets owner's full name.
        /// </summary>
        [DataMember(Name = "ownerFullName", EmitDefaultValue = false, IsRequired = false)]
        public string OwnerFullName { get; set; }
    }
}
