//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobApplicationExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.Util;
    using MS.GTA.TalentEngagementService.Data.Candidates.DocumentDB;
    using MS.GTA.TalentEntities.Enum;
    using MS.GTA.XrmData.Query.Attract.OptionSetMetadata;

    public static class JobApplicationExtensions
    {
        public static Application ToViewModel(this JobApplication jobApplication, OptionSetInfo optionSetInfo = null)
        {
            if (jobApplication != null)
            {
                var application = new Application()
                {
                    ApplicationDate = jobApplication.ApplicationDate,
                    ApplicationTalentSource = jobApplication.TalentSourceDetails?.Select(tsd => tsd.ToViewModel(optionSetInfo))?.FirstOrDefault(),
                    Candidate = jobApplication.Candidate?.ToViewModel(talentSourceCateogry: optionSetInfo),
                    Comment = jobApplication.Comment,
                    CurrentApplicationStage = jobApplication.CurrentJobOpeningStage?.ToViewModel(),
                    CurrentStage = default(JobStage),
                    CurrentStageStatus = JobApplicationStageStatus.Active,
                    CurrentStageStatusReason = JobApplicationStageStatusReason.Open,
                    ExtendedAttributes = AdditionalMetadataExtensions.DeserializeStringOnlyAdditionalMetadata(jobApplication.AdditionalMetadata),
                    ExternalId = jobApplication.ExternalReference,
                    ExternalSource = jobApplication.Source?.ToJobApplicationExternalSource(),
                    ExternalStatus = jobApplication.ExternalStatus,
                    HiringTeam = GetTeamMemberFromActivities(jobApplication.JobApplicationActivities, jobApplication.JobOpening?.JobOpeningStages, jobApplication.JobOpening?.JobOpeningParticipants),
                    Id = jobApplication.Autonumber,
                    InvitationId = jobApplication.InvitationId,
                    IsProspect = jobApplication.IsProspect.HasValue && jobApplication.IsProspect.Value,
                    JobOpeningPosition = jobApplication.JobPosition.ToViewModel(),
                    Notes = jobApplication.JobApplicationComments?.Where(c => c != null).Select(c => c.ToViewModel()).Where(c => !string.IsNullOrWhiteSpace(c?.Text)).ToArray(),
                    RejectionReason = GetRejectionReason(jobApplication, optionSetInfo),
                    Stages = GetApplicationStageActivities(jobApplication.JobApplicationActivities, jobApplication.JobOpening?.JobOpeningStages, jobApplication.Candidate?.Worker != null),
                    Status = jobApplication.Status.GetValueOrDefault(),
                    StatusReason = jobApplication.StatusReason.GetValueOrDefault(),
                    CustomFields = jobApplication.GetCustomFields()
                };

                if (application.Candidate != null)
                {
                    application.Candidate.Rank = jobApplication.Rank;
                }

                return application;
            }
            return null;
        }

        public static Application ToViewModelWithPermissions(this JobApplication jobApplication, JobOpeningParticipant jobOpeningParticipant, IList<Guid> userRoles, OptionSetInfo optionSetInfo = null)
        {
            var application = jobApplication.ToViewModel(optionSetInfo);
            if (application != null)
            {
                application.UserPermissions = GetJobApplicationPermissions(jobOpeningParticipant, userRoles);
            }
            return application;
        }

        public static JobApplicationDetails ToViewModelForCandidate(this JobApplication jobApplication, ApplicationData applicationData, IEnumerable<Common.Provisioning.Entities.XrmEntities.Attract.CandidatePersonalDetails> personalDetails = null, OptionSetInfo optionSetInfo = null)
        {
            if (jobApplication != null)
            {
                var isInternalCandidate = jobApplication.Candidate?.Worker != null;
                var stages = GetApplicationStageActivitiesForCandidate(jobApplication?.JobApplicationActivities, jobApplication?.JobOpening?.JobOpeningStages, isInternalCandidate);

                if (stages != null)
                {
                    foreach (var stage in stages)
                    {
                        foreach (var activity in stage.StageActivities)
                        {
                            activity.Id = jobApplication?.JobApplicationActivities?.FirstOrDefault(a =>
                                                    a.JobOpeningStageActivity.JobOpeningStage.Ordinal == stage.Order
                                                    && a.JobOpeningStageActivity.Ordinal == activity.Ordinal
                                                    && (a.JobOpeningStageActivity.SubOrdinal == null
                                                    || a.JobOpeningStageActivity.SubOrdinal == activity.SubOrdinal))?.Autonumber;
                        }
                    }
                }

                return new JobApplicationDetails
                {
                    TenantId = applicationData?.TenantID,
                    CompanyName = applicationData?.DisplayData?.CompanyName,
                    ApplicationId = applicationData?.JobApplicationID,
                    JobDescription = jobApplication.JobOpening?.Description,
                    PositionLocation = jobApplication.JobOpening?.SimpleLocation(),
                    PositionTitle = jobApplication.JobOpening?.JobTitle,
                    CurrentApplicationStage = jobApplication.CurrentJobOpeningStage?.ToViewModel(),
                    ExternalStatus = jobApplication.ExternalStatus,
                    ExternalSource = jobApplication.Source.GetValueOrDefault().ToJobApplicationExternalSource(),
                    Status = jobApplication.Status.GetValueOrDefault(),
                    StatusReason = jobApplication.StatusReason.GetValueOrDefault(),
                    RejectionReason = GetRejectionReason(jobApplication, optionSetInfo),
                    DateApplied = jobApplication.ApplicationDate.GetValueOrDefault(),
                    ApplicantAttachments = jobApplication.CandidateArtifacts?.Where(r => r != null).Select(r => r.ToViewModel()).ToList(),
                    ApplicationStages = stages,
                    JobPostLink = jobApplication.JobOpening?.JobPostings?.FirstOrDefault()?.JobPostURI,
                    /*
                    ApplicationSchedules = jobApplication.JobApplicationActivities == null
                            ? null
                            : (from activity in jobApplication.JobApplicationActivities
                               where activity?.JobOpeningStageActivity?.ActivityType == JobApplicationActivityType.Schedule
                               select new ApplicationSchedule()
                               {
                                   Id = activity.Autonumber,
                                   ScheduleEventId = activity.ScheduleEventReference,
                                   ////ScheduleState = activity.ScheduleState.GetValueOrDefault(),
                                   StageOrder = activity.JobOpeningStageActivity.JobOpeningStage?.Ordinal ?? 0,
                                   ActivityOrdinal = activity.JobOpeningStageActivity.Ordinal,
                                   ActivitySubOrdinal = activity.JobOpeningStageActivity.SubOrdinal,
                                   ScheduleAvailabilities = activity.JobApplicationActivityAvailabilities?.Where(sa => sa?.Start != null)?.Select(sa => sa.ToViewModel()).ToList()
                               })?.ToList(),
                    Interviews = jobApplication.JobApplicationActivities == null
                            ? null
                            : (from activity in jobApplication.JobApplicationActivities
                               let jobOpeningStageActivity = activity?.JobOpeningStageActivity
                               where jobOpeningStageActivity?.ActivityType == JobApplicationActivityType.Schedule
                                  && activity.JobApplicationActivityParticipants != null
                               from participant in activity.JobApplicationActivityParticipants
                               where participant?.JobApplicationActivityParticipantMeetings != null
                               from jobApplicationActivityAvailability in participant.JobApplicationActivityParticipantMeetings
                               where jobApplicationActivityAvailability != null
                               orderby jobApplicationActivityAvailability.Start ascending
                               select new JobApplicationInterview()
                               {
                                   LinkedinIdentity = participant?.JobOpeningParticipant?.Worker?.LinkedInIdentity,
                                   InterviewerName = participant?.JobOpeningParticipant?.Worker?.FullName,
                                   StartDate = jobApplicationActivityAvailability.Start.GetValueOrDefault(),
                                   EndDate = jobApplicationActivityAvailability.End.GetValueOrDefault(),
                                   StageOrdinal = jobOpeningStageActivity?.JobOpeningStage.Ordinal ?? 0,
                                   ActivityOrdinal = jobOpeningStageActivity?.Ordinal,
                                   ActivitySubOrdinal = jobOpeningStageActivity?.SubOrdinal ?? 0,
                               })?.ToList(),
                               */
                    CandidatePersonalDetails = personalDetails?.Select(c => c.ToViewModel())?.ToList(),
                    CustomFields = jobApplication.GetCustomFields()
                };
            }

            return null;
        }

        public static Job ToJobViewModel(this JobApplication jobApplication)
        {
            if (jobApplication != null)
            {
                var jobOpening = jobApplication.JobOpening;

                if (jobOpening != null)
                {
                    jobOpening.JobApplications = new[] { jobApplication };
                    jobApplication.JobOpening = null;
                    return jobOpening.ToViewModel();
                }
            }

            return null;
        }

        /// <summary>Convert job application data to display data view model.</summary>
        /// <param name="jobApplication">The job application.</param>
        /// <param name="applicationData">The application data.</param>
        /// <returns>The <see cref="DisplayData"/>.</returns>
        public static DisplayData ToDisplayDataViewModel(this JobApplication jobApplication, ApplicationData applicationData)
        {
            if (jobApplication != null)
            {
                var displayData = new DisplayData()
                {
                    ID = jobApplication.Autonumber,
                    JobTitle = jobApplication.JobOpening?.JobTitle,
                    JobDescription = jobApplication.JobOpening?.Description,
                    JobLocation = jobApplication.JobOpening?.SimpleLocation(),
                    CompanyName = applicationData?.DisplayData?.CompanyName,
                    JobApplicationStatus = jobApplication.Status.GetValueOrDefault().ToString(),
                    JobApplicationDate = jobApplication.ApplicationDate.GetValueOrDefault(),
                    JobPostLink = jobApplication.JobOpening?.JobPostings?.Where(p => p != null).Select(post => post.JobPostURI).ToList(),
                    CurrentApplicationStage = jobApplication.CurrentJobOpeningStage?.ToViewModel(),
                };

                return displayData;
            }

            return null;
        }

        public static JobApplicationExternalSource ToJobApplicationExternalSource(this TalentEntities.Enum.TalentSource source)
        {
            switch (source)
            {
                case TalentEntities.Enum.TalentSource.ICIMS:
                    return JobApplicationExternalSource.ICIMS;
                default:
                    return JobApplicationExternalSource.Internal;
            }
        }

        public static int? ConvertStatusReasonToRejectionReason(JobApplicationStatusReason? statusReason)
        {
            if (statusReason == null)
            {
                return null;
            }

            switch (statusReason)
            {
                case JobApplicationStatusReason.NicManagementExperience:
                    return 192350000;
                case JobApplicationStatusReason.NicJobRelatedEducation:
                    return 192350001;
                case JobApplicationStatusReason.NicJobTechnicalFunctionalExperience:
                    return 192350002;
                case JobApplicationStatusReason.NicInconsistentJobHistory:
                    return 192350003;
                case JobApplicationStatusReason.NoRequiredQualification:
                    return 192350004;
                case JobApplicationStatusReason.OthersMoreQualified:
                    return 192350005;
                case JobApplicationStatusReason.Application:
                    return 192350006;
                case JobApplicationStatusReason.Education:
                    return 192350007;
                case JobApplicationStatusReason.Experience:
                    return 192350008;
                case JobApplicationStatusReason.SkillSet:
                    return 192350009;
                case JobApplicationStatusReason.Competency:
                    return 192350010;
                case JobApplicationStatusReason.Licensure:
                    return 192350011;
                case JobApplicationStatusReason.Assessment:
                    return 192350012;
                case JobApplicationStatusReason.NicOther:
                    return 192350013;
            }

            return null;
        }

        public static JobApplicationStatusReason? ConvertRejectionReasonToStatusReason(int? rejectionReason)
        {
            if (rejectionReason == null)
            {
                return null;
            }

            switch (rejectionReason)
            {
                case 192350000:
                    return JobApplicationStatusReason.NicManagementExperience;
                case 192350001:
                    return JobApplicationStatusReason.NicJobRelatedEducation;
                case 192350002:
                    return JobApplicationStatusReason.NicJobTechnicalFunctionalExperience;
                case 192350003:
                    return JobApplicationStatusReason.NicInconsistentJobHistory;
                case 192350004:
                    return JobApplicationStatusReason.NoRequiredQualification;
                case 192350005:
                    return JobApplicationStatusReason.OthersMoreQualified;
                case 192350006:
                    return JobApplicationStatusReason.Application;
                case 192350007:
                    return JobApplicationStatusReason.Education;
                case 192350008:
                    return JobApplicationStatusReason.Experience;
                case 192350009:
                    return JobApplicationStatusReason.SkillSet;
                case 192350010:
                    return JobApplicationStatusReason.Competency;
                case 192350011:
                    return JobApplicationStatusReason.Licensure;
                case 192350012:
                    return JobApplicationStatusReason.Assessment;
                case 192350013:
                    return JobApplicationStatusReason.NicOther;
            }

            // Default to NicOther. Keep NicOther in the switch as well because 192350013 is its explicit value in case
            // the default changes in future.
            return JobApplicationStatusReason.NicOther;
        }

        public static IList<ApplicationPermission> ApplicationPermissionsBasedOnRoles(this IList<Guid> userRoles)
        {
            var applicationPermissions = new SortedSet<ApplicationPermission>();

            if (userRoles.Contains(XrmRoleIds.RecruitingAdminRoleId))
            {
                applicationPermissions.UnionWith(new[]
                {
                    ApplicationPermission.CreateNote,
                    ApplicationPermission.CreateOffer,
                    ApplicationPermission.DeleteApplication,
                    ApplicationPermission.ReadApplication,
                    ApplicationPermission.ReadNote,
                    ApplicationPermission.RejectApplicant,
                    ApplicationPermission.UpdateAllActivities,
                    ApplicationPermission.UpdateApplication,
                    ApplicationPermission.ViewAllActivities,
                });
            }

            if (userRoles.Contains(XrmRoleIds.RecruitingReadAllRoleId))
            {
                applicationPermissions.UnionWith(new[]
                {
                    ApplicationPermission.ReadApplication,
                    ApplicationPermission.ReadNote,
                    ApplicationPermission.ViewAllActivities,
                });
            }

            return applicationPermissions.ToArray();
        }

        public static IList<ApplicationStage> GetApplicationStageActivities(IList<JobApplicationActivity> jobApplicationActivities, IList<JobOpeningStage> jobOpeningStages, bool isInternalCandidate = false)
        {
            if (jobApplicationActivities == null || jobOpeningStages == null)
            {
                return null;
            }

            var applicationStages = new List<ApplicationStage>();
            foreach (var stage in jobOpeningStages.OrderBy(s => s.Ordinal))
            {
                if (stage.JobOpeningStageActivities != null)
                {
                    stage.JobOpeningStageActivities = stage.JobOpeningStageActivities?
                                                        .Where(a => jobApplicationActivities.Any(jaa => jaa.JobOpeningStageActivity?.RecId == a.RecId))
                                                        .ToList();
                }

                var storedActivities = jobApplicationActivities.Where(a => a.JobOpeningStageActivity?.JobOpeningStageId == stage.RecId).ToArray();

                var applicationStage = stage.ToViewModel();
                applicationStage.TotalActivities = storedActivities.Count();
                applicationStage.CompletedActivities = storedActivities.Where(r => r.Status == JobApplicationActivityStatus.Completed || r.Status == JobApplicationActivityStatus.Skipped).Count();
                if (applicationStage.TotalActivities > 0 && applicationStage.CompletedActivities == applicationStage.TotalActivities)
                {
                    applicationStage.LastCompletedActivityDateTime = storedActivities.Select(t => t.ActualEnd).Last().GetValueOrDefault();
                }

                applicationStages.Add(applicationStage);
            }

            return applicationStages;
        }

        public static IList<ApplicationStage> GetApplicationStageActivitiesForCandidate(IList<JobApplicationActivity> jobApplicationActivities, IList<JobOpeningStage> jobOpeningStages, bool isInternalCandidate = false)
        {
            if (jobApplicationActivities == null || jobOpeningStages == null)
            {
                return null;
            }

            var stages = new List<ApplicationStage>();

            // Allow only the following sets of activities and conditions within the stages

            stages = jobOpeningStages?
                .Select(s => new JobOpeningStage
                {
                    Ordinal = s.Ordinal,
                    DisplayName = s.DisplayName,
                    Description = s.Description,
                    JobOpeningStageActivities = s.JobOpeningStageActivities?
                                                    .Where(a => a.Audience != ActivityAudience.HiringTeam
                                                                && jobApplicationActivities.Any(jaa => jaa.JobOpeningStageActivity?.RecId == a.RecId)).ToList()
                }.ToViewModel()).ToList();

            return stages;
        }

        public static IList<ApplicationPermission> GetJobApplicationPermissions(JobOpeningParticipant jobOpeningParticipant, IList<Guid> userRoles)
        {
            var applicationPermissions = new SortedSet<ApplicationPermission>();

            applicationPermissions.UnionWith(userRoles.ApplicationPermissionsBasedOnRoles());

            var currentUserJobRole = jobOpeningParticipant?.Role;
            switch (currentUserJobRole)
            {
                case JobParticipantRole.HiringManager:
                    applicationPermissions.UnionWith(new[]
                    {
                        ApplicationPermission.CreateNote,
                        ApplicationPermission.ReadApplication,
                        ApplicationPermission.ReadNote,
                        ApplicationPermission.UpdateApplication,
                    });
                    break;

                case JobParticipantRole.Recruiter:
                    applicationPermissions.UnionWith(new[]
                    {
                        ApplicationPermission.CreateNote,
                        ApplicationPermission.CreateOffer,
                        ApplicationPermission.DeleteApplication,
                        ApplicationPermission.ReadApplication,
                        ApplicationPermission.ReadNote,
                        ApplicationPermission.RejectApplicant,
                        ApplicationPermission.UpdateApplication,
                    });
                    break;

                case JobParticipantRole.Contributor:
                case JobParticipantRole.Interviewer:
                    applicationPermissions.UnionWith(new[]
                    {
                        ApplicationPermission.ReadApplication,
                        ApplicationPermission.ReadNote,
                    });

                    applicationPermissions.UnionWith(new[]
                    {
                            ApplicationPermission.CreateNote,
                    });

                    break;
            }

            return applicationPermissions.ToArray();
        }

        // TODO
        private static IList<HiringTeamMember> GetTeamMemberFromActivities(IList<JobApplicationActivity> activities, IList<JobOpeningStage> jobOpeningStages, IList<JobOpeningParticipant> jobOpeningParticipants)
        {
            if (activities == null)
            {
                return null;
            }

            var hiringTeamMembers = new List<HiringTeamMember>();
            foreach (var t in
                from activity in activities
                from participant in activity.JobApplicationActivityParticipants ?? new JobApplicationActivityParticipant[] { }
                group Tuple.Create(participant, activity) by participant.JobOpeningParticipantId into ts
                select new
                {
                    participant = ts.First().Item1,
                    activities = ts.Select(t => t.Item2),
                    stages = ts.Select(t => t.Item2.JobOpeningStageActivity?.JobOpeningStageId).Distinct()
                })
            {
                var schedule = t.participant.JobApplicationActivityParticipantMeetings?.FirstOrDefault();

                var jobOpeningParticipant = t.participant.JobOpeningParticipant ?? jobOpeningParticipants?.FirstOrDefault(p => p.RecId == t.participant.JobOpeningParticipantId);
                var teamMember = jobOpeningParticipant?.ToViewModel();
                if (teamMember != null)
                {
                    teamMember.Activities = t.activities.Select(activity => new Activity
                    {
                        Stage = JobStage.Application,
                        Status = activity.Status.GetValueOrDefault(),
                        StatusReason = activity.StatusReason.GetValueOrDefault(),
                        ActivityType = activity.ActivityType ?? activity.JobOpeningStageActivity?.ActivityType ?? JobApplicationActivityType.Interview,
                        PlannedStartTime = schedule?.Start?.ToUniversalTime() ?? activity.PlannedStart?.ToUniversalTime(),
                        PlannedEndTime = schedule?.End?.ToUniversalTime() ?? activity.PlannedEnd?.ToUniversalTime(),
                        Description = activity.Description,
                        Location = activity.Location,
                    }).ToList();
                    teamMember.Metadata = new JobApplicationParticipantMetadata
                    {
                        Stages = t.stages
                            .Select(s => jobOpeningStages?.FirstOrDefault(stage => stage.RecId == s))
                            .Where(stage => stage != null)
                            .Select(stage => new JobApplicationStage
                            {
                                Name = stage.DisplayName,
                                Order = stage.Ordinal,
                                Stage = JobStage.Application,
                            })
                            .OrderBy(a => a.Order)
                            .ToList(),
                    };
                    hiringTeamMembers.Add(teamMember);
                }
            }

            return hiringTeamMembers;
        }

        private static OptionSetValue GetRejectionReason(JobApplication jobApplication, OptionSetInfo optionSetInfo)
        {
            if (jobApplication.RejectionReason == null)
            {
                jobApplication.RejectionReason = ConvertStatusReasonToRejectionReason(jobApplication.StatusReason);
            }

            return optionSetInfo?.GetOptionSetValue(jobApplication, j => j.RejectionReason);
        }
    }
}
