// <copyright file="Subscription.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

/*
 *  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
 *  See LICENSE in the source repository root for complete license information.
 */

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// A web hook subscription.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Gets or sets the type of change in the subscribed resource that raises a notification.
        /// </summary>
        [JsonProperty(PropertyName = "changeType")]
        public string ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the string that Microsoft Graph should send with each notification. Maximum length is 255 characters.
        /// To verify that the notification is from Microsoft Graph, compare the value received with the notification to the value you sent with the subscription request.
        /// </summary>
        [JsonProperty(PropertyName = "clientState")]
        public string ClientState { get; set; }

        /// <summary>
        /// Gets or sets the URL of the endpoint that receives the subscription response and notifications. Requires https.
        /// </summary>
        [JsonProperty(PropertyName = "notificationUrl")]
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets the resource to monitor for changes.
        /// </summary>
        [JsonProperty(PropertyName = "resource")]
        public string Resource { get; set; }

        /// <summary>
        /// Gets or sets the amount of time in UTC format when the web hook subscription expires, based on the subscription creation time.
        /// The maximum time varies for the resource subscribed to. This sample sets it to the 4230 minute maximum for messages.
        /// </summary>
        [JsonProperty(PropertyName = "expirationDateTime")]
        public DateTimeOffset ExpirationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the web hook subscription.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the refresh token to get the resources subscribed to on notification
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
