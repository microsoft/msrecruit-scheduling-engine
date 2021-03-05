//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TalentEntities.Enum
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
