//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using HR.TA..TalentJobPosting.Contract;

    /// <summary>The job post application with profile.</summary>
    [DataContract]
    public class JobPostApplicationProfile // TODO : JobPostApplication
    {
        /// <summary>Gets or sets the applicant profile.</summary>
        [DataMember(Name = "applicantProfile", IsRequired = false)]
        public ApplicantProfile ApplicantProfile { get; set; }
    }
}