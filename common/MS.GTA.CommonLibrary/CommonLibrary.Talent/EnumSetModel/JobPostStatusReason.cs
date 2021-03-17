//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum JobPostStatusReason
    {
        [EnumMember(Value = "active")]
        Active = 0,
        [EnumMember(Value = "closed")]
        Closed = 1
    }
}
