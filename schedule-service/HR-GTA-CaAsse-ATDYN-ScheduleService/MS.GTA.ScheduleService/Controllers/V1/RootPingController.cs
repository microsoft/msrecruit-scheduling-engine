//----------------------------------------------------------------------------
// <copyright file="RootPingController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Controllers.V1
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Web;
    using MS.GTA.ServicePlatform.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The Root Ping Controller.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RootPingController : HCMWebApiCommonController
    {
        /// <summary>
        /// The instance for <see cref="ILogger{RootPingController}"/>.
        /// </summary>
        private readonly ILogger<RootPingController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootPingController"/> class.
        /// Constructor for Root Ping Controller
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor instance.</param>
        /// <param name="logger">The instance for <see cref="ILogger{RootPingController}"/>.</param>
        public RootPingController(IHttpContextAccessor httpContextAccessor, ILogger<RootPingController> logger)
            : base(httpContextAccessor)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Generates a success response as a ping.
        /// </summary>
        /// <returns>The instance for <see cref="IActionResult"/></returns>
        [HttpGet("")]
        [MonitorWith("GTAGetRootPing")]
        public IActionResult GetResponse()
        {
            this.logger.LogInformation($"Invoked {nameof(this.GetResponse)} method in {nameof(RootPingController)}.");
            return this.Ok();
        }
    }
}