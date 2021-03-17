//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_offercaches", SingularName = "msdyn_offercache")]
    public class JobOfferCache : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_offercacheid")]
        public Guid? OfferCacheId { get; set; }        

        [DataMember(Name = "msdyn_sourceobjecttype")]
        public string SourceObjectType { get; set; }

        [DataMember(Name = "msdyn_sourceobjectid")]
        public string SourceObjectId { get; set; }

        [DataMember(Name = "msdyn_serializedobject")]
        public string SerializedObject { get; set; }

        [DataMember(Name = "msdyn_offercache_Annotations")]
        public IList<JobOfferCacheAnnotation> Annotation { get; set; }
    }
}
