// <copyright file="ApplicantsMetadata.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Talent.TalentContracts.InterviewService
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Applicants Metadata contract
    /// </summary>
    [DataContract]
    public class ApplicantsMetadata
    {
        /// <summary>
        /// Gets or sets the user related applicants
        /// </summary>
        [DataMember(Name = "IVApplicants", IsRequired = false, EmitDefaultValue = false)]
        public IList<IVApplicant> IVApplicants { get; set; }

        /// <summary>
        /// Gets or sets total IV applicants count
        /// </summary>
        [DataMember(Name = "Total", IsRequired = false, EmitDefaultValue = false)]
        public int Total { get; set; }
    }
}
