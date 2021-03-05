//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.XrmHttp;

    [ODataEntity(PluralName = "cdm_jobfunctions", SingularName = "cdm_jobfunction")]
    public class JobFunction : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "cdm_jobfunctionid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "cdm_description")]
        public string Name { get; set; }

        [DataMember(Name = "msdyn_jobopening_cdm_jobfunction")]
        public IList<JobOpening> JobOpenings { get; set; }
    }
}
