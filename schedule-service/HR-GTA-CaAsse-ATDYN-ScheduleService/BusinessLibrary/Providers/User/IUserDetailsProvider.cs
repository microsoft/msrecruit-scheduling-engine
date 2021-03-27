//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Providers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using HR.TA.ScheduleService.Contracts.V1;

    /// <summary>
    /// Interface to define room resource related operations
    /// </summary>
    public interface IUserDetailsProvider
    {
        /// <summary>
        /// Gets user photo
        /// </summary>
        /// <param name="userObjectId">user id</param>
        /// <returns>A list of rooms</returns>
        Task<string> GetUserPhotoAsync(string userObjectId);

        /// <summary>
        /// Gets user location
        /// </summary>
        /// <param name="userObjectId">user id</param>
        /// <returns>A list of rooms</returns>
        Task<Microsoft.Graph.User> GetUserAsync(string userObjectId);

        /// <summary>
        /// Get user by seraching with email
        /// </summary>
        /// <param name="email">Email of the User</param>
        /// <param name="serviceAccountName">Service Account for token generation</param>
        /// <returns>user</returns>
        Task<GraphUserResponse> SearchUserByEmail(string email, string serviceAccountName);
    }
}
