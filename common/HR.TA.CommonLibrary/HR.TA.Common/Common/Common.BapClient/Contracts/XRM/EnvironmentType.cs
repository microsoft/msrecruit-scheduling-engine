//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.Common.BapClient.Contracts.XRM
{
    using HR.TA.Common.Base.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// The environment type.
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter))]
    public enum EnvironmentType
    {
        /// <summary>
        /// The environment type is not specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// The environment type is Production.
        /// </summary>
        Production,

        /// <summary>
        /// The environment type is Sandbox.
        /// </summary>
        Sandbox,
    }
}
