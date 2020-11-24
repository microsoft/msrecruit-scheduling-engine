//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;

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