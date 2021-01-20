//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.TalentEntities.Enum;

    [ODataEntity(PluralName = "msdyn_jobopeningparticipants", SingularName = "msdyn_jobopeningparticipant")]
    public class JobOpeningParticipant : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobopeningparticipantid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "msdyn_istemplateparticipant")]
        public bool? IsTemplateParticipant { get; set; }

        [DataMember(Name = "msdyn_tenantobjectid")]
        public string TenantObjectId { get; set; }

        [DataMember(Name = "msdyn_groupobjectid")]
        public string GroupObjectId { get; set; }

        [DataMember(Name = "msdyn_isdefaulttemplate")]
        public bool? IsDefaultTemplate { get; set; }

        [DataMember(Name = "msdyn_canedittemplate")]
        public bool? CanEditTemplate { get; set; }

        [DataMember(Name = "_msdyn_jobopeningid_value")]
        public Guid? JobOpeningId { get; set; }

        [DataMember(Name = "msdyn_JobopeningId")]
        public JobOpening JobOpening { get; set; }

        [DataMember(Name = "_msdyn_workerid_value")]
        public Guid? WorkerId { get; set; }

        [DataMember(Name = "msdyn_WorkerId")]
        public Worker Worker { get; set; }

        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_role")]
        public JobParticipantRole? Role { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        // TODO
        /*
        [DataMember(Name = "msdyn_jobopeningparticipant_jobapplicationcomme")]
        public IList<JobApplicationComment> JobApplicationComments { get; set; }
        */
        [DataMember(Name = "msdyn_jobopeningparticipant_jobapplicationactiv")]
        public IList<JobApplicationActivityParticipant> JobApplicationActivityParticipants { get; set; }

        [DataMember(Name = "msdyn_jobopeningparticipant_cdm_worker")]
        public IList<Worker> Delegates { get; set; }
    }
}
