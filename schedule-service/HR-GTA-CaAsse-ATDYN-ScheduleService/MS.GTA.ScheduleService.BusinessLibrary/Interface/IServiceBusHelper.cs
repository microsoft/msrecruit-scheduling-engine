// <copyright file="IServiceBusHelper.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.Interface
{
    using System.Threading.Tasks;
    using MS.GTA.ScheduleService.Contracts.V1;

    /// <summary>
    /// Interface for the Service Bus Helper
    /// </summary>
    public interface IServiceBusHelper
    {
        /// <summary>
        /// Queues the message to the Service Bus Queue
        /// </summary>
        /// <param name="messageActionType">Type of message to be queued</param>
        /// <param name="connectorInegrationDetails">Object to be passed to the queue</param>
        /// <param name="rootActivityId">Root Activity Id</param>
        /// <param name="scheduleId">schedule Id</param>
        /// <returns>Returns Success or Failure</returns>
        Task QueueMessageToSendInvitationWorker(string messageActionType, ConnectorIntegrationDetails connectorInegrationDetails, string rootActivityId, string scheduleId);

        /// <summary>
        /// Queues the message to the Service Bus Queue
        /// </summary>
        /// <param name="serviceBusMessage">Service bus message object</param>
        /// <returns>Returns Success or Failure</returns>
        Task QueueMessageToNotificationWorker(ServiceBusMessage serviceBusMessage);

        /// <summary>
        /// Queues the message to the Service Bus Queue
        /// </summary>
        /// <param name="messageActionType">Type of message to be queued</param>
        /// <param name="connectorInegrationDetails">Object to be passed to the queue</param>
        /// <param name="rootActivityId">Root Activity Id</param>
        /// <param name="jobApplicationId">schedule Id</param>
        /// <returns>Returns Success or Failure</returns>
        Task QueueMessageToConnector(string messageActionType, ConnectorIntegrationDetails connectorInegrationDetails, string rootActivityId, string jobApplicationId);
    }
}
