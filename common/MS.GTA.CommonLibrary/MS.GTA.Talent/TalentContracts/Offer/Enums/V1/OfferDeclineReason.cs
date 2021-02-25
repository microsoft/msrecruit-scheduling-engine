//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer decline reason
    /// </summary>
    [DataContract]
    public enum OfferDeclineReason
    {
        /// <summary>
        /// Compensation Package
        /// </summary>
        CompensationPackage,

        /// <summary>
        /// Not Good Culture Fit
        /// </summary>
        NotGoodCultureFit,

        /// <summary>
        /// Job Title
        /// </summary>
        JobTitle,

        /// <summary>
        /// Relocation
        /// </summary>
        Relocation,

        /// <summary>
        /// Withdraw Interest
        /// </summary>
        WithdrawInterest,

        /// <summary>
        /// Accepted Another Offer
        /// </summary>
        AcceptedAnotherOffer,

        /// <summary>
        /// Accepted Another Offer With Your Company
        /// </summary>
        AcceptedAnotherOfferWithYourCompany,

        /// <summary>
        /// Other
        /// </summary>
        Other
    }
}
