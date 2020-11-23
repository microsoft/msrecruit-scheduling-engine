// <copyright file="SubscriptionController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Controllers.V1
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Web;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>
    /// The Graph subscription management Controller.
    /// </summary>
    [Route("v1/subscription")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SubscriptionController : HCMWebApiAuthenticatedController
    {
        private readonly IGraphSubscriptionManager graphSubscriptionManager;

        /// <summary>
        /// The instance for <see cref="ILogger{SubscriptionController}"/>.
        /// </summary>
        private readonly ILogger<SubscriptionController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor instance.</param>
        /// <param name="graphSubscriptionManager">The graph subscription manager.</param>
        /// <param name="logger">The instance for <see cref="ILogger{SubscriptionController}"/>.</param>
        public SubscriptionController(
            IHttpContextAccessor httpContextAccessor,
            IGraphSubscriptionManager graphSubscriptionManager,
            ILogger<SubscriptionController> logger)
                        : base(httpContextAccessor)
        {
            this.graphSubscriptionManager = graphSubscriptionManager;
            this.logger = logger;
        }

        /// <summary>
        /// Subscribe the inbox of an environment service account. Email service will call this endpoint
        /// </summary>
        /// <param name="serviceAccountName"> Service Account Name </param>
        /// <returns>If the subscription succeeded</returns>
        [HttpPost("subscribeToInbox")]
        public async Task<bool> SubscribeToInboxController(string serviceAccountName)
        {
            this.logger.LogInformation($"Started {nameof(this.SubscribeToInboxController)} method in {nameof(SubscriptionController)}.");
            if (string.IsNullOrEmpty(serviceAccountName))
            {
                throw new InvalidRequestDataValidationException("Invalid Access Token").EnsureLogged(this.logger);
            }

            bool isSubscribed = await this.graphSubscriptionManager.SubscribeToInbox(serviceAccountName, true);

            this.logger.LogInformation($"Finished {nameof(this.SubscribeToInboxController)} method in {nameof(SubscriptionController)}.");
            return isSubscribed;
        }
    }
}