//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum CandidateDisabilityStatus
    {
        [EnumMember(Value = "notSpecified")]
        NotSpecified = 0,
        [EnumMember(Value = "yes")]
        Yes = 1,
        [EnumMember(Value = "no")]
        No = 2,
        [EnumMember(Value = "doNotWantToAnswer")]
        DoNotWantToAnswer = 3
    }
}
