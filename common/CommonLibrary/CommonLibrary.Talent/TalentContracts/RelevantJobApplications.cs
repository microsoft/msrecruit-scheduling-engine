//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.TalentAttract.Contract;
  

    /// <summary>
    /// Relevant Job Applications
    /// </summary>
    [DataContract]
    public class RelevantJobApplications
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
        /// Gets or sets score
        /// </summary>
        [DataMember(Name = "score", IsRequired = false, EmitDefaultValue = false)]
        public double Score { get; set; }

        /// <summary>
        /// Gets or sets last updated by
        /// </summary>
        [DataMember(Name = "lastUpdatedBy", IsRequired = false, EmitDefaultValue = false)]
        public DateTime LastUpdatedBy { get; set; }
    }
}
