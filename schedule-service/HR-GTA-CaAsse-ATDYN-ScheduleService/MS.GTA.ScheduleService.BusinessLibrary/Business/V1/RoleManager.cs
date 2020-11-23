//----------------------------------------------------------------------------
// <copyright file="RoleManager.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Business.V1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Microsoft.Extensions.Logging;
    using Microsoft.Skype.ECS.Core.Collections;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.Common.Web.Contracts;
    using MS.GTA.ScheduleService.BusinessLibrary.Exceptions;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1.Flights;
    using MS.GTA.ScheduleService.Data.DataProviders;
    using MS.GTA.ScheduleService.FalconData.Query;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// Role Manager implementation
    /// </summary>
    public class RoleManager : IRoleManager
    {
        private readonly IScheduleQuery scheduleQuery;

        /// <summary>Falcon query client.</summary>
        private readonly FalconQuery falconQuery;

        /// <summary>
        /// The instance for <see cref="ILogger{RoleManager}"/>.
        /// </summary>
        private readonly ILogger<RoleManager> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleManager"/> class
        /// </summary>
        /// <param name="query">schedule query instance</param>
        /// <param name="falconQuery">falcon query</param>
        /// <param name="logger">The instance for <see cref="ILogger{RoleManager}"/>.</param>
        public RoleManager(
            IScheduleQuery query,
            FalconQuery falconQuery,
            ILogger<RoleManager> logger)
        {
            this.logger = logger;
            this.falconQuery = falconQuery;
            this.scheduleQuery = query;
        }

        /// <summary>
        /// Verify the user is Hiring Manger or Contributor or Recruiter
        /// </summary>
        /// <param name="userObjectId">user object id</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="scheduleID">schedule id</param>
        /// <returns>user present status</returns>
        public async Task<bool> IsUserHMorRecOrContributor(string userObjectId, string jobApplicationId, string scheduleID)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(jobApplicationId) && !string.IsNullOrWhiteSpace(scheduleID))
            {
                jobApplicationId = await this.scheduleQuery.GetJobApplicationIdForSchedule(scheduleID);
            }

            if (string.IsNullOrWhiteSpace(jobApplicationId) || jobApplicationId.IsNullOrEmpty())
            {
                this.logger.LogWarning($"IsUserHMorRecOrContributor - Application id {jobApplicationId} not found based on schedule id {scheduleID}, you can not perform this action !");
                throw new BusinessRuleViolationException($"Selected application or associated schedule does not exists").EnsureLogged(this.logger);
            }

            List<JobParticipantRole> jobParticipantRoles = new List<JobParticipantRole>();
            jobParticipantRoles.Add(JobParticipantRole.HiringManager);
            jobParticipantRoles.Add(JobParticipantRole.Recruiter);
            jobParticipantRoles.Add(JobParticipantRole.Contributor);

            if (EnableHiringManagerDelegateFlight.IsEnabled)
            {
                jobParticipantRoles.Add(JobParticipantRole.HiringManagerDelegate);
            }

            try
            {
                result = await this.ValidateUserRoles(userObjectId, jobApplicationId, jobParticipantRoles);
            }
            catch (Exception ex)
            {
                this.logger.LogError("IsUserHMorRecOrContributor failed with error: " + ex.Message + " and stack trace: " + ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// Verify the user is Hiring Manger or Contributor or Recruiter
        /// </summary>
        /// <param name="userObjectId">user object id</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>user present status</returns>
        public async Task<bool> IsUserInterviewerOrAA(string userObjectId, string jobApplicationId)
        {
            bool result = false;
            List<JobParticipantRole> jobParticipantRoles = new List<JobParticipantRole>();
            jobParticipantRoles.Add(JobParticipantRole.Interviewer);
            jobParticipantRoles.Add(JobParticipantRole.AA);

            try
            {
                result = await this.ValidateUserRoles(userObjectId, jobApplicationId, jobParticipantRoles);
            }
            catch (Exception ex)
            {
                this.logger.LogError("IsUserInterviewerOrAA failed with error: " + ex.Message + " and stack trace: " + ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// Verify whether user is Wob user or not
        /// </summary>
        /// <param name="isUserWobAuthenticated">user is a wob authenticated user or not</param>
        /// <returns>user present wob status</returns>
        public bool IsUserWobAuthenticated(bool isUserWobAuthenticated)
        {
            return WobUserFeatureFlight.IsEnabled && isUserWobAuthenticated;
        }

        /// <summary>
        /// Verify the user is  Contributor or not
        /// </summary>
        /// <param name="userObjectId">user object id</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>user present status</returns>
        public async Task<bool> IsUserContributor(string userObjectId, string jobApplicationId)
        {
            bool result = false;

            List<JobParticipantRole> jobParticipantRoles = new List<JobParticipantRole>();
            jobParticipantRoles.Add(JobParticipantRole.Contributor);

            try
            {
                result = await this.ValidateUserRoles(userObjectId, jobApplicationId, jobParticipantRoles);
            }
            catch (Exception ex)
            {
                this.logger.LogError("IsUserContributor failed with error: " + ex.Message + " and stack trace: " + ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// Verify the user present in the job application participants list
        /// </summary>
        /// <param name="userObjectId">user object id</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="scheduleID">scheduleID</param>
        /// <returns>user present status</returns>
        public async Task<bool> IsUserInJobApplicationParticipants(string userObjectId, string jobApplicationId, string scheduleID)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(jobApplicationId) && !string.IsNullOrWhiteSpace(scheduleID))
            {
                jobApplicationId = await this.scheduleQuery.GetJobApplicationIdForSchedule(scheduleID);
            }

            List<JobParticipantRole> jobParticipantRoles = new List<JobParticipantRole>();
            jobParticipantRoles.Add(JobParticipantRole.HiringManager);
            jobParticipantRoles.Add(JobParticipantRole.Recruiter);
            jobParticipantRoles.Add(JobParticipantRole.Contributor);
            jobParticipantRoles.Add(JobParticipantRole.AA);
            jobParticipantRoles.Add(JobParticipantRole.Interviewer);

            if (EnableHiringManagerDelegateFlight.IsEnabled)
            {
                jobParticipantRoles.Add(JobParticipantRole.HiringManagerDelegate);
            }

            try
            {
                result = await this.ValidateUserRoles(userObjectId, jobApplicationId, jobParticipantRoles);
            }
            catch (Exception ex)
            {
                this.logger.LogError("IsUserInJobApplicationParticipants failed with error: " + ex.Message + " and stack trace: " + ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// Verifies if all given object identifiers have interviewer role in job application or not
        /// </summary>
        /// <param name="participantOids">participant object id</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>returns true if all users are interviewer in job application</returns>
        public async Task<bool> AreParticipantsInterviewer(IList<string> participantOids, string jobApplicationId)
        {
            this.logger.LogInformation($"Started {nameof(this.AreParticipantsInterviewer)} method in {nameof(RoleManager)}.");
            if (!(participantOids?.Any() ?? false))
            {
                this.logger.LogInformation($"{nameof(this.AreParticipantsInterviewer)} method: participantOids cannot be null.");
                throw new InvalidRequestDataValidationException($"Input request does not contain a valid interviwer.").EnsureTraced();
            }

            if (string.IsNullOrEmpty(jobApplicationId))
            {
                this.logger.LogInformation($"{nameof(this.AreParticipantsInterviewer)} method: jobapplicationid cannot be null");
                throw new InvalidRequestDataValidationException($"Input request does not contain a valid jobapplication id").EnsureTraced();
            }

            var response = true;
            List<JobParticipantRole> jobParticipantRoles = new List<JobParticipantRole>();
            jobParticipantRoles.Add(JobParticipantRole.Interviewer);

            try
            {
                foreach (var oid in participantOids)
                {
                    if (!await this.ValidateUserRoles(oid, jobApplicationId, jobParticipantRoles))
                    {
                        response = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"{nameof(this.AreParticipantsInterviewer)} method in {nameof(RoleManager)} failed with error: " + ex.Message + " and stack trace: " + ex.StackTrace);
                response = false;
            }

            return response;
        }

        /// <summary>
        /// Verify if the user has read only admin role
        /// </summary>
        /// <param name="userOid">user object id</param>
        /// <returns>user read only admin status</returns>
        public async Task<bool> IsReadOnlyRole(string userOid)
        {
            if (!string.IsNullOrWhiteSpace(userOid))
            {
                try
                {
                    var roles = await this.falconQuery.GetRoleAssignment(userOid);
                    return roles?.Contains(IVApplicationRole.IVReadOnly) ?? false;
                }
                catch (Exception ex)
                {
                    this.logger.LogError("IsReadOnlyRole failed with error: " + ex.Message + " and stack trace: " + ex.StackTrace);
                }
            }

            return false;
        }

        private async Task<bool> ValidateUserRoles(string userObjectId, string jobApplicationId, List<JobParticipantRole> jobParticipantRoles)
        {
            if (!string.IsNullOrWhiteSpace(userObjectId) && !string.IsNullOrWhiteSpace(jobApplicationId))
            {
                var jobApplicationDetails = await this.scheduleQuery.GetJobApplication(jobApplicationId);
                if (jobApplicationDetails != null)
                {
                    IEnumerable<JobApplicationParticipant> participants = jobApplicationDetails.JobApplicationParticipants?.Where(a => a.OID.Equals(userObjectId, StringComparison.OrdinalIgnoreCase) &&
                    jobParticipantRoles.Contains(a.Role.Value));

                    return participants != null && participants.Any();
                }
            }

            return false;
        }
    }
}
