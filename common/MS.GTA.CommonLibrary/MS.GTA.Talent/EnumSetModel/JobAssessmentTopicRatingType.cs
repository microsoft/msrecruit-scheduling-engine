//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobAssessmentTopicRatingType
    {
        [EnumMember(Value = "yesNo")]
        YesNo = 0,
        [EnumMember(Value = "fiveStar")]
        FiveStar = 1
    }
}
