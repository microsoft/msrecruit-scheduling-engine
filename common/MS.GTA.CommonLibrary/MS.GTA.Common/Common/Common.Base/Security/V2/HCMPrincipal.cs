// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="HCMPrincipal.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Security.V2
{
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.ServicePlatform.Tracing;

    /// <summary>The hcm principal.</summary>
    public abstract class HCMPrincipal
    {
        /// <summary>Initializes a new instance of the <see cref="HCMPrincipal"/> class.</summary>
        /// <param name="httpContext">The http context.</param>
        /// <param name="logger">The logger.</param>
        public HCMPrincipal(HttpContext httpContext, ILogger logger)
        {
            Contract.CheckValue(httpContext, nameof(httpContext));
            Contract.CheckValue(logger, nameof(logger));

            logger.LogInformation("Initializting context user based on current http context.");
            var tokenHandler = new JwtSecurityTokenHandler();

            if (
                httpContext.Request?.Headers != null
                && httpContext.Request.Headers.ContainsKey("Authorization")
                && httpContext.Request.Headers["Authorization"].Count >= 1)
            {
                this.Token = httpContext.Request.Headers["Authorization"][0]?.Substring("Bearer ".Length);
            }

            if (Token != null)
            {
                this.Claims = tokenHandler.ReadJwtToken(this.Token).Claims;
            }
        }

        /// <summary>
        /// Gets the token from the request headers.
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Gets The audience.
        /// </summary>
        public string Audience => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADAudienceName)?.Value;

        /// <summary>
        /// Gets the issuer identifier
        /// </summary>
        public string Issuer => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADIssuerName)?.Value;

        /// <summary>
        /// Gets The claims principal.
        /// </summary>
        protected IEnumerable<Claim> Claims { get; }
    }
}
