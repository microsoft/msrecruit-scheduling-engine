//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.EnvironmentSettings.Contracts
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
