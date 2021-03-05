//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum JobPositionStatusReason
    {
        [EnumMember(Value = "open")]
        Open = 0,
        [EnumMember(Value = "closed")]
        Closed = 1
    }
}
