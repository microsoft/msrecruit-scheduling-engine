//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Email
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using GraphContracts;
    using SendGridContracts;

    /// <summary>
    /// Email Service for email sending.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Gets the DefaultEmailNameToSendFrom
        /// </summary>
        string DefaultEmailNameToSendFrom
        {
            get;
        }

        /// <summary>
        /// Gets the DefaultEmailNameToSendFrom
        /// </summary>
        string DefaultEmailAddressToSendFrom
        {
            get;
        }

        /// <summary>
        /// Gets the DefaultDomainToSendFrom
        /// </summary>
        string DefaultDomainToSendFrom
        {
            get;
        }

        /// <summary>
        /// Sending mail using http client from a default email address.
        /// </summary>
        /// <param name="personalize">False if "To emails" should be able to see each others.</param>
        /// <param name="templateType">Html body template for the email.</param>
        /// <param name="templateParams">Associated parameters for the template</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="headers">email header values</param>
        /// <returns>HttpStatusCode from the response</returns>
        Task<HttpStatusCode> SendMailUsingTemplateAsync(
            bool personalize,
            MessageTemplateType templateType,
            IDictionary<string, string> templateParams,
            IList<EmailAddress> toEmailAddresses,
            IList<Attachment> attachments = null,
            IDictionary<string, string> headers = null);

        /// <summary>
        /// Sending mail using http client.
        /// </summary>
        /// <param name="personalize">False if "To emails" should be able to see each others.</param>
        /// <param name="templateType">Html body template for the email.</param>
        /// <param name="templateParams">Associated parameters for the template</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="fromEmailAddress">from email address</param>
        /// <param name="replyToEmailAddress">Reply To email address</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="headers">email header values</param>
        /// <returns>HttpStatusCode from the response</returns>
        Task<HttpStatusCode> SendMailUsingTemplateAsync(
            bool personalize,
            MessageTemplateType templateType,
            IDictionary<string, string> templateParams,
            IList<EmailAddress> toEmailAddresses,
            EmailAddress fromEmailAddress,
            EmailAddress replyToEmailAddress = null,
            IList<Attachment> attachments = null,
            IDictionary<string, string> headers = null);

        /// <summary>
        /// Sending mail using http client.
        /// </summary>
        /// <param name="personalize">False if "To emails" should be able to see each others.</param>
        /// <param name="subject">Subject of the mail.</param>
        /// <param name="htmlBody">Html body of the email.</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="fromEmailAddress">from email address</param>
        /// <param name="replyToEmailAddress">Reply To email address</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="headers">email header values</param>
        /// <returns>HttpStatusCode from the response</returns>
        Task<HttpStatusCode> SendMailAsync(
            bool personalize,
            string subject,
            string htmlBody,
            IList<EmailAddress> toEmailAddresses,
            EmailAddress fromEmailAddress,
            EmailAddress replyToEmailAddress = null,
            IList<Attachment> attachments = null,
            IDictionary<string, string> headers = null);

        /// <summary>Sending mail using specified template from local resource.</summary>
        /// <param name="personalize">False if "To emails" should be able to see each others.</param>
        /// <param name="bodyTemplate">The email body Template.</param>
        /// <param name="subjectTemplate">The email subject Template.</param>
        /// <param name="templateParams">Associated parameters for the template</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="fromEmailAddress">from email address</param>
        /// <param name="replyToEmailAddress">Reply To email address</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="headers">email header values</param>
        /// <returns>HttpStatusCode from the response</returns>
        Task<HttpStatusCode> SendMailUsingTemplateAsync(
            bool personalize,
            string bodyTemplate,
            string subjectTemplate,
            IDictionary<string, string> templateParams,
            IList<EmailAddress> toEmailAddresses,
            EmailAddress fromEmailAddress,
            EmailAddress replyToEmailAddress = null,
            IList<Attachment> attachments = null,
            IDictionary<string, string> headers = null);

        /// <summary>Sending mail using specified template from local resource.</summary>
        /// <param name="bodyTemplate">The email body Template.</param>
        /// <param name="subjectTemplate">The email subject Template.</param>
        /// <param name="templateParams">Associated parameters for the template</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="authHeader">Authentication header. This is used for sender information. With this account send mail</param>
        /// <param name="replyToEmailAddress">Reply To email address list</param>
        /// <param name="ccEmailAddress">CC email address list</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="bccEmailAddresses">BCC email address list</param>
        /// <returns>HttpStatusCode from the response</returns>
        Task<HttpStatusCode> SendGraphMailUsingTemplateAsync(
            string bodyTemplate,
            string subjectTemplate,
            IDictionary<string, string> templateParams,
            IList<GraphEmailAddress> toEmailAddresses,
            AuthenticationHeaderValue authHeader,
            IList<GraphEmailAddress> replyToEmailAddress = null,
            IList<GraphEmailAddress> ccEmailAddress = null,
            IList<Attachment> attachments = null,
            IList<GraphEmailAddress> bccEmailAddresses = null);

        /// <summary>Sending mail using specified template from local resource, and by using list of logins.</summary>
        /// <param name="bodyTemplate">The email body Template.</param>
        /// <param name="subjectTemplate">The email subject Template.</param>
        /// <param name="templateParams">Associated parameters for the template</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="authHeader">Authentication header. This is used for sender information. With this account send mail</param>
        /// <param name="replyToEmailAddress">Reply To email address list</param>
        /// <param name="ccEmailAddress">CC email address list</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="bccEmailAddresses">BCC email address list</param>
        /// <returns>HttpStatusCode from the response</returns>
        Task<HttpStatusCode> SendEmailUsingGraph(
            string bodyTemplate,
            string subjectTemplate,
            IDictionary<string, string> templateParams,
            IList<GraphEmailAddress> toEmailAddresses,
            AuthenticationHeaderValue authHeader = null,
            IList<GraphEmailAddress> replyToEmailAddress = null,
            IList<GraphEmailAddress> ccEmailAddress = null,
            IList<Attachment> attachments = null,
            IList<GraphEmailAddress> bccEmailAddresses = null);

        /// <summary>Sending mail using specified template from local resource.</summary>
        /// <param name="bodyTemplate">The email body Template.</param>
        /// <param name="subjectTemplate">The email subject Template.</param>
        /// <param name="templateParams">Associated parameters for the template</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="replyToEmailAddress">Reply To email address list</param>
        /// <param name="ccEmailAddress">CC email address list</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="bccEmailAddresses">BCC email address list</param>
        /// <returns>HttpStatusCode from the response</returns>
        Task<HttpStatusCode> SendNotificationUsingTemplateAsync(
            string bodyTemplate,
            string subjectTemplate,
            IDictionary<string, string> templateParams,
            IList<GraphEmailAddress> toEmailAddresses,
            IList<GraphEmailAddress> replyToEmailAddress = null,
            IList<GraphEmailAddress> ccEmailAddress = null,
            IList<Attachment> attachments = null,
            IList<GraphEmailAddress> bccEmailAddresses = null);
    }
}
