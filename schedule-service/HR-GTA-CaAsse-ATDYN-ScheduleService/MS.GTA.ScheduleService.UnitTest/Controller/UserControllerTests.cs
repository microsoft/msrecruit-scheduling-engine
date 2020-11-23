namespace MS.GTA.ScheduleService.UnitTest.Controller
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Graph;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.Common.MSGraph;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Controllers.V1;
    using MS.GTA.ScheduleService.UnitTest.Mocks;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Tracing;

    [TestClass]
    public class UserControllerTests
    {
        private IHttpContextAccessor httpContextAccessorMock;

        private Mock<IHCMServiceContext> hCMServiceContextMock;

        private Mock<IUserDetailsProvider> userDetailsProviderMock;

        private Mock<ILogger<UserController>> loggerMock;

        private ILoggerFactory loggerFactory = new LoggerFactory();

        [TestInitialize]
        public void BeforEach()
        {
            this.httpContextAccessorMock = MockHttpContextAccessor.GetHttpContextAccessor();

            this.hCMServiceContextMock = new Mock<IHCMServiceContext>();
            this.userDetailsProviderMock = new Mock<IUserDetailsProvider>();
            this.loggerMock = new Mock<ILogger<UserController>>();
            TraceSourceMeta.LoggerFactory = this.loggerFactory;
        }

        /// <summary>
        /// GetPhotoTest
        /// </summary>
        [TestMethod]
        public void GetPhotoTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();

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
                   var controller = this.GetControllerInstance();

                   var exception = controller.GetPhoto(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// GetPhotoTest
        /// </summary>
        [TestMethod]
        public void GetPhotoTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();

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

                   this.userDetailsProviderMock.Setup(a => a.GetUserPhotoAsync(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));

                   var controller = this.GetControllerInstance();

                   var result = controller.GetPhoto("12345");
                   Assert.IsNotNull(result);
                   Assert.IsTrue(result.IsCompleted);
               });
        }

        /// <summary>
        /// GetLocationTest
        /// </summary>
        [TestMethod]
        public void GetLocationTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();

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
                   var controller = this.GetControllerInstance();

                   var exception = controller.GetLocation(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// GetLocationTest
        /// </summary>
        [TestMethod]
        public void GetLocationTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();

            User result = new User();
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

                   this.userDetailsProviderMock.Setup(a => a.GetUserAsync(It.IsAny<string>())).Returns(Task.FromResult(result));

                   var controller = this.GetControllerInstance();

                   var response = controller.GetLocation("12345");
                   Assert.IsNotNull(response);
               });
        }

        /// <summary>
        /// GetUserDetailsTest
        /// </summary>
        [TestMethod]
        public void GetUserDetailsTestWithNullInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();

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
                   var controller = this.GetControllerInstance();

                   var exception = controller.GetUserDetails(string.Empty).Exception;

                   Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidRequestDataValidationException));
               });
        }

        /// <summary>
        /// GetUserDetailsTest
        /// </summary>
        [TestMethod]
        public void GetUserDetailsTestWithValidInputs()
        {
            var logger = TraceSourceMeta.LoggerFactory.CreateLogger<ScheduleServiceController>();

            User result = new User();
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

                   this.userDetailsProviderMock.Setup(a => a.GetUserAsync(It.IsAny<string>())).Returns(Task.FromResult(result));

                   var controller = this.GetControllerInstance();

                   var response = controller.GetUserDetails("12345");
                   Assert.IsNotNull(response);
               });
        }

        private UserController GetControllerInstance()
        {
            return new UserController(this.httpContextAccessorMock, this.userDetailsProviderMock.Object, this.loggerMock.Object);
        }
    }
}
