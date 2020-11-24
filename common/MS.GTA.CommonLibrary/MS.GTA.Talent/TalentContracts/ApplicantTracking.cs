//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ApplicantTagTracking.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using MS.GTA.TalentEntities.Enum;

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
