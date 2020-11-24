//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum defining the delegation request status.
    /// </summary>
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum RequestStatus
    {
        /// <summary>
        /// To mark the request for delegtion as submitted.
        /// </summary>
        [EnumMember(Value = "submitted")]
        Submitted = 0,

        /// <summary>
        /// To mark the request for delegtion as accepted.
        /// </summary>
        [EnumMember(Value = "accepted")]
        Accepted = 1,

        /// <summary>
        /// To mark the request for delegtion as active.
        /// </summary>
        [EnumMember(Value = "active")]
        Active = 2,

        /// <summary>
        /// To mark the request for delegtion as rejected.
        /// </summary>
        [EnumMember(Value = "rejected")]
        Rejected = 3,

        /// <summary>
        /// To mark the request for delegtion as ended/expired.
        /// </summary>
        [EnumMember(Value = "ended")]
        Ended = 4,

        /// <summary>
        /// To mark the request for delegtion as revoked.
        /// </summary>
        [EnumMember(Value = "revoked")]
        Revoked = 5
    }
}
