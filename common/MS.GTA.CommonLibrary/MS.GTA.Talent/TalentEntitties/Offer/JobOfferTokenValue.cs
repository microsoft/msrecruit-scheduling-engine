//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using MS.GTA.Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_joboffertokenvalues", SingularName = "msdyn_joboffertokenvalue")]
    public class JobOfferTokenValue : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_joboffertokenvalueid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_tokenkey")]
        public string TokenKey { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "msdyn_tokenvalue")]
        public string TokenValue { get; set; }

        [DataMember(Name = "msdyn_displaytext")]
        public string DisplayText { get; set; }

        [DataMember(Name = "msdyn_isexplicitlyselected")]
        public bool? IsSelectedExplicitly { get; set; }

        [DataMember(Name = "msdyn_isoverridden")]
        public bool? IsOverridden { get; set; }

        [DataMember(Name = "msdyn_ordinalnumber")]
        public int? OrdinalNumber { get; set; }

        [DataMember(Name = "msdyn_defaultvalue")]
        public string DefaultValue { get; set; }

        [DataMember(Name = "_msdyn_joboffersectionid_value")]
        public Guid? JobOfferSectionId { get; set; }

        [DataMember(Name = "msdyn_Joboffersectionid")]
        public JobOfferSection JobOfferSection { get; set; }

        [DataMember(Name = "_msdyn_joboffer_joboffertokenvalue_value")]
        public Guid? JobOfferRecId { get; set; }

        [DataMember(Name = "msdyn_joboffer_joboffertokenvalue")]
        public JobOffer JobOffer { get; set; }

        [DataMember(Name = "msdyn_tokenvaluemultiline")]
        public string TokenValueMultiline { get; set; }

        [DataMember(Name = "msdyn_defaultvaluemultiline")]
        public string DefaultValueMultiline { get; set; }

        [DataMember(Name = "msdyn_displaytextmultiline")]
        public string DisplayTextMultiline { get; set; }

        [DataMember(Name = "msdyn_isoptional")]
        public bool? IsOptional { get; set; }

        [DataMember(Name = "msdyn_joboffertokenvalue_joboffertoken")]
        public IList<JobOfferToken> JobOfferTokens { get; set; }
    }
}
