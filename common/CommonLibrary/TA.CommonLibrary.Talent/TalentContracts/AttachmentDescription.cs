//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
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
