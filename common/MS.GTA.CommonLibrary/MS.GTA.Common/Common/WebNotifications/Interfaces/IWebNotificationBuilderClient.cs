// <copyright file="IWebNotificationBuilderClient.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.WebNotifications.Interfaces
{
    /// <summary>
    /// The <see cref="IWebNotificationBuilderClient"/> interface provides mechanism to hold the web notification builder and service client.
    /// </summary>
    public interface IWebNotificationBuilderClient
    {
        /// <summary>
        /// Gets the web notifiation builder.
        /// </summary>
        /// <value>
        /// The instance for <see cref="IWebNotificationBuilder"/>.
        /// </value>
        IWebNotificationBuilder Builder { get; }

        /// <summary>
        /// Gets the web notification service client.
        /// </summary>
        /// <value>
        /// The instance for <see cref="IWebNotificationClient"/>.
        /// </value>
        IWebNotificationClient Client { get; }
    }
}
