//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TalentEntities.Enum
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum defining the statuses for the delegation.
    /// </summary>
    [DataContract(Namespace = "TalentEngagement")]
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
