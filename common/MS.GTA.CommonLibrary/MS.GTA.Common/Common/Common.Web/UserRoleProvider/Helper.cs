//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Common.Web.UserRoleProvider
{
    using System;

    /// <summary>
    /// Class to encapsulate all the constants.
    /// </summary>
    public static class Helper
    {
        private const string CacheKeyPrefix = "userRoleProvider";

        /// <summary>
        /// Get Cache key
        /// </summary>
        /// <param name="tenantObjectId">Tenant Object Id</param>
        /// <param name="userObjectId">User Object Id</param>
        /// <returns></returns>
        public static string GetCacheKey(string tenantObjectId, string userObjectId)
        {
            return FormattableString.Invariant($"{CacheKeyPrefix}-{tenantObjectId}-{userObjectId}");
        }

        /// <summary>
        /// Get Cache key
        /// </summary>
        /// <param name="tenantObjectId">Tenant Object Id</param>
        /// <param name="environmentId">Environment Id</param>
        /// <param name="userObjectId">User Object Id</param>
        /// <returns></returns>
        public static string GetCacheKey(string tenantObjectId, string environmentId, string userObjectId)
        {
            return FormattableString.Invariant($"{CacheKeyPrefix}-{tenantObjectId}-{environmentId}-{userObjectId}");
        }
    }
}
