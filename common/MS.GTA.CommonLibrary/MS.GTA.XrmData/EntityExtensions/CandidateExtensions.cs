//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.RelevanceSearch;
    using MS.GTA.Common.XrmHttp.Util;
    using MS.GTA.TalentEntities.Enum;
    using MS.GTA.XrmData.Query.Attract.OptionSetMetadata;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public static class CandidateExtensions
    {
        public static Applicant ToViewModel(this Candidate candidate, List<RelevanceSearchHighlight> searchHighlights = null, IList<JobApplication> jobApplicationsWithRank = null, OptionSetInfo talentSourceCateogry = null)
        {
            if (candidate != null)
            {
                var applicant = new Applicant()
                {
                    Email = candidate.EmailPrimary,
                    Id = candidate.Autonumber,
                    Guid = candidate.RecId.GetValueOrDefault(),
                    LinkedIn = candidate.LinkedInId,
                    Facebook = candidate.FacebookId,
                    Twitter = candidate.TwitterId,
                    ////PreferredPhone = candidate.PreferredPhone.GetValueOrDefault(),
                    Internal = candidate.Worker != null || candidate.WorkerId != null,
                    HomePhone = candidate.HomePhone,
                    WorkPhone = candidate.WorkPhone,
                    MobilePhone = candidate.MobilePhone,
                    LinkedInAPI = candidate.LinkedInApiUrl,
                    GivenName = candidate.GivenName,
                    MiddleName = candidate.MiddleName,
                    Surname = candidate.Surname,
                    ObjectId = candidate.Worker?.OfficeGraphIdentifier,
                    B2CObjectId = candidate.B2CObjectId,
                    AlternateEmail = candidate.EmailAlternate,
                    ////Gender = candidate.Gender.GetValueOrDefault(),
                    ////EthnicOrigin = candidate.EthnicOrigin.GetValueOrDefault(),
                    ExternalID = candidate.ExternalReference,
                    ExternalSource = candidate.Source?.ToCandidateExternalSource() ?? CandidateExternalSource.Internal,
                    ////ExternalWorkerId = candidate.Worker?.ExternalReference,
                    ////ExternalWorkerSource = (CommonDataService.CommonEntitySets.Source?)candidate.Worker?.Source,
                    ////MailNickname = candidate.Worker?.Alias,
                    Attachments = candidate.CandidateArtifacts?.Where(attachment => attachment != null).Select(attachment => attachment.ToViewModel()).ToArray(),
                    Trackings = candidate.Trackings?.Where(tracking => tracking != null).Select(tracking => tracking.ToViewModel()).ToArray(),
                    ////ExtendedAttributes = candidate.CandidateExtendedAttributes?
                    ////    .Where(attribute => !string.IsNullOrEmpty(attribute.AttributeID) && !string.IsNullOrEmpty(attribute.Value))
                    ////    .ToDictionary(attribute => attribute.AttributeID, attribute => attribute.Value),

                    WorkExperience = candidate.CandidateWorkExperiences?.Where(experience => experience != null).Select(experience => experience.CopySearchHighlights(searchHighlights, experience.CandidateWorkExperienceID).ToViewModel()).ToArray(),
                    Education = candidate.CandidateEducations?.Where(education => education != null).Select(education => education.CopySearchHighlights(searchHighlights, education.CandidateEducationID).ToViewModel()).ToArray(),
                    SkillSet = candidate.CandidateSkills?.Where(skill => skill != null && !string.IsNullOrWhiteSpace(skill.Skill)).Select(skill => skill.CopySearchHighlights(searchHighlights, skill.CandidateSkillID).Skill).ToArray(),
                    CandidateTalentSource = candidate.TalentSourceDetails?.Select(tsd => tsd.ToViewModel(talentSourceCateogry))?.FirstOrDefault(),
                    SocialIdentities = candidate.SocialNetworks?.Where(socialNetwork => socialNetwork != null).Select(socialNetwork => socialNetwork.ToViewModel()).ToArray(),
                    CustomFields = candidate.GetCustomFields(),
                };
                if (jobApplicationsWithRank != null && jobApplicationsWithRank.Any(jobApplication => jobApplication.CandidateId == candidate.RecId))
                {
                    applicant.Rank = Rank.Silver;
                }

                return applicant;

            }
            return null;
        }


        public static Application ToApplicationViewModel(this Candidate candidate)
        {
            if (candidate != null)
            {
                var application = candidate.JobApplications?.SingleOrDefault()?.ToViewModel();
                if (application != null)
                {
                    application.Candidate = candidate.ToViewModel();
                }

                return application;
            }

            return null;
        }

        public static Applicant ToViewModelWithApplications(this Candidate candidate)
        {
            if (candidate != null)
            {
                var applicant = candidate.ToViewModel();

                if (candidate.JobApplications != null)
                {
                    var jobs = new List<Job>();
                    foreach (var jobApplication in candidate.JobApplications.Where(j => j != null))
                    {
                        var job = jobApplication.JobOpening?.ToViewModel();
                        if (job != null)
                        {
                            job.Applications = new List<Application>() { jobApplication.ToViewModel() };
                            jobs.Add(job);
                        }
                    }

                    applicant.Jobs = jobs;
                }

                return applicant;
            }

            return null;
        }

        /// <summary>Convert Candidate to subset of information used in assessment scenarios</summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>The <see cref="CandidateInfo"/>.</returns>
        public static CandidateInfo ToWireContract(this Candidate candidate)
        {
            if (candidate == null)
            {
                return null;
            }

            return new CandidateInfo
            {
                EmailPrimary = candidate.EmailPrimary,
                CandidateID = candidate.RecId.Value.ToString(),
                FullName = string.Join(" ", candidate.GivenName, candidate.Surname)
            };
        }

        public static CandidateExternalSource ToCandidateExternalSource(this TalentEntities.Enum.TalentSource source)
        {
            switch (source)
            {
                case TalentEntities.Enum.TalentSource.ICIMS: return CandidateExternalSource.ICIMS;
                default:
                    return CandidateExternalSource.Internal;
            }
        }

        /// <summary>Convert to Applicant Profile view model.</summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>The Applicant Profile.</returns>
        public static ApplicantProfile ToProfileViewModel(this Candidate candidate)
        {
            return candidate == null ? null : new ApplicantProfile()
            {
                Email = candidate.EmailPrimary,
                Id = candidate.Autonumber,
                Guid = candidate.RecId.GetValueOrDefault(),
                LinkedIn = candidate.LinkedInId,
                Facebook = candidate.FacebookId,
                Twitter = candidate.TwitterId,
                HomePhone = candidate.HomePhone,
                WorkPhone = candidate.WorkPhone,
                MobilePhone = candidate.MobilePhone,
                LinkedInAPI = candidate.LinkedInApiUrl,
                GivenName = candidate.GivenName,
                MiddleName = candidate.MiddleName,
                Surname = candidate.Surname,
                ObjectId = candidate.Worker?.OfficeGraphIdentifier,
                B2CObjectId = candidate.B2CObjectId,
                AlternateEmail = candidate.EmailAlternate,
                ExternalSource = candidate.Source == null ? CandidateExternalSource.Internal : candidate.Source?.ToCandidateExternalSource(),
                Attachments = candidate.CandidateArtifacts?.Where(attachment => attachment != null).Select(attachment => attachment.ToTalentViewModel()).ToArray(),
                WorkExperience = candidate.CandidateWorkExperiences?.Where(experience => experience != null).Select(experience => experience.ToViewModel()).ToArray(),
                Education = candidate.CandidateEducations?.Where(education => education != null).Select(education => education.ToViewModel()).ToArray(),
                SkillSet = candidate.CandidateSkills?.Where(skill => skill != null && !string.IsNullOrWhiteSpace(skill.Skill)).Select(skill => skill.Skill).ToArray(),
                ExtendedAttributes = AdditionalMetadataExtensions.DeserializeStringOnlyAdditionalMetadata(candidate.AdditionalMetadata),
                CustomFields = candidate.GetCustomFields()
            };
        }

        /// <summary>Convert Candidate to subset of information used in export scenarios</summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>The <see cref="CandidateInfo"/>.</returns>
        public static Person ToExportViewModel(this Candidate candidate)
        {
            if (candidate == null)
            {
                return null;
            }

            return new Person
            {
                Email = candidate.EmailPrimary,
                ObjectId = candidate.Autonumber,
                GivenName = candidate.GivenName,
                MiddleName = candidate.MiddleName,
                Surname = candidate.Surname,
            };
        }
    }
}
