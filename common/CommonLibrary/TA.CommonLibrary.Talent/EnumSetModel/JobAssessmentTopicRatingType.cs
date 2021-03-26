//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum JobAssessmentTopicRatingType
    {
        [EnumMember(Value = "yesNo")]
        YesNo = 0,
        [EnumMember(Value = "fiveStar")]
        FiveStar = 1
    }
}
