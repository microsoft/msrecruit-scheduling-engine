//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.OfferRule
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using HR.TA.Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_jobofferrulesetartifacts", SingularName = "msdyn_jobofferrulesetartifact")]
    public class JobOfferRulesetArtifact : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferrulesetartifactid")]
        public Guid? JobOfferRulesetArtifactId { get; set; }

        [DataMember(Name = "_msdyn_jobofferrulesetversionid_value")]
        public Guid? JobOfferRulesetVersionId { get; set; }

        [DataMember(Name = "msdyn_jobofferrulesetversionid")]
        public JobOfferRulesetVersion JobOfferRulesetVersion { get; set; }
    }
}
