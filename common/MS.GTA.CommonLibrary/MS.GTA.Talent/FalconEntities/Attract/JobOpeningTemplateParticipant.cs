using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class JobOpeningTemplateParticipant
    {
        [DataMember(Name = "JobOpeningTemplateParticipantID", EmitDefaultValue = false, IsRequired = false)]
        public string JobOpeningTemplateParticipantID { get; set; }

        [DataMember(Name = "UserObjectId", EmitDefaultValue = false, IsRequired = false)]
        public string UserObjectId { get; set; }

        [DataMember(Name = "GroupObjectId", EmitDefaultValue = false, IsRequired = false)]
        public string GroupObjectId { get; set; }

        [DataMember(Name = "TenantObjectId", EmitDefaultValue = false, IsRequired = false)]
        public string TenantObjectId { get; set; }

        [DataMember(Name = "IsDefault", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsDefault { get; set; }

        [DataMember(Name = "CanEdit", EmitDefaultValue = false, IsRequired = false)]
        public bool? CanEdit { get; set; }

        [DataMember(Name = "JobOpeningTemplate", EmitDefaultValue = false, IsRequired = false)]
        public JobOpening JobOpeningTemplate { get; set; }
    }
}
