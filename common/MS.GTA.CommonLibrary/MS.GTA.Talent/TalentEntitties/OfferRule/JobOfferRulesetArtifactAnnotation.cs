//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace Common.Provisioning.Entities.XrmEntities.OfferRule
{
    using System;
    using System.Runtime.Serialization;
    using Common.XrmHttp;
    using Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferRulesetArtifactAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_jobofferrulesetartifact")]
        public JobOfferRulesetArtifact JobOfferRulesetArtifact { get; set; }
    }
}
