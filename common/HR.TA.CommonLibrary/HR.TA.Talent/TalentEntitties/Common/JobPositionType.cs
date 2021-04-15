//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.XrmHttp;
    ////using HR.TA.Common.Provisioning.Entities.XrmEntities.Attract;

    [ODataEntity(PluralName = "cdm_positiontypes", SingularName = "cdm_positiontype")]
    public class JobPositionType : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "cdm_positiontypeid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "cdm_positiontypenumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "cdm_description")]
        public string Description { get; set; }

        [DataMember(Name = "cdm_positiontype_jobposition")]
        public IList<JobPosition> JobPositions { get; set; }

        /*
                <Property Name="cdm_classification" Type="Edm.Int32" />
        */
    }
}
