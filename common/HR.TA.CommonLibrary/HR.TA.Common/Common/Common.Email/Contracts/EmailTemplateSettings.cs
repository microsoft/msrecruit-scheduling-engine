//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Email.Contracts
{
    using HR.TA.Common.DocumentDB.Contracts;
    using System.Runtime.Serialization;

    /// <summary>
    /// Email Template Settings Class
    /// </summary>
    public class EmailTemplateSettings : DocDbEntity
    {
        [DataMember(Name = "emailTemplatePrivacyPolicyLink", IsRequired = false, EmitDefaultValue = false)]
        public string EmailTemplatePrivacyPolicyLink { get; set; }

        [DataMember(Name = "emailTemplateTermsAndConditionsLink", IsRequired = false, EmitDefaultValue = false)]
        public string EmailTemplateTermsAndConditionsLink { get; set; }

        [DataMember(Name = "emailTemplateHeaderImgUrl", IsRequired = false, EmitDefaultValue = false)]
        public string EmailTemplateHeaderImgUrl { get; set; }

        [DataMember(Name = "shouldDisableEmailEdits", IsRequired = false, EmitDefaultValue = false)]
        public bool ShouldDisableEmailEdits { get; set; }
    }
}
