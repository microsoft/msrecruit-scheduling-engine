//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Job Approval Process.
    /// </summary>
    [DataContract]
    public class JobApprovalProcess
    {
        /// <summary>
        /// Gets or sets job approval process type.
        /// </summary>
        [DataMember(Name = "jobApprovalProcessType", IsRequired = false, EmitDefaultValue = false)]
        public JobApprovalProcessType JobApprovalProcessType { get; set; }
    }
}
