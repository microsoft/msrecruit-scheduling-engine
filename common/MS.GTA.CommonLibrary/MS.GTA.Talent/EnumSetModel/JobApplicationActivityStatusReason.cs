//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobApplicationActivityStatusReason
    {
        [EnumMember(Value = "processCreated")]
        ProcessCreated = 0,
        [EnumMember(Value = "processCompleted")]
        ProcessCompleted = 1,
        [EnumMember(Value = "started")]
        Started = 2,
        [EnumMember(Value = "completed")]
        Completed = 3,
        [EnumMember(Value = "positionFilled")]
        PositionFilled = 4,
        [EnumMember(Value = "positionCancelled")]
        PositionCancelled = 5
    }
}
