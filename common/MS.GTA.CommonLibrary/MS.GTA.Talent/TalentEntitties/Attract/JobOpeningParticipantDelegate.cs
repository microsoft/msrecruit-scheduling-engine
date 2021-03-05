//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.Runtime.Serialization;
    using Common.XrmHttp;

    [ODataEntity(SingularName = "msdyn_jobopeningparticipant_cdm_worker", PluralName = "msdyn_jobopeningparticipant_cdm_workerset")]
    public class JobOpeningParticipantDelegate : ODataEntity
    {
        [DataMember(Name = "msdyn_jobopeningparticipantid")]
        public Guid? JobOpeningParticipantId { get; set; }

        [DataMember(Name = "cdm_workerid")]
        public Guid? WorkerId { get; set; }
    }
}
