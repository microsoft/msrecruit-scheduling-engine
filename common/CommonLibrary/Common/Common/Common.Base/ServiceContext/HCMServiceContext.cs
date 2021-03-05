//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.Base.ServiceContext
{
    using Microsoft.AspNetCore.Http;
    using Common.Contracts;
    using Security.V2;
    using System.Fabric.Management.ServiceModel;

    /// <summary>The HCM service context.</summary>
    public class HCMServiceContext : IHCMServiceContext
    {
        /// <summary>Initializes a new instance of the <see cref="HCMServiceContext"/> class.</summary>
        /// <param name="httpContextAccessor">The http Context Accessor.</param>
        /// <param name="hcmPrincipalRetriever">The HCM Principal Retriever.</param>
        public HCMServiceContext(IHttpContextAccessor httpContextAccessor, IHCMPrincipalRetriever hcmPrincipalRetriever)
        {
            if (httpContextAccessor?.HttpContext == null)
            {
                return;
            }

            var httpContext = httpContextAccessor.HttpContext;

            this.EnvironmentId = httpContext.Request.Headers["x-ms-environment-id"];
            this.InvitationToken = httpContext.Request.Headers["x-dynamics-token-value"];
            this.RootActivityId = httpContext.Request.Headers["x-ms-root-activity-id"];
            this.SessionId = httpContext.Request.Headers["x-ms-session-id"];
            this.AppName = httpContext.Request.Headers["x-ms-app-name"];
            this.WorkOnBehalfUserId = string.IsNullOrEmpty(httpContext.Request.Headers["x-ms-on-behalf-user-id"]) ?
                                       httpContext.Request.Query["onBehalfUserId"] : httpContext.Request.Headers["x-ms-on-behalf-user-id"];

            var environmentMode = httpContext.Request.Headers["x-ms-environment-mode"];
            if (!string.IsNullOrEmpty(environmentMode))
            {
                this.EnvironmentMode = (EnvironmentMode)long.Parse(environmentMode);
            }

            if (hcmPrincipalRetriever.Principal is IHCMUserPrincipal)
            {
                var userPrincipal = hcmPrincipalRetriever.Principal as IHCMUserPrincipal;

                this.TenantId = userPrincipal?.TenantId;
                this.ObjectId = userPrincipal?.ObjectId;
                this.Email = userPrincipal?.EmailAddress;
            }
            else if (hcmPrincipalRetriever.Principal is IHCMB2BPrincipal)
            {
                this.TenantId = ((IHCMB2BPrincipal)hcmPrincipalRetriever.Principal)?.TenantId;
            }
        }

        /// <summary>Gets or sets the environment mode.</summary>
        public EnvironmentMode EnvironmentMode { get; set; }

        /// <summary>Gets or sets the environment id.</summary>
        public string EnvironmentId { get; set; }

        /// <summary>Gets or sets the tenant id.</summary>
        public string TenantId { get; set; }

        /// <summary>Gets or sets the app name.</summary>
        public string AppName { get; set; }

        /// <summary>Gets or sets the invitation token value.</summary>
        public string InvitationToken { get; set; }

        /// <summary>Gets or sets the object id value.</summary>
        public string ObjectId { get; set; }

        /// <summary>Gets or sets the falcon database id.</summary>
        public string FalconDatabaseId { get; set; }

        /// <summary>Gets or sets the falcon resource name.</summary>
        public string FalconResourceName { get; set; }

        /// <summary>Gets or sets the falcon offer container id.</summary>
        public string FalconOfferContainerId { get; set; }

        /// <summary>Gets or sets the falcon common container identifier.</summary>
        /// <value>The falcon common container identifier.</value>
        public string FalconCommonContainerId { get; set; }

        /// <summary>Gets or sets the XRM instance API uri.</summary>
        public string XRMInstanceApiUri { get; set; }

        /// <summary>Gets or sets the Root activity id.</summary>
        public string RootActivityId { get; set; }

        /// <summary>Gets or sets the Session id.</summary>
        public string SessionId { get; set; }

        /// <summary>Gets or sets whether the Attract service endpoint is configured or not.</summary>
        public bool IsAttractServiceEndpointConfigured { get; set; }

        /// <summary>
        /// Gets or sets the User Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets workonbehalfuserid
        /// </summary>
        public string WorkOnBehalfUserId { get; set; }

        /// <summary>
        /// This is a get only property if the user is wob autheticated.
        /// </summary>
        public bool isWobAuthenticated {

            get { return string.Equals(this.UserId, this.WorkOnBehalfUserId); }

        }

        /// <summary>
        /// This will get the email id for the context user
        /// </summary>
        public string Email { get; set; }
    }
}
