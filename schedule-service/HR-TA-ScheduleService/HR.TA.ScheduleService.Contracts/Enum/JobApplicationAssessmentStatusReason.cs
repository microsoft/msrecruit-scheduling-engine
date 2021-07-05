//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
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
