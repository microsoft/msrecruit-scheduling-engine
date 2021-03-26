//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.V2
{
    using System;
    using System.Runtime.Serialization;
    using HR.TA..Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// The Offer participant feedback.
    /// </summary>
    [DataContract]
    public class OfferFeedback
    {
        /// <summary>
        /// Gets or sets Comment.
        /// </summary>
        [DataMember(Name = "comment", IsRequired = false, EmitDefaultValue = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets OfferParticipant's Status.
        /// </summary>
        [DataMember(Name = "status", IsRequired = false, EmitDefaultValue = false)]
        public OfferParticipantStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets OfferParticipant's Status Reason.
        /// </summary>
        [DataMember(Name = "statusReason", IsRequired = false, EmitDefaultValue = false)]
        public OfferParticipantStatusReason? StatusReason { get; set; }

        /// <summary>
        /// Gets or sets the feedback requested date.
        /// </summary>
        [DataMember(Name = "requestDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// Gets or sets feedback responded date.
        /// </summary>
        [DataMember(Name = "respondDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? RespondDate { get; set; }

        /// <summary>
        /// Gets or sets whether feedback was submitted.
        /// </summary>
        [DataMember(Name = "isSubmitted", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsSubmitted { get; set; }

        /// <summary>
        /// Gets or sets whether feedback was submitted.
        /// </summary>
        [DataMember(Name = "submittedBy", IsRequired = false, EmitDefaultValue = false)]
        public string SubmittedBy { get; set; }
    }
}
