// <copyright file="IUserDetailsProvider.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MS.GTA.ScheduleService.Contracts.V1;

namespace MS.GTA.ScheduleService.BusinessLibrary.Providers
{
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
