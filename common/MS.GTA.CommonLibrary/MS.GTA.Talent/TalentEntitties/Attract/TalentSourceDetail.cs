//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;

    [ODataEntity(PluralName = "msdyn_talentsourcedetails", SingularName = "msdyn_talentsourcedetail")]
    public class TalentSourceDetail : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_talentsourcedetailid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Comment { get; set; }

        [DataMember(Name = "_msdyn_workerid_value")]
        public Guid? WorkerId { get; set; }

        [DataMember(Name = "msdyn_workerid")]
        public Worker Worker { get; set; }

        [DataMember(Name = "_msdyn_candidateid_value")]
        public Guid? CandidateId { get; set; }

        [DataMember(Name = "msdyn_candidateid")]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationid_value")]
        public Guid? JobApplicationId { get; set; }

        [DataMember(Name = "msdyn_jobapplicationid")]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "_msdyn_talentsourceid_value")]
        public Guid? TalentSourceId { get; set; }

        [DataMember(Name = "msdyn_talentsourceid")]
        public TalentSourceEntity TalentSource { get; set; }

    }
}
