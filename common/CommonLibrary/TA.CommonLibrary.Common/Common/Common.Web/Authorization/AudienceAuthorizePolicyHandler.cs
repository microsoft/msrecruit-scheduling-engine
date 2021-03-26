//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Web.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using TA.CommonLibrary.Common.Base;
    using TA.CommonLibrary.Common.Base.Security.V2;
    using TA.CommonLibrary.Common.Web.Contracts;
    using TA.CommonLibrary.ServicePlatform.Tracing;

    /// <summary>
    /// Handler for evaluation of AudienceAuthorizeRequirement Policy.
    /// </summary>
    public class AudienceAuthorizePolicyHandler : AuthorizationHandler<AudienceAuthorizeRequirement>
    {
        /// <summary>
        /// Instance of <see cref="IHttpContextAccessor"/>.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Instance of <see cref="IConfiguration"/>.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Trace source
        /// </summary>
        private readonly ITraceSource trace;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudienceAuthorizePolicyHandler"/> class.
        /// </summary>
        /// <param name="trace">Trace source instance</param>
        /// <param name="httpContextAccessor">Instance of <see cref="IHttpContextAccessor"/>.</param>
        /// <param name="configuration">Instance of <see cref="IConfiguration"/>.</param>
        public AudienceAuthorizePolicyHandler(ITraceSource trace, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AudienceAuthorizeRequirement requirement)
        {
            this.trace.TraceInformation($"Checking authorization in {nameof(AudienceAuthorizePolicyHandler)}.");

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (requirement is null)
            {
                throw new ArgumentNullException(nameof(requirement));
            }

            var httpContext = this.httpContextAccessor.HttpContext;
            var hcmPrincipalRetriever = (IHCMPrincipalRetriever)httpContext.RequestServices.GetService(typeof(IHCMPrincipalRetriever));

            var userPrincipal = hcmPrincipalRetriever.Principal as IHCMUserPrincipal;
            var userAudienceInToken = userPrincipal?.Audience;
            var validAudienceForUserAuthorization = this.SplitToList(this.configuration["MSRecruitAuthorizedUserAudiences"]);

            var applicationPrincipal = hcmPrincipalRetriever.Principal as IHCMApplicationPrincipal;
            var applicationAudienceInToken = applicationPrincipal?.Audience;
            var validAudienceForApplicationAuthorization = this.SplitToList(this.configuration["MSRecruitAuthorizedAppAudiences"]);

            bool userAuthorized = false;
            bool applicationAuthorized = false;
            bool authorizationPolicySucceeded = false;

            if (context.User?.Identity?.IsAuthenticated ?? false)
            {
                if (userPrincipal != null &&
                !string.IsNullOrEmpty(userAudienceInToken) &&
                (validAudienceForUserAuthorization?.Any() ?? false) &&
                (validAudienceForUserAuthorization?.Contains(userAudienceInToken) ?? false))
                {
                    userAuthorized = true;
                }
                else if (applicationPrincipal != null &&
                    !string.IsNullOrEmpty(applicationAudienceInToken) &&
                    (validAudienceForApplicationAuthorization?.Any() ?? false) &&
                    (validAudienceForApplicationAuthorization?.Contains(applicationAudienceInToken) ?? false))
                {
                    applicationAuthorized = true;
                }

                switch (requirement.AuthorizedTokenType)
                {
                    case AuthorizedTokenType.UserToken:
                        authorizationPolicySucceeded = userAuthorized;
                        break;
                    case AuthorizedTokenType.ApplicationToken:
                        authorizationPolicySucceeded = applicationAuthorized;
                        break;
                    case AuthorizedTokenType.UserOrApplicationToken:
                        authorizationPolicySucceeded = userAuthorized || applicationAuthorized;
                        break;
                }

                if (authorizationPolicySucceeded)
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                context.Fail();
                this.trace.TraceInformation($"Token is not authenticated in {nameof(AudienceAuthorizePolicyHandler)}.");
            }

            this.trace.TraceInformation($"Finished checking authorization in {nameof(AudienceAuthorizePolicyHandler)}.");

            return Task.CompletedTask;
        }

        private IList<string> SplitToList(string valuesAsSingleString)
        {
            var values = new List<string>();

            if (!string.IsNullOrWhiteSpace(valuesAsSingleString))
            {
                values.AddRange(valuesAsSingleString.Split(Constants.SplitCharacter).Select(v => v.Trim()));
            }

            return values;
        }
    }
}
