//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Specifies the Data contract for Withdraw offer
    /// </summary>
    [DataContract]
    public class WithdrawOffer
    {
        /// <summary>
        /// Gets or sets offer withdraw reason
        /// </summary>
        [DataMember(Name = "reason", IsRequired = true, EmitDefaultValue = false)]
        public OfferStatusReason? Reason { get; set; }

        /// <summary>
        /// Gets or sets withdraw comment.
        /// </summary>
        [DataMember(Name = "comment", IsRequired = false, EmitDefaultValue = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets withdraw timestamp.
        /// </summary>
        [DataMember(Name = "timestamp", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// Gets or sets candidate notification if required
        /// </summary>
        [DataMember(Name = "isCandidateNotificationRequired", IsRequired = true, EmitDefaultValue = false)]
        public bool? IsCandidateNotificationRequired { get; set; }

        /// <summary>
        /// Gets or sets candidate notification subject
        /// </summary>
        [DataMember(Name = "candidateNotificationSubject", IsRequired = false, EmitDefaultValue = false)]
        public string CandidateNotificationSubject { get; set; }

        /// <summary>
        /// Gets or sets candidate notification body
        /// </summary>
        [DataMember(Name = "candidateNotificationBody", IsRequired = false, EmitDefaultValue = false)]
        public string CandidateNotificationBody { get; set; }

        /// <summary>
        /// Gets or sets the email details.
        /// </summary>
        [DataMember(Name = "emailDetails", IsRequired = false, EmitDefaultValue = false)]
        public Email EmailDetails { get; set; }
    }
}
