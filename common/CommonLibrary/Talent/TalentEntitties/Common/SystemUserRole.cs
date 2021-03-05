//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace Common.Provisioning.Entities.XrmEntities.Common
{
    using System;
    using System.Runtime.Serialization;
    using Common.XrmHttp;

    [ODataEntity(PluralName = "systemuserrolescollection", SingularName = "systemuserroles")]
    public class SystemUserRole : XrmODataEntity
    {
        [DataMember(Name = "systemuserid")]
        public Guid? SystemUserId { get; set; }

        [DataMember(Name = "roleid")]
        public Guid? RoleId { get; set; }
    }
}
