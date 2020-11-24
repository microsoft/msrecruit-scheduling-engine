//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CompnayData.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Talent.TalentContracts.TalentJobPosting.Contract
{
    using MS.GTA.Common.EnvironmentSettings.Contracts;
    using System.Runtime.Serialization;

    /// <summary>The company data.</summary>
    [DataContract]
    public class CompanyData
    {
        /// <summary>Gets or sets the branding image.</summary>
        [DataMember(Name = "brandingImage", IsRequired = true)]
        public BrandingImage BrandingImage { get; set; }

        /// <summary>Gets or sets the privacy link.</summary>
        [DataMember(Name = "privacyLink", IsRequired = false)]
        public string PrivacyLink { get; set; }

        /// <summary>Gets or sets the terms of service link.</summary>
        [DataMember(Name = "tosLink", IsRequired = false)]
        public string TosLink { get; set; }

        /// <summary>Gets or sets the terms and conditions link.</summary>
        [DataMember(Name = "termsAndConditionsLink", IsRequired = false)]
        public string TermsAndConditionsLink { get; set; }

        /// <summary>Gets or sets the terms and conditions text.</summary>
        [DataMember(Name = "termsAndConditionsText", IsRequired = false)]
        public string TermsAndConditionsText { get; set; }

        /// <summary>The flag that indicates whether terms and conditions are enabled or disabled.</summary>
        [DataMember(Name = "enableTermsAndConditions", IsRequired = false, EmitDefaultValue = true)]
        public bool EnableTermsAndConditions { get; set; }

        /// <summary>Gets or sets the display name.</summary>
        [DataMember(Name = "displayName", IsRequired = false)]
        public string DisplayName { get; set; }

        /// <summary>Gets or sets the tenant id.</summary>
        [DataMember(Name = "tenantId", IsRequired = false)]
        public string TenantId { get; set; }

        /// <summary>Gets or sets the environment id.</summary>
        [DataMember(Name = "environmentId", IsRequired = false)]
        public string EnvironmentId { get; set; }
    }
}
