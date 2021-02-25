//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2.Gdpr
{
    using MS.GTA.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// The person data contract.
    /// </summary>
    public class JobOfferParticipantDetail
    {
        /// <summary>
        /// Gets or sets Feedback
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// Gets or sets OfferParticipant Role
        /// </summary>
        public OfferParticipantRole? Role { get; set; }

        /// <summary>
        /// Gets or sets LinkedIn Id
        /// </summary>
        public string LinkedInId { get; set; }

        /// <summary>
        /// Gets or sets Twitter Id
        /// </summary>
        public string TwitterId { get; set; }

        /// <summary>
        /// Gets or sets Facebook Id
        /// </summary>
        public string FacebookId { get; set; }

        /// <summary>
        /// Gets or sets Phone number
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
