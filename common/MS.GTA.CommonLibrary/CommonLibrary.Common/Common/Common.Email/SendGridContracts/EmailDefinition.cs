//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Email.SendGridContracts
{    
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;    
    using Base.Utilities;
    using CommonDataService.Common.Internal;
    using CommonLibrary.Common.Email.GraphContracts;
    using Newtonsoft.Json;
    using Constants = Email.Constants;

    /// <summary>
    /// Class to create the EmailHttpRequestBody
    /// </summary>
    internal class EmailDefinition
    {
        /// <summary>
        /// Gets or sets the emailAddress from where the email is sent
        /// </summary>
        [JsonProperty("from", Order = 10)]
        public EmailAddress FromEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the emailAddress to reply to.
        /// </summary>
        [JsonProperty("reply_to", Order = 15, NullValueHandling = NullValueHandling.Ignore)]
        public EmailAddress ReplyToEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        [JsonProperty("subject", Order = 20)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets the Content of the email in html format.
        /// </summary>
        [JsonProperty("personalizations", Order = 30)]
        public ReadOnlyCollection<Personalization> Personalizations
        {
            get
            {
                var privateList = new List<Personalization>();
                if (this.Personalize)
                {
                    foreach (var emailAddress in this.ToEmailAddresses)
                    {
                        privateList.Add(new Personalization { ToEmailAddresses = new List<EmailAddress> { emailAddress } });
                    }
                }
                else
                {
                    privateList.Add(new Personalization { ToEmailAddresses = this.ToEmailAddresses });
                }

                return privateList.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the Content of the email in html format.
        /// </summary>
        [JsonProperty("content", Order = 40)]
        public ReadOnlyCollection<Content> Content
        {
            get
            {
                var privateList = new List<Content> { new Content { Type = "text/html", HtmlBody = this.EmailBodyHtml } };
                return privateList.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the Content of the email in html format.
        /// </summary>
        [JsonProperty("attachments", Order = 50, NullValueHandling = NullValueHandling.Ignore)]
        public ReadOnlyCollection<Attachment> AttachmentList
        {
            get
            {
                return this.Attachments?.ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the headers of the email in html format.
        /// </summary>
        [JsonProperty("headers", Order = 60, NullValueHandling = NullValueHandling.Ignore)]
        public ReadOnlyDictionary<string, string> HeaderDictionary
        {
            get
            {
                return this.Headers != null ? new ReadOnlyDictionary<string, string>(this.Headers) : null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this property is true or false. False, if email recipients should be able to see each others.
        /// </summary>
        public bool Personalize { private get; set; }

        /// <summary>
        /// Gets or sets the emailAddress where the email is sent to
        /// </summary>
        public IList<EmailAddress> ToEmailAddresses { private get; set; }

        /// <summary>
        /// Gets or sets the Attachments for the email.
        /// </summary>
        public IList<Attachment> Attachments { private get; set; }

        /// <summary>
        /// Gets or sets the Headers for the email.
        /// </summary>
        public IDictionary<string, string> Headers { private get; set; }

        /// <summary>
        /// Gets or sets the body of the email in html format.
        /// </summary>
        public string EmailBodyHtml { private get; set; }

        /// <summary>
        /// Validates the attachments.
        /// </summary>
        /// <param name="attachments">List of Attachments</param>
        internal static void ValidateAttachments(IList<Attachment> attachments)
        {
            Contract.Check(
                attachments == null || attachments.Count <= Constants.EmailLimitOnNumberOfAttachments,
                $"One can't send more than {Constants.EmailLimitOnNumberOfAttachments} attachments.");

            attachments.ForEach(attachment => Contract.Check(
                !string.IsNullOrEmpty(attachment.Base64Content) && attachment.Base64Content.Length <= Constants.EmailLimitOnSizeOfAttachmentsInKB * Constants.KiloBytes,
                "attachment is not as expected"));
        }

        /// <summary>
        /// Validates the headers.
        /// </summary>
        /// <param name="headers">Dictionary of headers</param>
        internal static void ValidateHeaders(IDictionary<string, string> headers)
        {
            Contract.Check(
                headers == null || headers.Count <= Constants.EmailLimitOnNumberOfHeaders,
                $"One can't send more than {Constants.EmailLimitOnNumberOfHeaders} headers.");
        }

        /// <summary>
        /// Validates the Email subject.
        /// </summary>
        /// <param name="subject">email subject</param>
        internal static void ValidateSubject(string subject)
        {
            Contract.CheckNonEmpty(subject, nameof(subject), "subject should not be null or empty");

            Contract.Check(subject.Length <= Constants.EmailSubjectLengthLimitInCharacters, $"Subject has more than {Constants.EmailSubjectLengthLimitInCharacters} chars.");
        }

        /// <summary>
        /// Validates the Email message.
        /// </summary>
        /// <param name="htmlBody">email message body</param>
        internal static void ValidateMessage(string htmlBody)
        {
            Contract.CheckNonEmpty(htmlBody, nameof(htmlBody), "htmlBody should not be null or empty");

            Contract.Check(htmlBody.Length <= Constants.EmailMessageLengthLimitInCharacters, $"Message has more than {Constants.EmailMessageLengthLimitInCharacters} chars.");
        }

        /// <summary>
        /// Validates the To Email addresses.
        /// </summary>
        /// <param name="toEmailAddresses">Email Addresses to validate.</param>
        internal static void ValidateToEmailAddresses(IList<EmailAddress> toEmailAddresses)
        {
            Contract.CheckNonEmpty(toEmailAddresses, nameof(toEmailAddresses));
            Contract.CheckAllValues(toEmailAddresses, nameof(toEmailAddresses));

            Contract.Check(toEmailAddresses.Count <= Constants.EmailLimitOnToAddresses, $"One can't send email to more than {Constants.EmailLimitOnToAddresses} email addresses.");

            toEmailAddresses.ForEach(toEmailAddress => Contract.Check(
                !string.IsNullOrEmpty(toEmailAddress.Email) && toEmailAddress.Email.Contains(Constants.EmailAtCharacter),
                "emailAddress is not as expected"));
        }

        /// <summary>
        /// Validates the To Email addresses.
        /// </summary>
        /// <param name="graphEmailAddresses">Email Address list to validate.</param>
        internal static void ValidateGraphEmailAddresses(IList<GraphEmailAddress> graphEmailAddresses)
        {
            Contract.CheckNonEmpty(graphEmailAddresses, nameof(graphEmailAddresses));
            Contract.CheckAllValues(graphEmailAddresses, nameof(graphEmailAddresses));

            Contract.Check(graphEmailAddresses.Count <= Constants.EmailLimitOnToAddresses, $"One can't send email to more than {Constants.EmailLimitOnToAddresses} email addresses.");

            graphEmailAddresses.ForEach(emailAddress => Contract.Check(
                !string.IsNullOrEmpty(emailAddress.emailAddress.Address) && emailAddress.emailAddress.Address.Contains(Constants.EmailAtCharacter),
                "emailAddress is not as expected"));
        }

        /// <summary>
        /// Check for retry needed on Graph Exception
        /// </summary>
        /// <param name="statusCode">status Code</param>
        /// <param name="retryCount">retry Count</param>
        /// <returns>Should Retry On GraphException result</returns>
        internal static bool ShouldRetryOnGraphException(HttpStatusCode statusCode, int retryCount)
        {
            return (statusCode == HttpStatusCode.ServiceUnavailable
                        || (int)statusCode == Constants.TooManyRequestCode
                        || statusCode == HttpStatusCode.GatewayTimeout) && retryCount < Constants.MaxRetryCount;
        }

        /// <summary>
        /// Check for retry needed on PAF API Exception
        /// </summary>
        /// <param name="statusCode">status Code</param>
        /// <param name="retryCount">retry Count</param>
        /// <returns>Should Retry On GraphException result</returns>
        internal static bool ShouldRetryOnPAFException(HttpStatusCode statusCode, int retryCount)
        {
            return (statusCode == HttpStatusCode.ServiceUnavailable
                        || (int)statusCode == Constants.TooManyRequestCode
                        || statusCode == HttpStatusCode.GatewayTimeout) && retryCount < Constants.MaxRetryCount;
        }
    }
}
