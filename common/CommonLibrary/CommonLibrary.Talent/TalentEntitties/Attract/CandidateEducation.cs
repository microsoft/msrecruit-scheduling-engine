//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_candidateeducations", SingularName = "msdyn_candidateeducation")]
    public class CandidateEducation : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_candidateeducationid")]
        public Guid? CandidateEducationID { get; set; }

        [DataMember(Name = "_msdyn_candidateid_value")]
        public Guid? CandidateId { get; set; }

        [DataMember(Name = "msdyn_CandidateId")]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationid_value")]
        public Guid? JobApplicationId { get; set; }

        [DataMember(Name = "msdyn_jobapplicationid")]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "msdyn_school")]
        public string School { get; set; }

        [DataMember(Name = "msdyn_degree")]
        public string Degree { get; set; }

        [DataMember(Name = "msdyn_fieldofstudy")]
        public string FieldOfStudy { get; set; }

        [DataMember(Name = "msdyn_grade")]
        public string Grade { get; set; }

        [DataMember(Name = "msdyn_activitiessocieties")]
        public string ActivitiesSocieties { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_frommonthyear")]
        public DateTime? FromMonthYear { get; set; }

        [DataMember(Name = "msdyn_tomonthyear")]
        public DateTime? ToMonthYear { get; set; }
    }
}
