//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobConfiguration.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
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
