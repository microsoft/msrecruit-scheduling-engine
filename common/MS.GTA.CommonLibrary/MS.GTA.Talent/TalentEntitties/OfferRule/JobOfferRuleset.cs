﻿// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferRuleset.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.OfferRule
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer;

    [ODataEntity(PluralName = "msdyn_jobofferrulesets", SingularName = "msdyn_jobofferruleset")]
    public class JobOfferRuleset : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferrulesetid")]
        public Guid? JobOfferRulesetId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "_msdyn_tokenid_value")]
        public Guid? JobOfferTokenId { get; set; }

        [DataMember(Name = "msdyn_tokenid")]
        public JobOfferToken JobOfferToken { get; set; }

        [DataMember(Name = "msdyn_status")]
        public JobOfferRulesetStatus Status { get; set; }

        [DataMember(Name = "msdyn_jobofferruleset_jobofferrulesetattribute")]
        public IList<JobOfferRulesetAttribute> Attributes { get; set; }

        [DataMember(Name = "msdyn_jobofferruleset_jobofferrulesetversion")]
        public IList<JobOfferRulesetVersion> Versions { get; set; }
    }
}