//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.V2
{
    using System.Runtime.Serialization;
    using HR.TA..Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Offer Contributor Update model
    /// </summary>
    [DataContract]
    public class OfferContributorUpdate : AadUser
    {
        /// <summary>
        /// Gets or sets Role of team member
        /// </summary>
        [DataMember(Name = "role")]
        public OfferParticipantRole Role { get; set; }
    }
}
