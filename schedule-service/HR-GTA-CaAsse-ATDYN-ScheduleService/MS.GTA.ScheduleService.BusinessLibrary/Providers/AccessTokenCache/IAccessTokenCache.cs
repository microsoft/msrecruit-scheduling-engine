// <copyright file="IAccessTokenCache.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.Providers
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Access token cache
    /// </summary>
    public interface IAccessTokenCache
    {
        /// <summary>
        /// Gets the item from the cache or adds the value returned from the populating function.
        /// </summary>
        /// <param name="userEmail">The user email.</param>
        /// <param name="populatingFunction">The function which provides the new value in the event of a cache miss.</param>
        /// <param name="environmentId">The environment Id.</param>
        /// <returns>The access token</returns>
        Task<string> GetOrAddAccessTokenAsync(string userEmail, Func<Task<string>> populatingFunction, string environmentId = null);
    }
}
