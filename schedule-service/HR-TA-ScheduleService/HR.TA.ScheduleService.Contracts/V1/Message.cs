//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

/*
 *  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
 *  See LICENSE in the source repository root for complete license information.
 */

namespace HR.TA.ScheduleService.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using HR.TA.ScheduleService.Contracts.V1;
    using Newtonsoft.Json;

    /// <summary>
    /// An Outlook mail message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the OData e-tag property.
        /// </summary>
        [JsonProperty(PropertyName = "@odata.etag")]
        public string ODataEtag { get; set; }

        /// <summary>
        /// Gets or sets the OData ID of the resource. This is the same value as the resource property.
        /// </summary>
        [JsonProperty(PropertyName = "@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the created date time
        /// </summary>
        [JsonProperty(PropertyName = "createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the last modified date time
        /// </summary>
        [JsonProperty(PropertyName = "lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the change key
        /// </summary>
        [JsonProperty(PropertyName = "changeKey")]
        public string ChangeKey { get; set; }

        /// <summary>
        /// Gets or sets the array of categories objects
        /// </summary>
        [JsonProperty(PropertyName = "categories")]
        public List<string> Categories { get; set; }

        /// <summary>
        /// Gets or sets the received date time
        /// </summary>
        [JsonProperty(PropertyName = "receivedDateTime")]
        public DateTimeOffset ReceivedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the sent date time
        /// </summary>
        [JsonProperty(PropertyName = "sentDateTime")]
        public DateTimeOffset SentDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message has attachments
        /// </summary>
        [JsonProperty(PropertyName = "hasAttachments")]
        public bool HasAttachments { get; set; }

        /// <summary>
        /// Gets or sets the internet message Id
        /// </summary>
        [JsonProperty(PropertyName = "internetMessageId")]
        public string InternetMessageId { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body
        /// </summary>
        [JsonProperty(PropertyName = "body")]
        public CalendarBody Body { get; set; }

        /// <summary>
        /// Gets or sets the body preview
        /// </summary>
        [JsonProperty(PropertyName = "bodyPreview")]
        public string BodyPreview { get; set; }

        /// <summary>
        /// Gets or sets the importance
        /// </summary>
        [JsonProperty(PropertyName = "importance")]
        public string Importance { get; set; }

        /// <summary>
        /// Gets or sets the parent folder Id
        /// </summary>
        [JsonProperty(PropertyName = "parentFolderId")]
        public string ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the conversation Id
        /// </summary>
        [JsonProperty(PropertyName = "conversationId")]
        public string ConversationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message requests delivery receipt
        /// </summary>
        [JsonProperty(PropertyName = "isDeliveryReceiptRequested", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsDeliveryReceiptRequested { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message requests read receipt
        /// </summary>
        [JsonProperty(PropertyName = "isReadReceiptRequested", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsReadReceiptRequested { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message has been read
        /// </summary>
        [JsonProperty(PropertyName = "IsRead")]
        public bool IsRead { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is draft
        /// </summary>
        [JsonProperty(PropertyName = "isDraft")]
        public bool IsDraft { get; set; }

        /// <summary>
        /// Gets or sets the web link
        /// </summary>
        [JsonProperty(PropertyName = "webLink")]
        public string WebLink { get; set; }

        /// <summary>
        /// Gets or sets the inference classification
        /// </summary>
        [JsonProperty(PropertyName = "inferenceClassification")]
        public string InferenceClassification { get; set; }

        /// <summary>
        /// Gets or sets the meeting message type
        /// </summary>
        [JsonProperty(PropertyName = "meetingMessageType")]
        public string MeetingMessageType { get; set; }

        /// <summary>
        /// Gets or sets the sender
        /// </summary>
        [JsonProperty(PropertyName = "sender")]
        public Organizer Sender { get; set; }

        /// <summary>
        /// Gets or sets the sender the email is from
        /// </summary>
        [JsonProperty(PropertyName = "from")]
        public Organizer From { get; set; }

        /// <summary>
        /// Gets or sets the recipients the message was sent to
        /// </summary>
        [JsonProperty(PropertyName = "toRecipients")]
        public List<Organizer> ToRecipients { get; set; }

        /// <summary>
        /// Gets or sets the ccRecipients the message was sent to
        /// </summary>
        [JsonProperty(PropertyName = "ccRecipients")]
        public List<Organizer> CcRecipients { get; set; }

        /// <summary>
        /// Gets or sets the bccRecipients the message was sent to
        /// </summary>
        [JsonProperty(PropertyName = "bccRecipients")]
        public List<Organizer> BccRecipients { get; set; }

        /// <summary>
        /// Gets or sets the recipients the message was replied to
        /// </summary>
        [JsonProperty(PropertyName = "replyTo")]
        public List<Organizer> ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the event odata context
        /// </summary>
        [JsonProperty(PropertyName = "event@odata.context")]
        public string EventOdataContext { get; set; }

        /// <summary>
        /// Gets or sets calendar event
        /// </summary>
        [JsonProperty(PropertyName = "event")]
        public CalendarEvent Event { get; set; }
    }
}
