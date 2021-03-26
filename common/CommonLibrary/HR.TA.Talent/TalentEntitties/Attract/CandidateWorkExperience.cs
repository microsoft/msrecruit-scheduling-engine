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

    [ODataEntity(PluralName = "msdyn_candidateworkexperiences", SingularName = "msdyn_candidateworkexperience")]
    public class CandidateWorkExperience : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_candidateworkexperienceid")]
        public Guid? CandidateWorkExperienceID { get; set; }

        [DataMember(Name = "_msdyn_candidateid_value")]
        public Guid? CandidateId { get; set; }

        [DataMember(Name = "msdyn_CandidateId")]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationid_value")]
        public Guid? JobApplicationId { get; set; }

        [DataMember(Name = "msdyn_jobapplicationid")]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "msdyn_title")]
        public string Title { get; set; }

        [DataMember(Name = "msdyn_organization")]
        public string Organization { get; set; }

        [DataMember(Name = "msdyn_location")]
        public string Location { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_iscurrentposition")]
        public bool? IsCurrentPosition { get; set; }

        [DataMember(Name = "msdyn_frommonthyear")]
        public DateTime? FromMonthYear { get; set; }

        [DataMember(Name = "msdyn_tomonthyear")]
        public DateTime? ToMonthYear { get; set; }
    }
}
