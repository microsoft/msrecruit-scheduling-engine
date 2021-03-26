//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Talent.EnumSetModel
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
