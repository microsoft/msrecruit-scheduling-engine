//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.OfferRule
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using HR.TA.Common.XrmHttp;
    using HR.TA.Common.Provisioning.Entities.XrmEntities.Offer;

    [ODataEntity(PluralName = "msdyn_jobofferrulesetattributedatas", SingularName = "msdyn_jobofferrulesetattributedata")]
    public class JobOfferRulesetAttributeData : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferrulesetattributedataid")]
        public Guid? JobOfferRuleAttributeDataId { get; set; }

        [DataMember(Name = "_msdyn_jobofferrulesetversionid_value")]
        public Guid? JobOfferRulesetVersionId { get; set; }

        [DataMember(Name = "msdyn_jobofferrulesetversionid")]
        public JobOfferRulesetVersion JobOfferRulesetVersion { get; set; }

        [DataMember(Name = "_msdyn_jobofferrulesetid_value")]
        public Guid? JobOfferRulesetId { get; set; }

        [DataMember(Name = "msdyn_jobofferrulesetid")]
        public JobOfferRuleset JobOfferRuleset { get; set; }

        [DataMember(Name = "_msdyn_parentattributedataid_value")]
        public string ParentAttributeDataId { get; set; }

        [DataMember(Name = "msdyn_parentattributedataid")]
        public JobOfferRulesetAttributeData ParentAttributeData { get; set; }

        [DataMember(Name = "msdyn_jobofferrulesetattributedata_parentdata")]
        public IList<JobOfferRulesetAttributeData> JobOfferRulesetAttributeDataNextLevel { get; set; }

        [DataMember(Name = "_msdyn_tokenid_value")]
        public Guid? JobOfferTokenId { get; set; }

        [DataMember(Name = "msdyn_tokenid")]
        public JobOfferToken JobOfferToken { get; set; }

        [DataMember(Name = "msdyn_tokenvalue")]
        public string TokenValue { get; set; }

        [DataMember(Name = "msdyn_tokenvaluepath")]
        public string TokenValuePath { get; set; }
    }
}
