// <copyright file="MockHttpContextAccessor.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.UnitTest.Mocks
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;
    using Microsoft.AspNetCore.Http;
    using Moq;

    /// <summary>
    /// Mock Http Context Accessor
    /// </summary>
    public static class MockHttpContextAccessor
    {
        /// <summary>First name of the user.</summary>
        public const string GivenName = "Admin";

        /// <summary>Last name of the user.</summary>
        public const string LastName = "Test";

        /// <summary>Email of the user.</summary>
        public const string Email = "admin@d365alpha.onmicrosoft.com";

        /// <summary>NameIdentifier of the user.</summary>
        public const string NameIdentifier = "Talent";

        /// <summary>UserObjectId of the user.</summary>
        public const string UserObjectId = "F51DEB0E-C87B-4440-A46C-9CF366BBCF91";

        /// <summary>TenantObjectId of the user.</summary>
        public const string TenantObjectId = "9050325B-1DCD-45EF-B3BB-DAA041E63621";

        /// <summary>UserObjectId ClaimType.</summary>
        private const string UserObjectIdClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        /// <summary>TenantObjectId ClaimType.</summary>
        private const string TenantObjectIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";

        /// <summary>
        /// Creates a new mock HttpContextAccessor.
        /// </summary>
        /// <param name="givenName">First name of the user.</param>
        /// <param name="lastName">Last name of the user.</param>
        /// <param name="upn">User principal name of the user</param>
        /// <param name="nameIdentifier">NameIdentifier of the user</param>
        /// <param name="userObjectId">UserObjectId of the user</param>
        /// <param name="tenantObjectId">TenantObjectId of the user</param>
        /// <param name="email">Email of the user</param>
        /// <returns>The mock HttpContextAccessor.</returns>
        public static IHttpContextAccessor GetHttpContextAccessor(
            string givenName = GivenName,
            string lastName = LastName,
            string upn = Email,
            string nameIdentifier = NameIdentifier,
            string userObjectId = UserObjectId,
            string tenantObjectId = TenantObjectId,
            string email = Email)
        {
            var httpContextAccessorMoq = new Mock<IHttpContextAccessor>();
            var mockContext = new Mock<HttpContext>();

            var expectedIdentity = new GenericIdentity(email);

            IList<Claim> list = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, givenName),
                new Claim(ClaimTypes.Surname, lastName),
                new Claim(ClaimTypes.Upn, upn),
                new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
                new Claim(UserObjectIdClaimType, userObjectId),
                new Claim(TenantObjectIdClaimType, tenantObjectId),
            };

            var user = new ClaimsPrincipal(expectedIdentity);
            user.AddIdentity(new ClaimsIdentity(list));

            mockContext.Setup(httpContextAccessor => httpContextAccessor.User).Returns(user);

            var headers = new HeaderDictionary();
            headers.Add("Authorization", "Bearer abc");
            var request = new Mock<HttpRequest>();
            request.Setup(r => r.Headers).Returns(headers);
            mockContext.Setup(cxt => cxt.Request).Returns(request.Object);

            var response = new Mock<HttpResponse>();
            response.Setup(r => r.Headers).Returns(new HeaderDictionary());
            mockContext.Setup(cxt => cxt.Response).Returns(response.Object);

            httpContextAccessorMoq.Setup(httpContextAccessor => httpContextAccessor.HttpContext).Returns(mockContext.Object);
            return httpContextAccessorMoq.Object;
        }
    }
}
