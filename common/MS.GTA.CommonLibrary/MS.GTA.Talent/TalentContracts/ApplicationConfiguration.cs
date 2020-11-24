//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ApplicationConfiguration.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Configuration for Assessment activity.
    /// </summary>
    [DataContract]
    public class ApplicationConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether to send mail to candidate when candidate is added to job application.
        /// </summary>
        [DataMember(Name = "sendMailToCandidate", IsRequired = false, EmitDefaultValue = false)]
        public bool SendMailToCandidate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether resume is required for an applicant to apply for a job.
        /// </summary>
        [DataMember(Name = "enforceApplicantForResume", IsRequired = false, EmitDefaultValue = false)]
        public bool EnforceApplicantForResume { get; set; }
    }
}
