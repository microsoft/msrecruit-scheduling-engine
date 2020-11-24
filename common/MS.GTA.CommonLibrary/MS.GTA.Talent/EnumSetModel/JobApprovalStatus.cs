// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobApprovalStatus.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
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
