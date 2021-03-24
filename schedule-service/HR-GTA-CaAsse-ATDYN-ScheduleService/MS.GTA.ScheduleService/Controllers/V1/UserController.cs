//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.Controllers.V1
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using CommonLibrary.Common.Base.Security;
    using CommonLibrary.Common.MSGraph;
    using CommonLibrary.Common.TalentEntities.Common;
    using CommonLibrary.Common.Web;
    using MS.GTA.ScheduleService.BusinessLibrary.Providers;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ServicePlatform.AspNetCore.Mvc.Filters;
    using MS.GTA.ServicePlatform.Context;
    using ServicePlatform.Exceptions;

    /// <summary>
    /// User Controller
    /// </summary>
    [Route("v1/user")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserController : HCMWebApiAuthenticatedController
    {
        /// <summary>
        /// Gets or sets the Metric Logger
        /// </summary>
        private readonly ILogger<UserController> logger;

        /// <summary>
        /// Gets or sets user details provider
        /// </summary>
        private readonly IUserDetailsProvider userDetailsProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor instance.</param>
        /// <param name="userDetailsProvider">user details provider instance</param>
        /// <param name="logger">The Logger</param>
        public UserController(IHttpContextAccessor httpContextAccessor, IUserDetailsProvider userDetailsProvider, ILogger<UserController> logger)
            : base(httpContextAccessor)
        {
            this.userDetailsProvider = userDetailsProvider;
            this.logger = logger;
        }

        /// <summary>
        /// GetPhoto service.
        /// </summary>
        /// <param name="userObjectId">The userObjectId instance.</param>
        /// <returns>photo content</returns>
        [HttpGet]
        [Authorize]
        [Route("{userObjectId}/photo")]
        [MonitorWith("IVGetUserImg")]
        [ResponseCache(Duration = 60 * 60 * 24, Location = ResponseCacheLocation.Client, VaryByHeader = "x-ms-environment-id")]
        public async Task<string> GetPhoto(string userObjectId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetPhoto)} method in {nameof(UserController)}");
            this.logger.LogInformation($"Start activity of type: {ServiceContext.Activity.Current.ActivityType}, userObjectId: {userObjectId}");

            var result = string.Empty;
            if (string.IsNullOrWhiteSpace(userObjectId))
            {
                throw new InvalidRequestDataValidationException("User Object ID should not be null or whitespace").EnsureLogged(this.logger);
            }

            try
            {
                result = await this.userDetailsProvider.GetUserPhotoAsync(userObjectId);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occurred while fetching photo for User Object ID {userObjectId} in {nameof(this.GetPhoto)} method in {nameof(UserController)}. Details : {ex.Message}");
            }

            this.logger.LogInformation($"Finished {nameof(this.GetPhoto)} method in {nameof(UserController)}");
            return result;
        }

        /// <summary>
        /// Get office location details.
        /// </summary>
        /// <param name="userObjectId">The userObjectId instance.</param>
        /// <returns>photo content</returns>
        [HttpGet]
        [Authorize]
        [Route("{userObjectId}/location")]
        [MonitorWith("IVGetUserLocation")]
        [ResponseCache(Duration = 60 * 60 * 24, Location = ResponseCacheLocation.Client, VaryByHeader = "x-ms-environment-id")]
        public async Task<IActionResult> GetLocation(string userObjectId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetLocation)} method in {nameof(UserController)}");
            this.logger.LogInformation($"Start activity of type: {ServiceContext.Activity.Current.ActivityType}, userObjectId: {userObjectId}");

            Room result = new Room();
            if (string.IsNullOrWhiteSpace(userObjectId))
            {
                throw new InvalidRequestDataValidationException("User Object ID should not be null or whitespace").EnsureLogged(this.logger);
            }

            var currentPrincipal = ServiceContext.Principal.TryGetCurrent<HCMApplicationPrincipal>();
            try
            {
                var user = await this.userDetailsProvider.GetUserAsync(userObjectId);
                if (user != null && !string.IsNullOrWhiteSpace(user.OfficeLocation))
                {
                    result.Address = user.OfficeLocation.Split('/')[0];
                    result.Name = user.OfficeLocation.Split('/')[1];
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occurred while fetching office location for User Object ID {userObjectId} in {nameof(this.GetLocation)} method in {nameof(UserController)}. Details : {ex.Message}");
            }

            this.logger.LogInformation($"Finished {nameof(this.GetLocation)} method in {nameof(UserController)}");
            return this.Ok(result);
        }

        /// <summary>
        /// Get graph user details.
        /// </summary>
        /// <param name="userObjectId">The userObjectId instance.</param>
        /// <returns>photo content</returns>
        [HttpGet]
        [Authorize]
        [Route("{userObjectId}")]
        [MonitorWith("IVGetUserDetails")]
        [ResponseCache(Duration = 60 * 60 * 24, Location = ResponseCacheLocation.Client, VaryByHeader = "x-ms-environment-id")]
        public async Task<IActionResult> GetUserDetails(string userObjectId)
        {
            this.logger.LogInformation($"Started {nameof(this.GetUserDetails)} method in {nameof(UserController)}");
            this.logger.LogInformation($"Start activity of type: {ServiceContext.Activity.Current.ActivityType}, userObjectId: {userObjectId}");

            Worker result = new Worker();
            if (string.IsNullOrWhiteSpace(userObjectId))
            {
                throw new InvalidRequestDataValidationException("User Object ID should not be null or whitespace").EnsureLogged(this.logger);
            }

            var currentPrincipal = ServiceContext.Principal.TryGetCurrent<HCMApplicationPrincipal>();
            try
            {
                var user = await this.userDetailsProvider.GetUserAsync(userObjectId);
                if (user != null)
                {
                    result.OfficeGraphIdentifier = user.Id;
                    result.EmailPrimary = user.UserPrincipalName;
                    result.Profession = user.JobTitle;
                    result.FullName = user.DisplayName;
                    result.Name = new PersonName
                    {
                        GivenName = user.GivenName,
                        Surname = user.Surname
                    };
                    result.WorkerId = user.Id;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occurred while fetching user details for User Object ID {userObjectId} in {nameof(this.GetUserDetails)} method in {nameof(UserController)}. Details : {ex.Message}");
            }

            this.logger.LogInformation($"Finished {nameof(this.GetUserDetails)} method in {nameof(UserController)}");
            return this.Ok(result);
        }
    }
}
