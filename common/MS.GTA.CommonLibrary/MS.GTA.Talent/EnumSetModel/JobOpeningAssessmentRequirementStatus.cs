//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobOpeningAssessmentRequirementStatus
    {
        [EnumMember(Value = "notRequired")]
        NotRequired = 0,
        [EnumMember(Value = "required")]
        Required = 1
    }
}
