using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
using MS.GTA.Common.TalentEntities.Common;
using MS.GTA.TalentEntities.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class SocialNetworkIdentity
    {
        [DataMember(Name = "Candidate", EmitDefaultValue = false, IsRequired = false)]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "Worker", EmitDefaultValue = false, IsRequired = false)]
        public Worker Worker { get; set; }

        [DataMember(Name = "Provider", EmitDefaultValue = false)]
        public SocialNetworkProvider? Provider { get; set; }

        [DataMember(Name = "ProviderMemberId", EmitDefaultValue = false)]
        public string ProviderMemberId { get; set; }

        [DataMember(Name = "ProfileUrl", EmitDefaultValue = false, IsRequired = false)]
        public string ProfileUrl { get; set; }
    }
}
