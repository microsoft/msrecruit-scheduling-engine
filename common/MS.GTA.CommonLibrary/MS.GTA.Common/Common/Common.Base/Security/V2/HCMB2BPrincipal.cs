//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace MS.GTA.Common.Base.Security.V2
{
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;

    /// <summary>The HCM B2B principal.</summary>
    public class HCMB2BPrincipal : HCMPrincipal, IHCMB2BPrincipal
    {
        /// <summary>Initializes a new instance of the <see cref="HCMB2BPrincipal"/> class.</summary>
        /// <param name="httpContext">The http context.</param>
        /// <param name="logger">The logger.</param>
        public HCMB2BPrincipal(HttpContext httpContext, ILogger logger) : base(httpContext, logger)
        {
        }

        /// <summary>The users name.</summary>
        public string Email => this.Claims?.FirstOrDefault(c => c.Type == AadNames.AADEmailName)?.Value;

        /// <summary>
        /// Gets the family name
        /// </summary>
        public string FamilyName => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADFamilyNameName)?.Value;

        /// <summary>
        /// Gets the given name
        /// </summary>
        public string GivenName => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADGivenNameName)?.Value;

        /// <summary>
        /// The users IP address.
        /// </summary>
        public string IpAddress => this.Claims?.FirstOrDefault(c => c.Type == AadNames.AADIPAddressName)?.Value;

        /// <summary>
        /// The users name
        /// </summary>
        public string Name => this.Claims?.FirstOrDefault(c => c.Type == AadNames.AADNameName)?.Value;

        /// <summary>
        /// Gets the Tenant Id.
        /// </summary>
        public string TenantId => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADTenantIdName)?.Value;

        /// <summary>
        /// Gets the unique name, likely an email address.
        /// </summary>
        public string UniqueName => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADUniqueNameName)?.Value;
    }
}
