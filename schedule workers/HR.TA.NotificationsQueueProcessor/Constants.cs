﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.NotificationsQueueProcessor
{
    /// <summary>
    /// Constants used by application.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Content type of the body/response of an Http Request.
        /// </summary>
        public const string JsonMIMEType = "application/json";

        /// <summary>
        /// Email notification type.
        /// </summary>
        public const string EmailNotificationType = "email";

        /// <summary>
        /// Meeting notification type.
        /// </summary>
        public const string MeetingNotificationType = "meet";

        /// <summary>
        /// Authority Environment variable name.
        /// </summary>
        public const string EnvVariableAuthority = "Authority";

        /// <summary>
        /// Client Id Environment variable name.
        /// </summary>
        public const string EnvVariableClientId = "ClientId";

        /// <summary>
        /// Client Secret Environment variable name.
        /// </summary>
        public const string EnvVariableClientSecret = "ClientSecret";

        /// <summary>
        /// Notification service endpoint Environment variable name.
        /// </summary>
        public const string EnvVariableNotificationServiceEndpoint = "NotificationServiceEndpoint";
    }
}
