//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Common.WebNotifications.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// The <see cref="IWebNotificationTemplateProvider"/> interface provides mechanism to get the web notification template.
    /// </summary>
    public interface IWebNotificationTemplateProvider
    {
        /// <summary>
        /// Provides the web notification body template.
        /// </summary>
        /// <param name="notificationProperties">The instance of <see cref="Dictionary{String, String}"/>.</param>
        /// <returns>The template string.</returns>
        string ProvideTemplate(Dictionary<string, string> notificationProperties);
    }
}
