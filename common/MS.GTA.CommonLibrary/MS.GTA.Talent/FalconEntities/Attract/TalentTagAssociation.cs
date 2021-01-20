using MS.GTA.TalentEntities.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class TalentTagAssociation
    {
        [DataMember(Name = "TalentTagAssociationID", EmitDefaultValue = false, IsRequired = false)]
        public string TalentTagAssociationID { get; set; }

        [DataMember(Name = "Source", EmitDefaultValue = false, IsRequired = false)]
        public TalentTagAssociationExternalSource? Source { get; set; }

        [DataMember(Name = "TalentTag", EmitDefaultValue = false, IsRequired = false)]
        public TalentTag TalentTag { get; set; }
    }
}
