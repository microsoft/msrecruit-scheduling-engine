//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using HR.TA.Common.XrmHttp;
    
    [ODataEntity(PluralName = "msdyn_jobskills", SingularName = "msdyn_jobskill")]
    public class JobSkill : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobskillid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "msdyn_jobopening_jobskill")]
        public IList<JobOpening> JobOpenings { get; set; }
    }
}
