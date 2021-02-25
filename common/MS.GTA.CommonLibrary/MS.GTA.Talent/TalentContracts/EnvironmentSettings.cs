//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

// Note: This namespace needs to stay the same since the docdb collection name depends on it.
namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The environment settings data contract.
    /// </summary>
    [DataContract]
    public class EnvironmentSettings
    {
        /// <summary>Gets or sets the document Id.</summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets template setting
        /// </summary>
        [DataMember(Name = "templateSetting", IsRequired = false, EmitDefaultValue = false)]
        public TemplateSettings TemplateSetting { get; set; }

        /// <summary>
        /// Gets or sets feature settings
        /// </summary>
        [DataMember(Name = "featureSettings", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<FeatureSettings> FeatureSettings { get; set; }

        /// <summary>
        /// Gets or sets integration settings
        /// </summary>
        [DataMember(Name = "integrationSettings", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<string, IntegrationSetting> IntegrationSettings { get; set; }

        /// <summary>
        /// Gets or sets offer settings
        /// </summary>
        [DataMember(Name = "offerSettings", IsRequired = false, EmitDefaultValue = false)]
        public OfferSettings OfferSettings { get; set; }

        /// <summary>
        /// Gets or sets ESign settings
        /// </summary>
        [DataMember(Name = "eSignSettings", IsRequired = false, EmitDefaultValue = false)]
        public ESignSettings ESignSettings { get; set; }


        /// <summary>
        /// Gets or sets email template settings
        /// </summary>
        [DataMember(Name = "emailTemplateSettings", IsRequired = false, EmitDefaultValue = false)]
        public EmailTemplateSettings EmailTemplateSettings { get; set; }


        /// <summary>
        /// Gets or sets isPremium field
        /// </summary>
        [DataMember(Name = "isPremium", IsRequired = false, EmitDefaultValue = false)]
        public bool isPremium { get; set; }
    }
}
