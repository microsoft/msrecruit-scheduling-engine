//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using HR.TA.Common.Provisioning.Entities.FalconEntities.Attract;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HR.TA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class CandidateSkill
    {
        [DataMember(Name = "CandidateSkillID", EmitDefaultValue = false, IsRequired = false)]
        public string CandidateSkillID { get; set; }

        [DataMember(Name = "Skill", EmitDefaultValue = false, IsRequired = false)]
        public string Skill { get; set; }
    }
}
