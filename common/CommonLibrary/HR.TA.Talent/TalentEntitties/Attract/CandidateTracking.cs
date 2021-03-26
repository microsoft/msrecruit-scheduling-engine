//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
using HR.TA..Common.XrmHttp;
using HR.TA..TalentEntities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace HR.TA..Common.Provisioning.Entities.XrmEntities.Attract
{
    [ODataEntity(PluralName = "msdyn_candidatetrackings", SingularName = "msdyn_candidatetracking")]
    public class CandidateTracking : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_candidatetrackingid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_trackingcategory")]
        public CandidateTrackingCategory? TrackingCategory { get; set; }

        [DataMember(Name = "_msdyn_candidateid_value")]
        public Guid? CandidateId { get; set; }

        [DataMember(Name = "msdyn_candidateid")]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "msdyn_candidatetracking_talenttagassociation")]
        public IList<TalentTagAssociation> TalentTagAssociations { get; set; }
    }
}
