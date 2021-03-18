//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.XrmHttp;
    using CommonLibrary.Common.Provisioning.Entities.XrmEntities.Attract;

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
