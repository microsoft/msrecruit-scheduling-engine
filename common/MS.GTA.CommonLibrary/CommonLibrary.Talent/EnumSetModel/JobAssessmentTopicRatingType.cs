//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum JobAssessmentTopicRatingType
    {
        [EnumMember(Value = "yesNo")]
        YesNo = 0,
        [EnumMember(Value = "fiveStar")]
        FiveStar = 1
    }
}
