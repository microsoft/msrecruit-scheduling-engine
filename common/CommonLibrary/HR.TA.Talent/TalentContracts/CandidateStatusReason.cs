//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
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
