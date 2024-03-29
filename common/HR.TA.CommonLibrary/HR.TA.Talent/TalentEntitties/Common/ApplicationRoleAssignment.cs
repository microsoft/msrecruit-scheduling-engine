//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using HR.TA.Common.Web.Contracts;
    using HR.TA.Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_applicationroleassignments", SingularName = "msdyn_applicationroleassignment")]
    public class ApplicationRoleAssignment : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_applicationroleassignmentid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "msdyn_talentapplicationrole")]
        public TalentApplicationRole? TalentApplicationRole { get; set; }

        [DataMember(Name = "_msdyn_workerid_value")]
        public Guid? WorkerId { get; set; }

        [DataMember(Name = "msdyn_workerid")]
        public Worker Worker { get; set; }
    }
}
