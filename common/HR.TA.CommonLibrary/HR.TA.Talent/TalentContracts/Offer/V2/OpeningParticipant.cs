//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V2
{
    using HR.TA.Common.OfferManagement.Contracts.Enums.V1;
    using HR.TA.Common.TalentEntities.Common;

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
