//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace HR.TA.Common.Base.Security.V2
{
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The HCM B2C principal, to be used for B2C Authentication scenarios where TFP is present.
    /// </summary>
    public class HCMB2CPrincipal : HCMPrincipal, IHCMB2CPrincipal
    {
        /// <summary>Initializes a new instance of the <see cref="HCMB2CPrincipal"/> class.</summary>
        /// <param name="httpContext">The http context.</param>
        /// <param name="logger">The instnce for <see cref="ILogger"/>.</param>
        public HCMB2CPrincipal(HttpContext httpContext, ILogger logger) : base(httpContext, logger)
        {
        }

        /// <summary>
        /// Gets the emails.
        /// </summary>
        public string Emails => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADEmailsName)?.Value;

        /// <summary>
        /// Gets the family name
        /// </summary>
        public string FamilyName => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADFamilyNameName)?.Value;

        /// <summary>
        /// Gets the given name
        /// </summary>
        public string GivenName => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADGivenNameName)?.Value;

        /// <summary>
        /// Gets the identity provider of the token.
        /// </summary>
        public string IdentityProvider => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADIdentityProviderName)?.Value;

        /// <summary>
        /// The trust framework policy.
        /// </summary>
        public string TrustFrameworkPolicy => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADTrustFrameworkPolicyName)?.Value;

        /// <summary>
        /// Gets the Object Id.
        /// </summary>
        public string ObjectId => this.Claims?.FirstOrDefault(c => c.Type.ToLower() == AadNames.AADObjectIdName)?.Value;
    }
}
