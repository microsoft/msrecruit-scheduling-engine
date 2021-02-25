//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

    /// <summary>
    /// The client data contract with the data to show the dashboard page.
    /// </summary>
    [DataContract]
    public class JobApprovalPayload
    {
        /// <summary>
        /// Gets or sets the job application status reason
        /// </summary>
        [DataMember(Name = "jobApprovalStatus", IsRequired = false, EmitDefaultValue = false)]
        public JobApprovalStatus JobApprovalStatus { get; set; }

        /// <summary>
        /// Gets or sets the job application comment
        /// </summary>
        [DataMember(Name = "comment", IsRequired = false, EmitDefaultValue = false)]
        public string Comment { get; set; }
    }
}
