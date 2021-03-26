//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Common.WebNotifications.Models
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
