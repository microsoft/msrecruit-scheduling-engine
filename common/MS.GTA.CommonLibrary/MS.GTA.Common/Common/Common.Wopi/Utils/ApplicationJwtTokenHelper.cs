//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ApplicationJwtTokenHelper.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using CommonDataService.Common.Internal;
    using Microsoft.Azure.KeyVault;
    using MS.GTA.Common.Base.KeyVault;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Security;
    
    
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using MS.GTA.Common.Wopi.Interfaces;

    public class ApplicationJwtTokenHelper : IApplicationJwtTokenHelper
    {
        /// <summary>
        /// The tenant id
        /// </summary>
        private const string TenantIdTokenClaim = "tenantId";

        /// <summary>
        /// The environment id
        /// </summary>
        private const string EnvironmentIdTokenClaim = "environmentId";

        /// <summary>
        /// The application id
        /// </summary>
        private const string ApplicationIdTokenClaim = "applicationId";

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<ApplicationJwtTokenHelper> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationJwtTokenHelper"/> class.
        /// </summary>
        /// <param name="logger">Logger instance</param>
        public ApplicationJwtTokenHelper(ILogger<ApplicationJwtTokenHelper> logger)
        {
            Contract.CheckValue(logger, nameof(logger), "logger should be provided");

            this.logger = logger;
        }

        /// <summary>
        /// Gets an access token
        /// </summary>
        /// <param name="tenantId">tenant id</param>
        /// <param name="environmentId">environment id</param>
        /// <param name="applicationId">application id</param>
        /// <returns>The token as a string</returns>
        public string GenerateToken(string tenantId, string environmentId, string applicationId)
        {
            Contract.CheckNonEmpty(tenantId, nameof(tenantId));
            Contract.CheckNonEmpty(environmentId, nameof(environmentId));
            Contract.CheckNonEmpty(applicationId, nameof(applicationId));

            var claims = new List<Claim>()
            {
                new Claim(TenantIdTokenClaim, tenantId),
                new Claim(EnvironmentIdTokenClaim, environmentId),
                new Claim(ApplicationIdTokenClaim, applicationId),
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityTokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Read an access token
        /// </summary>
        /// <param name="token">The token to extract claims</param>
        /// <returns>A tuple consisting tenantId, environmentId and applicationId if the token is valid</returns>
        public Tuple<string, string, string> ReadToken(string token)
        {
            Contract.CheckNonEmpty(token, nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(token))
            {
                JwtSecurityToken validatedToken = tokenHandler.ReadJwtToken(token);
                return this.ExtractTokenInfo(validatedToken.Claims);
            }

            this.logger.LogInformation($"Token is not in valid jwt format");
            return null;
        }

        /// <summary>
        /// Hydrates a token with the right fields
        /// </summary>
        /// <param name="claims">The claims to check against</param>
        /// <returns>The hydrated token</returns>
        private Tuple<string, string, string> ExtractTokenInfo(IEnumerable<Claim> claims)
        {
            Contract.CheckValue(claims, nameof(claims));
            this.logger.LogInformation($"Extracting claims from token");

            var tenantId = claims.SingleOrDefault(c => c.Type == TenantIdTokenClaim)?.Value;
            var environmentId = claims.SingleOrDefault(c => c.Type == EnvironmentIdTokenClaim)?.Value;
            var applicationId = claims.SingleOrDefault(c => c.Type == ApplicationIdTokenClaim)?.Value;

            return new Tuple<string, string, string>(tenantId, environmentId, applicationId);
        }
    }
}
