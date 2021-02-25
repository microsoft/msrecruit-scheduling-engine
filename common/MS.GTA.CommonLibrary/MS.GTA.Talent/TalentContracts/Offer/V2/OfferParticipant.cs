//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2
{
    using System;
    using System.Runtime.Serialization;
    using MS.GTA.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Participant in if an Offer.
    /// </summary>
    [DataContract]
    public class OfferParticipant : Person
    {
        /// <summary>
        /// Gets or sets OfferParticipantID.
        /// </summary>
        [DataMember(Name = "offerParticipantID", IsRequired = true, EmitDefaultValue = false)]
        public string OfferParticipantID { get; set; }

        /// <summary>
        /// Gets or sets role.
        /// </summary>
        [DataMember(Name = "role", IsRequired = true, EmitDefaultValue = false)]
        public OfferParticipantRole? Role { get; set; }

        /// <summary>
        /// Gets or sets Offer Feedback
        /// </summary>
        [DataMember(Name = "feedback", IsRequired = false, EmitDefaultValue = false)]
        public OfferFeedback Feedback { get; set; }

        /// <summary>
        /// Gets or sets Ordinal of team member
        /// </summary>
        [DataMember(Name = "Ordinal", IsRequired = false, EmitDefaultValue = false)]
        public long? Ordinal { get; set; }

        /// <summary>
        /// Gets or sets OfferLastEditedOn of team member
        /// </summary>
        [DataMember(Name = "offerLastEditedOn", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? OfferLastEditedOn { get; set; }
    }
}
