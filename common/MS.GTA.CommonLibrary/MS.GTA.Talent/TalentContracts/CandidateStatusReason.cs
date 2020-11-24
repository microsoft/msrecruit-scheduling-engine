//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateStatusReason.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum CandidateStatusReason
    {
        Available = 0,
        HappyInPosition = 1,
        Blacklisted = 2,
        CandidateNotInterested = 3
    }
}