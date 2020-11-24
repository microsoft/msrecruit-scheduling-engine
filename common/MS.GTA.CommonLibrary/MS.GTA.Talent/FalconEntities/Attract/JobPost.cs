using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
using MS.GTA.TalentEntities.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class JobPost
    {
        [DataMember(Name = "JobPostID", EmitDefaultValue = false, IsRequired = false)]
        public string JobPostID { get; set; }

        [DataMember(Name = "JobPostURI", EmitDefaultValue = false, IsRequired = false)]
        public string JobPostURI { get; set; }

        [DataMember(Name = "PublicationStartDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? PublicationStartDate { get; set; }

        [DataMember(Name = "PublicationCloseDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? PublicationCloseDate { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = false, IsRequired = false)]
        public JobPostStatus? Status { get; set; }

        [DataMember(Name = "StatusReason", EmitDefaultValue = false, IsRequired = false)]
        public JobPostStatusReason? StatusReason { get; set; }

        [DataMember(Name = "JobPostSupplier", EmitDefaultValue = false, IsRequired = false)]
        public JobPostSupplier? JobPostSupplier { get; set; }

        [DataMember(Name = "JobPostSupplierName", EmitDefaultValue = false, IsRequired = false)]
        public string JobPostSupplierName { get; set; }

        [DataMember(Name = "JobOpening", EmitDefaultValue = false, IsRequired = false)]
        public JobOpening JobOpening { get; set; }

        [DataMember(Name = "JobPostExtendedAttributes", EmitDefaultValue = false, IsRequired = false)]
        public IList<CustomAttributes> JobPostExtendedAttributes { get; set; }
    }
}
