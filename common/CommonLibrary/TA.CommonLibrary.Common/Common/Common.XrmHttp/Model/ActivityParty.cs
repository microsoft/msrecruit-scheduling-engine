//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Common.XrmHttp;
    using TA.CommonLibrary.Common.XrmHttp.Model;

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
