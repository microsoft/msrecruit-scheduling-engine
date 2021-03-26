//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "HR.TA.TalentEngagement")]
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
