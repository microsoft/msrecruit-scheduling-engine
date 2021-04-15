//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Configurations
{
    using HR.TA.ServicePlatform.Configuration;

    /// <summary>
    /// Service Bus Client Configuration
    /// </summary>
    [SettingsSection("ServiceBusClientConfigration")]
    public class ServiceBusClientConfiguration
    {
        /// <summary>
        /// Gets or sets holds key vault uri
        /// </summary>
        public string KeyVaultUri { get; set; }

        /// <summary>
        /// Gets or sets the Service Bus connection string secret name.
        /// </summary>
        public string ServiceBusConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the export Service Bus queue name.
        /// </summary>
        public string ExportServiceBusQueueName { get; set; }

        /// <summary>
        /// Gets or sets notification servicebusqueue Name
        /// </summary>
        public string NotificationServiceBusQueueName { get; set; }

        /// <summary>
        /// Gets or sets the Service Bus connection string secret name.
        /// </summary>
        public string ConnectorServiceBusConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the export Service Bus queue name.
        /// </summary>
        public string ConnectorServiceBusQueueName { get; set; }
    }
}
