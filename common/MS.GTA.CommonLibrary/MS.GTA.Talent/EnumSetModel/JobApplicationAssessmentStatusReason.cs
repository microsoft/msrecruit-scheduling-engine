//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobApplicationAssessmentStatusReason
    {
        [EnumMember(Value = "notStarted")]
        NotStarted = 0,
        [EnumMember(Value = "inProgress")]
        InProgress = 1,
        [EnumMember(Value = "returned")]
        Returned = 2,
        [EnumMember(Value = "completed")]
        Completed = 3
    }
}
