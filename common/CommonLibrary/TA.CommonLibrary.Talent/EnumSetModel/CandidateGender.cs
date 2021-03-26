//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum CandidateGender
    {
        [EnumMember(Value = "notSpecified")]
        NotSpecified = 0,
        [EnumMember(Value = "male")]
        Male = 1,
        [EnumMember(Value = "female")]
        Female = 2,
        [EnumMember(Value = "doNotWantToAnswer")]
        DoNotWantToAnswer = 3
    }
}
