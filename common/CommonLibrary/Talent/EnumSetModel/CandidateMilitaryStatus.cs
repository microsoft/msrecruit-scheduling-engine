//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum CandidateMilitaryStatus
    {
        [EnumMember(Value = "notSpecified")]
        NotSpecified = 0,
        [EnumMember(Value = "servingOrServed")]
        ServingOrServed  = 1,
        [EnumMember(Value = "notServed")]
        NotServed = 2,
        [EnumMember(Value = "doNotWantToAnswer")]
        DoNotWantToAnswer = 3
    }
}
