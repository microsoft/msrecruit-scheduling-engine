//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum AssessmentReportType
    {
        [EnumMember(Value = "unspecified")]
        Unspecified = 0,
        [EnumMember(Value = "backgroundCheck")]
        BackgroundCheck = 1,
        [EnumMember(Value = "skill")]
        Skill = 2,
        [EnumMember(Value = "personality")]
        Personality = 3
    }
}
