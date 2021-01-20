// <copyright file="OfferNonStandardReason.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Non Standard Offer Reasons
    /// </summary>
    [DataContract]
    public enum OfferNonStandardReason
    {
        /// <summary>
        /// Salary negotiation
        /// </summary>
        SalaryNegotiation,

        /// <summary>
        /// Benefits negotiation
        /// </summary>
        BenefitsNegotiation,

        /// <summary>
        /// Location negotiation
        /// </summary>
        LocationNegotiation,

        /// <summary>
        /// Candidate information
        /// </summary>
        CandidateInformation,

        /// <summary>
        /// Job information
        /// </summary>
        JobInformation,
    }
}