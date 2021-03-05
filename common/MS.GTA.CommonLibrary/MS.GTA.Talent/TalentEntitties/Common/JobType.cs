//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace Common.Provisioning.Entities.XrmEntities.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.XrmHttp;
    using Common.Provisioning.Entities.XrmEntities.Attract;

    [ODataEntity(PluralName = "cdm_jobtypes", SingularName = "cdm_jobtype")]
    public class JobType : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "cdm_jobtypeid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "cdm_jobtypenumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "cdm_description")]
        public string Description { get; set; }

        [DataMember(Name = "cdm_jobtype_job")]
        public IList<Job> Jobs { get; set; }

        /*
                <Property Name="cdm_exemptstatus" Type="Edm.Int32" />
        */
    }
}
