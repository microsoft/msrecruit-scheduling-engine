//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum JobApplicationStageStatus
    {
        [EnumMember(Value = "active")]
        Active = 0,
        [EnumMember(Value = "closed")]
        Closed = 1
    }
}
