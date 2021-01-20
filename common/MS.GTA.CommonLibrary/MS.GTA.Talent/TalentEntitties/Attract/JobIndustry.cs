using MS.GTA.Common.XrmHttp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    [ODataEntity(PluralName = "msdyn_jobindustries", SingularName = "msdyn_jobindustry")]
    public class JobIndustry : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobindustryid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "msdyn_jobopening_jobindustry")]
        public IList<JobOpening> JobOpenings { get; set; }
    }
}
