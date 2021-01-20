//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Data
{
    using System.Threading.Tasks;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.TalentEntities.Enum;

    public interface IDataAccess
    {
        /// <summary>
        /// Update the job application status reason
        /// </summary>
        /// <param name="jobApplicationId">Job application Id</param>
        /// <param name="statusReason">The reason to update</param>
        /// <param name="invitationId">The invitation Id</param>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> UpdateOfferedJobApplicationStatusReason(string jobApplicationId, JobApplicationStatusReason statusReason, string invitationId = null);

        /// <summary>Get a job application by ID</summary>
        /// <param name="jobApplicationId">Job application Id </param>
        /// <param name="isAccessedByAdmin">Is accessed by Admin</param>
        /// <returns>The job application entity. </returns>
        Task<Job> GetJobWithApplication(string jobApplicationId, bool isAccessedByAdmin = false);
    }
}
