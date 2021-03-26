//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum JobOpeningAssessmentRequirementStatus
    {
        [EnumMember(Value = "notRequired")]
        NotRequired = 0,
        [EnumMember(Value = "required")]
        Required = 1
    }
}
