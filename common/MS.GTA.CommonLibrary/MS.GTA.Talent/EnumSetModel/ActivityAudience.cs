//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
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
