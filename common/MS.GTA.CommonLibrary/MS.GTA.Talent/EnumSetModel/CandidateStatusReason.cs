//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum CandidateStatusReason
    {
        [EnumMember(Value = "available")]
        Available = 0,
        [EnumMember(Value = "happyInPosition")]
        HappyInPosition = 1,
        [EnumMember(Value = "blacklisted")]
        Blacklisted = 2,
        [EnumMember(Value = "candidateNotInterested")]
        CandidateNotInterested = 3
    }
}
