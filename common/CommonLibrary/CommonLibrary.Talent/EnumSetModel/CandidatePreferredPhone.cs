//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
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
