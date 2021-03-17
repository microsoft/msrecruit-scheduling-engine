//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
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
