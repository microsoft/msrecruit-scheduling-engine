//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Wopi.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using CommonDataService.Common.Internal;
    using Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Interfaces;
    using ServicePlatform.Configuration;
    using ServicePlatform.Security;
    using ServicePlatform.Tracing;

    /// <summary>
    /// The WOPI TokenHelper
    /// </summary>
    public class WopiTokenHelper : IAccessTokenHelper
    {
        /// <summary>
        /// Certificate Manager
        /// </summary>
        private CertificateManager certificateManager;

        /// <summary>
        /// Trace source
        /// </summary>
        private readonly ITraceSource trace;

        /// <summary>
        /// The token's audience
        /// </summary>
        private readonly string audience;

        /// <summary>
        /// The token's issuer
        /// </summary>
        private readonly string issuer;

        /// <summary>
        /// The certificate's thumbprint
        /// </summary>
        private readonly IList<string> certThumbprints;

        /// <summary>
        /// The token's lifetime in minutes
        /// </summary>
        private readonly int tokenValidityInMinutes;

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiTokenHelper"/> class.
        /// </summary>
        /// <param name="configurationManager">Configuration settings</param>
        /// <param name="trace">Trace source instance</param>
        public WopiTokenHelper(IConfigurationManager configurationManager, ITraceSource trace)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager), "configurationManager must be provided");
            Contract.CheckValue(trace, nameof(trace), "trace should be provided");

            this.trace = trace;
            var settings = configurationManager.Get<WopiSetting>();
            this.audience = settings.WopiValidAudience;
            this.issuer = settings.WopiValidIssuer;
            this.certThumbprints = settings.CertThumbprintList;
            this.tokenValidityInMinutes = settings.WopiTokenValidityInMinutes;
            this.certificateManager = new CertificateManager();

        }

        /// <summary>
        /// Gets an access token
        /// </summary>
        /// <param name="tokenInfo">The token information</param>
        /// <returns>The token as a string</returns>
        public string GenerateToken(TokenInfo tokenInfo)
        {
            Contract.CheckValue(tokenInfo, nameof(tokenInfo), "tokenInfo must be defined");

            var exceptions = new List<Exception>();

            foreach (var thumbprint in this.certThumbprints)
            {
                try
                {
                    var certificate = this.certificateManager.FindByThumbprint(thumbprint, StoreName.My, StoreLocation.CurrentUser);
                    var signingKey = new X509SecurityKey(certificate);
                    var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256Signature);

                    var claims = new List<Claim>()
                    {
                        new Claim(Constants.FirstName, tokenInfo.FirstName),
                        new Claim(Constants.Surname, tokenInfo.Surname),
                        // new Claim(Constants.Puid, tokenInfo.Puid),
                        new Claim(Constants.ObjectId, tokenInfo.UserObjectId),
                        new Claim(Constants.TenantId, tokenInfo.TenantId),
                        new Claim(Constants.Email, tokenInfo.Email),
                        // new Claim(Constants.EnvironmentId, tokenInfo.EnvironmentId),
                    };

                    //if (!string.IsNullOrEmpty(tokenInfo.InvitationId))
                    //{
                    //    claims.Add(new Claim(Constants.InvitationId, tokenInfo.InvitationId));
                    //}

                    var claimsIdentity = new ClaimsIdentity(claims);
                    var notBefore = DateTime.UtcNow;
                    var expires = notBefore + TimeSpan.FromMinutes(this.tokenValidityInMinutes);

                    var securityTokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Audience = this.audience,
                        Issuer = this.issuer,
                        Subject = claimsIdentity,
                        SigningCredentials = signingCredentials,
                        NotBefore = notBefore,
                        Expires = expires
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(securityTokenDescriptor);
                    return tokenHandler.WriteToken(token);
                }
                catch (CertificateNotFoundException ex)
                {
                    exceptions.Add(ex);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException($"Could not successfully acquire certificate using thumbprints: {string.Join(", ", this.certThumbprints)}", exceptions);
        }

        /// <summary>
        /// Validates an access token
        /// </summary>
        /// <param name="wopiToken">The token to validate</param>
        /// <returns>The Security token if the token is valid</returns>
        public TokenInfo ValidateToken(string wopiToken)
        {
            Contract.CheckNonEmpty(wopiToken, nameof(wopiToken), "token must be defined");

            try
            {
                JwtSecurityToken validatedToken = null;

                validatedToken = (JwtSecurityToken)this.ValidateWopiJWTToken(wopiToken);
                this.trace.TraceInformation($"WopiTokenHelper: Token validated");
                return this.ExtractTokenInfo(validatedToken.Claims);
            }
            catch (SecurityTokenException ex)
            {
                this.trace.TraceInformation($"WopiTokenHelper: token validation failed. Message: {ex.ToString()}");
                return null;
            }
        }

        /// <summary>
        /// Validates an access token
        /// </summary>
        /// <param name="token">The token string</param>
        /// <returns>The security token</returns>
        private SecurityToken ValidateWopiJWTToken(string token)
        {
            Contract.CheckNonEmpty(token, nameof(token), "token must be defined");

            var exceptions = new List<Exception>();

            foreach (var thumbprint in this.certThumbprints)
            {
                try
                {
                    var certificate = this.certificateManager.FindByThumbprint(thumbprint, StoreName.My, StoreLocation.CurrentUser);
                    var signingKey = new X509SecurityKey(certificate);

                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = this.audience,
                        ValidIssuer = this.issuer,
                        IssuerSigningKey = signingKey,
                    };

                    // ValidateToken throws Exceptions for invalid tokens (e.g. expiry, invalid, null, empty etc. )
                    var tokenHandler = new JwtSecurityTokenHandler();
                    SecurityToken validatedToken;
                    tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                    return validatedToken;
                }
                catch (CertificateNotFoundException ex)
                {
                    exceptions.Add(ex);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException($"Could not successfully acquire certificate using thumbprints: {string.Join(", ", this.certThumbprints)}", exceptions);
        }

        /// <summary>
        /// Hydrates a token with the right fields
        /// </summary>
        /// <param name="claims">The claims to check against</param>
        /// <returns>The hydrated token</returns>
        private TokenInfo ExtractTokenInfo(IEnumerable<Claim> claims)
        {
            Contract.AssertValue(claims, nameof(claims));

            var token = new TokenInfo();
            this.trace.TraceInformation($"WopiTokenHelper: Extracting claims from token");

            //token.Puid = claims.SingleOrDefault(c => c.Type.ToLower() == Constants.Puid).Value;
            token.FirstName = claims.SingleOrDefault(c => c.Type.ToLower() == Constants.FirstName).Value;
            token.Surname = claims.SingleOrDefault(c => c.Type.ToLower() == Constants.Surname).Value;
            token.TenantId = claims.SingleOrDefault(c => c.Type.ToLower() == Constants.TenantId).Value;
            token.UserObjectId = claims.SingleOrDefault(c => c.Type.ToLower() == Constants.ObjectId).Value;
            token.Email = claims.FirstOrDefault(c => c.Type.ToLower() == Constants.Email).Value;
            //token.EnvironmentId = claims.FirstOrDefault(c => c.Type.ToLower() == Constants.EnvironmentId).Value;
            //token.InvitationId = claims.FirstOrDefault(c => c.Type.ToLower() == Constants.InvitationId)?.Value;
            return token;
        }
    }
}
