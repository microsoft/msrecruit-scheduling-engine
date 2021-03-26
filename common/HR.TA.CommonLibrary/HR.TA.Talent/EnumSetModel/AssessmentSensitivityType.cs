//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum AssessmentSensitivityType
    {
        [EnumMember(Value = "high")]
        High = 0,
        [EnumMember(Value = "medium")]
        Medium = 1,
        [EnumMember(Value = "low")]
        Low = 2
    }
}
