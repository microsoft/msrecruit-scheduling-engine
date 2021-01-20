//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AudienceAuthorizeRequirement.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Web.Authorization
{
    using Microsoft.AspNetCore.Authorization;
    using MS.GTA.Common.Web.Contracts;

    /// <summary>
    /// Authorization Requirement to validate the request for the given token type with Valid Audiences.
    /// </summary>
    public class AudienceAuthorizeRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudienceAuthorizeRequirement"/> class.
        /// </summary>
        /// <param name="authorizedTokenType">token type.</param>
        public AudienceAuthorizeRequirement(AuthorizedTokenType authorizedTokenType)
        {
            this.AuthorizedTokenType = authorizedTokenType;
        }

        /// <summary>
        /// Gets type of <see cref="AuthorizedTokenType"/>.
        /// </summary>
        public AuthorizedTokenType AuthorizedTokenType { get; private set; }
    }
}