//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
using Newtonsoft.Json;

namespace CommonLibrary.Common.BapClient.Contracts.XRM
{
    /// <summary>
    /// The environment entity provisioning details.
    /// </summary>
    public class EnvironmentProvisioningDetails
    {
        /// <summary>
        /// Gets or sets the provisioning message.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the provisioning operations.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public EnvironmentProvisioningOperation[] Operations { get; set; }

        /// <summary>
        /// Gets or sets the operation id for an async provisioning operation.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string OperationId { get; set; }
    }
}
