// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IHCMB2CPrincipal.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Security.V2
{
    /// <summary>
    /// The HCM B2C principal, to be used for B2C Authentication scenarios where TFP is present.
    /// </summary>
    public interface IHCMB2CPrincipal : IHCMPrincipal
    {
        /// <summary>
        /// Gets the emails.
        /// </summary>
        string Emails { get; }

        /// <summary>
        /// Gets the family name
        /// </summary>
        string FamilyName { get; }

        /// <summary>
        /// Gets the given name
        /// </summary>
        string GivenName { get; }

        /// <summary>
        /// Gets the identity provider of the token.
        /// </summary>
        string IdentityProvider { get; }

        /// <summary>
        /// Gets the trust framework policy.
        /// </summary>
        string TrustFrameworkPolicy { get; }

        /// <summary>
        /// Gets the Object Id.
        /// </summary>
        string ObjectId { get; }
    }
}
