//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Attract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using TA.CommonLibrary.Common.Email.Contracts;


    /// <summary>
    /// The offer setting data contract.
    /// </summary>
    [DataContract]
    public class ContactHiringTeam
    {
        /// <summary>
        /// Gets or sets email template
        /// </summary>
        [DataMember(Name = "template", IsRequired = false, EmitDefaultValue = true)]
        public EmailTemplate  Template { get; set; }

        /// <summary>
        /// Gets or sets attachment ids
        /// </summary>
        [DataMember(Name = "attachmentIds", IsRequired = false, EmitDefaultValue = true)]
        public List<string> AttachmentIds { get; set; }

        /// <summary>
        /// Gets or sets candidate id
        /// </summary>
        [DataMember(Name = "candidateId", IsRequired = false, EmitDefaultValue = true)]
        public string CandidateId { get; set; }

        /// <summary>
        /// Gets or sets job id
        /// </summary>
        [DataMember(Name = "jobId", IsRequired = false, EmitDefaultValue = true)]
        public string JobId { get; set; }

    }
}
