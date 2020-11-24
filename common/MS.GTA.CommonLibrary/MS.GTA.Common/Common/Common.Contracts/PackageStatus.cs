//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="PackageStatus.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Contracts
{
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Provisioning package status contract
    /// </summary>
    [DataContract]
    public class PackageStatus
    {
        /// <summary>
        /// Gets or sets package status
        /// </summary>
        [DataMember(Name = "status")]
        public PackageStatusCode Status { get; set; }

        /// <summary>
        /// Gets or sets the installed package version
        /// </summary>
        [DataMember(Name = "installedVersion")]
        public string InstalledVersion { get; set; }

        /// <summary>
        /// Gets or sets the installed package name
        /// </summary>
        [DataMember(Name = "packageName")]
        public string PackageName { get; set; }

        /// <summary>
        /// Gets or sets the package details
        /// </summary>
        [DataMember(Name = "details")]
        public string Details { get; set; }
    }
}