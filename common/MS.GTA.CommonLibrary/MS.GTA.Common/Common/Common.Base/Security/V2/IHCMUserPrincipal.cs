//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.Base.Security.V2
{
    /// <summary>
    /// HCM user principal to be used in a user context.
    /// </summary>
    public interface IHCMUserPrincipal : IHCMPrincipal
    {
        /// <summary>
        /// Gets the email address
        /// </summary>
        string EmailAddress { get; }

        /// <summary>
        /// Gets given name
        /// </summary>
        string GivenName { get; }

        /// <summary>
        /// Gets the family name
        /// </summary>
        string FamilyName { get; }

        /// <summary>
        /// Gets the passport unique identifier
        /// </summary>
        string Puid { get; }

        /// <summary>
        /// Gets the Tenant Id.
        /// </summary>
        string TenantId { get; }

        /// <summary>
        /// Gets the user principal name
        /// </summary>
        string UserPrincipalName { get; }

        /// <summary>
        /// Gets the Object Id.
        /// </summary>
        string ObjectId { get; }
    }
}
