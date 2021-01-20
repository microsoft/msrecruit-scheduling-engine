//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using MS.GTA.Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_joboffertemplatesections", SingularName = "msdyn_joboffertemplatesection")]
    public class JobOfferTemplateSection : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_joboffertemplatesectionid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_sectionname")]
        public string SectionName { get; set; }

        [DataMember(Name = "msdyn_sectioncolor")]
        public string SectionColor { get; set; }

        [DataMember(Name = "_msdyn_joboffertemplateid_value")]
        public Guid? JobOfferTemplateId { get; set; }

        [DataMember(Name = "msdyn_JoboffertemplateId")]
        public JobOfferTemplate JobOfferTemplate { get; set; }

        [DataMember(Name = "msdyn_ordinalnumber")]
        public int? OrdinalNumber { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatesection_templatetetoken")]
        public IList<JobOfferTemplateToken> Tokens { get; set; }
    }
}
