using MS.GTA.Common.Provisioning.Entities.XrmEntities;
using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
using MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset;
using MS.GTA.Common.TalentAttract.Contract;
using MS.GTA.TalentEntities.Enum;
using MS.GTA.TalentEntities.Enum.Common;
using MS.GTA.TalentJobPosting.Contract;
using MS.GTA.XrmData.Query.Attract.Dashboard;
using MS.GTA.XrmData.Query.Attract.OptionSetMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MS.GTA.Data.Utils;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.Common.Base.Security;
using MS.GTA.Common.XrmHttp.Util;
using MS.GTA.Common.TalentEntities.Common;

namespace MS.GTA.XrmData.EntityExtensions
{
    public static class JobOpeningExtensions
    {
        public static Job ToViewModel(this JobOpening jobOpening, OptionSetInfo optionSetInfo = null)
        {
            if (jobOpening == null)
            {
                return null;
            }

            var job = new Job
            {
                ApplicationStartDate = jobOpening.ApplicationStartDate,
                ApplicationCloseDate = jobOpening.ApplicationCloseDate,
                ApplyURI = jobOpening.ApplyURL,
                CreatedDate = jobOpening.JobActivationDate.HasValue ? jobOpening.JobActivationDate.GetValueOrDefault() : (jobOpening.JobStartDate.HasValue ? jobOpening.JobStartDate.GetValueOrDefault() : jobOpening.XrmCreatedOn),
                Description = jobOpening.Description,
                ExternalId = jobOpening.ExternalReference,
                Id = jobOpening.Autonumber,
                IsTemplate = jobOpening.IsTemplate,
                Location = jobOpening.JobLocation,
                NumberOfOpenings = jobOpening.NumberOfOpenings,
                PositionStartDate = jobOpening.JobStartDate,
                EmploymentType = EmploymentTypeToString(jobOpening.EmploymentType, optionSetInfo),
                EmploymentTypeValue = optionSetInfo?.GetOptionSetValue(jobOpening, j => j.EmploymentType),
                SeniorityLevel = SeniorityLevelToString(jobOpening.SeniorityLevel, optionSetInfo),
                SeniorityLevelValue = optionSetInfo?.GetOptionSetValue(jobOpening, j => j.SeniorityLevel),
                Skills = jobOpening.Skills?.Where(s => s != null).Select(s => s.Name).ToArray(),
                CompanyIndustries = jobOpening.Industries?.Where(s => s != null).Select(s => s.Name).ToArray(),
                JobFunctions = jobOpening.Functions?.Where(s => s != null).Select(s => s.Name).ToArray(),
                Source = jobOpening.Source?.ToJobOpeningExternalSource() ?? JobOpeningExternalSource.Internal,
                Status = jobOpening.Status,
                StatusReason = jobOpening.StatusReason,
                Comment = jobOpening.Comment,
                Title = jobOpening.JobTitle,
                Applications = jobOpening.JobApplications?.Where(a => a != null).Select(a => a.ToViewModel()).ToArray(),
                HiringTeam = jobOpening.JobOpeningParticipants?.Where(p => p != null && p.IsTemplateParticipant != true).Select(a => a.ToViewModel()).ToArray(),
                ApprovalParticipants = jobOpening.JobOpeningApprovalParticipants?.Where(p => p != null).Select(a => a.ToViewModel()).ToArray(),
                Stages = jobOpening.JobOpeningStages?.Where(a => a != null).Select(a => a.ToViewModel()).OrderBy(a => a.Order).ToArray(),
                ExternalJobPosts = jobOpening.JobPostings?.Where(r => r != null).Select(r => r.ToViewModel(jobOpening.XrmModifiedOn)).ToArray(),
                JobOpeningPositions = jobOpening.JobPositions?.Select(p => p.ToViewModel()).ToArray(),
                TemplateParticipants = jobOpening.JobOpeningParticipants?.Where(p => p != null && p.IsTemplateParticipant == true).Select(p => p.ToTemplateViewModel()).ToList(),
                ////Delegates = jobOpening.JobOpeningParticipants?.Where(jop => jop?.Worker != null)
                ////            ?.SelectMany(jop => jop.Delegates?.Select(d => d?.ToDelegateModel(jop.Worker.OfficeGraphIdentifier))).ToArray(),
                Delegates = jobOpening.JobOpeningParticipants?.Where(jop => jop?.Worker != null)
                        ?.SelectMany(jop => jop.Delegates?.Select(d => d?.ToDelegateModel(jop.Worker.OfficeGraphIdentifier)) ?? Enumerable.Empty<Common.TalentAttract.Contract.Delegate>()).ToArray(),
                PostalAddress = jobOpening.JobOpeningAddressToJobPostAddress(),
                Configuration = jobOpening.Configuration,
                JobOpeningVisibility = jobOpening.Visibility,
                ExtendedAttributes = AdditionalMetadataExtensions.DeserializeStringOnlyAdditionalMetadata(jobOpening.AdditionalMetadata),
                CustomFields = jobOpening.GetCustomFields()
            };

            if (job.ExtendedAttributes != null
                && job.ExtendedAttributes.TryGetValue("ExternalStatus", out var externalStatus))
            {
                job.ExternalStatus = externalStatus;
            }

            return job;
        }

