//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "TalentEngagement")]
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
