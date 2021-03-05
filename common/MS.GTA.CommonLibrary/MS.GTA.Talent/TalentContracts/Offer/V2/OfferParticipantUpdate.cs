//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V2
{
    using System.Runtime.Serialization;
    using Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Offer Participant update model
    /// </summary>
    [DataContract]
    public class OfferParticipantUpdate : AadUser
    {
        /// <summary>
        /// Gets or sets Role of team member
        /// </summary>
        [DataMember(Name = "role")]
        public OfferParticipantRole Role { get; set; }

        /// <summary>
        /// Gets or sets Ordinal of team member
        /// </summary>
        [DataMember(Name = "Ordinal", IsRequired = false, EmitDefaultValue = false)]
        public long? Ordinal { get; set; }
    }
}
