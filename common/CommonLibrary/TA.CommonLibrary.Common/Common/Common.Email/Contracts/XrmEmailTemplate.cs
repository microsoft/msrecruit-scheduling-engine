//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Email.Contracts
{
    using TA.CommonLibrary.Common.XrmHttp;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Xrm Email Template Class
    /// </summary>
    /// <seealso cref="TA.CommonLibrary.Common.XrmHttp.XrmODataEntity" />
    [ODataEntity(PluralName = "msdyn_talentemailtemplates", SingularName = "msdyn_talentemailtemplate")]
    public class XrmEmailTemplate : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_talentemailtemplateid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_autonumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "msdyn_templatename")]
        public string TemplateName { get; set; }

        [DataMember(Name = "msdyn_applicationname")]
        public ApplicationName? ApplicationName { get; set; }

        [DataMember(Name = "msdyn_templatetype")]
        public string TemplateType { get; set; }

        [DataMember(Name = "msdyn_to")]
        public string To { get; set; }

        [DataMember(Name = "msdyn_cc")]
        public string Cc { get; set; }

        [DataMember(Name = "msdyn_bcc")]
        public string Bcc { get; set; }

        [DataMember(Name = "msdyn_subject")]
        public string Subject { get; set; }

        [DataMember(Name = "msdyn_header")]
        public string Header { get; set; }

        [DataMember(Name = "msdyn_body")]
        public string Body { get; set; }

        [DataMember(Name = "msdyn_closing")]
        public string Closing { get; set; }

        [DataMember(Name = "msdyn_footer")]
        public string Footer { get; set; }

        [DataMember(Name = "msdyn_isdefaulttemplate")]
        public bool IsDefault { get; set; }

        [DataMember(Name = "msdyn_language")]
        public string Language { get; set; }

        [DataMember(Name = "msdyn_additionalproperties")]
        public string AdditionalProperties { get; set; }
    }
}
