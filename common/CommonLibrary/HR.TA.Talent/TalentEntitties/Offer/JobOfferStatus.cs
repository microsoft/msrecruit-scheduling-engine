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

    [ODataEntity(PluralName = "msdyn_jobofferstatuses", SingularName = "msdyn_jobofferstatus")]
    public class JobOfferStatus : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferstatusid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_status")]
        public Optionset.JobOfferStatus? Status { get; set; }

        [DataMember(Name = "msdyn_statusreason")]
        public Optionset.JobOfferStatusReason? StatusReason { get; set; }

        [DataMember(Name = "msdyn_jobofferstatus_joboffer_jobofferstatusid")]
        public IList<JobOffer> JobOffers { get; set; }
    }
}
