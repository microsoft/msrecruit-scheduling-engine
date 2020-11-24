//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnvironmentProvisioningOperation.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

using System.Net;
using Newtonsoft.Json;

namespace MS.GTA.Common.BapClient.Contracts.XRM
{
    /// <summary>
    /// The environment entity provisioning operation.
    /// </summary>
    public class EnvironmentProvisioningOperation
    {
        /// <summary>
        /// Gets or sets the operation name.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the operation http status code.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public HttpStatusCode HttpStatus { get; set; }

        /// <summary>
        /// Gets or sets the operation provisioning state code.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public EnvironmentProvisioningState Code { get; set; }
    }
}