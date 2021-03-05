//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum JobOpeningAssessmentRequirementStatus
    {
        [EnumMember(Value = "notRequired")]
        NotRequired = 0,
        [EnumMember(Value = "required")]
        Required = 1
    }
}
