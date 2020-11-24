//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="HCMApplicationPrincipal.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Security
{
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using MS.GTA.ServicePlatform.Tracing;

    /// <summary>
    /// HCM Application Principal which leverages HttpContext as a container.
    /// </summary>
    public class HCMApplicationPrincipal : IHCMApplicationPrincipal
    {
        /// <summary>
        /// User objectID claim type
        /// </summary>
        private const string UserObjectIdClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        /// <summary>
        /// Tenant objectID claim type
        /// </summary>
        private const string TenantObjectIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";

        /// <summary>
        /// Identity Provider Claim Type
        /// </summary>
        private const string IdentityProviderClaimType = "http://schemas.microsoft.com/identity/claims/identityprovider";

        /// <summary>
        /// Passport unique identifier ID
        /// </summary>
        private const string PuidClaimIdentifier = "puid";

        /// <summary>
        /// Audience claim
        /// </summary>
        private const string AudienceClaimIdentifier = "aud";

        /// <summary>
        /// Issuer claim
        /// </summary>
        private const string IssuerClaimIdentifier = "iss";

        /// <summary>
        /// Trust framework policy
        /// </summary>
        private const string TrustFrameworkPolicyClaimIdentifier = "tfp";

        /// <summary>
        /// Trace source instance
        /// </summary>
        private static ITraceSource trace;

        /// <summary>
        /// User context
        /// </summary>
        private readonly ClaimsPrincipal contextUser;

        /// <summary>
        /// User objectID
        /// </summary>
        private string userObjectId;

        /// <summary>
        /// Family name
        /// </summary>
        private string familyName;

        /// <summary>
        /// Given name
        /// </summary>
        private string givenName;

        /// <summary>
        /// User principal name
        /// </summary>
        private string userPrincipalName;

        /// <summary>
        /// Name Identifier
        /// </summary>
        private string nameIdentifier;

        /// <summary>
        /// Tenant objectID
        /// </summary>
        private string tenantObjectId;

        /// <summary>
        /// Email address
        /// </summary>
        private string emailAddress;

        /// <summary>
        /// Passport unique identifier
        /// </summary>
        private string puid;

        /// <summary>
        /// Audience identifier
        /// </summary>
        private string audience;

        /// <summary>
        /// Issuer identifier
        /// </summary>
        private string issuer;

        /// <summary>
        /// Trust framework policy identifier
        /// </summary>
        private string tfp;

        /// <summary>
        /// Initializes a new instance of the <see cref="HCMApplicationPrincipal" /> class.
        /// </summary>
        /// <param name="context">HTTP context</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "The call stack was reviewed and no problems were found.")]
        public HCMApplicationPrincipal(HttpContext context)
        {
            if (context != null && context.User != null && context.User.Identity.IsAuthenticated)
            {
                this.contextUser = context.User;
                this.EncryptedUserToken = context.Request.Headers["Authorization"][0].Substring("Bearer ".Length);
                this.EnvironmentId = context.Request.Headers["x-ms-environment-id"];
                this.TenantId = context.Request.Headers["x-ms-tenant-id"];
                this.InvitationToken = context.Request.Headers["x-dynamics-token-value"];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HCMApplicationPrincipal" /> class.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="trace">Trace source</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "The call stack was reviewed and no problems were found.")]
        public HCMApplicationPrincipal(HttpContext context, ITraceSource trace)
        {
            HCMApplicationPrincipal.trace = trace;
            if (context != null && context.User != null && context.User.Identity.IsAuthenticated)
            {
                trace?.TraceInformation("Initializting context user based on current http context.");
                this.contextUser = context.User;
                this.EncryptedUserToken = context.Request.Headers["Authorization"][0].Substring("Bearer ".Length);
                this.EnvironmentId = context.Request.Headers["x-ms-environment-id"];
                this.TenantId = context.Request.Headers["x-ms-tenant-id"];
                this.InvitationToken = context.Request.Headers["x-dynamics-token-value"];
            }
        }

        /// <summary>
        /// Gets the identity provider
        /// </summary>
        public string IdentityProvider => this.contextUser?.FindFirst(IdentityProviderClaimType)?.Value;

        /// <summary>
        /// Gets or sets the user's objectID
        /// </summary>
        public string UserObjectId
        {
            get
            {
                if (this.userObjectId == null)
                {
                    this.userObjectId = this.contextUser?.FindFirst(UserObjectIdClaimType) == null ? string.Empty : this.contextUser.FindFirst(UserObjectIdClaimType).Value;
                }

                return this.userObjectId;
            }

            set
            {
                this.userObjectId = value;
            }
        }

        /// <summary>
        /// Gets family name
        /// </summary>
        public string FamilyName
        {
            get
            {
                if (this.familyName == null)
                {
                    this.familyName = this.contextUser?.FindFirst(ClaimTypes.Surname) == null ? string.Empty : this.contextUser.FindFirst(ClaimTypes.Surname).Value;
                }

                return this.familyName;
            }
        }

        /// <summary>
        /// Gets given name
        /// </summary>
        public string GivenName
        {
            get
            {
                if (this.givenName == null)
                {
                    this.givenName = this.contextUser?.FindFirst(ClaimTypes.GivenName) == null ? string.Empty : this.contextUser.FindFirst(ClaimTypes.GivenName).Value;
                }

                return this.givenName;
            }
        }

        /// <summary>
        /// Gets user principal name
        /// </summary>
        public string UserPrincipalName
        {
            get
            {
                if (this.userPrincipalName == null)
                {
                    this.userPrincipalName = this.contextUser?.FindFirst(ClaimTypes.Upn) == null ? string.Empty : this.contextUser.FindFirst(ClaimTypes.Upn).Value;
                }

                return this.userPrincipalName;
            }
        }

        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        public string EmailAddress
        {
            get
            {
                if (this.emailAddress == null)
                {
                    this.emailAddress = this.contextUser?.FindFirst(ClaimTypes.Name) == null ? string.Empty : this.contextUser.FindFirst(ClaimTypes.Name).Value;
                }

                if (string.IsNullOrEmpty(this.emailAddress))
                {
                    this.emailAddress = this.contextUser?.FindFirst(user => user.Type == "emails")?.Value;
                }

                return this.emailAddress;
            }

            set
            {
                this.emailAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets tenant objectID
        /// </summary>
        public string TenantObjectId
        {
            get
            {
                if (this.tenantObjectId == null)
                {
                    this.tenantObjectId = this.contextUser?.FindFirst(TenantObjectIdClaimType) == null ? string.Empty : this.contextUser.FindFirst(TenantObjectIdClaimType).Value;
                }

                return this.tenantObjectId;
            }

            set
            {
                this.tenantObjectId = value;
            }
        }

        /// <summary>
        /// Gets name identifier
        /// </summary>
        public string NameIdentifier
        {
            get
            {
                if (this.nameIdentifier == null)
                {
                    this.nameIdentifier = this.contextUser?.FindFirst(ClaimTypes.NameIdentifier) == null ? string.Empty : this.contextUser?.FindFirst(ClaimTypes.NameIdentifier).Value;
                }

                return this.nameIdentifier;
            }
        }

        /// <summary>
        /// Gets the passport unique identifier
        /// </summary>
        public string Puid
        {
            get
            {
                if (this.puid == null)
                {
                    var puid = this.contextUser?.Claims?.SingleOrDefault(c => c.Type.ToLower() == PuidClaimIdentifier);
                    if (puid != null)
                    {
                        this.puid = puid.Value;
                    }
                    else
                    {
                        this.puid = string.Empty;
                    }
                }

                return this.puid;
            }
        }

        /// <summary>
        /// Gets the audience identifier
        /// </summary>
        public string Audience
        {
            get
            {
                if (this.audience == null)
                {
                    var audience = this.contextUser?.Claims?.SingleOrDefault(c => c.Type.ToLower() == AudienceClaimIdentifier);
                    if (audience != null)
                    {
                        this.audience = audience.Value;
                    }
                    else
                    {
                        this.audience = string.Empty;
                    }
                }

                return this.audience;
            }
        }

        /// <summary>
        /// Gets the issuer identifier
        /// </summary>
        public string Issuer
        {
            get
            {
                if (this.issuer == null)
                {
                    var issuer = this.contextUser?.Claims?.SingleOrDefault(c => c.Type.ToLower() == IssuerClaimIdentifier);
                    if (issuer != null)
                    {
                        this.issuer = issuer.Value;
                    }
                    else
                    {
                        this.issuer = string.Empty;
                    }
                }

                return this.issuer;
            }
        }

        /// <summary>
        /// Gets the trust framework policy, only present in B2C tokens
        /// </summary>
        public string TrustFrameworkPolicy
        {
            get
            {
                if (this.tfp == null)
                {
                    var tfp = this.contextUser?.Claims?.SingleOrDefault(c => c.Type.ToLower() == TrustFrameworkPolicyClaimIdentifier);
                    if (tfp != null)
                    {
                        this.tfp = tfp.Value;
                    }
                    else
                    {
                        this.tfp = string.Empty;
                    }
                }

                return this.tfp;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the principal is in B2C mode
        /// </summary>
        public bool IsB2CPrincipal
        {
            get
            {
                return !string.IsNullOrEmpty(this.TrustFrameworkPolicy);
            }
        }

        /// <summary>
        /// Gets the user token (Base64 encoded)
        /// </summary>
        public string EncryptedUserToken { get; }

        /// <summary>
        /// Gets or sets the user  BAP(Business Application Platform) environment Id 
        /// </summary>
        public string EnvironmentId { get; set; }

        /// <summary>
        /// Gets or sets the tenant Id from the request header
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the invitation token from the request header
        /// </summary>
        public string InvitationToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the principal is Admin
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}