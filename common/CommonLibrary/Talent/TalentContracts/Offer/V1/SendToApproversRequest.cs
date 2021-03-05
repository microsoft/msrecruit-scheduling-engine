//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Gets the send to approver details.
    /// </summary>
    [DataContract]
    public class SendToApproversRequest
    {
        /// <summary>
        /// Gets or sets non standard reason
        /// </summary>
        [DataMember(Name = "nonStandardReason", IsRequired = false, EmitDefaultValue = false)]
        public string NonStandardReason { get; set; }

        /// <summary>
        /// Gets or sets whether owner should be copied in the email
        /// </summary>
        [DataMember(Name = "copyToOwner", IsRequired = false, EmitDefaultValue = false)]
        public bool? CopyToOwner { get; set; }

        /// <summary>
        /// Gets or sets Email contents
        /// </summary>
        [DataMember(Name = "email", IsRequired = false, EmitDefaultValue = false)]
        public Email Email { get; set; }
    }
}
