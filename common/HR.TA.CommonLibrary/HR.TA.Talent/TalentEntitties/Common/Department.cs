//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using HR.TA.Common.XrmHttp;

    [ODataEntity(PluralName = "cdm_departments", SingularName = "cdm_department")]
    public class Department : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "cdm_departmentid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "cdm_departmentnumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "cdm_description")]
        public string Description { get; set; }

        [DataMember(Name = "cdm_name")]
        public string Name { get; set; }

        [DataMember(Name = "_cdm_parentdepartmentid_value")]
        public Guid? ParentDepartmentId { get; set; }

        [DataMember(Name = "cdm_parentdepartmentid")]
        public Department ParentDepartment { get; set; }

        [DataMember(Name = "cdm_department_jobposition")]
        public IList<JobPosition> JobPositions { get; set; }
    }
}
