//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The base class for controllers requiring bearer token authorization.
    /// </summary>
    [Authorize(AuthenticationSchemes = Base.Constants.BearerAuthenticationScheme)]
    public abstract class HCMWebApiAuthenticatedController : HCMWebApiCommonController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HCMWebApiAuthenticatedController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor instance.</param>
        public HCMWebApiAuthenticatedController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
