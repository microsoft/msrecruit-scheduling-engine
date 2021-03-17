//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Gets the Extend offer details.
    /// </summary>
    [DataContract]
    public class ExtendOfferDetails
    {
        /// <summary>
        /// Gets or sets additional Notes
        /// </summary>
        [DataMember(Name = "additionalNotes", IsRequired = false, EmitDefaultValue = false)]
        public string AdditionalNotes { get; set; }

        /// <summary>
        /// Gets or sets whether signed document is required or not
        /// </summary>
        [DataMember(Name = "isSignedDocumentRequired", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsSignedDocumentRequired { get; set; }

        /// <summary>
        /// Gets or sets whether Adobe Sign is required or not
        /// </summary>
        [DataMember(Name = "isAdobeSignRequired", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsAdobeSignRequired { get; set; }

        /// <summary>
        /// Gets or sets whether DocuSign is required or not
        /// </summary>
        [DataMember(Name = "isDocuSignRequired", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsDocuSignRequired { get; set; }

        /// <summary>
        /// Gets or sets expiration date
        /// </summary>
        [DataMember(Name = "expirationDate", IsRequired = false, EmitDefaultValue = true)]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets email subject
        /// </summary>
        [DataMember(Name = "emailSubject", IsRequired = false, EmitDefaultValue = false)]
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets email body
        /// </summary>
        [DataMember(Name = "emailBody", IsRequired = false, EmitDefaultValue = false)]
        public string EmailBody { get; set; }

        /// <summary>
        /// Gets or sets required documents
        /// </summary>
        [DataMember(Name = "requiredCandidateDocuments", IsRequired = false, EmitDefaultValue = false)]
        public List<string> RequiredCandidateDocuments { get; set; }

        /// <summary>
        /// Gets or sets required documents
        /// </summary>
        [DataMember(Name = "ccEmailAddresses", IsRequired = false, EmitDefaultValue = false)]
        public List<string> CCEmailAddresses { get; set; }
    }
}
