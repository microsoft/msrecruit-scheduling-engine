//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..Common.TalentAttract.Contract;

    /// <summary>
    /// Job Applications
    /// </summary>
    [DataContract]
    public class JobApplications
    {
        /// <summary>
        /// Gets or sets the collection of applications
        /// </summary>
        [DataMember(Name = "applications", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Application> Applications { get; set; }

        /// <summary>
        /// Gets or sets total application count
        /// </summary>
        [DataMember(Name = "total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets Has Offer Applicant
        /// </summary>
        [DataMember(Name = "hasOfferApplicant", IsRequired = false, EmitDefaultValue = false)]
        public bool HasOfferApplicant { get; set; }
    }
}
