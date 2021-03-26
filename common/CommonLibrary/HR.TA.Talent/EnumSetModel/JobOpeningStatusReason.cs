//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA..TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA..TalentEngagement")]
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
