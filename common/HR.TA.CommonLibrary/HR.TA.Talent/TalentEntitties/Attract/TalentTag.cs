//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
using HR.TA.Common.XrmHttp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace HR.TA.Common.Provisioning.Entities.XrmEntities.Attract
{
    [ODataEntity(PluralName = "msdyn_talenttags", SingularName = "msdyn_talenttag")]
    public class TalentTag : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_talenttagid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_tag")]
        public string Tag { get; set; }

        [DataMember(Name = "msdyn_talenttag_talenttagassociation")]
        public IList<TalentTagAssociation> TalentTagAssociations { get; set; }
    }
}
