//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
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
