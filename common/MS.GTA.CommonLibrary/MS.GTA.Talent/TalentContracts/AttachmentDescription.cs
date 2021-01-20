// <copyright company="Microsoft Corporation" file="AttachmentDescription.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{    
    using System.Runtime.Serialization;

    /// <summary>
    /// Candidate file attachment description
    /// </summary>
    [DataContract]
    public class AttachmentDescription
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
