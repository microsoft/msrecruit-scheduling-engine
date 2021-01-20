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
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset;

    [ODataEntity(PluralName = "msdyn_jobofferparticipants", SingularName = "msdyn_jobofferparticipant")]
    public class JobOfferParticipant : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferparticipantid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_role")]
        public JobOfferParticipantRole? Role { get; set; }

        [DataMember(Name = "msdyn_jobofferparticipantordinal")]
        public long? JobOfferParticipantOrdinal { get; set; }

        [DataMember(Name = "_msdyn_workerid_value")]
        public Guid? WorkerRecId { get; set; }

        [DataMember(Name = "msdyn_workerId")]
        public Worker Worker { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationactivityparticipaid_value")]
        public Guid? JobApplicationActivityParticipantRecId { get; set; }

        [DataMember(Name = "msdyn_jobapplicationactivityparticipaid")]
        public JobApplicationActivityParticipant JobApplicationActivityParticipant { get; set; }

        [DataMember(Name = "_msdyn_approval_value")]
        public Guid? ApprovalRecId { get; set; }

        [DataMember(Name = "msdyn_approval")]
        public JobOfferApproval Approval { get; set; }

        [DataMember(Name = "_msdyn_jobofferid_value")]
        public Guid? JobOfferRecId { get; set; }

        [DataMember(Name = "msdyn_jobofferId")]
        public JobOffer JobOffer { get; set; }

        [DataMember(Name = "msdyn_jobofferlasteditedon")]
        public DateTime? JobOfferLastEditedOn { get; set; }
    }
}