// <copyright company="Microsoft Corporation" file="ArtifactDescription.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Offer Letter description
    /// </summary>
    [DataContract]
    public class ArtifactDescription
    {
        /// <summary>
        /// Gets or sets File Size
        /// </summary>
        [DataMember(Name = "fileSize")]
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets file uploader ID
        /// </summary>
        [DataMember(Name = "fileUploaderId")]
        public string FileUploaderId { get; set; }
    }
}
