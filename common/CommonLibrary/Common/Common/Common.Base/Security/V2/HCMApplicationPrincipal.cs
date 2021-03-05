//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.Base.Security.V2
{
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The HCM application principal, used for application context requests.
    /// </summary>
    public class HCMApplicationPrincipal : HCMPrincipal, IHCMApplicationPrincipal
    {
        /// <summary>Initializes a new instance of the <see cref="HCMApplicationPrincipal"/> class.</summary>
        /// <param name="httpContext">The http context.</param>
        /// <param name="logger">The logger.</param>
        public HCMApplicationPrincipal(HttpContext httpContext, ILogger logger) : base(httpContext, logger)
        {
        }

        /// <summary>The application id.</summary>
        public string ApplicationId => this.Claims?.FirstOrDefault(c => c.Type == AadNames.AADApplicationIdName)?.Value;

        /// <summary>
        /// The identity provider of the token.
        /// </summary>
        public string IdentityProvider => this.Claims?.FirstOrDefault(c => c.Type == AadNames.AADIdentityProviderName)?.Value;

        /// <summary>
        /// Gets the Tenant Id.
        /// </summary>
        public string TenantId => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADTenantIdName)?.Value;

        /// <summary>
        /// Gets the Object Id.
        /// </summary>
        public string ObjectId => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADObjectIdName)?.Value;
    }
}
