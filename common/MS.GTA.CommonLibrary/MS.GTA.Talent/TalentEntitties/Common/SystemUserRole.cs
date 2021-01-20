//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Common
{
    using System;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;

    [ODataEntity(PluralName = "systemuserrolescollection", SingularName = "systemuserroles")]
    public class SystemUserRole : XrmODataEntity
    {
        [DataMember(Name = "systemuserid")]
        public Guid? SystemUserId { get; set; }

        [DataMember(Name = "roleid")]
        public Guid? RoleId { get; set; }
    }
}