using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
using MS.GTA.Talent.FalconEntities.Attract;
using MS.GTA.TalentEntities.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Talent.FalconEntities
{
    [DataContract]
    public class JobOpeningStage
    {
        [DataMember(Name = "JobOpeningStageID", EmitDefaultValue = false, IsRequired = false)]
        public string JobOpeningStageID { get; set; }

        [DataMember(Name = "Name", EmitDefaultValue = false, IsRequired = false)]
        public string Name { get; set; }

        [DataMember(Name = "JobStage", EmitDefaultValue = false, IsRequired = false)]
        public JobStage? JobStage { get; set; }

        [DataMember(Name = "DurationInMinutes", EmitDefaultValue = false, IsRequired = false)]
        public long? DurationInMinutes { get; set; }

        [DataMember(Name = "Description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        [DataMember(Name = "Ordinal", EmitDefaultValue = false, IsRequired = false)]
        public long? Ordinal { get; set; }

        [DataMember(Name = "JobOpening", EmitDefaultValue = false, IsRequired = false)]
        public JobOpening JobOpening { get; set; }

        [DataMember(Name = "PluginData", EmitDefaultValue = false, IsRequired = false)]
        public string PluginData { get; set; }

        [DataMember(Name = "JobApplication", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplication> JobApplication { get; set; }

        [DataMember(Name = "JobApplicationActivities", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationActivity> JobApplicationActivities { get; set; }
    }
}
