//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Talent.FalconEntities.Common
{
    using MS.GTA.Common.DocumentDB.Contracts;
    using System.Runtime.Serialization;

    [DataContract]
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
