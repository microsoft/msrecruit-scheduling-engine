//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnvironmentDefinition.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.BapClient.Contracts
{
    /// <summary>
    /// The environment definition contract
    /// </summary>
    public class EnvironmentDefinition
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the resource properties.
        /// </summary>
        public EnvironmentProperties Properties { get; set; }
    }
}
