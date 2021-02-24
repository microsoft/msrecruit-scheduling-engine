//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Email.Contracts
{
    using MS.GTA.Common.DocumentDB.Contracts;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Falcon Email template Class
    /// </summary>
    /// <seealso cref="MS.GTA.Common.DocumentDB.Contracts.DocDbEntity" />
    [DataContract]
    public class FalconEmailTemplate : DocDbEntity
    {
        [DataMember(Name = "templateName", IsRequired = true)]
        public string TemplateName { get; set; }

        [DataMember(Name = "appName", IsRequired = true)]
        public string AppName { get; set; }

        [DataMember(Name = "templateType", IsRequired = true)]
        public string TemplateType { get; set; }

        [DataMember(Name = "to", EmitDefaultValue = false)]
        public List<string> To { get; set; }

        [DataMember(Name = "cc", EmitDefaultValue = false)]
        public List<string> Cc { get; set; }

        [DataMember(Name = "additionalCc", EmitDefaultValue = false)]
        public List<string> AdditionalCc { get; set; }

        [DataMember(Name = "bcc", EmitDefaultValue = false)]
        public List<string> Bcc { get; set; }

        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        [DataMember(Name = "header")]
        public string Header { get; set; }

        [DataMember(Name = "body")]
        public string Body { get; set; }

        [DataMember(Name = "closing")]
        public string Closing { get; set; }

        [DataMember(Name = "footer")]
        public string Footer { get; set; }

        [DataMember(Name = "isDefault")]
        public bool IsDefault { get; set; }

        [DataMember(Name = "language")]
        public string Language { get; set; }
    }
}
