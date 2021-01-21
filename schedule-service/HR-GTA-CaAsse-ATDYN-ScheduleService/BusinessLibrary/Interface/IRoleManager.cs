//----------------------------------------------------------------------------
// <copyright file="IRoleManager.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// IRoleManager interface
    /// </summary>
    public interface IRoleManager
    {
        /// <summary>
        /// Is user role hm or rec or contributor
        /// </summary>
        /// <param name="userID">user id</param>
        /// <param name="jobApplicationID">job applicationid</param>
        /// <param name="scheduleID">schedule id</param>
        /// <returns>response</returns>
        Task<bool> IsUserHMorRecOrContributor(string userID, string jobApplicationID, string scheduleID);

        /// <summary>
        /// Is user role interviewer or AA
        /// </summary>
        /// <param name="userID">user id</param>
        /// <param name="jobApplicationID">job applicationid</param>
        /// <returns>response</returns>
        Task<bool> IsUserInterviewerOrAA(string userID, string jobApplicationID);

        /// <summary>
        /// Is user role interviewer or AA
        /// </summary>
        /// <param name="userID">user id</param>
        /// <param name="jobApplicationID">job applicationid</param>
        /// <returns>response</returns>
        Task<bool> IsUserContributor(string userID, string jobApplicationID);

        /// <summary>
        /// Is user part of participants list or not
        /// </summary>
        /// <param name="userObjectId">user id</param>
        /// <param name="jobApplicationId">job applicationid</param>
        /// <param name="scheduleID">schedule id</param>
        /// <returns>response</returns>
        Task<bool> IsUserInJobApplicationParticipants(string userObjectId, string jobApplicationId, string scheduleID);

        /// <summary>
        /// Verifies if all given object identifiers have interviewer role in job application or not
        /// </summary>
        /// <param name="participantOids">user object id</param>
        /// <param name="jobApplicationId">job application id</param>
        /// <returns>returns true if all users are interviewer in job application</returns>
        Task<bool> AreParticipantsInterviewer(IList<string> participantOids, string jobApplicationId);

        /// <summary>
        /// Verify if the user has read only admin role
        /// </summary>
        /// <param name="userOid">user object id</param>
        /// <returns>user read only admin status</returns>
        Task<bool> IsReadOnlyRole(string userOid);

        /// <summary>
        /// Verify whether user is Wob user or not
        /// </summary>
        /// <param name="isUserWobAuthenticated">user is a wob authenticated user or not</param>
        /// <returns>user present wob status</returns>
        bool IsUserWobAuthenticated(bool isUserWobAuthenticated);
    }
}
