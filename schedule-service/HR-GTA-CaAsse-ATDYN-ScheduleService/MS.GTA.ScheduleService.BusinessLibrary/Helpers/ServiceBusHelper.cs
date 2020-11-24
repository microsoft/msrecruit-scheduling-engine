//----------------------------------------------------------------------------
// <copyright file="ServiceBusHelper.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Helpers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using MS.GTA.Common.Base.Configuration;
    using MS.GTA.ScheduleService.BusinessLibrary.Configurations;
    using MS.GTA.ScheduleService.BusinessLibrary.Interface;
    using MS.GTA.ScheduleService.Contracts;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.ServicePlatform.Azure.AAD;
    using MS.GTA.ServicePlatform.Azure.Security;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;
    using Newtonsoft.Json;

    /// <summary>The view model extensions for meeting info.</summary>
    public class ServiceBusHelper : IServiceBusHelper
    {
        private readonly ServiceBusClientConfiguration servicebusClientConfiguration;

        private readonly AADClientConfiguration aadConfiguration;

        private readonly IAzureActiveDirectoryClient azureActiveDirectoryClient;

        private readonly ISecretManager secretManager;

        private readonly ITraceSource traceSource;

        private IQueueClient queueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusHelper"/> class.
        /// </summary>
        /// <param name="azureClient">azure client instance</param>
        /// <param name="secret">secret manager instance</param>
        /// <param name="traceSource">Trace source object</param>
        public ServiceBusHelper(
           IAzureActiveDirectoryClient azureClient,
           ISecretManager secret,
           ITraceSource traceSource)
        {
            this.secretManager = secret;
            this.azureActiveDirectoryClient = azureClient;
            this.servicebusClientConfiguration = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<ServiceBusClientConfiguration>();
            this.aadConfiguration = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<AADClientConfiguration>();
            this.traceSource = traceSource;
            if (!string.IsNullOrEmpty(this.servicebusClientConfiguration?.KeyVaultUri))
            {
                this.secretManager = new SecretManager(
                    this.azureActiveDirectoryClient,
                    new KeyVaultConfiguration { KeyVaultUri = this.servicebusClientConfiguration.KeyVaultUri });
            }
        }

        /// <summary>
        /// Queues the message to the Service Bus Queue
        /// </summary>
        /// <param name="messageActionType">Type of message to be queued</param>
        /// <param name="connectorInegrationDetails">Object to be passed to the queue</param>
        /// <param name="rootActivityId">Root Activity Id</param>
        /// <param name="scheduleId">schedule Id</param>
        /// <returns>Returns Success or Failure</returns>
        public async Task QueueMessageToSendInvitationWorker(string messageActionType, ConnectorIntegrationDetails connectorInegrationDetails, string rootActivityId, string scheduleId)
        {
            try
            {
                var secretValue = await this.secretManager.TryGetSecretAsync(this.servicebusClientConfiguration.ServiceBusConnectionString);
                //// Send the message to the queue
                this.queueClient = new QueueClient(
                    secretValue.Result.Value,
                    this.servicebusClientConfiguration.ExportServiceBusQueueName);
                string messageObject = JsonConvert.SerializeObject(connectorInegrationDetails);
                this.traceSource.TraceInformation("QueueMessage: " + connectorInegrationDetails.ScheduleID + " with RID " + rootActivityId + " sending");
                await this.queueClient.SendAsync(new Microsoft.Azure.ServiceBus.Message(System.Text.Encoding.UTF8.GetBytes(messageObject))
                {
                    UserProperties =
                    {
                        { "ActionType", messageActionType },
                        { "TenantId", this.aadConfiguration.TenantID },
                        { "ScheduleID", connectorInegrationDetails.ScheduleID },
                        { "ScheduleRootActivityId", rootActivityId }
                    },
                    MessageId = Guid.NewGuid().ToString()
                });

                this.traceSource.TraceInformation("QueueMessage: " + connectorInegrationDetails.ScheduleID + " with messageActionType as " + messageActionType + " and RID " + rootActivityId + " sent successfully");
            }
            catch (Exception ex)
            {
                this.traceSource.TraceError("QueueMessage: " + connectorInegrationDetails.ScheduleID + " with messageActionType as " + messageActionType + " and RID " + rootActivityId + " sent failed. Reason: " + ex.Message + " . StackTrace:" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Queues the message to the Service Bus Queue
        /// </summary>
        /// <param name="serviceBusMessage">Service bus message object</param>
        /// <returns>Returns Success or Failure</returns>
        public async Task QueueMessageToNotificationWorker(ServiceBusMessage serviceBusMessage)
        {
            if (serviceBusMessage == null)
            {
                throw new InvalidRequestDataValidationException("Invalid service bus message object").EnsureTraced(this.traceSource);
            }

            try
            {
                var secretValue = await this.secretManager.TryGetSecretAsync(serviceBusMessage.ConnectionStringKey);
                this.queueClient = new QueueClient(secretValue.Result.Value, serviceBusMessage.QueueName);

                var messageContent = new Microsoft.Azure.ServiceBus.Message(System.Text.Encoding.UTF8.GetBytes(serviceBusMessage.Message));

                foreach (var keyValuePair in serviceBusMessage.UserProperties)
                {
                    messageContent.UserProperties.Add(keyValuePair.Key, keyValuePair.Value);
                }

                messageContent.MessageId = Guid.NewGuid().ToString();

                this.traceSource.TraceInformation($"QueueMessageToServiceBus: sending  message to queue: {serviceBusMessage.QueueName}. and message id: {messageContent.MessageId}");
                await this.queueClient.SendAsync(messageContent);

                this.traceSource.TraceInformation($"QueueMessageToServiceBus: sent message successfully to queue: {serviceBusMessage.QueueName} and message id : {messageContent.MessageId}.");
            }
            catch (Exception ex)
            {
                this.traceSource.TraceError($"QueueMessageToServiceBus: failed to send messageObject {serviceBusMessage.Message} to queue: {serviceBusMessage.QueueName} , Reason: {ex.Message}, StackTrace: {ex.StackTrace}.");
            }
        }

        /// <summary>
        /// Queues the message to the Service Bus Queue
        /// </summary>
        /// <param name="messageActionType">Type of message to be queued</param>
        /// <param name="connectorInegrationDetails">Object to be passed to the queue</param>
        /// <param name="rootActivityId">Root Activity Id</param>
        /// <param name="jobApplicationId">schedule Id</param>
        /// <returns>Returns Success or Failure</returns>
        public async Task QueueMessageToConnector(string messageActionType, ConnectorIntegrationDetails connectorInegrationDetails, string rootActivityId, string jobApplicationId)
        {
            try
            {
                var secretValue = await this.secretManager.TryGetSecretAsync(this.servicebusClientConfiguration.ConnectorServiceBusConnectionString);

                //// Send the message to the queue
                this.queueClient = new QueueClient(
                    secretValue.Result.Value,
                    this.servicebusClientConfiguration.ConnectorServiceBusQueueName);

                string messageObject = JsonConvert.SerializeObject(connectorInegrationDetails);
                this.traceSource.TraceInformation("QueueMessage: " + connectorInegrationDetails.ScheduleID + " with RID " + rootActivityId + " sending");
                await this.queueClient.SendAsync(new Microsoft.Azure.ServiceBus.Message(System.Text.Encoding.UTF8.GetBytes(messageObject))
                {
                    UserProperties =
                    {
                        { "ActionType", messageActionType },
                        { "TenantId", this.aadConfiguration.TenantID },
                        { "JobApplicationID", jobApplicationId },
                        { "ScheduleID", connectorInegrationDetails.ScheduleID },
                        { "ScheduleRootActivityId", rootActivityId }
                    },
                    MessageId = Guid.NewGuid().ToString()
                });

                this.traceSource.TraceInformation("QueueMessage: " + connectorInegrationDetails.ScheduleID + " with messageActionType as " + messageActionType + " and RID " + rootActivityId + " sent successfully");
            }
            catch (Exception ex)
            {
                this.traceSource.TraceInformation($"QueueMessageToServiceBus: sent message failed to queu {jobApplicationId}  with messageActionType as {messageActionType} and with error message : {ex.StackTrace}.");
            }
        }
    }
}
