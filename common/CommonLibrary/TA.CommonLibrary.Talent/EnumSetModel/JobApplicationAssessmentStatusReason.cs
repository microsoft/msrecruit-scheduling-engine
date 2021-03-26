//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
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
