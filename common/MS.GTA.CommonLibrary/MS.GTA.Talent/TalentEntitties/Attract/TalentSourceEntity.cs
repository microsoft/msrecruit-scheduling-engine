//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_talentsources", SingularName = "msdyn_talentsource")]
    public class TalentSourceEntity : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_talentsourceid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_domain")]
        public string Domain { get; set; }

        [DataMember(Name = "msdyn_talentsourcecategory")]
        public int? TalentSourceCategory { get; set; }

    }
}
