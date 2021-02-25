//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.Common.DocumentDB.Contracts;
    using MS.GTA.Common.TalentEntities.Common;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// Entity to store the delegation request
    /// </summary>
    [DataContract]
    public class Delegation : DocDbEntity
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
        /// Get and Set Request Status Reason
        /// </summary>
        [DataMember(Name = "RequestStatusReason", EmitDefaultValue = false, IsRequired = false)]
        public string RequestStatusReason { get; set; }

        /// <summary>
        /// Get and Set Period
        /// </summary>
        [DataMember(Name = "Period", EmitDefaultValue = false, IsRequired = true)]
        public Period Period { get; set; }

        /// <summary>
        /// Get or set From Delegator
        /// </summary>
        [DataMember(Name = "From", EmitDefaultValue = false, IsRequired = true)]
        public Worker From { get; set; }

        /// <summary>
        /// Get or set To delegator
        /// </summary>
        [DataMember(Name = "To", EmitDefaultValue = false, IsRequired = true)]
        public Worker To { get; set; }

        /// <summary>
        /// Get or set RequestedBy delegation request
        /// </summary>
        [DataMember(Name = "RequestedBy", EmitDefaultValue = false, IsRequired = true)]
        public Worker RequestedBy { get; set; }

        /// <summary>
        /// Get and Set Notes submited by
        /// </summary>
        [DataMember(Name = "Notes", EmitDefaultValue = false, IsRequired = false)]
        public string Notes { get; set; }

        /// <summary>
        /// Get and Set Scope
        /// </summary>
        [DataMember(Name = "Scope", EmitDefaultValue = false, IsRequired = false)]
        public IList<string> Scope { get; set; }
    }
}
