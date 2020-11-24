﻿// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferDeclineReason.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferDeclineReason
    {
        /// <summary> compensationPackage </summary>
        CompensationPackage = 0,

        /// <summary> notGoodCultureFit </summary>
        NotGoodCultureFit = 1,

        /// <summary> jobTitle </summary>
        JobTitle = 2,

        /// <summary> relocation </summary>
        Relocation = 3,

        /// <summary> withdrawInterest </summary>
        WithdrawInterest = 4,

        /// <summary> acceptedAnotherOffer </summary>
        AcceptedAnotherOffer = 5,

        /// <summary> acceptedAnotherOfferWithYourCompany </summary>
        AcceptedAnotherOfferWithYourCompany = 6,

        /// <summary> other </summary>
        Other = 7,

    }
}
