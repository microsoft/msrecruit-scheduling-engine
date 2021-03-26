//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA..Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using HR.TA..Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_candidateskills", SingularName = "msdyn_candidateskill")]
    public class CandidateSkill : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_candidateskillid")]
        public Guid? CandidateSkillID { get; set; }

        [DataMember(Name = "_msdyn_candidateid_value")]
        public Guid? CandidateId { get; set; }

        [DataMember(Name = "msdyn_CandidateId")]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationid_value")]
        public Guid? JobApplicationId { get; set; }

        [DataMember(Name = "msdyn_jobapplicationid")]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "msdyn_skill")]
        public string Skill { get; set; }
    }
}
