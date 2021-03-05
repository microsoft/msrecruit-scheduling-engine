//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Common.XrmHttp;
    using Common.Provisioning.Entities.XrmEntities.Common;
    using Common.Provisioning.Entities.XrmEntities.Optionset;
    using TalentEntities.Enum;

    [ODataEntity(PluralName = "msdyn_candidates", SingularName = "msdyn_candidate")]
    public class Candidate : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_candidateid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_autonumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "_msdyn_workerid_value")]
        public Guid? WorkerId { get; set; }

        [DataMember(Name = "msdyn_WorkerId")]
        public Worker Worker { get; set; }

        [DataMember(Name = "msdyn_candidatestatus")]
        public CandidateStatus? Status { get; set; }

        [DataMember(Name = "msdyn_candidatestatusreason")]
        public CandidateStatusReason? StatusReason { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_comment")]
        public string Comment { get; set; }

        [DataMember(Name = "msdyn_emailalternate")]
        public string EmailAlternate { get; set; }

        [DataMember(Name = "msdyn_emailprimary")]
        public string EmailPrimary { get; set; }

        [DataMember(Name = "msdyn_givenname")]
        public string GivenName { get; set; }

        [DataMember(Name = "msdyn_middlename")]
        public string MiddleName { get; set; }

        [DataMember(Name = "msdyn_surname")]
        public string Surname { get; set; }

        [DataMember(Name = "msdyn_b2cobjectid")]
        public string B2CObjectId { get; set; }

        [DataMember(Name = "msdyn_facebookid")]
        public string FacebookId { get; set; }

        [DataMember(Name = "msdyn_linkedinapiurl")]
        public string LinkedInApiUrl { get; set; }

        [DataMember(Name = "msdyn_linkedinid")]
        public string LinkedInId { get; set; }

        [DataMember(Name = "msdyn_linkedinprofile")]
        public string LinkedInProfile { get; set; }

        [DataMember(Name = "msdyn_twitterid")]
        public string TwitterId { get; set; }

        [DataMember(Name = "msdyn_homephone")]
        public string HomePhone { get; set; }

        [DataMember(Name = "msdyn_mobilephone")]
        public string MobilePhone { get; set; }

        [DataMember(Name = "msdyn_workphone")]
        public string WorkPhone { get; set; }

        [DataMember(Name = "msdyn_isopentonewjob")]
        public bool? IsOpenToNewJob { get; set; }

        [DataMember(Name = "msdyn_iswillingtorelocate")]
        public bool? IsWillingToRelocate { get; set; }

        [DataMember(Name = "msdyn_gender")]
        public Gender? Gender { get; set; }
        
        [DataMember(Name = "msdyn_additionalmetadata")]
        public string AdditionalMetadata { get; set; }
        
        [DataMember(Name = "msdyn_candidate_jobapplication")]
        public IList<JobApplication> JobApplications { get; set; }

        // TODO
        /*
        [DataMember(Name = "msdyn_talentpool_candidate")]
        public IList<TalentPool> TalentPools { get; set; }
        */
        [DataMember(Name = "msdyn_candidate_candidateeducation")]
        public IList<CandidateEducation> CandidateEducations { get; set; }

        [DataMember(Name = "msdyn_candidate_candidateworkexperience")]
        public IList<CandidateWorkExperience> CandidateWorkExperiences { get; set; }

        [DataMember(Name = "msdyn_candidate_candidateskill")]
        public IList<CandidateSkill> CandidateSkills { get; set; }
        
        [DataMember(Name = "msdyn_candidate_candidateartifact")]
        public IList<CandidateArtifact> CandidateArtifacts { get; set; }

        [DataMember(Name = "msdyn_candidate_candidatetracking")]
        public IList<CandidateTracking> Trackings { get; set; }

        // TODO
        /*
        [DataMember(Name = "msdyn_candidate_assessmentreportpackage")]
        public IList<AssessmentReportPackage> AssessmentReportPackages { get; set; }
        */

        [DataMember(Name = "msdyn_candidate_candidatesocialnetwork")]
        public IList<CandidateSocialNetwork> SocialNetworks { get; set; }

        [DataMember(Name = "msdyn_issyncedwithlinkedin")]
        public bool? IsSyncedWithLinkedIn { get; set; }

        [DataMember(Name = "msdyn_candidate_talentsourcedetail")]
        public IList<TalentSourceDetail> TalentSourceDetails { get; set; }
    }
}
