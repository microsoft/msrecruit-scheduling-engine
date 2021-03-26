//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Attract
{
    using System;
    using System.Runtime.Serialization;
    using HR.TA..TalentEntities.Enum;

    [DataContract]
    public class JobApplicationParticipant
    {
        [DataMember(Name = "OID", EmitDefaultValue = false, IsRequired = false)]
        public string OID { get; set; }

        [DataMember(Name = "Role", EmitDefaultValue = false, IsRequired = false)]
        public JobParticipantRole? Role { get; set; }

        /// <summary>Gets or sets the AddedOnDate </summary>
        [DataMember(Name = "AddedOnDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? AddedOnDate { get; set; }
    }
}
