//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.WebNotifications
{
    using System;
    using MS.GTA.Common.WebNotifications.Interfaces;

    /// <summary>
    /// The <see cref="WebNotificationBuilderClient"/> class holds web notication client and builder together.
    /// </summary>
    /// <seealso cref="IWebNotificationBuilderClient"/>
    public class WebNotificationBuilderClient : IWebNotificationBuilderClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebNotificationBuilderClient"/> class.
        /// </summary>
        /// <param name="webNotificationBuilder">The instance for <see cref="IWebNotificationBuilder"/>.</param>
        /// <param name="webNotificationClient">The instance for <see cref="IWebNotificationClient"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// webNotificationBuilder
        /// or
        /// webNotificationClient.
        /// </exception>
        public WebNotificationBuilderClient(IWebNotificationBuilder webNotificationBuilder, IWebNotificationClient webNotificationClient)
        {
            this.Builder = webNotificationBuilder ?? throw new ArgumentNullException(nameof(webNotificationBuilder));
            this.Client = webNotificationClient ?? throw new ArgumentNullException(nameof(webNotificationClient));
        }

        /// <summary>
        /// Gets the web notifiation builder.
        /// </summary>
        /// <value>
        /// The instance for <see cref="IWebNotificationBuilder" />.
        /// </value>
        public IWebNotificationBuilder Builder { get; }

        /// <summary>
        /// Gets the web notification service client.
        /// </summary>
        /// <value>
        /// The instance for <see cref="IWebNotificationClient" />.
        /// </value>
        public IWebNotificationClient Client { get; }
    }
}
