//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Talent.TalentContracts.Flighting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Flight context type.
    /// </summary>
    [DataContract]
    public enum FlightingContextType
    {
        /// <summary>
        /// Tenant Id.
        /// </summary>
        [EnumMember(Value = "TenantObjectId")]
        TenantObjectId = 0,

        /// <summary>
        /// User Id.
        /// </summary>
        [EnumMember(Value = "UserObjectId")]
        UserObjectId = 1,

        /// <summary>
        /// User Principal number.
        /// </summary>
        [EnumMember(Value = "Upn")]
        Upn = 2,

        /// <summary>
        /// Environment Id.
        /// </summary>
        [EnumMember(Value = "EnvironmentId")]
        EnvironmentId = 3,

        /// <summary>
        /// Generic type.
        /// </summary>
        [EnumMember(Value = "Generic")]
        Generic = 4,
    }
}
