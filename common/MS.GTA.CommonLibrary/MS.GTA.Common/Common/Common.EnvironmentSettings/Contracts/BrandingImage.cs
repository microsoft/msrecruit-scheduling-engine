//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="BrandingImage.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.EnvironmentSettings.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>The branding image.</summary>
    [DataContract]
    public class BrandingImage
    {
        /// <summary>Gets or sets the uri.</summary>
        [DataMember(Name = "uri", IsRequired = true)]
        public string URI { get; set; }

        /// <summary>Gets or sets the type.</summary>
        [DataMember(Name = "type", IsRequired = true)]
        public BrandingImageType Type { get; set; }
    }
}
