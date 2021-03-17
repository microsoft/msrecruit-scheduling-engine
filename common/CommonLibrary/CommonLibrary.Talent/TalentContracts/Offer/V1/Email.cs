//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Email properties
    /// </summary>
    [DataContract]
    public class Email
    {
        /// <summary>
        /// Gets or sets from address.
        /// </summary>
        [DataMember(Name = "fromAddress", IsRequired = false)]
        public EmailAddress FromAddress { get; set; }

        /// <summary>
        /// Gets or sets Reply To Address.
        /// </summary>
        [DataMember(Name = "replyToAddress", IsRequired = false)]
        public EmailAddress ReplyToAddress { get; set; }

        /// <summary>
        /// Gets or sets To Address.
        /// </summary>
        [DataMember(Name = "toAddress", IsRequired = false)]
        public List<EmailAddress> ToAddress { get; set; }

        /// <summary>
        /// Gets or sets To Address Name.
        /// </summary>
        [DataMember(Name = "toAddresName", IsRequired = false)]
        public string ToAddressName { get; set; }

        /// <summary>
        /// Gets or sets Company Info Footer.
        /// </summary>
        [DataMember(Name = "companyInfoFooter", IsRequired = false)]
        public string CompanyInfoFooter { get; set; }

        /// <summary>
        /// Gets or sets Company Name.
        /// </summary>
        [DataMember(Name = "companyName", IsRequired = false)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets Recuiter Name.
        /// </summary>
        [DataMember(Name = "recuiterName", IsRequired = false)]
        public string RecuiterName { get; set; }

        /// <summary>
        /// Gets or sets Recuiter Name.
        /// </summary>
        [IgnoreDataMember]
        public string RecuiterEmail { get; set; }

        /// <summary>
        /// Gets or sets email subject.
        /// </summary>
        [DataMember(Name = "messageSubject", IsRequired = true)]
        public string MessageSubject { get; set; }

        /// <summary>
        /// Gets or sets Greeting.
        /// </summary>
        [DataMember(Name = "greeting", IsRequired = false)]
        public string Greeting { get; set; }

        /// <summary>
        /// Gets or sets email closing.
        /// </summary>
        [DataMember(Name = "closing", IsRequired = false)]
        public string Closing { get; set; }

        /// <summary>
        /// Gets or sets paragraph one.
        /// </summary>
        [DataMember(Name = "paragraph1", IsRequired = true)]
        public string Paragraph1 { get; set; }

        /// <summary>
        /// Gets or sets paragraph two.
        /// </summary>
        [DataMember(Name = "paragraph2", IsRequired = false)]
        public string Paragraph2 { get; set; }

        /// <summary>
        /// Gets or sets Offer Management App Link.
        /// </summary>
        [DataMember(Name = "OfferManagementAppLink", IsRequired = false)]
        public string OfferManagementAppLink { get; set; }

        /// <summary>
        /// Gets or sets email header image url.
        /// </summary>
        [DataMember(Name = "emailHeaderUrl", IsRequired = false)]
        public string EmailHeaderUrl { get; set; }

        /// <summary>
        /// Gets or sets email header image height.
        /// </summary>
        [DataMember(Name = "emailHeaderHeight", IsRequired = false)]
        public string EmailHeaderHeight { get; set; }

        /// <summary>
        /// Gets or sets Job Title.
        /// </summary>
        [DataMember(Name = "jobTitle", IsRequired = false)]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets Job Title.
        /// </summary>
        [DataMember(Name = "buttonText", IsRequired = false)]
        public string ButtonText { get; set; }

        /// <summary>
        /// Gets or sets the cc addresses.
        /// </summary>
        [DataMember(Name = "ccAddresses", IsRequired = false)]
        public List<string> CCAddresses { get; set; }

        /// <summary>
        /// Gets or sets the bcc addresses.
        /// </summary>
        [DataMember(Name = "bccAddresses", IsRequired = false)]
        public List<string> BCCAddresses { get; set; }
    }
}
