//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;
    using MS.GTA.Common.DocumentDB.Contracts;

    [DataContract]
    public class JobApplicationActivityParticipant : DocDbEntity
    {
        [DataMember(Name = "JobApplicationActivityParticipantID", EmitDefaultValue = false, IsRequired = false)]
        public string JobApplicationActivityParticipantID { get; set; }

        [DataMember(Name = "JobParticipantRole", EmitDefaultValue = false, IsRequired = false)]
        public JobParticipantRole? JobParticipantRole { get; set; }

        [DataMember(Name = "JobApplicationParticipant", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationParticipant JobApplicationParticipant { get; set; }

        [DataMember(Name = "JobApplicationActivity", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationActivity JobApplicationActivity { get; set; }

        [DataMember(Name = "JobOpeningParticipant", EmitDefaultValue = false, IsRequired = false)]
        public JobOpeningParticipant JobOpeningParticipant { get; set; }
    }
}
