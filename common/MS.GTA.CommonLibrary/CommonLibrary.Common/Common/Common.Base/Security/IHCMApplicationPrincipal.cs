//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace CommonLibrary.Common.Base.Security
{
    using ServicePlatform.Security;

    /// <summary>
    /// Defines a contract for HCM Application Principal.
    /// </summary>
    public interface IHCMApplicationPrincipal : IServiceContextPrincipal
    {
        /// <summary>
        /// Gets or sets the user's objectID
        /// </summary>
        string UserObjectId { get; set; }

        /// <summary>
        /// Gets family name
        /// </summary>
        string FamilyName { get; }

        /// <summary>
        /// Gets given name
        /// </summary>
        string GivenName { get; }

        /// <summary>
        /// Gets user principal name
        /// </summary>
        string UserPrincipalName { get; }

        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets tenant objectID
        /// </summary>
        string TenantObjectId { get; set; }

        /// <summary>
        /// Gets name identifier
        /// </summary>
        string NameIdentifier { get; }

        /// <summary>
        /// Gets the passport unique identifier
        /// </summary>
        string Puid { get; }

        /// <summary>
        /// Gets the audience identifier
        /// </summary>
        string Audience { get; }

        /// <summary>
        /// Gets the issuer identifier
        /// </summary>
        string Issuer { get; }

        /// <summary>
        /// Gets the trust framework policy, only present in B2C tokens
        /// </summary>
        string TrustFrameworkPolicy { get; }

        /// <summary>
        /// Gets the user token (Base64 encoded)
        /// </summary>
        string EncryptedUserToken { get; }

        /// <summary>
        /// Gets or sets the user  BAP(Business Application Platform) environment Id 
        /// </summary>
        string EnvironmentId { get; set; }

        /// <summary>
        /// Gets or sets the tenant Id from the request header
        /// </summary>
        string TenantId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the principal is in B2C mode
        /// </summary>
        bool IsB2CPrincipal { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the principal is Admin
        /// </summary>
        bool IsAdmin { get; set; }
    }
}
