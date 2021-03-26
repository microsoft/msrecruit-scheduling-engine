//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Email.Contracts
{
    using TA.CommonLibrary.Common.Common.Common.Email.Contracts;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class EmailTemplate
    {
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        [DataMember(Name = "templateName", IsRequired = true)]
        public string TemplateName { get; set; }

        [DataMember(Name = "appName", IsRequired = true)]
        public string AppName { get; set; }

        [DataMember(Name = "templateType", IsRequired = true)]
        public string TemplateType { get; set; }

        [DataMember(Name = "isGlobal", IsRequired = false)]
        public bool IsGlobal { get; set; }

        [DataMember(Name = "to", IsRequired = false, EmitDefaultValue = false)]
        public List<string> To { get; set; }

        [DataMember(Name = "cc", IsRequired = false, EmitDefaultValue = false)]
        public List<string> Cc { get; set; }

        [DataMember(Name = "additionalCc", IsRequired = false, EmitDefaultValue = false)]
        public List<string> AdditionalCc { get; set; }

        [DataMember(Name = "bcc", IsRequired = false, EmitDefaultValue = false)]
        public List<string> Bcc { get; set; }

        [DataMember(Name = "subject", IsRequired = false)]
        public string Subject { get; set; }

        [DataMember(Name = "header", IsRequired = false)]
        public string Header { get; set; }

        [DataMember(Name = "body", IsRequired = false)]
        public string Body { get; set; }

        [DataMember(Name = "attachments", IsRequired = false)]
        public FileAttachmentRequest Attachments { get; set; }

        [DataMember(Name = "closing", IsRequired = false)]
        public string Closing { get; set; }

        [DataMember(Name = "footer", IsRequired = false)]
        public string Footer { get; set; }

        [DataMember(Name = "isDefault", IsRequired = false)]
        public bool IsDefault { get; set; }

        [DataMember(Name = "isAutosent", IsRequired = false)]
        public bool isAutosent { get; set; }

        [DataMember(Name = "creator", IsRequired = false)]
        public string Creator { get; set; }

        [DataMember(Name = "language", IsRequired = false)]
        public string Language { get; set; }
    }
}
