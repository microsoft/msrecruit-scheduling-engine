// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnvironmentStatus.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provisioning statuses
    /// </summary>
    [DataContract]
    public enum EnvironmentStatusCode
    {
        /// <summary>
        /// Environment status unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Environment is installing.
        /// </summary>
        Installing = 1,

        /// <summary>
        /// Environment is provisioned.
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Environment is upgrading.
        /// </summary>
        Upgrading = 3,

        /// <summary>
        /// Environment status error.
        /// </summary>
        Error = 4,

        /// <summary>
        /// The environment has not started installing yet.
        /// </summary>
        NotStarted = 5,

        /// <summary>
        /// The environment is being deleted.
        /// </summary>
        Deleting = 6,

        /// <summary>
        /// The environment has been soft-deleted.
        /// </summary>
        SoftDeleted = 7,

        /// <summary>
        /// The environment has been deleted.
        /// </summary>
        Deleted = 8,

        /// <summary>
        /// The migration status.
        /// </summary>
        Migrating = 9,
    }
}
