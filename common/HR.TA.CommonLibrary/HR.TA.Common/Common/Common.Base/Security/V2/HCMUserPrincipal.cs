//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.Base.Security.V2
{
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// HCM user principal to be used in a user context.
    /// </summary>
    public class HCMUserPrincipal : HCMPrincipal, IHCMUserPrincipal
    {
        /// <summary>Initializes a new instance of the <see cref="HCMUserPrincipal"/> class this should only be used in a user context.</summary>
        /// <param name="httpContext">The http context.</param>
        /// <param name="logger">The logger.</param>
        public HCMUserPrincipal(HttpContext httpContext, ILogger logger) : base(httpContext, logger)
        {
        }

        /// <summary>
        /// Gets the email address
        /// </summary>
        public string EmailAddress => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADUniqueNameName)?.Value;

        /// <summary>
        /// Gets the family name
        /// </summary>
        public string FamilyName => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADFamilyNameName)?.Value;

        /// <summary>
        /// Gets the given name
        /// </summary>
        public string GivenName => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADGivenNameName)?.Value;

        /// <summary>
        /// Gets the passport unique identifier
        /// </summary>
        public string Puid => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADPuidName)?.Value;

        /// <summary>
        /// Gets the Tenant Id.
        /// </summary>
        public string TenantId => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADTenantIdName)?.Value;

        /// <summary>
        /// Gets the user principal name
        /// </summary>
        public string UserPrincipalName => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADUserPrincipalNameName)?.Value;

        /// <summary>
        /// Gets the Object Id.
        /// </summary>
        public string ObjectId => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADObjectIdName)?.Value;
    }
}
