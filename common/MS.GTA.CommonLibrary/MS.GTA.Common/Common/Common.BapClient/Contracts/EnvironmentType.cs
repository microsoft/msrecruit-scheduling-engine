//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnvironmentType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.BapClient.Contracts
{
    using Base.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// The environment type
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
