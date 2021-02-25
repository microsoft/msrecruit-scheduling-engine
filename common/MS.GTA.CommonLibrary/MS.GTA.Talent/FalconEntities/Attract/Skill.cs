//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class Skill
    {
        [DataMember(Name = "SkillID", EmitDefaultValue = false, IsRequired = false)]
        public string SkillID { get; set; }

        [DataMember(Name = "Name", EmitDefaultValue = false, IsRequired = false)]
        public string Name { get; set; }
    }
}
