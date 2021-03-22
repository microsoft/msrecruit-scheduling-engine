//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.OfferRule
{
    using System;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.XrmHttp;
    using CommonLibrary.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferRulesetArtifactAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_jobofferrulesetartifact")]
        public JobOfferRulesetArtifact JobOfferRulesetArtifact { get; set; }
    }
}