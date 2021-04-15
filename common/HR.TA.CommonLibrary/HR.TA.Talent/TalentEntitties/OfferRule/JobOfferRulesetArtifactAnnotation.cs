//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.OfferRule
{
    using System;
    using System.Runtime.Serialization;
    using HR.TA.Common.XrmHttp;
    using HR.TA.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferRulesetArtifactAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_jobofferrulesetartifact")]
        public JobOfferRulesetArtifact JobOfferRulesetArtifact { get; set; }
    }
}
