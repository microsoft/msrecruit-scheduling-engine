//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="DelegationRequest.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.Contracts;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The Delegation request data contract.
    /// </summary>
    [DataContract]
    public class DelegationRequest : TalentBaseContract
    {
        /// <summary>
        /// Get or set delegation ID
        /// </summary>
        [DataMember(Name = "DelegationId", EmitDefaultValue = false, IsRequired = false)]
        public string DelegationId { get; set; }

        /// <summary>
        /// Get and set Delegation Status
        /// </summary>
        [DataMember(Name = "DelegationStatus", EmitDefaultValue = false, IsRequired = false)]
        public DelegationStatus? DelegationStatus { get; set; }

        /// <summary>
        /// Get and set Delegation Status reason
        /// </summary>
        [DataMember(Name = "DelegationStatusReason", EmitDefaultValue = false, IsRequired = false)]
        public string DelegationStatusReason { get; set; }

        /// <summary>
        /// Get and Set Request Status
        /// </summary>
        [DataMember(Name = "RequestStatus", EmitDefaultValue = false, IsRequired = false)]
        public RequestStatus? RequestStatus { get; set; }

        /// <summary>
        /// Get and Set Request Status reason
        /// </summary>
        [DataMember(Name = "RequestStatusReason", EmitDefaultValue = false, IsRequired = false)]
        public string RequestStatusReason { get; set; }

        /// <summary>
        /// Get or set From delegation person
        /// </summary>
        [DataMember(Name = "From", EmitDefaultValue = false, IsRequired = true)]
        public Person From { get; set; }

        /// <summary>
        /// Get or set To delegation person
        /// </summary>
        [DataMember(Name = "To", EmitDefaultValue = false, IsRequired = true)]
        public Person To { get; set; }

        /// <summary>
        /// Get or set RequestedBy
        /// </summary>
        [DataMember(Name = "RequestedBy", EmitDefaultValue = false, IsRequired = true)]
        public Person RequestedBy { get; set; }

        /// <summary>
        /// Get and Set Notes
        /// </summary>
        [DataMember(Name = "Notes", EmitDefaultValue = false, IsRequired = false)]
        public string Notes { get; set; }

        /// <summary>
        /// Get or set delegator from date
        /// </summary>
        [DataMember(Name = "FromDate", EmitDefaultValue = false, IsRequired = true)]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Get or set delegator to date
        /// </summary>
        [DataMember(Name = "ToDate", EmitDefaultValue = false, IsRequired = true)]
        public DateTime? ToDate { get; set; }
    }
}

