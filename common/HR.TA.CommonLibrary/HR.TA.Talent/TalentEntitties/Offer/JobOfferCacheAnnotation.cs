//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using HR.TA.Common.XrmHttp;
    using HR.TA.Common.XrmHttp.Model;
    using System.Runtime.Serialization;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferCacheAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_offercache")]
        public JobOfferCache JobOfferCache { get; set; }
    }
}
