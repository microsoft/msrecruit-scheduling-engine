//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.V2
{
    using CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;
    using CommonLibrary.Common.TalentEntities.Common;

    /// <summary>
    ///  Opening Participant Contract.
    /// </summary>
    public class OpeningParticipant
    {
        /// <summary>
        /// Opening Participant Id
        /// </summary>
        public string OpeningParticipantID { get; set; }

        /// <summary>
        /// Worker details
        /// </summary>
        public Worker Worker { get; set; }

        /// <summary>
        /// Opening Participant role
        /// </summary>
        public OpeningParticipantRole? Role { get; set; }

        /// <summary>
        /// OID
        /// </summary>
        public string OID { get; set; }

    }
}
