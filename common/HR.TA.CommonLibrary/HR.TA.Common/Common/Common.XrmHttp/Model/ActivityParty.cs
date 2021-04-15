//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using HR.TA.Common.XrmHttp;
    using HR.TA.Common.XrmHttp.Model;

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
