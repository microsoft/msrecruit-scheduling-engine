//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.UnitTest.Common.Authorization
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TA.CommonLibrary.Common.Base.Security.V2;
    using TA.CommonLibrary.Common.Web.Authorization;
    using TA.CommonLibrary.Common.Web.Contracts;
    using TA.CommonLibrary.ServicePlatform.Tracing;

    /// <summary>
    /// Tests for the Audience Authorize Policy Handlers.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AudienceAuthorizePolicyHandlerTests
    {
        private AudienceAuthorizePolicyHandler audienceAuthorizePolicyHandler;
        private IHttpContextAccessor httpContextAccessorMock;
        private Mock<IConfiguration> mockConfig;
        private Mock<ITraceSource> mockTrace;
        private string validTestUserAudience = "testUser";
        private string validTestAppAudience = "testApplication";

        /// <summary>
        /// Test initialization.
        /// </summary>
        [TestInitialize]
        public void BeforeEach()
        {
            this.mockConfig = new Mock<IConfiguration>();
            this.mockTrace = new Mock<ITraceSource>();

            this.mockTrace.Setup(a => a.TraceInformation(It.IsAny<string>()));
            this.mockConfig.Setup(m => m[It.Is<string>(s => s == "MSRecruitAuthorizedUserAudiences")]).Returns(validTestUserAudience);
            this.mockConfig.Setup(m => m[It.Is<string>(s => s == "MSRecruitAuthorizedAppAudiences")]).Returns(validTestAppAudience);
        }

        /// <summary>
        /// Tests for the AudienceAuthorizeRequirement.
        /// </summary>
        [TestMethod]
        public void AuthorizeRequirementTests()
        {
            AuthorizedTokenType authorizedTokenType = AuthorizedTokenType.UserToken;

            this.httpContextAccessorMock = this.GetHttpContextAccessor(authorizedTokenType);

            var requirements = new[] { new AudienceAuthorizeRequirement(authorizedTokenType) };
            var appUser = "user";
            var application = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, appUser),
                    },
                    "Basic"));

            this.audienceAuthorizePolicyHandler = new AudienceAuthorizePolicyHandler(this.mockTrace.Object, this.httpContextAccessorMock, this.mockConfig.Object);
            var authzContext = new AuthorizationHandlerContext(requirements, application, new object());
            Task result = this.audienceAuthorizePolicyHandler.HandleAsync(authzContext);
            Assert.AreEqual(result.Status.ToString(), "RanToCompletion");
            Assert.IsTrue(authzContext.HasSucceeded);

            authorizedTokenType = AuthorizedTokenType.ApplicationToken;

            this.httpContextAccessorMock = this.GetHttpContextAccessor(authorizedTokenType);

            requirements = new[] { new AudienceAuthorizeRequirement(authorizedTokenType) };
            appUser = "application";
            application = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, appUser),
                    },
                    "Basic"));

            this.audienceAuthorizePolicyHandler = new AudienceAuthorizePolicyHandler(this.mockTrace.Object, this.httpContextAccessorMock, this.mockConfig.Object);
            authzContext = new AuthorizationHandlerContext(requirements, application, new object());
            result = this.audienceAuthorizePolicyHandler.HandleAsync(authzContext);
            Assert.AreEqual(result.Status.ToString(), "RanToCompletion");
            Assert.IsTrue(authzContext.HasSucceeded);
        }

        private IHttpContextAccessor GetHttpContextAccessor(AuthorizedTokenType authorizedTokenType)
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var hcmPrincipalRetriverMock = new Mock<IHCMPrincipalRetriever>();
            var userPrincipalMock = new Mock<IHCMUserPrincipal>();
            var applicationPrincipalMock = new Mock<IHCMApplicationPrincipal>();

            httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);
            httpContextAccessorMock.Setup(_ => _.HttpContext.RequestServices.GetService(typeof(IHCMPrincipalRetriever))).Returns(hcmPrincipalRetriverMock.Object);

            if (authorizedTokenType == AuthorizedTokenType.ApplicationToken)
            {
                hcmPrincipalRetriverMock.Setup(_ => _.Principal).Returns(applicationPrincipalMock.Object);

                applicationPrincipalMock.Setup(_ => _.Audience).Returns(validTestAppAudience);
            }
            else
            {
                hcmPrincipalRetriverMock.Setup(_ => _.Principal).Returns(userPrincipalMock.Object);

                userPrincipalMock.Setup(_ => _.Audience).Returns(validTestUserAudience);
            }

            return httpContextAccessorMock.Object;
        }
    }
}
