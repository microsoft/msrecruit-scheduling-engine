//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace TA.CommonLibrary.Common.Provisioning.Entities.XrmEntities.Offer
{
    using TA.CommonLibrary.Common.XrmHttp;
    using TA.CommonLibrary.Common.XrmHttp.Model;
    using System.Runtime.Serialization;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferCacheAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_offercache")]
        public JobOfferCache JobOfferCache { get; set; }
    }
}
