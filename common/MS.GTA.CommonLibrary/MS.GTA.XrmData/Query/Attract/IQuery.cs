//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.XrmData.Query
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.Web.Contracts;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// Query layer Interface
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// Gets the job application metadata.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <param name="useAdminClient">if set to <c>true</c> [use admin client].</param>
        /// <returns>Job Application</returns>
        Task<JobApplication> GetJobApplicationMetadata(string jobApplicationId, bool useAdminClient = false);

        /// <summary>
        /// Gets the job application reference.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <param name="useAdminClient">if set to <c>true</c> [use admin client].</param>
        /// <returns>Job Application</returns>
        Task<JobApplication> GetJobApplicationReference(string jobApplicationId, bool useAdminClient = false);

        /// <summary>
        /// Removes the position from application.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <param name="useAdminClient">if set to <c>true</c> [use admin client].</param>
        /// <returns>True or False</returns>
        Task<bool> RemovePositionFromApplication(string jobApplicationId, bool useAdminClient = false);

        /// <summary>
        /// Updates the job application status reason.
        /// </summary>
        /// <param name="jobApplicationId">The job application identifier.</param>
        /// <param name="statusReason">The status reason.</param>
        /// <param name="useAdminClient">if set to <c>true</c> [use admin client].</param>
        /// <returns>True or False</returns>
        Task<bool> UpdateJobApplicationStatusReason(string jobApplicationId, JobApplicationStatusReason statusReason, bool useAdminClient = false);

        /// <summary>
        /// Add or delete team members for a job application activity.
        /// </summary>
        /// <param name="teamMembers">Team members.</param>
        /// <param name="jobApplicationId">Job application id.</param>
        /// <param name="activityId">Application Activity id.</param>
        /// <returns>Task performed.</returns>
        /// <returns>True or false.</returns>
        Task UpdateJobApplicationActivityTeamMembers(IList<HiringTeamMember> teamMembers, string jobApplicationId, string activityId);

        /// <summary>
        /// Get a job application by ID
        /// </summary>
        /// <param name="jobApplicationId">Job application Id</param>
        /// <param name="useAdminClient">true if the admin client should be used explicitly, otherwise false; optional</param>
        /// <returns>The job application entity.</returns>
        Task<JobApplication> GetJobApplicationWithDetails(string jobApplicationId, bool useAdminClient = false);

        /// <summary>
        /// Get msdyn_jobopening participant by job Id
        /// </summary>
        /// <param name="jobOpeningId">Job opening id.</param>
        /// <returns>List of worker collection.</returns>
        Task<IEnumerable<JobOpeningParticipant>> GetJobOpeningParticipantByJobId(string jobOpeningId);

        /// <summary>
        /// Check if user is Application Administrator. 
        /// </summary>
        /// <param name="applicationRole">Application</param>
        /// <returns>APPlication Admin check status</returns>
        Task<bool> IsAppAdmin(TalentApplicationRole applicationRole);
    }
}
