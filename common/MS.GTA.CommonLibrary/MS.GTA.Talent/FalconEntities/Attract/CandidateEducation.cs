using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class CandidateEducation
    {
        [DataMember(Name = "CandidateEducationID", EmitDefaultValue = false, IsRequired = false)]
        public string CandidateEducationID { get; set; }

        [DataMember(Name = "School", EmitDefaultValue = false, IsRequired = false)]
        public string School { get; set; }

        [DataMember(Name = "Degree", EmitDefaultValue = false, IsRequired = false)]
        public string Degree { get; set; }

        [DataMember(Name = "FieldOfStudy", EmitDefaultValue = false, IsRequired = false)]
        public string FieldOfStudy { get; set; }

        [DataMember(Name = "Grade", EmitDefaultValue = false, IsRequired = false)]
        public string Grade { get; set; }

        [DataMember(Name = "ActivitiesSocieties", EmitDefaultValue = false, IsRequired = false)]
        public string ActivitiesSocieties { get; set; }

        [DataMember(Name = "Description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "FromMonthYear", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? FromMonthYear { get; set; }

        [DataMember(Name = "ToMonthYear", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ToMonthYear { get; set; }
    }
}
