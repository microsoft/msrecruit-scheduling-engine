//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Invitation Response Status
    /// </summary>
    [DataContract]
    public enum InvitationResponseStatus
    {
        /// <summary>
        /// None
        /// </summary>
        [EnumMember(Value = "none")]
        None = 0,
        /// <summary>
        /// Accepted
        /// </summary>
        [EnumMember(Value = "accepted")]
        Accepted = 1,
        /// <summary>
        /// Tentatively Accepted
        /// </summary>
        [EnumMember(Value = "tentativelyAccepted")]
        TentativelyAccepted = 2,
        /// <summary>
        /// Declined
        /// </summary>
        [EnumMember(Value = "declined")]
        Declined = 3,
        /// <summary>
        /// Pending
        /// </summary>
        [EnumMember(Value = "pending")]
        Pending = 4,
        /// <summary>
        /// Sending
        /// </summary>
        [EnumMember(Value = "sending")]
        Sending = 5,
        /// <summary>
        /// Resend Required
        /// </summary>
        [EnumMember(Value = "resendRequired")]
        ResendRequired = 6,
    }
}
