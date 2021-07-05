//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum CandidatePreferredPhone
    {
        [EnumMember(Value = "home")]
        Home = 0,
        [EnumMember(Value = "work")]
        Work = 1,
        [EnumMember(Value = "mobile")]
        Mobile = 2
    }
}
