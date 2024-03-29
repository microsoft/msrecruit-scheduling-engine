//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum JobApplicationActivityStatus
    {
        [EnumMember(Value = "planned")]
        Planned = 0,
        [EnumMember(Value = "started")]
        Started = 1,
        [EnumMember(Value = "completed")]
        Completed = 2,
        [EnumMember(Value = "cancelled")]
        Cancelled = 3,
        [EnumMember(Value = "skipped")]
        Skipped = 4
    }
}
