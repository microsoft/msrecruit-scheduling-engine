//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.Common.BapClient.Contracts.XRM
{
    using HR.TA.Common.Base.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// The environment provisioning state.
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter))]
    public enum EnvironmentProvisioningState
    {
        /// <summary>
        /// The provisioning state is not specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// The provisioning state is succeeded.
        /// </summary>
        Succeeded,

        /// <summary>
        /// The provisioning state is deleting.
        /// </summary>
        Deleting,

        /// <summary>
        /// The provisioning state is deleted.
        /// </summary>
        Deleted,

        /// <summary>
        /// The provisioning state is failed to delete.
        /// </summary>
        FailedDeleting,

        /// <summary>
        /// The provisioning state when the environment is created, but the linked database is still provisioning.
        /// </summary>
        LinkedDatabaseProvisioning,

        /// <summary>
        /// The provisioning state when an environment is created, but the database failed to provision.
        /// </summary>
        LinkedDatabaseFailedProvisioning,

        /// <summary>
        /// The provisioning state when an environment is created, the database is created, but it failed to configure.
        /// </summary>
        LinkedDatabaseFailedConfiguring,
    }
}
