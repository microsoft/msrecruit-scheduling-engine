//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="BrandingImageType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.EnvironmentSettings.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>The branding image type.</summary>
    [DataContract]
    public enum BrandingImageType
    {
        /// <summary>Logo type.</summary>
        Logo = 0,
    }
}
