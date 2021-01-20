namespace MS.GTA.Common.Base.Security.V2
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.Common.Base.Exceptions;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;

    using Newtonsoft.Json;

    /// <summary>The hcm principal retriever.</summary>
    public class HCMPrincipalRetriever : IHCMPrincipalRetriever
    {
        /// <summary>The logger.</summary>
        private readonly ILogger logger;

        /// <summary>The http context accessor.</summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>Initializes a new instance of the <see cref="HCMPrincipalRetriever"/> class.</summary>
        /// <param name="httpContextAccessor">The http Context Accessor.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="PrincipalException">An exception if we cannot determine what type of principal to create.</exception>
        public HCMPrincipalRetriever(IHttpContextAccessor httpContextAccessor, ILogger<HCMPrincipalRetriever> logger)
        {
            Contract.CheckValue(httpContextAccessor, nameof(httpContextAccessor));
            Contract.CheckValue(logger, nameof(logger));

            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;

            var httpContext = this.httpContextAccessor.HttpContext;

            var headers = httpContext?.Request?.Headers;
            
            if (headers != null && headers.ContainsKey("Authorization") && headers["Authorization"].Count >= 1 && headers["Authorization"][0].StartsWith("Bearer "))
            {
                var token = headers["Authorization"][0].Substring("Bearer ".Length);
                var tokenHandler = new JwtSecurityTokenHandler();
                var claims = tokenHandler.ReadJwtToken(token).Claims;

                if (claims.FirstOrDefault(c => c.Type == AadNames.AADUserPrincipalNameName) != null)
                {
                    this.Principal = new HCMUserPrincipal(httpContext, this.logger);
                }
                else if (claims.FirstOrDefault(c => c.Type == AadNames.AADAppIdName) != null)
                {
                    this.Principal = new HCMApplicationPrincipal(httpContext, this.logger);
                }
                else if (claims.FirstOrDefault(c => c.Type == AadNames.AADTrustFrameworkPolicyName) != null)
                {
                    this.Principal = new HCMB2CPrincipal(httpContext, this.logger);
                }
                else if (claims.FirstOrDefault(c => c.Type == AadNames.AADUniqueNameName) != null
                    && claims.FirstOrDefault(c => c.Type == AadNames.AADIdenityProviderName) != null)
                {
                    /* If we are using a B2B principal. I.e I use bmollet@tenanta.com to login to tenantb.com
                     * I will have a unique name, as usual but I will also have an identity provider that indicates the tenant I came from.
                     * We need to key off of both so we know how to differentiate between B2C scnearios and B2B scenarios.
                     */
                    this.Principal = new HCMB2BPrincipal(httpContext, this.logger);
                }
                else
                {
                    this.logger.LogWarning($"Unable to determine the context of the request. Claims were : {JsonConvert.SerializeObject(claims.Select(c => c.Type))}");
                }
            }
        }

        /// <summary>Gets the principal.</summary>
        public IHCMPrincipal Principal { get; }
    }

    /// <summary>The principal exception.</summary>
    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, "MS.GTA.Common.Principals.Exceptions", "PrincipalException", MonitoredExceptionKind.Benign)]
    public class PrincipalException : BenignException
    {
        /// <summary>Initializes a new instance of the <see cref="PrincipalException"/> class.</summary>
        /// <param name="message">The message.</param>
        public PrincipalException(string message)
            : base(message)
        {
        }
    }
}