//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright> {{?/plural/key}}
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;
    using System.Runtime.Serialization;

    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferCacheAnnotation : Annotation
    {
        [DataMember(Name = "objectid_msdyn_offercache")]
        public JobOfferCache JobOfferCache { get; set; }
    }
}
