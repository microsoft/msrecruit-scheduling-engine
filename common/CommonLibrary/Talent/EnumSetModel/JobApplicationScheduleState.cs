//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum JobApplicationScheduleState
    {
        [EnumMember(Value = "notScheduled")]
        NotScheduled = 0,
        [EnumMember(Value = "purposed")]
        Purposed = 1,
        [EnumMember(Value = "invited")]
        Invited = 2,
        [EnumMember(Value = "scheduled")]
        Scheduled = 3
    }
}
