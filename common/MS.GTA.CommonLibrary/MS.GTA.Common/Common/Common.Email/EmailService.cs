//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Email
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Castle.Core.Logging;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using MS.GTA.Common.Base.Configuration;
    using MS.GTA.Common.Base.Utilities;
    using MS.GTA.Common.Common.Common.Email;
    using MS.GTA.Common.Common.Common.Email.Contracts;
    using MS.GTA.Common.Email.Exceptions;
    using MS.GTA.Common.Email.GraphContracts;
    using MS.GTA.Common.Email.SendGridContracts;
    using MS.GTA.Common.MSGraph;
    using MS.GTA.Common.MSGraph.Configuration;
    using MS.GTA.Common.TestBase;
    using MS.GTA.CommonDataService.Common;
    using MS.GTA.ServicePlatform.Azure.Security;
    using MS.GTA.ServicePlatform.Configuration;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;
    using Newtonsoft.Json;

    public class EmailService : IEmailService
    {
        /// <summary>
        /// Trace source
        /// </summary>
        private readonly ILogger<EmailService> logger;

        /// <summary> The Service Platform Secret Manager. </summary>
        private readonly ISecretManager secretManager;

        /// <summary>
        /// Email Configuration
        /// </summary>
        private readonly EmailConfiguration emailConfiguration;

        /// <summary>
        /// JSON serializer settings for http calls.
        /// </summary>
        private JsonSerializerSettings jsonSerializerSettings;

        /// <summary>
        /// Configuration manager instance
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// MS Graph Provider instance
        /// </summary>
        private readonly IMsGraphProvider msGraphProvider;

        /// <summary>
        /// AAD Configuration
        /// </summary>
        private readonly AADClientConfiguration aadConfigurationManager;

        /// <summary>
        /// PAF Configuration
        /// </summary>
        private readonly PAFConfiguration pafConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configManager">The configuration manager instance to use.</param>
        /// <param name="secretManager">The ServicePlatform Secret Manager.</param>
        /// <param name="msGraphProvider">MS Graph Provider instance</param>
        public EmailService(ILogger<EmailService> logger, IConfigurationManager configManager, ISecretManager secretManager, IMsGraphProvider msGraphProvider)
        {
            Contract.CheckValue(logger, nameof(logger));
            Contract.CheckValue(configManager, nameof(configManager));
            Contract.CheckValue(secretManager, nameof(secretManager));
            Contract.CheckValue(msGraphProvider, nameof(msGraphProvider));

            this.emailConfiguration = configManager.Get<EmailConfiguration>();
            this.logger = logger;
            this.secretManager = secretManager;
            this.configurationManager = configManager;
            this.msGraphProvider = msGraphProvider;
            this.aadConfigurationManager = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<AADClientConfiguration>();
            this.pafConfiguration = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<PAFConfiguration>();

            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };
        }

        /// <summary>
        /// Gets the DefaultEmailNameToSendFrom
        /// </summary>
        public string DefaultEmailNameToSendFrom
        {
            get
            {
                return Constants.DefaultEmailNameToSendFrom;
            }
        }

        /// <summary>
        /// Gets the DefaultEmailNameToSendFrom
        /// </summary>
        public string DefaultEmailAddressToSendFrom
        {
            get
            {
                return $"{Constants.DefaultEmailAliasToSendFrom}@{this.emailConfiguration.DefaultEmailDomainToSendFrom}";
            }
        }

        /// <summary>
        /// Gets the DefaultDomainToSendFrom
        /// </summary>
        public string DefaultDomainToSendFrom
        {
            get
            {
                return this.emailConfiguration.DefaultEmailDomainToSendFrom;
            }
        }

        /// <summary>
        /// Sending mail from a default email address, using specified template.
        /// </summary>
        /// <param name="personalize">False if "To emails" should be able to see each others.</param>
        /// <param name="templateType">Html body template for the email.</param>
        /// <param name="templateParams">Associated parameters for the template</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="headers">email header values</param>
        /// <returns>HttpStatusCode from the response</returns>
        public async Task<HttpStatusCode> SendMailUsingTemplateAsync(
            bool personalize,
            MessageTemplateType templateType,
            IDictionary<string, string> templateParams,
            IList<EmailAddress> toEmailAddresses,
            IList<Attachment> attachments = null,
            IDictionary<string, string> headers = null)
        {
            Contract.CheckValue(templateParams, nameof(templateParams));
            EmailDefinition.ValidateToEmailAddresses(toEmailAddresses);
            EmailDefinition.ValidateAttachments(attachments);
            EmailDefinition.ValidateHeaders(headers);

            var fromEmailAddress = new EmailAddress { Name = this.DefaultEmailNameToSendFrom, Email = this.DefaultEmailAddressToSendFrom };
            Contract.CheckValue(fromEmailAddress, nameof(fromEmailAddress));

            return await this.SendMailUsingTemplateAsync(personalize, templateType, templateParams, toEmailAddresses, fromEmailAddress, null, attachments, headers);
        }

        /// <summary>
        /// Sending mail using specified template.
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
        public async Task<HttpStatusCode> SendMailUsingTemplateAsync(
            bool personalize,
            MessageTemplateType templateType,
            IDictionary<string, string> templateParams,
            IList<EmailAddress> toEmailAddresses,
            EmailAddress fromEmailAddress,
            EmailAddress replyToEmailAddress = null,
            IList<Attachment> attachments = null,
            IDictionary<string, string> headers = null)
        {
            Contract.CheckValue(templateParams, nameof(templateParams));
            Contract.CheckValue(fromEmailAddress, nameof(fromEmailAddress));
            EmailDefinition.ValidateToEmailAddresses(toEmailAddresses);
            EmailDefinition.ValidateAttachments(attachments);
            EmailDefinition.ValidateHeaders(headers);

            this.AddEmailFooterToTemplateParamsDictionary(templateParams);
            this.AddCidParamsFromAttachmentsToTemplateParamsDictionary(templateParams, attachments);

            var messageTemplates = new MessageTemplate(templateType, templateParams);
            var htmlBody = messageTemplates.MessageBody;
            var subject = messageTemplates.MessageSubject;

            return await this.SendMailAsync(personalize, subject, htmlBody, toEmailAddresses, fromEmailAddress, replyToEmailAddress, attachments, headers);
        }

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
        public async Task<HttpStatusCode> SendMailUsingTemplateAsync(
            bool personalize,
            string bodyTemplate,
            string subjectTemplate,
            IDictionary<string, string> templateParams,
            IList<EmailAddress> toEmailAddresses,
            EmailAddress fromEmailAddress,
            EmailAddress replyToEmailAddress = null,
            IList<Attachment> attachments = null,
            IDictionary<string, string> headers = null)
        {
            Contract.CheckValue(templateParams, nameof(templateParams));
            Contract.CheckValue(fromEmailAddress, nameof(fromEmailAddress));
            Contract.CheckValue(bodyTemplate, nameof(bodyTemplate));
            Contract.CheckValue(subjectTemplate, nameof(subjectTemplate));

            EmailDefinition.ValidateToEmailAddresses(toEmailAddresses);
            EmailDefinition.ValidateAttachments(attachments);
            EmailDefinition.ValidateHeaders(headers);

            this.AddEmailFooterToTemplateParamsDictionary(templateParams);
            this.AddCidParamsFromAttachmentsToTemplateParamsDictionary(templateParams, attachments);

            var messageTemplates = new MessageTemplate(bodyTemplate, subjectTemplate, templateParams);
            var htmlBody = messageTemplates.MessageBody;
            var subject = messageTemplates.MessageSubject;

            return await this.SendMailAsync(personalize, subject, htmlBody, toEmailAddresses, fromEmailAddress, replyToEmailAddress, attachments, headers);
        }

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
        public async Task<HttpStatusCode> SendMailAsync(
            bool personalize,
            string subject,
            string htmlBody,
            IList<EmailAddress> toEmailAddresses,
            EmailAddress fromEmailAddress,
            EmailAddress replyToEmailAddress = null,
            IList<Attachment> attachments = null,
            IDictionary<string, string> headers = null)
        {
            var sendGridTestApiKey = await this.secretManager.GetSecretAsync(Constants.SendGridApiKeyTokenSecretName, this.logger);
            Contract.CheckNonEmpty(sendGridTestApiKey, nameof(sendGridTestApiKey));
            Contract.CheckValue(fromEmailAddress, nameof(fromEmailAddress));

            EmailDefinition.ValidateToEmailAddresses(toEmailAddresses);
            EmailDefinition.ValidateSubject(subject);
            EmailDefinition.ValidateMessage(htmlBody);
            EmailDefinition.ValidateAttachments(attachments);
            EmailDefinition.ValidateHeaders(headers);

            var httpRequest = new EmailDefinition
            {
                FromEmailAddress = fromEmailAddress,
                ReplyToEmailAddress = replyToEmailAddress,
                ToEmailAddresses = toEmailAddresses,
                Personalize = personalize,
                Subject = subject,
                EmailBodyHtml = htmlBody,
                Attachments = attachments,
                Headers = headers,
            };

            var client = new HttpClient { BaseAddress = new Uri(Constants.SendGridUrl) };

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Base.Constants.ApplicationJsonMediaType));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Base.Constants.BearerAuthenticationScheme, sendGridTestApiKey);

            using (var response = await CommonLogger.Logger.ExecuteAsync(
                "HcmCmnSendEmail",
                async () =>
                {
                    return await client.PostAsJsonAsync(Constants.SendGridEmailSendEndpoint, httpRequest);
                }))
            {
                var statusCode = response.StatusCode;
                this.logger.LogInformation($"Email send response statusCode is {statusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    this.logger.LogInformation($"Email send response body is {content}");

                    var responseHeaders = response.Headers.ToString();
                    this.logger.LogInformation($"Email send response headers are {responseHeaders}");
                }
                else
                {
                    this.logger.LogInformation($"Email send failure. StatusCode: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                }

                return statusCode;
            }
        }

        /// <summary>Sending mail using specified template from local resource.</summary>
        /// <param name="bodyTemplate">The email body Template.</param>
        /// <param name="subjectTemplate">The email subject Template.</param>
        /// <param name="templateParams">Associated parameters for the template</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="authHeader">Authentication header. This is used for sender information. With this account send mail</param>
        /// <param name="replyToEmailAddress">Reply To email address list</param>
        /// <param name="ccEmailAddress">CC email address list</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="bccEmailAddresses">CC email address list</param>
        /// <returns>HttpStatusCode from the response</returns>
        public async Task<HttpStatusCode> SendGraphMailUsingTemplateAsync(
            string bodyTemplate,
            string subjectTemplate,
            IDictionary<string, string> templateParams,
            IList<GraphEmailAddress> toEmailAddresses,
            AuthenticationHeaderValue authHeader,
            IList<GraphEmailAddress> replyToEmailAddress = null,
            IList<GraphEmailAddress> ccEmailAddress = null,
            IList<Attachment> attachments = null,
            IList<GraphEmailAddress> bccEmailAddresses = null)
        {
            Contract.CheckValue(templateParams, nameof(templateParams));
            Contract.CheckValue(bodyTemplate, nameof(bodyTemplate));
            Contract.CheckValue(subjectTemplate, nameof(subjectTemplate));
            Contract.CheckValue(toEmailAddresses, nameof(toEmailAddresses));

            EmailDefinition.ValidateAttachments(attachments);

            this.AddEmailFooterToTemplateParamsDictionary(templateParams);
            this.AddCidParamsFromAttachmentsToTemplateParamsDictionary(templateParams, attachments);

            var messageTemplates = new MessageTemplate(bodyTemplate, subjectTemplate, templateParams);
            var htmlBody = messageTemplates.MessageBody;
            var subject = messageTemplates.MessageSubject;

            return await this.SendGraphMailAsync(subject, htmlBody, toEmailAddresses, authHeader, replyToEmailAddress, ccEmailAddress, attachments, bccEmailAddresses: bccEmailAddresses);
        }

        /// <summary>Sending mail using specified template from local resource, and by using list of logins.</summary>
        /// <param name="bodyTemplate">The email body Template.</param>
        /// <param name="subjectTemplate">The email subject Template.</param>
        /// <param name="templateParams">Associated parameters for the template</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="authHeader">Authentication header. This is used for sender information. With this account send mail</param>
        /// <param name="replyToEmailAddress">Reply To email address</param>
        /// <param name="ccEmailAddress">CC email address list</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="bccEmailAddresses">BCC email address list</param>
        /// <returns>HttpStatusCode from the response</returns>
        public async Task<HttpStatusCode> SendEmailUsingGraph(
            string bodyTemplate,
            string subjectTemplate,
            IDictionary<string, string> templateParams,
            IList<GraphEmailAddress> toEmailAddresses,
            AuthenticationHeaderValue authHeader = null,
            IList<GraphEmailAddress> replyToEmailAddress = null,
            IList<GraphEmailAddress> ccEmailAddress = null,
            IList<Attachment> attachments = null,
            IList<GraphEmailAddress> bccEmailAddresses = null)
        {
            Contract.CheckValue(templateParams, nameof(templateParams));
            Contract.CheckValue(bodyTemplate, nameof(bodyTemplate));
            Contract.CheckValue(subjectTemplate, nameof(subjectTemplate));
            Contract.CheckValue(toEmailAddresses, nameof(toEmailAddresses));

            EmailDefinition.ValidateAttachments(attachments);

            this.AddEmailFooterToTemplateParamsDictionary(templateParams);
            this.AddCidParamsFromAttachmentsToTemplateParamsDictionary(templateParams, attachments);

            var messageTemplates = new MessageTemplate(bodyTemplate, subjectTemplate, templateParams);
            var htmlBody = messageTemplates.MessageBody;
            var subject = messageTemplates.MessageSubject;

            if (authHeader == null)
            {
                var graphMailConfiguration = this.configurationManager.Get<GraphMailConfiguration>();
                var secretValue = await this.secretManager.ReadSecretAsync(graphMailConfiguration.GraphEmailsSecret);
                var userEmailPasswords = JsonConvert.DeserializeObject<IList<UserEmailPasswordSecret>>(secretValue?.Value);
                var random = new Random();

                while (userEmailPasswords?.Count > 0)
                {
                    int index = random.Next(0, userEmailPasswords.Count);
                    try
                    {
                        authHeader = await msGraphProvider.GetGraphEmailResourceToken(userEmailPasswords[index]);
                        return await this.SendGraphMailAsync(subject, htmlBody, toEmailAddresses, authHeader, replyToEmailAddress, ccEmailAddress, attachments, bccEmailAddresses: bccEmailAddresses);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError($"Send email failed with account: {userEmailPasswords[index].UserEmail}, exception: {ex}");
                        userEmailPasswords.RemoveAt(index);
                    }
                }

                this.logger.LogError($"Send email failed with all the accounts");
                throw new GraphEmailException($"Send email failed with all the accounts");
            }
            else
            {
                try
                {
                    return await this.SendGraphMailAsync(subject, htmlBody, toEmailAddresses, authHeader, replyToEmailAddress, ccEmailAddress, attachments, bccEmailAddresses: bccEmailAddresses);
                }
                catch (Exception ex)
                {
                    this.logger.LogError($"Failed to send email with current auth header, exception: {ex}");
                    return await SendEmailUsingGraph(bodyTemplate, subjectTemplate, templateParams, toEmailAddresses, null, replyToEmailAddress, ccEmailAddress, attachments, bccEmailAddresses: bccEmailAddresses);

                }
            }
        }

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
        public async Task<HttpStatusCode> SendNotificationUsingTemplateAsync(string bodyTemplate, string subjectTemplate, IDictionary<string, string> templateParams, IList<GraphEmailAddress> toEmailAddresses, IList<GraphEmailAddress> replyToEmailAddress = null, IList<GraphEmailAddress> ccEmailAddress = null, IList<Attachment> attachments = null, IList<GraphEmailAddress> bccEmailAddresses = null)
        {
            Contract.CheckValue(templateParams, nameof(templateParams));
            Contract.CheckValue(bodyTemplate, nameof(bodyTemplate));
            Contract.CheckValue(subjectTemplate, nameof(subjectTemplate));
            Contract.CheckValue(toEmailAddresses, nameof(toEmailAddresses));

            EmailDefinition.ValidateAttachments(attachments);

            this.AddEmailFooterToTemplateParamsDictionary(templateParams);
            this.AddCidParamsFromAttachmentsToTemplateParamsDictionary(templateParams, attachments);

            var messageTemplates = new MessageTemplate(bodyTemplate, subjectTemplate, templateParams);
            var htmlBody = messageTemplates.MessageBody;
            var subject = messageTemplates.MessageSubject;

            return await this.SendNotificationAsync(subject, htmlBody, toEmailAddresses, replyToEmailAddress, ccEmailAddress, attachments, bccEmailAddresses: bccEmailAddresses);
        }

        /// <summary>
        /// Adds Cid Parameter From Attachments To Template Parameters Dictionary
        /// </summary>
        /// <param name="templateParams">template Parameters Dictionary.</param>
        /// <param name="attachments">List of attachments.</param>
        public void AddCidParamsFromAttachmentsToTemplateParamsDictionary(IDictionary<string, string> templateParams, IList<Attachment> attachments)
        {
            Contract.AssertValue(templateParams, nameof(templateParams));

            if (attachments != null)
            {
                attachments.ForEach(attachment =>
                {
                    var inlineCidParam = attachment.GetInlineCidParameterName();

                    if (!string.IsNullOrEmpty(inlineCidParam) && !string.IsNullOrEmpty(attachment.ContentId))
                    {
                        AddToDictionay(templateParams, inlineCidParam, attachment.ContentId);
                    }
                });
            }
        }

        /// <summary>
        /// Adds email footer to the template Parameters Dictionary
        /// </summary>
        /// <param name="templateParams">template Parameters Dictionary.</param>
        public void AddEmailFooterToTemplateParamsDictionary(IDictionary<string, string> templateParams)
        {
            Contract.AssertValue(templateParams, nameof(templateParams));

            var footerVariableName = Constants.EmailFooterFieldName;

            AddToDictionay(templateParams, footerVariableName, EmailStrings.EmailMessageTemplateFooter);
        }

        /// <summary>
        /// Helper method to add the key to the dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary value</param>
        /// <param name="paramkey">key of the parameter to add.</param>
        /// <param name="paramValue">value of the parameter to add.</param>
        private static void AddToDictionay(IDictionary<string, string> dictionary, string paramkey, string paramValue)
        {
            Contract.AssertValue(dictionary, nameof(dictionary));
            Contract.AssertNonEmpty(paramkey, nameof(paramkey));
            Contract.AssertValue(paramValue, nameof(paramValue));

            if (!dictionary.ContainsKey(paramkey))
            {
                dictionary.Add(paramkey, paramValue);
            }
            else
            {
                dictionary[paramkey] = paramValue;
            }
        }

        /// <summary>
        /// Sending mail using http client.
        /// </summary>
        /// <param name="subject">Subject of the mail.</param>
        /// <param name="htmlBody">Html body of the email.</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="authHeader">Authentication header. This is used for sender information. With this account send mail</param>
        /// <param name="replyToEmailAddress">Reply To email address list</param>
        /// <param name="ccEmailAddress">CC email address list</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="retryCount">retry count</param>
        /// <param name="bccEmailAddresses">BCC email address list</param>
        /// <returns>HttpStatusCode from the response</returns>
        private async Task<HttpStatusCode> SendGraphMailAsync(
            string subject,
            string htmlBody,
            IList<GraphEmailAddress> toEmailAddresses,
            AuthenticationHeaderValue authHeader,
            IList<GraphEmailAddress> replyToEmailAddress = null,
            IList<GraphEmailAddress> ccEmailAddress = null,
            IList<Attachment> attachments = null,
            int retryCount = 1,
            IList<GraphEmailAddress> bccEmailAddresses = null)
        {

            // If address is not mentioned then send email will fail with Internal server error.
            replyToEmailAddress = replyToEmailAddress?.Where(e => e.emailAddress?.Address != null).ToList();
            ccEmailAddress = ccEmailAddress?.Where(e => e.emailAddress?.Address != null).ToList();
            bccEmailAddresses = bccEmailAddresses?.Where(e => e.emailAddress?.Address != null).ToList();

            this.logger.LogInformation($"validating 'TO' email address list");
            EmailDefinition.ValidateGraphEmailAddresses(toEmailAddresses.ToList());

            if (replyToEmailAddress?.Count > 0)
            {
                this.logger.LogInformation($"validating 'Reply To' email address list");
                EmailDefinition.ValidateGraphEmailAddresses(replyToEmailAddress);
            }
            if (ccEmailAddress?.Count > 0)
            {
                this.logger.LogInformation($"validating 'CC' email address list");
                EmailDefinition.ValidateGraphEmailAddresses(ccEmailAddress);
            }
            if (bccEmailAddresses?.Count > 0)
            {
                this.logger.LogInformation($"validating 'BCC' email address list");
                EmailDefinition.ValidateGraphEmailAddresses(bccEmailAddresses);
            }

            EmailDefinition.ValidateSubject(subject);

            Contract.CheckNonEmpty(htmlBody, nameof(htmlBody), "htmlBody should not be null or empty");

            var fileAttachments = new List<FileAttachment>();
            if (attachments != null && attachments.Count > 0)
            {
                attachments.ForEach(a => fileAttachments.Add(new FileAttachment
                {
                    Base64Content = a.Base64Content,
                    Name = a.Name,
                    Type = a.Type
                }));
            }

            var emialBody = new EmailBody { EmailBodyHtml = htmlBody, ContentType = Constants.EmailContentType };

            var emailData = new EmailMessage
            {
                emailMessage = new EmailContract
                {
                    ToRecipients = toEmailAddresses,
                    ReplyTo = replyToEmailAddress,
                    Body = emialBody,
                    Subject = subject,
                    CCRecipients = ccEmailAddress,
                    BCCRecipients = bccEmailAddresses,
                    FileAttachments = fileAttachments,
                }
            };
            this.logger.LogInformation($"Sending email with Microsoft graph api");

            // Serialize the request object.
            var requestData = JsonConvert.SerializeObject(emailData, this.jsonSerializerSettings);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = authHeader;

                using (var response = await httpClient.PostAsync(Constants.GraphSendMailUrl, new StringContent(requestData, Encoding.UTF8, "application/json")))
                {
                    var statusCode = response.StatusCode;
                    var responseHeaders = response.Headers.ToString();
                    this.logger.LogInformation($"Email send response headers are {responseHeaders}");

                    if (response.IsSuccessStatusCode)
                    {
                        this.logger.LogInformation($"Email using microsoft graph api sent successfully");
                    }
                    else if (EmailDefinition.ShouldRetryOnGraphException(response.StatusCode, retryCount))
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        this.logger.LogWarning($"Send email using microsoft graph api failed, retry {retryCount} StatusCode: {response.StatusCode}, error: {content}");

                        await ExponentialDelay(response, retryCount);

                        await SendGraphMailAsync(subject, htmlBody, toEmailAddresses, authHeader, replyToEmailAddress, ccEmailAddress, attachments, ++retryCount, bccEmailAddresses);
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        throw new GraphEmailException($"Send email using microsoft graph api failed, StatusCode: {response.StatusCode}, error: {content}");
                    }

                    return statusCode;
                }
            }
        }

        /// <summary>
        /// Sending mail using http client.
        /// </summary>
        /// <param name="subject">Subject of the mail.</param>
        /// <param name="htmlBody">Html body of the email.</param>
        /// <param name="toEmailAddresses">To email addresses</param>
        /// <param name="replyToEmailAddress">Reply To email address list</param>
        /// <param name="ccEmailAddress">CC email address list</param>
        /// <param name="attachments">attachments list</param>
        /// <param name="retryCount">retry count</param>
        /// <param name="bccEmailAddresses">BCC email address list</param>
        /// <returns>HttpStatusCode from the response</returns>
        private async Task<HttpStatusCode> SendNotificationAsync(
            string subject,
            string htmlBody,
            IList<GraphEmailAddress> toEmailAddresses,            
            IList<GraphEmailAddress> replyToEmailAddress = null,
            IList<GraphEmailAddress> ccEmailAddress = null,
            IList<Attachment> attachments = null,
            int retryCount = 1,
            IList<GraphEmailAddress> bccEmailAddresses = null)
        {

            // If address is not mentioned then send email will fail with Internal server error.
            replyToEmailAddress = replyToEmailAddress?.Where(e => e.emailAddress?.Address != null).ToList();
            ccEmailAddress = ccEmailAddress?.Where(e => e.emailAddress?.Address != null).ToList();
            bccEmailAddresses = bccEmailAddresses?.Where(e => e.emailAddress?.Address != null).ToList();
            List<NotificationItem> notificationItems = new List<NotificationItem>();

            this.logger.LogInformation($"validating 'TO' email address list");
            EmailDefinition.ValidateGraphEmailAddresses(toEmailAddresses.ToList());

            if (replyToEmailAddress?.Count > 0)
            {
                this.logger.LogInformation($"validating 'Reply To' email address list");
                EmailDefinition.ValidateGraphEmailAddresses(replyToEmailAddress);
            }
            if (ccEmailAddress?.Count > 0)
            {
                this.logger.LogInformation($"validating 'CC' email address list");
                EmailDefinition.ValidateGraphEmailAddresses(ccEmailAddress);
            }
            if (bccEmailAddresses?.Count > 0)
            {
                this.logger.LogInformation($"validating 'BCC' email address list");
                EmailDefinition.ValidateGraphEmailAddresses(bccEmailAddresses);
            }

            EmailDefinition.ValidateSubject(subject);

            Contract.CheckNonEmpty(htmlBody, nameof(htmlBody), "htmlBody should not be null or empty");

            var fileAttachments = new List<NotificationAttachment>();
            if (attachments != null && attachments.Count > 0)
            {
                attachments.ForEach(a => fileAttachments.Add(new NotificationAttachment
                {
                    FileBase64 = a.Base64Content,
                    FileName = a.Name,
                }));
            }

            notificationItems.Add(new NotificationItem()
            {
                To = toEmailAddresses?.StringJoin(";", (ge) => ge.emailAddress.Address),
                ReplyTo = replyToEmailAddress?.StringJoin(";", (ge) => ge.emailAddress.Address),
                Body = htmlBody,
                Subject = subject,                
                CC = ccEmailAddress?.StringJoin(";", (ge) => ge.emailAddress.Address),
                BCC = bccEmailAddresses?.StringJoin(";", (ge) => ge.emailAddress.Address),
                Attachments = fileAttachments,
            });
            this.logger.LogInformation($"Sending email with PAF api");

            var serializedObject = JsonConvert.SerializeObject(notificationItems);
            var stringContent = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var clientId = this.aadConfigurationManager.ClientId;
                var clientSecret = await this.secretManager.GetSecretAsync(this.aadConfigurationManager.ClientCredential, this.logger);

                string authority = String.Format(CultureInfo.InvariantCulture, this.aadConfigurationManager.AADInstance, this.pafConfiguration.Tenant);
                var accessToken = await GetAccessToken(authority, this.pafConfiguration.PAFResource, clientId, clientSecret);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = 
                    await httpClient.PostAsync($"{this.pafConfiguration.PafServiceUrl}/notif/{this.pafConfiguration.PafServiceVersion}/{this.pafConfiguration.PafAppName}/notification/send", 
                    stringContent))
                {
                    var statusCode = response.StatusCode;
                    var responseHeaders = response.Headers.ToString();
                    this.logger.LogInformation($"Email send response headers are {responseHeaders}");

                    if (response.IsSuccessStatusCode)
                    {
                        this.logger.LogInformation($"Email using PAF api sent successfully");
                    }
                    else if (EmailDefinition.ShouldRetryOnPAFException(response.StatusCode, retryCount))
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        this.logger.LogWarning($"Send email using PAF api failed, retry {retryCount} StatusCode: {response.StatusCode}, error: {content}");

                        await ExponentialDelay(response, retryCount);

                        await SendNotificationAsync(subject, htmlBody, toEmailAddresses, replyToEmailAddress, ccEmailAddress, attachments, ++retryCount, bccEmailAddresses);
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        throw new PAFEmailException($"Send email using paf api failed, StatusCode: {response.StatusCode}, error: {content}");
                    }

                    return statusCode;
                }
            }
        }
        
        /// <summary>
        /// Gets the Access token to be used in Authentication Header
        /// </summary>
        /// <param name="authority">Address of the authority to issue token</param>
        /// <param name="resource">Target resource recipient of the token</param>
        /// <param name="clientId">Identifier of the client requesting token</param>
        /// <param name="clientSecret">Secure secret of the client requesting token</param>
        /// <returns></returns>
        private async Task<string> GetAccessToken(string authority, string resource, string clientId, string clientSecret)
        {
            AuthenticationContext authContext = new AuthenticationContext(authority);
            var token = await authContext.AcquireTokenAsync(resource, new ClientCredential(clientId, clientSecret));
            return token.AccessToken;
        }

        /// <summary>
        /// Exponential Delay
        /// </summary>
        /// <param name="response">response</param>
        /// <param name="retryAttempt">retry Attempt</param>
        /// <returns>Task</returns>
        private async Task ExponentialDelay(HttpResponseMessage response, int retryAttempt)
        {
            var delayInSeconds = (1d / 2d) * (Math.Pow(2d, retryAttempt) - 1d);

            var waitTimeSpan = response.Headers?.RetryAfter?.Delta.Value;
            var defaultTimeSpan = TimeSpan.FromSeconds(delayInSeconds);
            if (waitTimeSpan == null || waitTimeSpan > defaultTimeSpan || waitTimeSpan <= TimeSpan.FromSeconds(0))
            {
                waitTimeSpan = defaultTimeSpan;
            }

            this.logger.LogInformation($"Delay thread with {waitTimeSpan ?? defaultTimeSpan} seconds before retry");

            await Task.Delay(waitTimeSpan ?? defaultTimeSpan);

            this.logger.LogInformation($"Processing retry  after {waitTimeSpan ?? defaultTimeSpan} delay");
        }
    }
}
