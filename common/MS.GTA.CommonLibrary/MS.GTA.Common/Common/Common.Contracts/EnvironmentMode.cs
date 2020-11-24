// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EnvironmentMode.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>The environment mode.</summary>
    [DataContract]
    public enum EnvironmentMode
    {
        /// <summary>
        /// The default CDS mode.
        /// </summary>
        Cds = 0,

        /// <summary>
        /// Falcon Mode
        /// </summary>
        Falcon = 1,

        /// <summary>
        /// Running on both CDS and Falcon
        /// </summary>
        Both = 2,

        /// <summary>
        /// XRM Mode
        /// </summary>
        Xrm = 3,

        /// <summary>
        /// XRM and Falcon Mode
        /// </summary>
        XrmAndFalcon = 4,
    }
}