        public static Job ToViewModelWithPermissions(
            this JobOpening jobOpening,
            JobOpeningParticipant jobOpeningParticipant,
            IList<Guid> userRoles,
            OptionSetInfo optionSetInfo = null)
        {
            var job = jobOpening.ToViewModel(optionSetInfo);
            if (job != null)
            {
                job.UserPermissions = GetJobPermissions(jobOpeningParticipant, userRoles);
            }
            return job;
        }

        public static Job ToTemplateViewModelWithPermissions(this JobOpening jobDetails, JobOpeningParticipant templateParticipant, IList<Guid> userRoles, string userObjectId)
        {
            var job = jobDetails?.ToViewModel();
            if (job != null
                    && job.TemplateParticipants != null
                    && templateParticipant == null
                    && userRoles.Contains(XrmRoleIds.RecruitingAdminRoleId))
            {
                job.TemplateParticipants = job.TemplateParticipants.Concat(new[]
                {
                    new JobOpeningTemplateParticipant()
                    {
                        UserObjectId = userObjectId,
                        CanEdit = true,
                    }
                }).ToArray();
            }
            return job;
        }

        /// <summary>The job opening to view model.</summary>
        /// <param name="jobOpening">The job opening.</param>
        /// <returns>The <see cref="Job"/>.</returns>
        public static DashboardActivity ToDashboardPendingJobViewModel(this JobOpening jobOpening)
        {
            if (jobOpening != null)
            {
                var activity = new DashboardActivity()
                {
                    Job = new Job
                    {
                        Id = jobOpening.Autonumber,
                        Title = jobOpening.JobTitle,
                    }
                };

                return activity;
            }

            return null;
        }

