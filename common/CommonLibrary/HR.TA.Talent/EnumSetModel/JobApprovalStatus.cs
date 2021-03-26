//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobApprovalStatus
    {
        [EnumMember(Value = "notStarted")]
        NotStarted = 0,
        [EnumMember(Value = "approved")]
        Approved = 1,
        [EnumMember(Value = "rejected")]
        Rejected = 2,
    }
}
