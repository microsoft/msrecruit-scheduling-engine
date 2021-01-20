//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobApprovalParticipant.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The hiring team member data contract.
    /// </summary>
    [DataContract]
    public class JobApprovalParticipant : Person
    {
        /// <summary>
        /// Gets or sets comment.
        /// </summary>
        [DataMember(Name = "comment", EmitDefaultValue = false, IsRequired = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets Job approval status.
        /// </summary>
        [DataMember(Name = "jobApprovalStatus", EmitDefaultValue = false, IsRequired = false)]
        public JobApprovalStatus? JobApprovalStatus { get; set; }

        /// <summary>
        /// Gets or sets User action.
        /// </summary>
        [DataMember(Name = "userAction", IsRequired = false, EmitDefaultValue = false)]
        public UserAction UserAction { get; set; }
    }
}
