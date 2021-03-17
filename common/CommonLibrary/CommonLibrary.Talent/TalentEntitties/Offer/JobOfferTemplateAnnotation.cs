//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.XrmHttp;
    using CommonLibrary.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferTemplateAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_joboffertemplate")]
        public JobOfferTemplate JobOfferTemplate { get; set; }
    }
}
