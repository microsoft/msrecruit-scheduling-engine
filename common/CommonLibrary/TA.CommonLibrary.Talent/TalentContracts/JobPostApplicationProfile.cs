//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using TA.CommonLibrary.TalentJobPosting.Contract;

    /// <summary>The job post application with profile.</summary>
    [DataContract]
    public class JobPostApplicationProfile // TODO : JobPostApplication
    {
        /// <summary>Gets or sets the applicant profile.</summary>
        [DataMember(Name = "applicantProfile", IsRequired = false)]
        public ApplicantProfile ApplicantProfile { get; set; }
    }
}
