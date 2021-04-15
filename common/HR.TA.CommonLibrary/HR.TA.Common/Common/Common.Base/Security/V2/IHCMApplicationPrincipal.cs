//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n
namespace HR.TA.Common.Base.Security.V2
{
    /// <summary>
    /// The HCM application principal, used for application context requests.
    /// </summary>
    public interface IHCMApplicationPrincipal : IHCMPrincipal
    {
        /// <summary>
        /// Gets the application id.
        /// </summary>
        string ApplicationId { get; }

        /// <summary>
        /// Gets the identity provider of the token.
        /// </summary>
        string IdentityProvider { get; }

        /// <summary>
        /// Gets the Tenant Id.
        /// </summary>
        string TenantId { get; }

        /// <summary>
        /// Gets the Object Id.
        /// </summary>
        string ObjectId { get; }
    }
}
