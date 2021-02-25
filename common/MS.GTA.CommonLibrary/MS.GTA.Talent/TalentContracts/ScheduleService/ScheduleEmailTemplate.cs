//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using MS.GTA.Talent.EnumSetModel.SchedulingService;
    using MS.GTA.TalentEntities.Enum;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ScheduleEmailTemplate
    {
        /// <summary>
        /// Gets or sets Template Id - New GUID
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets Tenant Id- Creator
        /// </summary>
        [DataMember(Name = "tenantId", IsRequired = false)]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets Creator of template
        /// </summary>
        [DataMember(Name = "userObjectId", IsRequired = false)]
        public string UserObjectId { get; set; }

        /// <summary>
        /// Gets or sets Template Name
        /// </summary>
        [DataMember(Name = "templateName", IsRequired = false)]
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets Template Type
        /// </summary>
        [DataMember(Name = "templateType", IsRequired = false)]
        public TemplateType TemplateType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets Is Default
        /// </summary>
        [DataMember(Name = "isDefault", IsRequired = false)]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets Is Autosent
        /// </summary>
        [DataMember(Name = "isAutosent", IsRequired = false)]
        public bool IsAutosent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets Is template for tentative schedule
        /// </summary>
        [DataMember(Name = "isTentative", IsRequired = false)]
        public bool IsTentative { get; set; }

        /// <summary>
        /// Gets or sets Cc Email Address Roles List - used in Attract for candidate emails
        /// </summary>
        [DataMember(Name = "ccEmailAddressRoles", IsRequired = false)]
        public List<JobParticipantRole> CcEmailAddressRoles { get; set; }

        /// <summary>
        /// Gets or sets Cc actual email address List- client will fill this for scheduling service.
        /// </summary>
        [DataMember(Name = "ccEmailAddressList", IsRequired = false)]
        public List<string> CcEmailAddressList { get; set; }

        /// <summary>
        /// Gets or sets Bcc Email Address Roles List - used in Attract for candidate emails
        /// </summary>
        [DataMember(Name = "bccEmailAddressRoles", IsRequired = false)]
        public List<JobParticipantRole> BccEmailAddressRoles { get; set; }

        /// <summary>
        /// Gets or sets Bcc actual email address List- client will fill this for scheduling service.
        /// </summary>
        [DataMember(Name = "bccEmailAddressList", IsRequired = false)]
        public List<string> BccEmailAddressList { get; set; }

        /// <summary>
        /// Gets or sets primary email recipients - in the 'to' line
        /// </summary>
        [DataMember(Name = "primaryEmailRecipients", IsRequired = false)]
        public List<string> PrimaryEmailRecipients { get; set; }

        /// <summary>
        /// Gets or sets primary email recipients - in the 'to' line
        /// </summary>
        [DataMember(Name = "subject", IsRequired = false)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets  Email Content
        /// </summary>
        [DataMember(Name = "emailContent", IsRequired = false)]
        public string EmailContent { get; set; }

        /// <summary>
        /// Gets or sets Email Token List
        /// </summary>
        [DataMember(Name = "emailTokenList", IsRequired = false)]
        public List<EmailTemplateTokens> EmailTokenList { get; set; }

        /// <summary>
        /// Gets or sets Send email from address mode
        /// </summary>
        [DataMember(Name = "fromAddressMode", IsRequired = false)]
        public SendEmailFromAddressMode FromAddressMode { get; set; }
    }
}
