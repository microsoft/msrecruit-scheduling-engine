//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidatePersonalDetails.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.TalentEntities.Enum;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text;


    [ODataEntity(PluralName = "msdyn_candidatepersonaldetails", SingularName = "msdyn_candidatepersonaldetail")]
    public class CandidatePersonalDetails : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_candidatepersonaldetailid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_gender")]
        public CandidateGender? Gender { get; set; }

        [DataMember(Name = "msdyn_birthdate")]
        public DateTime? Birthdate { get; set; }

        [DataMember(Name = "msdyn_ethnicorigin")]
        public CandidateEthnicOrigin? Ethnicity { get; set; }

        [DataMember(Name = "msdyn_disabilitystatus")]
        public CandidateDisabilityStatus? DisabilityStatus { get; set; }

        [DataMember(Name = "msdyn_veteranstatus")]
        public CandidateVeteranStatus? VeteranStatus { get; set; }

        [DataMember(Name = "msdyn_militarystatus")]
        public CandidateMilitaryStatus? MilitaryStatus { get; set; }

        [DataMember(Name = "_msdyn_candidateid_value")]
        public Guid? CandidateId { get; set; }

        [DataMember(Name = "msdyn_CandidateId")]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationactivityid_value")]
        public Guid? JobApplicationActivityId { get; set; }

        [DataMember(Name = "msdyn_JobapplicationactivityId")]
        public JobApplicationActivity JobApplicationActivity { get; set; }
    }
}
