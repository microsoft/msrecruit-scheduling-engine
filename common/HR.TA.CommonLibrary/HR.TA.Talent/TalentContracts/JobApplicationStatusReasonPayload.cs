//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using HR.TA.TalentEntities.Enum;

    /// <summary>
    /// The client data contract with the data to show the dashboard page.
    /// </summary>
    [DataContract]
    public class JobApplicationStatusReasonPayload
    {
        /// <summary>
        /// Gets or sets the job application status reason
        /// </summary>
        [DataMember(Name = "StatusReason", IsRequired = false)]
        public JobApplicationStatusReason StatusReason { get; set; }

        /// <summary>
        /// Gets or sets the job application rejection reason
        /// </summary>
        [DataMember(Name = "RejectionReason", IsRequired = false)]
        public OptionSetValue RejectionReason { get; set; }

        /// <summary>
        /// Gets or sets the job application comment
        /// </summary>
        [DataMember(Name = "Comment", IsRequired = false)]
        public string Comment { get; set; }
    }
}
