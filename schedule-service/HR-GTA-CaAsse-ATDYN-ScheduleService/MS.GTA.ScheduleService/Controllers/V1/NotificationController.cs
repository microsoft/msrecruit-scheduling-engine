//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="NotificationController.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.ScheduleService.BusinessLibrary.Configurations;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.Enum;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ServicePlatform.AspNetCore.Mvc.Filters;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Exceptions;
    using RestSharp.Extensions;

    /// <summary>
    /// The notification controller.
    /// </summary>
    [Route("v1/notification")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NotificationController : Controller
    {
        private const string GraphSubScriptionTokenPattern = @"^[a-zA-Z0-9: -]*$";
        private readonly IConfigurationManager configurationManager;
        private readonly IServiceBusHelper serviceBusHelper;
        private readonly IHCMServiceContext hcmServiceContext;

        /// <summary>
        /// The instance for <see cref="ILogger{NotificationController}"/>.
        /// </summary>
        private readonly ILogger<NotificationController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController" /> class.
        /// </summary>
        /// <param name="configurationManager"> The configuration manager instance.</param>
        /// <param name="serviceBusHelper">Service bus helper</param>
        /// <param name="hCMServiceContext">Holds Service Context</param>
        /// <param name="logger">The instance for <see cref="ILogger{NotificationController}"/>.</param>
        public NotificationController(
            IConfigurationManager configurationManager,
            IServiceBusHelper serviceBusHelper,
            IHCMServiceContext hCMServiceContext,
            ILogger<NotificationController> logger)
        {
            this.configurationManager = configurationManager;
            this.serviceBusHelper = serviceBusHelper;
            this.hcmServiceContext = hCMServiceContext;
            this.logger = logger;
        }

        /// <summary>
        /// The notificationUrl endpoint that's registered with the web hook subscription.
        /// </summary>
        /// <param name="token">Validation token if present</param>
        /// <returns>A http 202 response</returns>
        [HttpPost("listenNotification")]
        [MonitorWith("GTASSNListener")]
        public async Task<ActionResult> Listen([FromQuery(Name = "validationToken")] string token)
        {
            ActionResult actionResult;
            this.logger.LogInformation($"Started {nameof(this.Listen)} method in {nameof(NotificationController)}.");

            // Validate the new subscription by sending the token back to Microsoft Graph.
            // This response is required for each subscription.
            if (!string.IsNullOrWhiteSpace(token))
            {
                this.logger.LogInformation($"SchedulingService: POST Notification ValidationToken found {token}");
                if (Regex.IsMatch(token, GraphSubScriptionTokenPattern))
                {
                    actionResult = this.Content(token, "plain/text");
                }
                else
                {
                    actionResult = null;
                    this.logger.LogWarning($"SchedulingService: POST Notification ValidationToken received {token} and format is invalid.");
                }
            }
            else
            {
                this.logger.LogInformation($"SchedulingService: POST Notification received");

                if (this.Request?.Body != null)
                {
                    // Parse the received notifications.
                    try
                    {
                        using (var inputStream = new System.IO.StreamReader(this.Request.Body))
                        {
                            var content = await inputStream.ReadToEndAsync();
                            this.logger.LogInformation($"SchedulingService: Notification content length {content.Length}");

                            await this.serviceBusHelper.QueueMessageToNotificationWorker(this.GetServiceBusMessage(content));
                        }
                    }
                    catch (Exception e)
                    {
                        throw new NotificationProcessingException(e).EnsureLogged(this.logger);
                    }
                }

                actionResult = this.Accepted();
            }

            this.logger.LogInformation($"Finished {nameof(this.Listen)} method in {nameof(NotificationController)}.");
            return actionResult;
        }

        private ServiceBusMessage GetServiceBusMessage(string messageContent)
        {
            var serviceBusClientConfiguration = this.configurationManager.Get<ServiceBusClientConfiguration>();
            return new ServiceBusMessage()
            {
                ConnectionStringKey = serviceBusClientConfiguration.ServiceBusConnectionString,
                QueueName = serviceBusClientConfiguration.NotificationServiceBusQueueName,
                KeyVaultUri = serviceBusClientConfiguration.KeyVaultUri,
                UserProperties = new Dictionary<string, string>()
                {
                    { "ActionType", ActionType.Process.ToString() },
                    { "ScheduleRootActivityId", this.hcmServiceContext.RootActivityId?.ToString() ?? Guid.NewGuid().ToString() }
                },
                Message = messageContent
            };
        }
    }
}
