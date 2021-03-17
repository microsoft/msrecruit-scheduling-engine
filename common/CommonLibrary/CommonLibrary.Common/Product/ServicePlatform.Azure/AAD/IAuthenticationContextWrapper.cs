//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace CommonLibrary.ServicePlatform.Azure.AAD
{
    public interface IAuthenticationContextWrapper
    {
        /// <summary>
        /// Acquires an access token from the authority.
        /// </summary>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <param name="clientCertificate">The client certificate to use for token acquisition.</param>
        /// <returns>
        /// Result containing the Access Token and the Access Token's expiration time.
        /// </returns>
        Task<AuthenticationResult> AcquireTokenAsync(string resource, ClientAssertionCertificate clientCertificate);

        /// <summary>
        /// Acquires an access token from the authority.
        /// </summary>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <param name="clientId">The client Id to use for token acquisition.</param>
        /// <param name="userCredential">The credentials of the account being used </param>
        /// <returns>
        /// Result containing the Access Token and the Access Token's expiration time.
        /// </returns>
        Task<AuthenticationResult> AcquireTokenAsync(string resource, string clientId, UserCredential userCredential);

        /// <summary>
        /// Acquires an access token from the authority on behalf of a user. It requires using a user token previously received.
        /// </summary>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <param name="clientCertificate">The client certificate to use for token acquisition.</param>
        /// <param name="userAssertion">The user assertion (token) to use for token acquisition.</param>
        /// <returns>
        /// Result containing the Access Token and the Access Token's expiration time.
        /// </returns>
        Task<AuthenticationResult> AcquireTokenAsync(string resource, ClientAssertionCertificate clientCertificate, UserAssertion userAssertion);

        /// <summary>
        /// Acquires security token without asking for user credential.
        /// </summary>
        /// <param name="resource">Identifier of the target resource that is the recipient of the requested token.</param>
        /// <param name="clientCertificate">The client certificate to use for token acquisition.</param>
        /// <param name="userId">Identifier of the user token is requested for.</param>
        /// <returns>
        /// Result contains Access Token, its expiration time, user information. Throw AdalException if fail. 
        /// </returns>
        Task<AuthenticationResult> AcquireTokenSilentAsync(string resource, IClientAssertionCertificate clientCertificate, UserIdentifier userId);

        /// <summary>
        /// Acquires security token from the authority using an authorization code previously received.
        /// </summary>
        /// <param name="authorizationCode">The authorization code received from service authorization endpoint.</param>
        /// <param name="redirectUri">The redirect address used for obtaining authorization code.</param>
        /// <param name="clientCertificate">The client certificate to use for token acquisition.</param>
        /// <returns>
        /// Result contains Access Token, its expiration time, user information.
        /// </returns>
        Task<AuthenticationResult> AcquireTokenByAuthorizationCodeAsync(string authorizationCode, System.Uri redirectUri, ClientAssertionCertificate clientCertificate);
    }
}
