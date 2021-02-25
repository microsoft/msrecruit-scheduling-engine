//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset;

    [ODataEntity(PluralName = "msdyn_jobofferapprovals", SingularName = "msdyn_jobofferapproval")]
    public class JobOfferApproval : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferapprovalid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_status")]
        public JobOfferApprovalStatus? Status { get; set; }

        [DataMember(Name = "msdyn_statusreason")]
        public JobOfferApprovalStatusReason? StatusReason { get; set; }

        [DataMember(Name = "msdyn_comment")]
        public string Comment { get; set; }

        [DataMember(Name = "msdyn_requestdate")]
        public DateTime? RequestDate { get; set; }

        [DataMember(Name = "msdyn_responddate")]
        public DateTime? RespondDate { get; set; }

        [DataMember(Name = "msdyn_issubmitted")]
        public bool? IsSubmitted { get; set; }

        [DataMember(Name = "msdyn_submittedby")]
        public Worker SubmittedBy { get; set; }

        [DataMember(Name = "_msdyn_submittedby_value")]
        public Guid? SubmittedById { get; set; }

        [DataMember(Name = "msdyn_jobofferapproval_jobofferparticipant_approval")]
        public JobOfferParticipant JobOfferParticipant { get; set; }

        [DataMember(Name = "_msdyn_jobofferapproval_jobofferparticipant_approval_value")]
        public Guid? JobOfferParticipantId { get; set; }
    }
}
