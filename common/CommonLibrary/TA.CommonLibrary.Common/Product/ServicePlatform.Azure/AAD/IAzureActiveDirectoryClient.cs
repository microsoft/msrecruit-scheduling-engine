//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace TA.CommonLibrary.ServicePlatform.Azure.AAD
{
    /// <summary>
    /// An interface for class to return the access token to authenticate access to azure resource.
    /// </summary>
    public interface IAzureActiveDirectoryClient
    {
        /// <summary>
        /// Acquires an access token from Azure Active Directory with the default authority.
        /// </summary>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <returns>
        /// Result containing the Access Token and the Access Token's expiration time.
        /// </returns>
        Task<AuthenticationResult> GetAppOnlyAccessTokenAsync(string resource);

        /// <summary>
        /// Acquires an access token from Azure Active Directory with a specific authority.
        /// </summary>
        /// <param name="authority">Override address of the authority to issue token.</param>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <returns>
        /// Result containing the Access Token and the Access Token's expiration time.
        /// </returns>
        Task<AuthenticationResult> GetAppOnlyAccessTokenAsync(string authority, string resource);

        /// <summary>
        /// Acquires an access token from Azure Active Directory with the default authority on behalf of a user. It requires using a user token previously received.
        /// </summary>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <param name="userAccessToken">Assertion representing the user.</param>
        /// <returns>
        /// Result containing the Access Token and the Access Token's expiration time.
        /// </returns>
        Task<AuthenticationResult> GetOnBehalfOfAccessTokenAsync(string resource, string userAccessToken);

        /// <summary>
        /// Acquires an access token from Azure Active Directory with a specific authority on behalf of a user. It requires using a user token previously received.
        /// </summary>
        /// <param name="authority">Override address of the authority to issue token.</param>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <param name="userAccessToken">Assertion representing the user.</param>
        /// <returns>
        /// Result containing the Access Token and the Access Token's expiration time.
        /// </returns>
        Task<AuthenticationResult> GetOnBehalfOfAccessTokenAsync(string authority, string resource, string userAccessToken);

        /// <summary>
        /// Acquires security token from the authority using an authorization code previously received.
        /// </summary>
        /// <param name="authorizationCode">The authorization code received from service authorization endpoint.</param>
        /// <param name="redirectUrl">The redirect Url address used for obtaining authorization code.</param>
        /// <returns>
        /// Result contains Access Token, its expiration time, user information.
        /// </returns>
        Task<AuthenticationResult> GetAccessTokenByAuthorizationCode(string authorizationCode, Uri redirectUrl);

        /// <summary>
        /// Acquires security token without asking for user credential.
        /// </summary>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <param name="userIdentifier">Identifier of the user token is requested for.</param>
        /// <returns>
        /// Result contains Access Token, its expiration time, user information. Throw AdalException if fail. 
        /// </returns>
        Task<AuthenticationResult> GetAccessTokenSilentAsync(string resource, UserIdentifier userIdentifier);

        /// <summary>
        /// Acquires security token without asking for user credential.
        /// </summary>
        /// <param name="authority">Override address of the authority to issue token.</param>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <param name="userIdentifier">Identifier of the user token is requested for.</param>
        /// <returns>
        /// Result contains Access Token, its expiration time, user information. 
        /// </returns>
        Task<AuthenticationResult> GetAccessTokenSilentAsync(string authority, string resource, UserIdentifier userIdentifier);
    }
}
