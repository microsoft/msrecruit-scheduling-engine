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
    using MS.GTA.Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_jobofferapprovalemails", SingularName = "msdyn_jobofferapprovalemail")]
    public class JobOfferApprovalEmail : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferapprovalemailid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_emailsubject")]
        public string EmailSubject { get; set; }

        [DataMember(Name = "msdyn_emailcontent")]
        public string EmailContent { get; set; }

        [DataMember(Name = "msdyn_emailcc")]
        public string EmailCc { get; set; }

        [DataMember(Name = "msdyn_jobofferId")]
        public JobOffer JobOffer { get; set; }

        [DataMember(Name = "_msdyn_jobofferid_value")]
        public Guid? JobOfferRecId { get; set; }
    }
}