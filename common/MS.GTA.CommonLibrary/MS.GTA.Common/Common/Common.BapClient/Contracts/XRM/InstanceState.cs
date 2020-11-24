
//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="InstanceState.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.Common.BapClient.Contracts.XRM
{
    /// <summary>
    /// The instance type.
    /// </summary>
    public enum InstanceState
    {
        /// <summary>
        /// There is no instance state.
        /// </summary>
        [EnumMember]
        None,

        /// <summary>
        /// The instance type is disabled.
        /// </summary>
        [EnumMember]
        Disabled,

        /// <summary>
        /// The instance failed provisioning
        /// </summary>
        [EnumMember]
        FailedProvisioning,

        /// <summary>
        /// The instance is inactive
        /// </summary>
        [EnumMember]
        Inactive,

        /// <summary>
        /// The instance locked.
        /// </summary>
        [EnumMember]
        Locked,

        /// <summary>
        /// The instance is pending.
        /// </summary>
        [EnumMember]
        Pending,

        /// <summary>
        /// The instance is preparing
        /// </summary>
        [EnumMember]
        PreparingInstance,

        /// <summary>
        /// The instance is ready.
        /// </summary>
        [EnumMember]
        Ready,

        /// <summary>
        /// The instance is ready to configure
        /// </summary>
        [EnumMember]
        ReadyToConfigure
    }
}
