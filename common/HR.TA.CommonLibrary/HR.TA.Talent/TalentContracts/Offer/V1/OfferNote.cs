﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;
    ////using Microsoft.AspNetCore.Http;
    using HR.TA.Common.OfferManagement.Contracts.Enums.V1;
    using HR.TA.Common.OfferManagement.Contracts.V2;

    /// <summary>
    /// The Offer note data contract.
    /// </summary>
    [DataContract]
    public class OfferNote
    {
        /// <summary>Gets or sets id.</summary>
        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>Gets or sets the text./// </summary>
        [DataMember(Name = "text", IsRequired = false, EmitDefaultValue = false)]
        public string Text { get; set; }

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

        /// <summary>Gets or sets Application Note type./// </summary>
        [DataMember(Name = "noteType", EmitDefaultValue = false, IsRequired = false)]
        public ApplicationNoteType? NoteType { get; set; }

        /// <summary>Gets or sets the OfferNote attachment file./// </summary>
        [DataMember(Name = "noteAttachment", EmitDefaultValue = false, IsRequired = false)]
        public OfferNoteAttachment NoteAttachment { get; set; }
    }
}
