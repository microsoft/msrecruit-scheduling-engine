//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;
    using CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// The Applicant Attachment data contract.
    /// </summary>
    [DataContract]
    public class ApplicantAttachment
    {
        /// <summary>
        /// Gets or sets attachment id.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets attachment document type.
        /// </summary>
        [DataMember(Name = "documentType", IsRequired = false)]
        public CandidateAttachmentDocumentType DocumentType { get; set; }

        /// <summary>
        /// Gets or sets attachment type.
        /// </summary>
        [DataMember(Name = "type", IsRequired = false)]
        public CandidateAttachmentType Type { get; set; }

        /// <summary>
        /// Gets or sets attachment name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets attachment description.
        /// </summary>
        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets token generated from CDS blob reference. 
        /// </summary>
        [DataMember(Name = "reference", IsRequired = false, EmitDefaultValue = false)]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets user action. 
        /// </summary>
        [DataMember(Name = "userAction", IsRequired = false, EmitDefaultValue = false)]
        public UserAction UserAction { get; set; }
        
        /// <summary>
        /// Gets or sets value indicating whether the attachment is selected as one of the job appplication attachments. 
        /// </summary>
        [DataMember(Name = "isJobApplicationAttachment", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsJobApplicationAttachment { get; set; }

        /// <summary>
        /// Gets or sets last uploaded by.
        /// </summary>
        [DataMember(Name = "uploadedBy", IsRequired = false, EmitDefaultValue = false)]
        public Person UploadedBy { get; set; }

        /// <summary>
        /// Gets or sets last uploaded date time.
        /// </summary>
        [DataMember(Name = "uploadedDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? UploadedDateTime { get; set; }
    }
}
