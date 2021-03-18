//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Configuration for Job.
    /// </summary>
    [DataContract]
    public class JobConfiguration
    {
        /// <summary>
        /// Gets or sets job approval process type.
        /// </summary>
        [DataMember(Name = "jobApprovalProcess", IsRequired = false, EmitDefaultValue = false)]
        public JobApprovalProcess JobApprovalProcess { get; set; }
    }
}
