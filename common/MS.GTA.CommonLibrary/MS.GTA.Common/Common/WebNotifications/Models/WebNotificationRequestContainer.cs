// <copyright file="WebNotificationRequestContainer.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.WebNotifications.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="WebNotificationRequestContainer"/> class stores the web notification requests.
    /// </summary>
    [DataContract]
    public class WebNotificationRequestContainer
    {
        /// <summary>
        /// Gets the web notifications.
        /// </summary>
        /// <value>
        /// The web notifications.
        /// </value>
        [DataMember(Name = "notifications")]
        public List<WebNotificationRequest> Notifications { get; } = new List<WebNotificationRequest>();
    }
}
