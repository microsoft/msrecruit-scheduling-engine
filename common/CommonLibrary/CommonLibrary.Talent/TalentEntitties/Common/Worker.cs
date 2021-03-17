//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    using CommonLibrary.Common.XrmHttp;   
    ////using CommonLibrary.Common.Provisioning.Entities.XrmEntities.Onboard;
    using CommonLibrary.Common.XrmHttp.Model;
    ////using CommonLibrary.Common.Provisioning.Entities.XrmEntities.Attract;

    [ODataEntity(PluralName = "cdm_workers", SingularName = "cdm_worker")]
    public class Worker : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "cdm_workerid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "cdm_officegraphidentifier")]
        public string OfficeGraphIdentifier { get; set; }

        [DataMember(Name = "cdm_primaryemailaddress")]
        public string EmailPrimary { get; set; }

        [DataMember(Name = "_msdyn_userid_value")]
        public Guid? SystemUserId { get; set; }

        [DataMember(Name = "msdyn_UserId")]
        public SystemUser SystemUser { get; set; }

        [DataMember(Name = "cdm_primarytelephone")]
        public string PhonePrimary { get; set; }

        [DataMember(Name = "cdm_profession")]
        public string Profession { get; set; }

        [DataMember(Name = "cdm_fullname")]
        public string FullName { get; set; }

        [DataMember(Name = "cdm_firstname")]
        public string GivenName { get; set; }

        [DataMember(Name = "cdm_middlename")]
        public string MiddleName { get; set; }

        [DataMember(Name = "cdm_lastname")]
        public string Surname { get; set; }

        [DataMember(Name = "cdm_linkedinidentity")]
        public string LinkedInIdentity { get; set; }

        // TODO
        /*
        [DataMember(Name = "msdyn_cdm_worker_msdyn_onboardingartifact")]
        public IEnumerable<OnboardingArtifact> OnboardingArtifacts { get; set; }

        [DataMember(Name = "msdyn_cdm_worker_msdyn_onboardingprojectpart")]
        public IEnumerable<OnboardingProjectParticipant> OnboardingProjectParticipants { get; set; }

        [DataMember(Name = "msdyn_cdm_worker_msdyn_onboardprojactpart")]
        public IEnumerable<OnboardingProjectActivityParticipant> OnboardingActivityParticipants { get; set; }

        [DataMember(Name = "msdyn_cdm_worker_msdyn_onboardingproject")]
        public IEnumerable<OnboardingProject> OnboardingProjects { get; set; }
        */
        [DataMember(Name = "msdyn_linkedinmemberreference")]
        public string LinkedInMemberReference { get; set; }
    }
}
