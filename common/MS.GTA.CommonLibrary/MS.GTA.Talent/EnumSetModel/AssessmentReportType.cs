//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
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
