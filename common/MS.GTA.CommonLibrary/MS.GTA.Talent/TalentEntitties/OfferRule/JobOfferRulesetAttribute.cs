//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.OfferRule
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer;

    [ODataEntity(PluralName = "msdyn_jobofferrulesetattributes", SingularName = "msdyn_jobofferrulesetattribute")]
    public class JobOfferRulesetAttribute : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferrulesetattributeid")]
        public Guid? JobOfferRulesetAttributeId { get; set; }

        [DataMember(Name = "_msdyn_jobofferrulesetid_value")]
        public Guid? JobOfferRulesetId { get; set; }

        [DataMember(Name = "msdyn_JobofferrulesetId")]
        public JobOfferRuleset JobOfferRuleset { get; set; }

        [DataMember(Name = "msdyn_level")]
        public int? Level { get; set; }

        [DataMember(Name = "_msdyn_tokenid_value")]
        public Guid? JobOfferTokenId { get; set; }

        [DataMember(Name = "msdyn_tokenid")]
        public JobOfferToken JobOfferToken { get; set; }
    }
}
