//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
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
