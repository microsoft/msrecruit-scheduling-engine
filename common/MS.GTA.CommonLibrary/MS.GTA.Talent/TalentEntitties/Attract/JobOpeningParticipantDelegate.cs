//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;

    [ODataEntity(SingularName = "msdyn_jobopeningparticipant_cdm_worker", PluralName = "msdyn_jobopeningparticipant_cdm_workerset")]
    public class JobOpeningParticipantDelegate : ODataEntity
    {
        [DataMember(Name = "msdyn_jobopeningparticipantid")]
        public Guid? JobOpeningParticipantId { get; set; }

        [DataMember(Name = "cdm_workerid")]
        public Guid? WorkerId { get; set; }
    }
}