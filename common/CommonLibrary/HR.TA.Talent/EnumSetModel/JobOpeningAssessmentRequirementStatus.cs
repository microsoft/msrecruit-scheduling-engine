//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum JobOpeningAssessmentRequirementStatus
    {
        [EnumMember(Value = "notRequired")]
        NotRequired = 0,
        [EnumMember(Value = "required")]
        Required = 1
    }
}
