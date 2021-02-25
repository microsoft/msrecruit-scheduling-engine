//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;
    [DataContract]
    public enum JobOfferNonStandardReason
    {
        /// <summary> salaryNegotiation </summary>
        SalaryNegotiation = 0,

        /// <summary> benefitsNegotiation </summary>
        BenefitsNegotiation = 1,

        /// <summary> locationNegotiation </summary>
        LocationNegotiation = 2,

        /// <summary> candidateInformation </summary>
        CandidateInformation = 3,

        /// <summary> jobInformation </summary>
        JobInformation = 4,
    }
}