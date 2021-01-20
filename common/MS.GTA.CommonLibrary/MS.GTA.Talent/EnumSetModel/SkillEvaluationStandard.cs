//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="SkillEvaluationStandards.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Talent.EnumSetModel
{
     using System.Runtime.Serialization;

    [DataContract]
    public enum SkillEvaluationStandard
    {
        [EnumMember(Value = "aboveExpectations")]
        AboveExpectations = 0,
        [EnumMember(Value = "meetsExpectations")]
        MeetsExpectations = 1,
        [EnumMember(Value = "doesNotMeetExpectations")]
        DoesNotMeetExpectations = 2

    }
}
