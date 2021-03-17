//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.EnvironmentSettings.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>The environment map.</summary>
    [DataContract]
    public class EnvironmentMap
    {
        /// <summary>Gets or sets the ID.</summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>Gets or sets autoNumber.</summary>
        [DataMember(Name = "autoNumber", IsRequired = false)]
        public string AutoNumber { get; set; }

        /// <summary>Gets or sets the company display name.</summary>
        [DataMember(Name = "displayName", IsRequired = false)]
        public string DisplayName { get; set; }

        /// <summary>Gets or sets the unique client alias.</summary>
        [DataMember(Name = "alias", IsRequired = false)]
        public string Alias { get; set; }

        /// <summary>Gets or sets the company's logo URI.</summary>
        [DataMember(Name = "brandingImages", IsRequired = false)]
        public IList<BrandingImage> BrandingImages { get; set; }

        /// <summary>Gets or sets the company headquarters.</summary>
        [DataMember(Name = "companyHeadquarters", IsRequired = false)]
        public string CompanyHeadquarters { get; set; }

        /// <summary>Gets or sets the company web page.</summary>
        [DataMember(Name = "companyWebpage", IsRequired = false)]
        public string CompanyWebpage { get; set; }

        /// <summary>Gets or sets the contact email.</summary>
        [DataMember(Name = "contactEmail", IsRequired = false)]
        public string ContactEmail { get; set; }

        /// <summary>Gets or sets the phone number.</summary>
        [DataMember(Name = "phoneNumber", IsRequired = false)]
        public string PhoneNumber { get; set; }

        /// <summary>Gets or sets the environment ID.</summary>
        [DataMember(Name = "environmentId", IsRequired = false)]
        public string EnvironmentId { get; set; }

        /// <summary>Gets or sets the tenant ID.</summary>
        [DataMember(Name = "tenantId", IsRequired = false)]
        public string TenantId { get; set; }

        /// <summary>Gets or sets the privacy website link.</summary>
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
    }
}