        /// <summary>The job openings to view model.</summary>
        /// <param name="jobOpening">The job opening.</param>
        /// <param name="supplier">The supplier.</param>
        /// <param name="company">The company.</param>
        /// <param name="companyId">The company Id.</param>
        /// <returns>The <see cref="Job"/>.</returns>
        public static JobPost ToJobPostViewModel(this JobOpening jobOpening, string supplier = null, string company = null, string companyId = null)
        {
            if (jobOpening != null)
            {
                var delimeter = new string[] { Constants.AttractSeperator };
                var principal = ServiceContext.Principal.TryGetCurrent<HCMApplicationPrincipal>();

                var post = new Common.Attract.Data.DocumentDB.JobPost()
                {
                    ExternalId = jobOpening.Autonumber,
                    Title = jobOpening.JobTitle,
                    Description = jobOpening.Description,
                    Status = jobOpening.Status.GetValueOrDefault(),
                    StatusReason = jobOpening.StatusReason.GetValueOrDefault(),
                    Location = jobOpening.JobLocation,
                    PostalAddress = jobOpening.JobOpeningAddressToJobPostAddress(),
                    Skills = jobOpening.Skills?.Select(f => f.Name).ToArray(),
                    JobFunctions = jobOpening.Functions?.Where(f => f != null).Select(f => f.Name).ToArray(),
                    CompanyIndustry = jobOpening.Industries?.Where(f => f != null).Select(f => f.Name).ToArray(),
                    SeniorityLevel = SeniorityLevelToString(jobOpening.SeniorityLevel),
                    EmploymentType = EmploymentTypeToString(jobOpening.EmploymentType),
                    JobPostVisibility = jobOpening.Visibility.GetValueOrDefault() == JobOpeningVisibility.InternalOnly
                                        ? JobPostVisibility.Internal
                                        : JobPostVisibility.Public,
                    ApplyURI = jobOpening.ApplyURL,
                    TenantId = principal?.TenantObjectId,
                    EnvironmentId = principal?.EnvironmentId,
                    Supplier = supplier,
                    Company = company,
                    CompanyId = companyId,
                };

                if (jobOpening.JobOpeningParticipants != null)
                {
                    post.HiringTeam = jobOpening
                        .JobOpeningParticipants
                        .Where(par => par != null && par.Role != JobParticipantRole.Interviewer && par.IsTemplateParticipant != true)
                        .Select(
                            par => new HiringTeamMember()
                            {
                                ObjectId = par.Worker?.OfficeGraphIdentifier,
                                Role = par.Role.GetValueOrDefault(),
                                GivenName = par.Worker?.GivenName,
                                MiddleName = par.Worker?.MiddleName,
                                Surname = par.Worker?.Surname,
                                FullName = par.Worker?.FullName,
                                WorkPhone = par.Worker?.PhonePrimary,
                                Email = par.Worker?.EmailPrimary,
                                Profession = par.Worker?.Profession,
                                LinkedIn = par.Worker?.LinkedInIdentity
                            }).ToList();
                }

                return post;
            }

            return null;
        }

        public static Address JobOpeningAddressToJobPostAddress(this JobOpening jobOpening)
        {
            if (string.IsNullOrEmpty(jobOpening.AddressLine1) &&
                string.IsNullOrEmpty(jobOpening.AddressLine2) &&
                string.IsNullOrEmpty(jobOpening.AddressCity) &&
                string.IsNullOrEmpty(jobOpening.AddressState) &&
                string.IsNullOrEmpty(jobOpening.AddressPostalCode) &&
                string.IsNullOrEmpty(jobOpening.AddressCountryCode))
            {
                return null;
            }

            var address = new Address
            {
                Line1 = jobOpening.AddressLine1,
                Line2 = jobOpening.AddressLine2,
                City = jobOpening.AddressCity,
                State = jobOpening.AddressState,
                PostalCode = jobOpening.AddressPostalCode
            };

            if (Enum.TryParse(
                    jobOpening.AddressCountryCode,
                    true,
                    out CountryCode countryCodeValue))
            {
                if (Enum.IsDefined(typeof(CountryCode), countryCodeValue))
                {
                    address.Country = countryCodeValue.ToEntityEnum<Common.TalentEntities.Enum.Common.CountryCode>();
                }
            }

            return address;
        }

        /// <summary>
        /// Return location as a simple string either using the custom-address (JobLocation)
        /// or from the structured data in AddressLine1 and AddressLine2
        /// </summary>
        /// <param name="jobOpening">the job opening xrm entity</param>
        /// <returns>Location as a simple string</returns>
        public static string SimpleLocation(this JobOpening jobOpening)
        {
            if (!string.IsNullOrEmpty(jobOpening.JobLocation))
            {
                return jobOpening.JobLocation;
            }

            string location = string.Empty;
            if (!string.IsNullOrEmpty(jobOpening.AddressLine1))
            {
                location += jobOpening.AddressLine1;
            }

            if (!string.IsNullOrEmpty(jobOpening.AddressLine2))
            {
                if (!string.IsNullOrWhiteSpace(location))
                {
                    location += ", ";
                }

                location += jobOpening.AddressLine2;
            }

            return location;
        }

        public static JobOpeningExternalSource ToJobOpeningExternalSource(this TalentEntities.Enum.TalentSource source)
        {
            switch (source)
            {
                case TalentEntities.Enum.TalentSource.ICIMS: return JobOpeningExternalSource.ICIMS;
                default:
                    return JobOpeningExternalSource.Internal;
            }
        }

