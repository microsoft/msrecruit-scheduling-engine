//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace TA.CommonLibrary.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.XrmHttp;
    using TA.CommonLibrary.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferArtifactAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_jobofferartifact")]
        public JobOfferArtifact JobOfferArtifact { get; set; }
    }
}
