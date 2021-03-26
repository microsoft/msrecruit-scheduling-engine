//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using TA.CommonLibrary.Common.Provisioning.Entities.FalconEntities.Attract;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TA.CommonLibrary.Talent.FalconEntities.Attract
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
