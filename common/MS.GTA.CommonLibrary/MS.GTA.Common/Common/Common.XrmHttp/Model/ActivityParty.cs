//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "activityparties", SingularName = "activityparty")]
    public class ActivityParty : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "activitypartyid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "partyid_systemuser")]
        public SystemUser SystemUser { get; set; }

        [DataMember(Name = "partyid_contact")]
        public Contact Contact { get; set; }

    }
}
