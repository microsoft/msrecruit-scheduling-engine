//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.XrmHttp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract(Name = "annotations")]
    public class Annotation : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "annotationid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        [DataMember(Name = "filename")]
        public string FileName { get; set; }

        [DataMember(Name = "documentbody")]
        public string DocumentBody { get; set; }

        [DataMember(Name = "mimetype")]
        public string MimeType { get; set; }

        [DataMember(Name = "_objectid_value")]
        public Guid? RelatedEntityRecId { get; set; }

        [DataMember(Name = "objecttypecode")]
        public string ObjectTypeCode { get; set; }

        [DataMember(Name = "notetext")]
        public string NoteText { get; set; }

        [DataMember(Name = "objectid_contact")]
        public Contact Contact { get; set; }

        [DataMember(Name ="objectid_email")]
        public Email Email { get; set; }

        [DataMember(Name = "objectid_task")]
        public TaskActivity Task { get; set; }
    }
}
