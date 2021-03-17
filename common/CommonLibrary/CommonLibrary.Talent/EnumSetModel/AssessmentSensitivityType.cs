//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
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
