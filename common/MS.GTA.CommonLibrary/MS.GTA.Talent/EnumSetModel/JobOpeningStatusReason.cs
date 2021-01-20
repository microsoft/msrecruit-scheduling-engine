//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobOpeningStatusReason
    {
        [EnumMember(Value = "new")]
        New = 0,
        [EnumMember(Value = "reactivated")]
        Reactivated = 1,
        [EnumMember(Value = "filled")]
        Filled = 2,
        [EnumMember(Value = "cancelled")]
        Cancelled = 3
    }
}
