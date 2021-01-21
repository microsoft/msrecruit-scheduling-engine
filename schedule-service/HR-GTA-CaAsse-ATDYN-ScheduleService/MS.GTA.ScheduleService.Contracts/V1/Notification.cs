// <copyright file="Notification.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// A change notification.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets the type of change
        /// </summary>
        [JsonProperty(PropertyName = "changeType")]
        public string ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the client state used to verify that the notification is from Microsoft Graph. Compare the value received with the notification to the value you sent with the subscription request.
        /// </summary>
        [JsonProperty(PropertyName = "clientState")]
        public string ClientState { get; set; }

        /// <summary>
        /// Gets or sets the endpoint of the resource that changed. For example, a message uses the format ../Users/{user-id}/Messages/{message-id}
        /// </summary>
        [JsonProperty(PropertyName = "resource")]
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the web hooks subscription expires.
        /// </summary>
        [JsonProperty(PropertyName = "subscriptionExpirationDateTime")]
        public DateTimeOffset SubscriptionExpirationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the web hooks subscription.
        /// </summary>
        [JsonProperty(PropertyName = "subscriptionId")]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets properties of the changed resource.
        /// </summary>
        [JsonProperty(PropertyName = "resourceData")]
        public ResourceData ResourceData { get; set; }

        /// <summary>
        /// Gets or sets properties of the service Account Email.
        /// </summary>
        [JsonProperty(PropertyName = "serviceAccountEmail")]
        public string ServiceAccountEmail { get; set; }
    }
}
