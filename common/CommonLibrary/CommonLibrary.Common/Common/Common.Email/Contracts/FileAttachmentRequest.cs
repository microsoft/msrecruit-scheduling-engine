//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CommonLibrary.Common.Common.Common.Email.Contracts
{
    /// <summary>
    /// Specifies the Data Contract for Email Attachment Request
    /// </summary>
    [DataContract]
    public class FileAttachmentRequest
    {
        /// <summary>
        /// Gets or sets file which has attachment content
        /// </summary>
        [DataMember(Name = "files")]
        public IFormFileCollection Files { get; set; }

        /// <summary>
        /// Gets or sets name with which attachment should be tagged.
        /// </summary>
        [DataMember(Name = "fileLabels")]
        public IEnumerable<string> FileLabels { get; set; }
    }
}
