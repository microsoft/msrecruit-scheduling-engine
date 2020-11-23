namespace MS.GTA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ScheduleService.Controllers.V1;
    using MS.GTA.ScheduleService.UnitTest.Mocks;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    public class SubscribeToInboxTest
    {
        private Mock<IGraphSubscriptionManager> scheduleManagerMock;
        private Mock<ILogger<SubscriptionController>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        [TestInitialize]
        public void BeforEach()
        {
            this.scheduleManagerMock = new Mock<IGraphSubscriptionManager>();
            this.loggerMock = new Mock<ILogger<SubscriptionController>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// SubscribeToInbox
        /// </summary>
        [TestMethod]
        public void SubscribeToInboxTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<SubscriptionController>();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();
                   var controller = new SubscriptionController(httpContextAccessorMock,this.scheduleManagerMock.Object, this.loggerMock.Object);

                   var exception = controller.SubscribeToInboxController(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// SubscribeToInbox
        /// </summary>
        [TestMethod]
        public void SubscribeToInboxTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<SubscriptionController>();
            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Id = Guid.NewGuid().ToString();

            logger.ExecuteRoot(
               new RootExecutionContext
               {
                   SessionId = Guid.NewGuid(),
                   RootActivityId = Guid.NewGuid(),
               },
               TestActivityType.Instance,
               () =>
               {
                   var httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();

                   this.scheduleManagerMock.Setup(a => a.SubscribeToInbox(It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(false));

                   var controller = new SubscriptionController(httpContextAccessorMock,this.scheduleManagerMock.Object, this.loggerMock.Object);

                   var result = controller.SubscribeToInboxController("12345");
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }
    }
}
