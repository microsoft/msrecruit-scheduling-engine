//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="InvitationResponseStatus.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum InvitationResponseStatus
    {
        [EnumMember(Value = "none")]
        None = 0,
        [EnumMember(Value = "accepted")]
        Accepted = 1,
        [EnumMember(Value = "tentativelyAccepted")]
        TentativelyAccepted = 2,
        [EnumMember(Value = "declined")]
        Declined = 3,
        [EnumMember(Value = "pending")]
        Pending = 4,
        [EnumMember(Value = "sending")]
        Sending = 5,
        [EnumMember(Value = "resendRequired")]
        ResendRequired = 6,
    }
}