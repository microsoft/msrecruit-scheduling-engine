//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum CandidateVeteranStatus
    {
        [EnumMember(Value = "notSpecified")]
        NotSpecified = 0,
        [EnumMember(Value = "protectedVeteran")]
        ProtectedVeteran  = 1,
        [EnumMember(Value = "notProtectedVeteran")]
        NotProtectedVeteran = 2,
        [EnumMember(Value = "doNotWantToAnswer")]
        DoNotWantToAnswer = 3
    }
}
