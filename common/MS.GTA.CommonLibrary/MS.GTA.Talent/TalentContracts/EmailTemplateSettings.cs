//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The email template settings contract.
    /// </summary>
    [DataContract]
    public class EmailTemplateSettings
    {
        /// <summary>
        /// Gets or sets email template footer text.
        /// </summary>
        [DataMember(Name = "emailTemplateFooterText", IsRequired = false, EmitDefaultValue = false)]
        public string EmailTemplateFooterText { get; set; }

        /// <summary>
        /// Gets or sets email template communication privacy policy link.
        /// </summary>
        [DataMember(Name = "emailTemplatePrivacyPolicyLink", IsRequired = false, EmitDefaultValue = false)]
        public string EmailTemplatePrivacyPolicyLink { get; set; }

        /// <summary>
        /// Gets or sets email template communication tearms and conditions link.
        /// </summary>
        [DataMember(Name = "emailTemplateTermsAndConditionsLink", IsRequired = false, EmitDefaultValue = false)]
        public string EmailTemplateTermsAndConditionsLink { get; set; }

        /// <summary>
        /// Gets or sets whether we should disable email edits
        /// </summary>
        [DataMember(Name = "shouldDisableEmailEdits", IsRequired = false, EmitDefaultValue = false)]
        public bool ShouldDisableEmailEdits { get; set; }

        /// <summary>
        /// Gets or sets email template header image url.
        /// </summary>
        [DataMember(Name = "emailTemplateHeaderImgUrl", IsRequired = false, EmitDefaultValue = false)]
        public string EmailTemplateHeaderImgUrl { get; set; }

        /// <summary>
        /// Gets or sets last modified by.
        /// </summary>
        [DataMember(Name = "modifiedBy", IsRequired = false, EmitDefaultValue = false)]
        public Person ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets last modified date time.
        /// </summary>
        [DataMember(Name = "modifiedDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime ModifiedDateTime { get; set; }
    }
}