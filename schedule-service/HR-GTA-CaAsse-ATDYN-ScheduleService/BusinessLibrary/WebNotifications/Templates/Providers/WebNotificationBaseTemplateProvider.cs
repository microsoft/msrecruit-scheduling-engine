//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.BusinessLibrary.WebNotifications.Templates.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Resources;
    using CommonLibrary.Common.WebNotifications.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// The <see cref="WebNotificationBaseTemplateProvider"/> class acts as a base type for web notification template provider types.
    /// </summary>
    /// <seealso cref="IWebNotificationTemplateProvider" />
    internal abstract class WebNotificationBaseTemplateProvider : IWebNotificationTemplateProvider
    {
        /// <summary>
        /// The resource manager.
        /// </summary>
        private readonly ResourceManager resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebNotificationBaseTemplateProvider"/> class.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <exception cref="ArgumentException">The template name is missing. - templateName</exception>
        public WebNotificationBaseTemplateProvider(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentException("The template name is missing.", nameof(templateName));
            }

            this.resourceManager = new ResourceManager(typeof(WebNotificationTemplates));
            string templateJSON = this.resourceManager.GetString(templateName);
            this.TemplateObject = JsonConvert.DeserializeObject(templateJSON);
        }

        /// <summary>
        /// Gets the template object.
        /// </summary>
        /// <value>
        /// The template object.
        /// </value>
        protected dynamic TemplateObject { get; }

        /// <inheritdoc cref="IWebNotificationTemplateProvider"/>
        public abstract string ProvideTemplate(Dictionary<string, string> notificationProperties);
    }
}
