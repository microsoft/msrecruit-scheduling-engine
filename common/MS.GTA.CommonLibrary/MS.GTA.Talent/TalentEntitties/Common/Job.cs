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

    [ODataEntity(PluralName = "cdm_jobs", SingularName = "cdm_job")]
    public class Job : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "cdm_jobid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "cdm_jobnumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "cdm_description")]
        public string Description { get; set; }

        [DataMember(Name = "cdm_allowedunlimitedpositions")]
        public bool? AllowUnlimitedPositions { get; set; }

        [DataMember(Name = "cdm_defaultfulltimeequivalent")]
        public decimal? DefaultFullTimeEquivalent { get; set; }

        [DataMember(Name = "cdm_maximumnumberofpositions")]
        public int? MaximumNumberOfPositions { get; set; }

        [DataMember(Name = "cdm_validfrom")]
        public DateTime? ValidFrom { get; set; }

        [DataMember(Name = "cdm_validto")]
        public DateTime? ValidTo { get; set; }

        [DataMember(Name = "_cdm_jobfunctionid_value")]
        public Guid? JobFunctionId { get; set; }

        [DataMember(Name = "cdm_jobfunctionid")]
        public JobFunction JobFunction { get; set; }

        [DataMember(Name = "_cdm_jobtypeid_value")]
        public Guid? JobTypeId { get; set; }

        [DataMember(Name = "cdm_jobtypeid")]
        public JobType JobType { get; set; }

        [DataMember(Name = "cdm_job_jobposition")]
        public IList<JobPosition> JobPositions { get; set; }

        /*
        <Property Name="cdm_name" Type="Edm.String" Unicode="false" />
        <Property Name="cdm_title" Type="Edm.Int32" />
        */
    }
}
