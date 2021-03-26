//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.UnitTest.Common.WebNotifications.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Protected;
    using TA.CommonLibrary.Common.Common.WebNotifications.Configurations;
    using TA.CommonLibrary.Common.WebNotifications.Internals;
    using TA.CommonLibrary.Common.WebNotifications.Models;
    using TA.CommonLibrary.ServicePlatform.Configuration;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DefaultWebNotificationClient_Tests
    {
        private static readonly IEnumerable<WebNotificationRequest> webNotificationRequests = new List<WebNotificationRequest>
        {
            new WebNotificationRequest
            {
                Title = "Notification #1",
                Body = "I'm the notification body 1.",
                Sender = new ParticipantData
                {
                    Name = "Sender Partiipant #1",
                    Email = "sender1@somedomain.com",
                    ObjectIdentifier = Guid.NewGuid().ToString(),
                },
                Recipient = new ParticipantData
                {
                    Name = "Recipient Partiipant #1",
                    Email = "recipient1@somedomain.com",
                    ObjectIdentifier = Guid.NewGuid().ToString(),
                },
                Properties = new Dictionary<string, string>(),
            },
            new WebNotificationRequest
            {
                Title = "Notification #2",
                Body = "I'm the notification body 2.",
                Sender = new ParticipantData
                {
                    Name = "Sender Partiipant #2",
                    Email = "sender2@somedomain.com",
                    ObjectIdentifier = Guid.NewGuid().ToString(),
                },
                Recipient = new ParticipantData
                {
                    Name = "Recipient Partiipant #2",
                    Email = "recipient2@somedomain.com",
                    ObjectIdentifier = Guid.NewGuid().ToString(),
                },
                Properties = new Dictionary<string, string>(),
            },
        };

        private Mock<HttpMessageHandler> httpHandlerMock;

        private Mock<IConfigurationManager> configManagerMock;

        private DefaultWebNotificationClient webNotificationClient;

        private HttpClient httpClient;

        [TestInitialize]
        public void BeforeEach()
        {
            this.httpHandlerMock = new Mock<HttpMessageHandler>();
            this.configManagerMock = new Mock<IConfigurationManager>();
            this.configManagerMock.Setup(cmm => cmm.Get<WebNotificationServiceConfiguration>()).Returns(new WebNotificationServiceConfiguration
            {
                PostWebNotificationsRelativeUrl = "/v1/notifications",
            });

            this.httpClient = new HttpClient(this.httpHandlerMock.Object)
            {
                BaseAddress = new Uri("https://www.somedomain.com/iv/")
            };
            this.webNotificationClient = new DefaultWebNotificationClient(this.httpClient, this.configManagerMock.Object, NullLogger<DefaultWebNotificationClient>.Instance);
        }

        [TestMethod]
        public void Ctor_NullHttpClient_ThrowException()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => new DefaultWebNotificationClient(null, this.configManagerMock.Object, NullLogger<DefaultWebNotificationClient>.Instance));
            Assert.AreEqual("httpClient", ex.ParamName, ignoreCase: false);
        }

        [TestMethod]
        public void Ctor_NullConfigurationManager_ThrowException()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => new DefaultWebNotificationClient(this.httpClient, null, NullLogger<DefaultWebNotificationClient>.Instance));
            Assert.AreEqual("configurationManager", ex.ParamName, ignoreCase: false);
        }

        [TestMethod]
        public void Ctor_NoLogger_ThrowException()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => new DefaultWebNotificationClient(this.httpClient, this.configManagerMock.Object, null));
            Assert.AreEqual("logger", ex.ParamName, ignoreCase: false);
        }

        [TestMethod]
        public void Ctor_ValidInput_MatchInstanceType()
        {
            // Arranged in BeforeEach method.
            Assert.IsInstanceOfType(this.webNotificationClient, typeof(DefaultWebNotificationClient));
        }

        [TestMethod]
        public async Task PostNotificationsAsync_NullWebNotificationsRequest_ThrowException()
        {
            var ex = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await this.webNotificationClient.PostNotificationsAsync(null).ConfigureAwait(false));
            Assert.AreEqual("webNotificationRequests", ex.ParamName, ignoreCase: false);
        }

        [TestMethod]
        public async Task PostNotificationsAsync_ValidWebNotificationsRequests_UnauthorizedResponse_NoExceptionInResult()
        {
            this.httpHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized)).Verifiable();
            await this.webNotificationClient.PostNotificationsAsync(webNotificationRequests).ConfigureAwait(false);
            this.httpHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task PostNotificationsAsync_ValidWebNotificationsRequests_AcceptedResponse_NoExceptionInResult()
        {
            this.httpHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Accepted));
            await this.webNotificationClient.PostNotificationsAsync(webNotificationRequests).ConfigureAwait(false);
            this.httpHandlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }
    }
}
