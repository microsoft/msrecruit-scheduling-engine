//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace MS.GTA.Common.BapClient.Contracts.XRM
{
    using MS.GTA.Common.Base.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// The environment type.
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter))]
    public enum LinkedResourceType
    {
        /// <summary>
        /// The linked resource is not specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// There is no linked resource.
        /// </summary>
        None,

        /// <summary>
        /// The linked resource is a D365 Instance.
        /// </summary>
        Dynamics365Instance
    }
}