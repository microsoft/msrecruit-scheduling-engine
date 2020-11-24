using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class JobFamily
    {
        [DataMember(Name = "JobFamilyID", EmitDefaultValue = false, IsRequired = false)]
        public string JobFamilyID { get; set; }

        [DataMember(Name = "Name", EmitDefaultValue = false, IsRequired = false)]
        public string Name { get; set; }

        [DataMember(Name = "Description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "JobOpenings", EmitDefaultValue = false, IsRequired = false)]
        public List<JobOpening> JobOpenings { get; set; }
    }
}

