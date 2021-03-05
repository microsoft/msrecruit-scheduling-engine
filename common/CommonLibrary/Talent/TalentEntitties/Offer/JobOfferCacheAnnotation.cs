//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace Common.Provisioning.Entities.XrmEntities.Offer
{
    using Common.XrmHttp;
    using Common.XrmHttp.Model;
    using System.Runtime.Serialization;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferCacheAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_offercache")]
        public JobOfferCache JobOfferCache { get; set; }
    }
}
