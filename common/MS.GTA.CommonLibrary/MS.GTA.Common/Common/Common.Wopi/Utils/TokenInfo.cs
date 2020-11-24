//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TokenInfo.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Utils
{
    /// <summary>
    /// Token information
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// Gets or sets the string representing the PUID in the access token
        /// </summary>
        public string Puid { get; set; }

        /// <summary>
        /// Gets or sets the string representing the user's first name in the access token
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the string representing the user's surname in the access token
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the tenant id in the access token
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the user object Id
        /// </summary>
        public string UserObjectId { get; set; }

        /// <summary>
        /// Gets or sets the user email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user environment Id
        /// </summary>
        public string EnvironmentId { get; set; }

        /// <summary>
        /// Gets or sets the applicant Invitation Id, this is for B2C access. 
        /// </summary>
        public string InvitationId { get; set; }
    }
}
