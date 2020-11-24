//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.XrmData.DataAccess.Attract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MS.GTA.Common.Base.Exceptions;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.Data;
    using MS.GTA.TalentEntities.Enum;
    using MS.GTA.XrmData.EntityExtensions;

    public partial class DataAccess : IDataAccess
    {
        public async Task<bool> UpdateOfferedJobApplicationStatusReason(string jobApplicationId, JobApplicationStatusReason statusReason, string invitationId = null)
        {
            Contract.CheckNonEmpty(jobApplicationId, nameof(jobApplicationId));
            if (statusReason != JobApplicationStatusReason.OfferAccepted && statusReason != JobApplicationStatusReason.OfferRejected && statusReason != JobApplicationStatusReason.New)
            {
                throw new BenignException($"The JOb Application Status - {statusReason} for Job Application Id - {jobApplicationId} is not correct");
            }

            bool useAdminClient = false;
            if (invitationId != null)
            {
                // confirm validity of the invitation token
                var application = await this.query.GetJobApplicationMetadata(jobApplicationId, true);
                if (application != null && application.InvitationId != null && application.InvitationId == invitationId)
                {
                    useAdminClient = true;
                }
            }

            if (statusReason != JobApplicationStatusReason.OfferAccepted && statusReason != JobApplicationStatusReason.New)
            {
                await this.query.RemovePositionFromApplication(jobApplicationId, useAdminClient);
            }

            return await this.query.UpdateJobApplicationStatusReason(jobApplicationId, statusReason, useAdminClient);
        }

        /// <summary>
        /// Add or delete team members for a job application activity.
        /// </summary>
        /// <param name="teamMembersEnumerable">Team members.</param>
        /// <param name="jobApplicationId">Job application id.</param>
        /// <param name="stageOrder">Stage ordinal.</param>
        /// <param name="activityId">Application Activity id.</param>
        /// <param name="assessmentProviderDataAccess">Assessment provider data access.</param>
        /// <param name="notificationClient">Notification client</param>
        /// <param name="schedulingServiceClient">Scheduling service client</param>
        /// <returns>Task performed.</returns>
        public async Task UpdateJobAppActivitiesTeamMembers(
            IEnumerable<HiringTeamMember> teamMembersEnumerable,
            string jobApplicationId,
            int stageOrder,
            string activityId)
        {
            Contract.CheckNonEmpty(jobApplicationId, nameof(jobApplicationId));

            try
            {
                var teamMembers = teamMembersEnumerable.ToArray();
                await this.query.UpdateJobApplicationActivityTeamMembers(teamMembers, jobApplicationId, activityId);
            }
            catch (Exception e)
            {
                this.Trace.TraceError($"UpdateJobAppActivitiesTeamMembers failed: {e}");
                throw new  Exception($"Add remove team members for a job application: {jobApplicationId} and for activity: {activityId} failed");
            }
        }

        /// <summary>Get a job application by ID</summary>
        /// <param name="jobApplicationId">Job application Id </param>
        /// <param name="isAccessedByAdmin">Is accessed by Admin</param>
        /// <returns>The job application entity. </returns>
        public async Task<Job> GetJobWithApplication(string jobApplicationId, bool isAccessedByAdmin = false)
        {
            Contract.CheckNonEmpty(jobApplicationId, nameof(jobApplicationId));

            var jobApplication = await this.query.GetJobApplicationWithDetails(jobApplicationId, isAccessedByAdmin);
            if (jobApplication != null)
            {
                var job = jobApplication.JobOpening?.ToViewModel();
                if (job != null)
                {
                    job.Applications = new List<Application>() { jobApplication.ToViewModel() };
                    return job;
                }
            }

            return null;
        }
    }
}
