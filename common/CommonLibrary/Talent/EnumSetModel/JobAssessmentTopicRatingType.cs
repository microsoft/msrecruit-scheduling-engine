//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum JobAssessmentTopicRatingType
    {
        [EnumMember(Value = "yesNo")]
        YesNo = 0,
        [EnumMember(Value = "fiveStar")]
        FiveStar = 1
    }
}
