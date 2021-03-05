//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "TalentEngagement")]
    public enum ActivityAudience
    {
        [EnumMember(Value = "HiringTeam")]
        HiringTeam = 0,

        [EnumMember(Value = "InternalCandidates")]
        InternalCandidates = 1,

        [EnumMember(Value = "ExternalCandidates")]
        ExternalCandidates = 2,

        [EnumMember(Value = "AllCandidates")]
        AllCandidates = 3,
    }
}
