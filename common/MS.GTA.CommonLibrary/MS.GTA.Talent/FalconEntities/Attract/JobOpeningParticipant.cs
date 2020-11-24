//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.TalentEntities.Enum;

    [DataContract]
    public class JobOpeningParticipant
    {
        [DataMember(Name = "JobOpeningParticipantID", EmitDefaultValue = false, IsRequired = false)]
        public string JobOpeningParticipantID { get; set; }

        [DataMember(Name = "Worker", EmitDefaultValue = false, IsRequired = false)]
        public Worker Worker { get; set; }

        [DataMember(Name = "Role", EmitDefaultValue = false, IsRequired = false)]
        public JobParticipantRole? Role { get; set; }

        [DataMember(Name = "OID", EmitDefaultValue = false, IsRequired = false)]
        public string OID { get; set; }

        [DataMember(Name = "Delegates", EmitDefaultValue = false, IsRequired = false)]
        public IList<Worker> Delegates { get; set; }
    }
}
