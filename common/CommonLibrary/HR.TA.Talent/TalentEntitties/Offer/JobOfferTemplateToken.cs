//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using HR.TA.Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_joboffertemplatetokens", SingularName = "msdyn_joboffertemplatetoken")]
    public class JobOfferTemplateToken : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_joboffertemplatetokenid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_tokenkey")]
        public string TokenKey { get; set; }

        [DataMember(Name = "msdyn_inuse")]
        public bool? InUse { get; set; }

        [DataMember(Name = "_msdyn_joboffertemplatesectionid_value")]
        public Guid? JobOfferTemplateSectionId { get; set; }

        [DataMember(Name = "msdyn_ordinalnumber")]
        public int? OrdinalNumber { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatesectionid")]
        public JobOfferTemplateSection JobOfferTemplateSection { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatetoken_joboffer")]
        public IList<JobOfferToken> JobOfferTokens { get; set; }
    }
}
