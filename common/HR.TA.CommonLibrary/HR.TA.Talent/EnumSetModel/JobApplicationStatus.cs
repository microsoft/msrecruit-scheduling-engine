//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum JobApplicationStatus
    {
        [EnumMember(Value = "active")]
        Active = 0,
        [EnumMember(Value = "offered")]
        Offered = 1,
        [EnumMember(Value = "closed")]
        Closed = 2
    }
}
