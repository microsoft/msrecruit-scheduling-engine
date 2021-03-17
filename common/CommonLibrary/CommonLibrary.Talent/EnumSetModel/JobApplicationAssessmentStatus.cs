//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum JobApplicationAssessmentStatus
    {
        [EnumMember(Value = "notStarted")]
        NotStarted = 0,
        [EnumMember(Value = "inProgress")]
        InProgress = 1,
        [EnumMember(Value = "submitted")]
        Submitted = 2
    }
}
