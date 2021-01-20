//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace MS.GTA.ServicePlatform.Azure.AAD
{
    /// <summary>
    /// Wrapper of ADAL AuthenticationContext to allow mocking in unit tests.
    /// </summary>
    internal sealed class AuthenticationContextWrapper : IAuthenticationContextWrapper
    {
        private AuthenticationContext authenticationContext;

        public AuthenticationContextWrapper(string authority, TokenCache tokenCache)
        {
            authenticationContext = new AuthenticationContext(authority, tokenCache);
        }

        /// <inheritdoc />
        public Task<AuthenticationResult> AcquireTokenAsync(string resource, ClientAssertionCertificate clientCertificate)
        {
            return authenticationContext.AcquireTokenAsync(resource, clientCertificate);
        }
        
        /// <inheritdoc />
        public Task<AuthenticationResult> AcquireTokenAsync(string resource, string clientId, UserCredential userCredential)
        {
            return authenticationContext.AcquireTokenAsync(resource, clientId,userCredential);
        }

        /// <inheritdoc />
        public Task<AuthenticationResult> AcquireTokenAsync(string resource, ClientAssertionCertificate clientCertificate, UserAssertion userAssertion)
        {
            return authenticationContext.AcquireTokenAsync(resource, clientCertificate, userAssertion);
        }

        /// <inheritdoc />
        public Task<AuthenticationResult> AcquireTokenSilentAsync(string resource, IClientAssertionCertificate clientCertificate, UserIdentifier userId)
        {
            return authenticationContext.AcquireTokenSilentAsync(resource, clientCertificate, userId);
        }

        /// <inheritdoc />
        public Task<AuthenticationResult> AcquireTokenByAuthorizationCodeAsync(string authorizationCode, System.Uri redirectUri, ClientAssertionCertificate clientCertificate)
        {
            return authenticationContext.AcquireTokenByAuthorizationCodeAsync(authorizationCode, redirectUri, clientCertificate);
        }
    }
}
