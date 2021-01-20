using MS.GTA.Common.XrmHttp;
using MS.GTA.TalentEntities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    [ODataEntity(PluralName = "msdyn_talenttagassociations", SingularName = "msdyn_talenttagassociation")]
    public class TalentTagAssociation : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_talenttagassociationid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "_msdyn_talenttagid_value")]
        public Guid? TalentTagId { get; set; }

        [DataMember(Name = "msdyn_talenttagid")]
        public TalentTag TalentTag { get; set; }

        [DataMember(Name = "_msdyn_candidatetrackingid_value")]
        public Guid? CandidateTrackingId { get; set; }

        [DataMember(Name = "msdyn_candidatetrackingid")]
        public CandidateTracking CandidateTracking { get; set; }
    }
}
