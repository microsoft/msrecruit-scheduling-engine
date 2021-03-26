//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..TalentEntities.Enum;

    /// <summary>
    /// The Applicant Tracking data contract.
    /// </summary>
    [DataContract]
    public class ApplicantTagTracking
    {
        /// <summary>
        /// Gets or sets tracking id.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets tracking owner.
        /// </summary>
        [DataMember(Name = "owner", IsRequired = false, EmitDefaultValue = false)]
        public Person Owner { get; set; }

        /// <summary>
        /// Gets or sets tracking category.
        /// </summary>
        [DataMember(Name = "category", IsRequired = false, EmitDefaultValue = false)]
        public CandidateTrackingCategory Category { get; set; }

        /// <summary>
        /// Gets or sets tracking tags.
        /// </summary>
        [DataMember(Name = "tags", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<ApplicantTag> Tags { get; set; }
    }
}