        /// <summary>
        /// Cast jobOpening to assignedTask
        /// </summary>
        /// <param name="jobOpening">jobOpening</param>
        /// <returns>AssingdTask</returns>
        public static AssignedTask ToAssignedActivityViewModel(this JobOpening jobOpening)
        {
            return jobOpening == null ? null : jobOpening.JobApprovalRequesterDate == null ? null : new AssignedTask
            {
                AssignedTaskType = AssignedTaskType.JobApproval,
                Date = jobOpening.JobApprovalRequesterDate,
                RecId = jobOpening.RecId,
            };
        }

        private static string EmploymentTypeToString(EmploymentType? employmentType, OptionSetInfo optionSetInfo = null)
        {
            if (employmentType == null)
            {
                return null;
            }

            var label = optionSetInfo?.GetOptionValueLabel<JobOpening>(j => j.EmploymentType, (int)employmentType.Value);
            if (!string.IsNullOrEmpty(label))
            {
                return label;
            }

            switch (employmentType.Value)
            {
                case EmploymentType.Contract: return "Contract";
                case EmploymentType.FullTime: return "Full-time";
                case EmploymentType.PartTime: return "Part-time";
                case EmploymentType.Temporary: return "Temporary";
                case EmploymentType.Volunteer: return "Volunteer";
                default: return null;
            }
        }

        private static string SeniorityLevelToString(JobOpeningSeniorityLevel? seniorityLevel, OptionSetInfo optionSetInfo = null)
        {
            if (seniorityLevel == null)
            {
                return null;
            }

            var label = optionSetInfo?.GetOptionValueLabel<JobOpening>(j => j.SeniorityLevel, (int)seniorityLevel.Value);
            if (!string.IsNullOrEmpty(label))
            {
                return label;
            }

            switch (seniorityLevel.Value)
            {
                case JobOpeningSeniorityLevel.Associate: return "Associate";
                case JobOpeningSeniorityLevel.Default: return "Default";
                case JobOpeningSeniorityLevel.Director: return "Director";
                case JobOpeningSeniorityLevel.EntryLevel: return "Entry level";
                case JobOpeningSeniorityLevel.Executive: return "Executive";
                case JobOpeningSeniorityLevel.Internship: return "Internship";
                case JobOpeningSeniorityLevel.MidSeniorLevel: return "Mid-senior level";
                default: return null;
            }
        }

        private static IList<JobPermission> GetJobPermissions(JobOpeningParticipant jobOpeningParticipant, IList<Guid> userRoles)
        {
            if (userRoles.Contains(XrmRoleIds.RecruitingAdminRoleId))
            {
                return new[]
                {
                    JobPermission.ActivateJob,
                    JobPermission.CloseJob,
                    JobPermission.CreateApplicant,
                    JobPermission.CreateHiringTeam,
                    JobPermission.CreateJobApproval,
                    JobPermission.CreateJobPosting,
                    JobPermission.DeleteJob,
                    JobPermission.UpdateJobDetails,
                    JobPermission.UpdateJobProcess,
                };
            }

            var currentUserJobRole = jobOpeningParticipant?.Role;
            switch (currentUserJobRole)
            {
                case JobParticipantRole.HiringManager:
                    // TODO: wrong?
                    return new[]
                    {
                        JobPermission.ActivateJob,
                        JobPermission.CloseJob,
                        JobPermission.CreateApplicant,
                        JobPermission.CreateHiringTeam,
                        JobPermission.CreateJobApproval,
                        JobPermission.DeleteJob,
                        JobPermission.UpdateJobDetails,
                        JobPermission.UpdateJobProcess,
                    };

                case JobParticipantRole.Recruiter:
                    return new[]
                    {
                        JobPermission.ActivateJob,
                        JobPermission.CloseJob,
                        JobPermission.CreateApplicant,
                        JobPermission.CreateHiringTeam,
                        JobPermission.CreateJobApproval,
                        JobPermission.CreateJobPosting,
                        JobPermission.DeleteJob,
                        JobPermission.UpdateJobDetails,
                        JobPermission.UpdateJobProcess,
                    };

                case JobParticipantRole.Contributor:
                case JobParticipantRole.Interviewer:
                default:
                    return new JobPermission[] { };
            }
        }
    }
}
