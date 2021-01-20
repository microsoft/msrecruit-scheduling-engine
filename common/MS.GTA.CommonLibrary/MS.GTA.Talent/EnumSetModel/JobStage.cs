//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobStage
    {
        [EnumMember(Value = "application")]
        Application = 0,
        [EnumMember(Value = "assessment")]
        Assessment = 1,
        [EnumMember(Value = "screening")]
        Screening = 2,
        [EnumMember(Value = "interview")]
        Interview = 3,
        [EnumMember(Value = "offer")]
        Offer = 4,
        [EnumMember(Value = "prospect")]
        Prospect = 5,
    }
}
