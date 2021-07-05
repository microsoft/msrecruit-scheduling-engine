//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Email Template
    /// </summary>
    [DataContract]
    public class EmailTemplate
    {
        /// <summary>
        /// Gets or sets Template Name
        /// </summary>
        [DataMember(Name = "templateName", IsRequired = false)]
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets Cc actual email address List- client will fill this for scheduling service.
        /// </summary>
        [DataMember(Name = "ccEmailAddressList", IsRequired = false)]
        public List<string> CcEmailAddressList { get; set; }

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
    }
}
