//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
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
