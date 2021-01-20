//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOpeningApprovalParticipant.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.TalentEntities.Enum;
    
    [ODataEntity(PluralName = "msdyn_jobopeningapprovalparticipants", SingularName = "msdyn_jobopeningapprovalparticipant")]
    public class JobOpeningApprovalParticipant : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobopeningapprovalparticipantid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "_msdyn_jobopeningid_value")]
        public Guid? JobOpeningId { get; set; }

        [DataMember(Name = "msdyn_JobopeningId")]
        public JobOpening JobOpening { get; set; }

        [DataMember(Name = "_msdyn_workerid_value")]
        public Guid? WorkerId { get; set; }

        [DataMember(Name = "msdyn_workerid")]
        public Worker Worker { get; set; }

        [DataMember(Name = "msdyn_jobopeningapprovalcomment")]
        public string Comment { get; set; }

        [DataMember(Name = "msdyn_jobopeningapprovalstatus")]
        public JobApprovalStatus? JobApprovalStatus { get; set; }
    }
}
