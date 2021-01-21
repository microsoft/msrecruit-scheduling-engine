//----------------------------------------------------------------------------
// <copyright file="PingController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Controllers.V1
{
    using System.Net;
    using System.Net.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Web;
    using MS.GTA.ServicePlatform.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The Meeting Controller.
    /// </summary>
    [Route("api/ping")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PingController : HCMWebApiCommonController
    {
        /// <summary>
        /// The instance for <see cref="ILogger{PingController}"/>.
        /// </summary>
        private readonly ILogger<PingController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PingController"/> class.
        /// Constructor for Ping Controller
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor instance.</param>
        /// <param name="logger">The instance for <see cref="ILogger{PingController}"/>.</param>
        public PingController(IHttpContextAccessor httpContextAccessor, ILogger<PingController> logger)
            : base(httpContextAccessor)
        {
            this.logger = logger;
        }

        /// <summary>
        /// The Ping API
        /// </summary>
        /// <returns>Http Response</returns>
        [HttpGet("")]
        [MonitorWith("GTAV1PGetPing")]
        public HttpResponseMessage GetResponse()
        {
            this.logger.LogInformation($"Started {nameof(this.GetResponse)} method in {nameof(PingController)}.");
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Ping Successful") };
            this.Response.RegisterForDispose(responseMessage);
            this.logger.LogInformation($"Finished {nameof(this.GetResponse)} method in {nameof(PingController)}.");
            return responseMessage;
        }
    }
}