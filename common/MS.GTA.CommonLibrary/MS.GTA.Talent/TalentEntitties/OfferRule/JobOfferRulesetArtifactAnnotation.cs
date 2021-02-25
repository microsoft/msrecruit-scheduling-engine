//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.OfferRule
{
    using System;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferRulesetArtifactAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_jobofferrulesetartifact")]
        public JobOfferRulesetArtifact JobOfferRulesetArtifact { get; set; }
    }
}
