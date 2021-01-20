//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.MSGraph
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Graph;
    using MS.GTA.Common.TestBase;

    public interface IMsGraphProvider
    {
        Task<AuthenticationHeaderValue> GetGraphSendEmailResourceToken(int maxRetries = 1);

        Task<IGraphServiceOrganizationCollectionPage> GetOrganizationAsync(string token, int maxRetries = 1);

        /// <summary>
        /// Acquire talent service authentication token
        /// </summary>
        /// <param name="userEmailPasswordSecret">Instance of User Email Password Secret</param>
        /// <param name="userSecretPassword">Email Password </param>
        /// <returns>The talent service token</returns>
        [Obsolete("Please use the user directory service client implementation for any future work on MSGraph, any questions/issues email vanguard@microsoft.com. Going forward this will turn into an error.")]
        Task<AuthenticationHeaderValue> GetGraphEmailResourceToken(UserEmailPasswordSecret userEmailPasswordSecret, string userSecretPassword = null);

        /// <summary>
        /// Get user details
        /// </summary>
        /// <param name="userId">User objectID</param>
        /// <param name="token">User access token</param>
        /// <param name="selectedFields">Select specific fields not returned by default. See <see cref="https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_get"/></param>
        /// <param name="maxRetries">The maximum number of retries.</param>
        /// <returns>User details collection</returns>
        [Obsolete("Please use the user directory service client implementation for any future work on MSGraph, any questions/issues email vanguard@microsoft.com. Going forward this will turn into an error.")]
        Task<User> GetUserAsync(string userId, string token, IEnumerable<Expression<Func<User, object>>> selectedFields = null, int maxRetries = 1);

        /// <summary>
        /// Get users using email addresses
        /// </summary>
        /// <param name="emails">List of email addresses</param>
        /// <param name="token">User access token</param>
        /// <returns>List of User objects</returns>
        [Obsolete("Please use the user directory service client implementation for any future work on MSGraph, any questions/issues email vanguard@microsoft.com. Going forward this will turn into an error.")]
        Task<IEnumerable<User>> GetBulkUsersWithEmail(IEnumerable<string> emails, string token);

        /// <summary>
        /// Gets the user photo asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="token">The token.</param>
        /// <param name="maxRetries">The maximum retries.</param>
        /// <returns>User Image</returns>
        Task<string> GetUserPhotoAsync(string userId, string token, int maxRetries = 1);
    }
}
