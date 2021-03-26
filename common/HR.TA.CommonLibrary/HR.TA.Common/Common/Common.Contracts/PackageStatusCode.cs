//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provisioning statuses
    /// </summary>
    [DataContract]
    public enum PackageStatusCode
    {
        /// <summary>
        /// Package status unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Package is installing into the environment
        /// </summary>
        Installing = 1,

        /// <summary>
        /// Package is upgrading the environment
        /// </summary>
        Upgrading = 2,

        /// <summary>
        /// Package is provisioned to the environment
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Package status error
        /// </summary>
        Error = 4,

        /// <summary>
        /// Package has not started installing yet.
        /// </summary>
        NotStarted = 5,
    }
}
