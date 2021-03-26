//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA..TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA..TalentEngagement")]
    public enum AssessmentStatus
    {
        [EnumMember(Value = "notStarted")]
        NotStarted = 0,
        [EnumMember(Value = "started")]
        Started = 1,
        [EnumMember(Value = "needGrading")]
        NeedGrading = 2,
        [EnumMember(Value = "completed")]
        Completed = 3
    }
}
