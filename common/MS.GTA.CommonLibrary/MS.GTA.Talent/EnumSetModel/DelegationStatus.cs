//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum defining the statuses for the delegation.
    /// </summary>
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum DelegationStatus
    {
        /// <summary>
        /// To mark the delegation as active
        /// </summary>
        [EnumMember(Value = "active")]
        Active = 0,

        /// <summary>
        /// To mark the delegation as inactive
        /// </summary>
        [EnumMember(Value = "inactive")]
        Inactive = 1
    }
}
