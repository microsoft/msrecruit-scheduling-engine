//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Security;

namespace MS.GTA.ServicePlatform.Security
{
    /// <summary>
    /// Represents the result provided by specific authenticator implementations.
    /// </summary>
    public sealed class AuthenticationResult
    {
        private static readonly AuthenticationResult failAndContinue = new AuthenticationResult(AuthenticationResultType.FailAndContinue);
        private static readonly AuthenticationResult failAndStop = new AuthenticationResult(AuthenticationResultType.FailAndStop);

        // This should stay private, no need to expose
        private AuthenticationResult(AuthenticationResultType resultType)
        {
            ResultType = resultType;
        }

        /// <summary>
        /// Creates a successful <see cref="AuthenticationResult"/> with the provided principal.
        /// </summary>
        public AuthenticationResult(IServiceContextPrincipal principal)
        {
            Contract.CheckValue(principal, nameof(principal));

            ResultType = AuthenticationResultType.Success;
            Principal = principal;
        }

        /// <summary>
        /// Gets the singleton instance for <see cref="AuthenticationResultType.FailAndContinue"/> <see cref="AuthenticationResult"/>.
        /// </summary>
        public static AuthenticationResult FailAndContinue
        {
            get { return failAndContinue; }
        }

        /// <summary>
        /// Gets the singleton instance for <see cref="AuthenticationResultType.FailAndStop"/> <see cref="AuthenticationResult"/>.
        /// </summary>
        public static AuthenticationResult FailAndStop
        {
            get { return failAndStop; }
        }

        /// <summary>
        /// Gets authenticator result type regardless of the outcome.
        /// </summary>
        public AuthenticationResultType ResultType { get; }

        /// <summary>
        /// Gets the authentication result principal. The value is only present when the result type 
        /// is set to <see cref="AuthenticationResultType.Success"/> and null otherwise.
        /// </summary>
        public IServiceContextPrincipal Principal { get; }
    }

    /// <summary>
    /// Determines the action that the platform authenticator will take when 
    /// an authentication result is received.
    /// </summary>
    public enum AuthenticationResultType
    {
        /// <summary>
        /// Denotes a successful authentication result. The containing <see cref="AuthenticationResult"/> is 
        /// expected to contain an instance of <see cref="IServiceContextPrincipal"/> and all remaining 
        /// authenticators left in the platform authentication pipeline will be skipped.
        /// </summary>
        Success = 1,

        /// <summary>
        /// Denotes a failed authentication attempt by a specific authenticator with a "don't know" semantic. An 
        /// authenticator cannot authenticate the request but also cannot say that the authentication should 
        /// fail as this may be handled by a different authenticator in the pipeline since based on the request
        /// headers it uses an authentication method not known to this authenticator.
        /// </summary>
        FailAndContinue = 2,

        /// <summary>
        /// Denotes a terminal failed authentication attempt by a specific authenticator with a "deny" semantic. An 
        /// authenticator cannot authenticate the request even though the provided authentication method was intended 
        /// for this authenticator. In such case the authentication pipeline can be stopped and no further authenticators
        /// need to be run.
        /// </summary>
        FailAndStop = 3,
    }
}
