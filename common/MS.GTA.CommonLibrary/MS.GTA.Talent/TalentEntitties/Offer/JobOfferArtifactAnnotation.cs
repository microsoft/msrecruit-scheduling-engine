//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferArtifactAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_jobofferartifact")]
        public JobOfferArtifact JobOfferArtifact { get; set; }
    }
}
