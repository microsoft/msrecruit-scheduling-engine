//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.MSGraph
{
    using System.Threading.Tasks;
    using AD = Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Toke cache enum
    /// </summary>
    public enum TokenCachingOptions
    {
        /// <summary>
        /// Issue a call to the AAD and update the cache accordingly
        /// </summary>
        ForceRefreshCache,

        /// <summary>
        /// Issue a call to cache and up to AAD if not found in cache
        /// </summary>
        PreferCache,
    }

    /// <summary>
    /// Azure Active Directory client interface
    /// </summary>
    public interface IAzureActiveDirectoryClient
    {
        /// <summary>
        /// Gets an Access token for the given resource on behalf of the user in the provided access token
        /// </summary>      
        /// <param name="userAccessToken">The access token</param>
        /// <param name="tokenCachingOptions">Used to indicate if we prefer to refresh or prefer cache</param>
        /// <returns>AuthenticationResult object</returns>
        Task<AD.AuthenticationResult> GetAccessTokenFromUserTokenAsync(
            string userAccessToken,
            TokenCachingOptions tokenCachingOptions);

        /// <summary>
        /// Get on-behalf-of user access token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <param name="tokenCachingOptions">Token caching options</param>
        /// <returns>Authentication result which contains on behalf of token</returns>
        Task<AD.AuthenticationResult> GetAccessTokenFromRefreshTokenAsync(
            string refreshToken,
            TokenCachingOptions tokenCachingOptions);

        /// <summary>
        /// Gets an Access token for the given resource on behalf of the user in the provided access token
        /// </summary>
        /// <param name="userAccessToken">The access token</param>
        /// <param name="tenantId">Tenant ID</param>
        /// <param name="resource">Resource ID</param>
        /// <param name="tokenCachingOptions">Token caching options</param>
        /// <returns>Authentication result which contains on behalf of token</returns>
        Task<AD.AuthenticationResult> GetAccessTokenForResourceFromUserTokenAsync(
            string userAccessToken,
            string tenantId,
            string resource,
            TokenCachingOptions tokenCachingOptions);

        /// <summary>Get a token in the app token format for a given resource.</summary>
        /// <param name="tenant">The tenant which the token will be for. Note, the app needs to be inside of the given tenant for this to work.</param>
        /// <param name="resource">The resource the token have access to.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<AD.AuthenticationResult> GetAppToken(string tenant, string resource = null);
    }
}
