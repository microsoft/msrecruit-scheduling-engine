//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Email.GraphContracts
{
    using CommonLibrary.Common.Email.SendGridContracts;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class EmailContract
    {
        /// <summary>
        /// Email body
        /// </summary>
        [JsonProperty("body")]
        public EmailBody Body { get; set; }

        /// <summary>
        /// From email
        /// </summary>
        [JsonProperty("from")]
        public GraphEmailAddress From { get; set; }

        /// <summary>
        /// Reply to email
        /// </summary>
        [JsonProperty("replyTo")]
        public IList<GraphEmailAddress> ReplyTo { get; set; }

        /// <summary>
        /// Primary email recipients
        /// </summary>
        [JsonProperty("toRecipients")]
        public IList<GraphEmailAddress> ToRecipients { get; set; }

        /// <summary>
        /// Copied email recipients
        /// </summary>
        [JsonProperty("ccRecipients")]
        public IList<GraphEmailAddress> CCRecipients { get; set; }

        /// <summary>
        /// Blind copy email recipients
        /// </summary>
        [JsonProperty("bccRecipients")]
        public IList<GraphEmailAddress> BCCRecipients { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the attachments of the email.
        /// </summary>
        [JsonProperty("attachments")]
        public IList<FileAttachment> FileAttachments { get; set; }
    }

    public class EmailMessage
    {
        /// <summary>
        /// Gets or sets the graph email message.
        /// </summary>
        [JsonProperty("message")]
        public EmailContract emailMessage { get; set; }
    }
}
