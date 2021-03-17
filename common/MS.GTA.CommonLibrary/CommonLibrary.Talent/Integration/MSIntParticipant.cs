//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Integration.Contracts
{
    using System.Collections.Generic;

    /// <summary>
    /// MSIntParticipant
    /// </summary>
    public class MSIntParticipant : MSIntPerson
    {
        public string OfferParticipantExternalID { get; set; }

        public int? OfferParticipantOrdinal { get; set; }

        public string OfferParticipantRole { get; set; }

        public MSIntFeedback OfferParticipantFeedback { get; set; }
    }
}
