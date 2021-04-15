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


    [ODataEntity(PluralName = "msdyn_jobofferrulesetversions", SingularName = "msdyn_jobofferrulesetversion")]
    public class JobOfferRulesetVersion : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferrulesetversionid")]
        public Guid? JobOfferRulesetVersionId { get; set; }

        [DataMember(Name = "_msdyn_jobofferrulesetid_value")]
        public Guid? JobOfferRulesetId { get; set; }

        [DataMember(Name = "msdyn_jobofferrulesetid")]
        public JobOfferRuleset JobOfferRuleset { get; set; }

        [DataMember(Name = "msdyn_processingstatus")]
        public JobOfferRulesetProcessingStatus ProcessingStatus { get; set; }

        [DataMember(Name = "msdyn_rulesetfileuri")]
        public string RulesetFileUri { get; set; }

        [DataMember(Name = "msdyn_errorfileuri")]
        public string ErrorFileUri { get; set; }

        [DataMember(Name = "msdyn_islive")]
        public bool? IsLive { get; set; }

        [DataMember(Name = "msdyn_jobofferrulesetversion_jobofferrulesetart")]
        public IList<JobOfferRulesetArtifact> JobOfferRulesetArtifacts { get; set; }
    }
}
