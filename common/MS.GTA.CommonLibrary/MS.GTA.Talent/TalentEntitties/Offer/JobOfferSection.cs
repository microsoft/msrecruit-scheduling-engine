//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_joboffersections", SingularName = "msdyn_joboffersection")]
    public class JobOfferSection : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_joboffersectionid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_sectionname")]
        public string SectionName { get; set; }

        [DataMember(Name = "msdyn_sectioncolor")]
        public string SectionColor { get; set; }

        [DataMember(Name = "msdyn_ordinalnumber")]
        public int? OrdinalNumber { get; set; }

        [DataMember(Name = "_msdyn_jobofferid_value")]
        public Guid? JobOfferRecId { get; set; }

        [DataMember(Name = "msdyn_jobofferid")]
        public JobOffer JobOffer { get; set; }

        [DataMember(Name = "msdyn_joboffersection_joboffertokenvalue")]
        public IList<JobOfferTokenValue> Tokens { get; set; }
    }
}
