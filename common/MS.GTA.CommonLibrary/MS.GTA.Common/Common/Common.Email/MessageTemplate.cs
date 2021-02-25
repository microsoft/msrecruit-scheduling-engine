//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Email
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using MS.GTA.Common.Base.Utilities;
    using MS.GTA.Common.Common.Common.Email;
    using MS.GTA.CommonDataService.Common.Internal;
    
    /// <summary>
    /// Templates class
    /// </summary>
    public class MessageTemplate
    {
        /// <summary>
        /// Type of Template for the current message.
        /// </summary>
        private readonly MessageTemplateType template;

        /// <summary>
        /// Property bag to replace the parameters inside the template.
        /// </summary>
        private readonly IDictionary<string, string> templateParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTemplate" /> class.
        /// </summary>
        /// <param name="templateType">Type of the template</param>
        /// <param name="templateParams">Property bag to replace the parameters with their values.</param>
        public MessageTemplate(MessageTemplateType templateType, IDictionary<string, string> templateParams)
        {
            Contract.CheckValue(templateParams, nameof(templateParams));

            this.template = templateType;
            this.templateParams = templateParams;
            this.Render();
        }

        /// <summary>Initializes a new instance of the <see cref="MessageTemplate"/> class.</summary>
        /// <param name="messageBodyTemplate">The message Body Template.</param>
        /// <param name="messageSubjectTemplate">The message Subject Template.</param>
        /// <param name="templateParams">The template parameters.</param>
        public MessageTemplate(string messageBodyTemplate, string messageSubjectTemplate, IDictionary<string, string> templateParams)
        {
            Contract.CheckValue(messageBodyTemplate, nameof(messageBodyTemplate));
            Contract.CheckValue(messageSubjectTemplate, nameof(messageSubjectTemplate));
            Contract.CheckValue(templateParams, nameof(templateParams));

            this.templateParams = templateParams;
            this.ParseTemplate(messageSubjectTemplate, messageBodyTemplate);
        }

        /// <summary>
        /// Gets the MessageSubject for current message.
        /// </summary>
        public string MessageSubject { get; private set; }

        /// <summary>
        /// Gets the MessageBody for current message.
        /// </summary>
        public string MessageBody { get; private set; }

        /// <summary>
        /// Gets the list of parameters (placeholders inside a template).
        /// </summary>
        /// <param name="messageTemplateType">Type of the template</param>
        /// <returns>The list of placeholders.</returns>
        public static List<string> GetPlaceHoldersInTemplates(MessageTemplateType messageTemplateType)
        {
            string messageBodyTemplate;
            string messageSubjectTemplate;
            if (!TryGetTemplates(messageTemplateType, out messageBodyTemplate, out messageSubjectTemplate))
            {
                throw new InvalidOperationException($"Message body or subject does not exist in the Resource file for {messageTemplateType}");
            }

            return GetPlaceHoldersInGivenString(messageBodyTemplate).Union(GetPlaceHoldersInGivenString(messageSubjectTemplate)).ToList();
        }

        /// <summary>
        /// Gets the Body and Subject templates.
        /// </summary>
        /// <param name="messageTemplateType">Type of the template.</param>
        /// <param name="messageBodyTemplate">Outputs the body template.</param>
        /// <param name="messageSubjectTemplate">Outputs the subject template.</param>
        /// <returns>True if successful.</returns>
        private static bool TryGetTemplates(MessageTemplateType messageTemplateType, out string messageBodyTemplate, out string messageSubjectTemplate)
        {
            var enumValue = messageTemplateType.ToString();
            messageSubjectTemplate = EmailStrings.ResourceManager.GetString($"{Constants.EmailSubjectTemplateResourcePrefix}{enumValue}", EmailStrings.Culture);
            messageBodyTemplate = EmailStrings.ResourceManager.GetString($"{Constants.EmailMessageTemplateResourcePrefix}{enumValue}", EmailStrings.Culture);

            return !string.IsNullOrEmpty(messageSubjectTemplate) && !string.IsNullOrEmpty(messageBodyTemplate);
        }

        /// <summary>
        /// Gets the list of parameters (placeholders inside a template).
        /// </summary>
        /// <param name="template">template to search the placeholders in.</param>
        /// <returns>The list of placeholders.</returns>
        private static List<string> GetPlaceHoldersInGivenString(string template)
        {
            Contract.CheckValue(template, nameof(template));
            var list = new List<string>();

            string pat = @"{(\w+)}";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(template);
            while (m.Success)
            {
                Group g = m.Groups[1];
                list.Add(g.Value);
                m = m.NextMatch();
            }

            return list;
        }

        /// <summary>
        /// Renders the final string from the templates.
        /// </summary>
        private void Render()
        {
            string messageBodyTemplate;
            string messageSubjectTemplate;

            if (TryGetTemplates(this.template, out messageBodyTemplate, out messageSubjectTemplate))
            {
                this.ParseTemplate(messageSubjectTemplate, messageBodyTemplate);
            }
        }

        /// <summary>Parses the subject and body templates.</summary>
        /// <param name="messageSubjectTemplate">The message Subject Template.</param>
        /// <param name="messageBodyTemplate">The message Body Template.</param>
        private void ParseTemplate(string messageSubjectTemplate, string messageBodyTemplate)
        {
            Contract.CheckValue(messageSubjectTemplate, nameof(messageSubjectTemplate));
            Contract.CheckValue(messageBodyTemplate, nameof(messageBodyTemplate));

            foreach (var key in this.templateParams.Keys)
            {
                var value = this.templateParams[key] ?? string.Empty;
                messageSubjectTemplate = messageSubjectTemplate.Replace(
                    $"{{{key}}}",
                    value,
                    StringComparison.InvariantCultureIgnoreCase);
                messageBodyTemplate = messageBodyTemplate.Replace(
                    $"{{{key}}}",
                    value,
                    StringComparison.InvariantCultureIgnoreCase);
            }

            this.MessageBody = messageBodyTemplate;
            this.MessageSubject = messageSubjectTemplate;
        }
    }
}
