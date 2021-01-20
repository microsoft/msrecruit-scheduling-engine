//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
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
