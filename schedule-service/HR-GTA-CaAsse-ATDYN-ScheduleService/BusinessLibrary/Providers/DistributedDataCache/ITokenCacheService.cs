// ----------------------------------------------------------------------------
// <copyright file="ITokenCacheService.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Providers
{
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Interface for distribute token service.
    /// </summary>
    public interface ITokenCacheService
    {
        /// <summary>
        /// Returns an instance of <see cref="Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache"/>.
        /// </summary>
        /// <param name="cacheKey">Key to cache>.</param>
        /// <returns>An instance of <see cref="Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache"/>.</returns>
        Task<TokenCache> GetCacheAsync(string cacheKey);

        /// <summary>
        /// Clears the token cache.
        /// </summary>
        /// <param name="cacheKey">Key to clear from cache.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ClearCacheAsync(string cacheKey);
    }
}
