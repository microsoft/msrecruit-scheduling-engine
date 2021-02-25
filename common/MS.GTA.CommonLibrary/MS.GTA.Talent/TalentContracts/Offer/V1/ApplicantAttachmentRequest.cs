//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Specifies the Data Contract for Applicant Attachment Request
    /// </summary>
    [DataContract]
    public class ApplicantAttachmentRequest
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
