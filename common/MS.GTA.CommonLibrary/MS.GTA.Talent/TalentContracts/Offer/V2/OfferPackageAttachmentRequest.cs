//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V2
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.AspNetCore.Http;

    ////TODO
    /// <summary>
    /// Specifies the Data Contract for Applicant Attachment Request
    /// </summary>
    [DataContract]
    public class OfferPackageAttachmentRequest
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
