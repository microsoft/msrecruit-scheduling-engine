//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.Talent.TalentContracts.TalentMatch
{
    /// <summary>
    /// Job Opening Properties
    /// </summary>
    [DataContract]
    public class JobOpeningProperties
    {
        /// <summary>
        /// Gets or sets AddressBookTitle
        /// </summary>
        [DataMember(Name = "addressBookTitle", EmitDefaultValue = false, IsRequired = false)]
        public string AddressBookTitle { get; set; }

        /// <summary>
        /// Gets or sets AddressBookTitlePrefix
        /// </summary>
        [DataMember(Name = "addressBookTitlePrefix", EmitDefaultValue = false, IsRequired = false)]
        public string AddressBookTitlePrefix { get; set; }

        /// <summary>
        /// Gets or sets AddressBookTitleSuffix
        /// </summary>
        [DataMember(Name = "addressBookTitleSuffix", EmitDefaultValue = false, IsRequired = false)]
        public string AddressBookTitleSuffix { get; set; }

        /// <summary>
        /// Gets or sets PayScaleArea
        /// </summary>
        [DataMember(Name = "payScaleArea", EmitDefaultValue = false, IsRequired = false)]
        public string PayScaleArea { get; set; }

        /// <summary>
        /// Gets or sets Profession
        /// </summary>
        [DataMember(Name = "profession", EmitDefaultValue = false, IsRequired = false)]
        public string Profession { get; set; }

        /// <summary>
        /// Gets or sets Qualifier1
        /// </summary>
        [DataMember(Name = "qualifier1", EmitDefaultValue = false, IsRequired = false)]
        public string Qualifier1 { get; set; }

        /// <summary>
        /// Gets or sets Qualifier2
        /// </summary>
        [DataMember(Name = "qualifier2", EmitDefaultValue = false, IsRequired = false)]
        public string Qualifier2 { get; set; }

        /// <summary>
        /// Gets or sets Organization
        /// </summary>
        [DataMember(Name = "organization", EmitDefaultValue = false, IsRequired = false)]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets Discipline
        /// </summary>
        [DataMember(Name = "discipline", EmitDefaultValue = false, IsRequired = false)]
        public string Discipline { get; set; }

        /// <summary>
        /// Gets or sets StandardTitle
        /// </summary>
        [DataMember(Name = "standardTitle", EmitDefaultValue = false, IsRequired = false)]
        public string StandardTitle { get; set; }

        /// <summary>
        /// Gets or sets IncentivePlan
        /// </summary>
        [DataMember(Name = "incentivePlan", EmitDefaultValue = false, IsRequired = false)]
        public string IncentivePlan { get; set; }

        /// <summary>
        /// Gets or sets Level
        /// </summary>
        [DataMember(Name = "level", EmitDefaultValue = false, IsRequired = false)]
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets CareerStage
        /// </summary>
        [DataMember(Name = "careerStage", EmitDefaultValue = false, IsRequired = false)]
        public string CareerStage { get; set; }

        /// <summary>
        /// Gets or sets RoleType
        /// </summary>
        [DataMember(Name = "roleType", EmitDefaultValue = false, IsRequired = false)]
        public string RoleType { get; set; }

        /// <summary>
        /// Gets or sets AdditionalWorkLocation
        /// </summary>
        [DataMember(Name = "additionalWorkLocation", EmitDefaultValue = false, IsRequired = false)]
        public string AdditionalWorkLocation { get; set; }

        /// <summary>
        /// Gets or sets PrimaryPositionID
        /// </summary>
        [DataMember(Name = "primaryPositionID", EmitDefaultValue = false, IsRequired = false)]
        public string PrimaryPositionID { get; set; }

        /// <summary>
        /// Gets or sets HireType
        /// </summary>
        [DataMember(Name = "hireType", EmitDefaultValue = false, IsRequired = false)]
        public string HireType { get; set; }

        /// <summary>
        /// Gets or sets AdminOnboardingContact
        /// </summary>
        [DataMember(Name = "adminOnboardingContact", EmitDefaultValue = false, IsRequired = false)]
        public string AdminOnboardingContact { get; set; }

        /// <summary>
        /// Gets or sets TalentSourcer
        /// </summary>
        [DataMember(Name = "talentSourcer", EmitDefaultValue = false, IsRequired = false)]
        public string TalentSourcer { get; set; }
    }
}
