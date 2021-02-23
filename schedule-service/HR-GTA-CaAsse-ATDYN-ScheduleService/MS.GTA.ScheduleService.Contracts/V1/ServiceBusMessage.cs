//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace MS.GTA.ScheduleService.Contracts.V1
{
    /// <summary>
    /// Service Bus connection
    /// </summary>
    public class ServiceBusMessage
    {
        /// <summary>Gets or sets ConnectionString Key</summary>
        public string ConnectionStringKey { get; set; }

        /// <summary>Gets or sets Queue Name</summary>
        public string QueueName { get; set; }

        /// <summary>Gets or sets KeyVault Uri</summary>
        public string KeyVaultUri { get; set; }

        /// <summary>Gets or sets Message</summary>
        public string Message { get; set; }

        /// <summary>Gets or sets UserProperties</summary>
        public IDictionary<string, string> UserProperties { get; set; }
    }
}
