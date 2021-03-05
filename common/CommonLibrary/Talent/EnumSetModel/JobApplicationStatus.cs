//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
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
