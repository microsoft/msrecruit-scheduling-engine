//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum JobApplicationStageStatusReason
    {
        [EnumMember(Value = "open")]
        Open = 0,
        [EnumMember(Value = "complete")]
        Complete = 1,
        [EnumMember(Value = "didNotPass")]
        DidNotPass = 2
    }
}
