//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Common.Web.UserRoleProvider
{
    using System;
    using System.Threading.Tasks;
    using Base.Security.V2;

    /// <summary>
    /// Interface for user role
    /// </summary>
    public interface IRoleProvider
    {
        /// <summary>Method to identify if user has Admin role.</summary>
        /// <param name="principal">Current user principal</param>
        /// <param name="environmentId">The environment Id.</param>
        /// <returns>Flag to identify whether current user is Admin</returns>
        [Obsolete("This IsAdmin implementation is deprecated, please use the new implementation HasAdminPermission instead", error: false)]
        Task<bool> IsAdmin(IHCMUserPrincipal principal, string environmentId);

        /// <summary>Method checks whether the user has admin permission.</summary>
        /// <param name="principal">Current user principal</param>
        /// <param name="environmentId">The environment Id.</param>
        /// <returns>Flag to identify whether current user has admin permission.</returns>
        Task<bool> HasAdminPermission(IHCMUserPrincipal principal, string environmentId);
    }
}
