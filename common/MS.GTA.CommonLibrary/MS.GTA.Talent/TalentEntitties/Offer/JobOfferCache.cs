//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;

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
