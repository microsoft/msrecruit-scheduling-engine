//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Web.Authorization
{
    using Microsoft.AspNetCore.Authorization;
    using HR.TA.Common.Web.Contracts;

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
