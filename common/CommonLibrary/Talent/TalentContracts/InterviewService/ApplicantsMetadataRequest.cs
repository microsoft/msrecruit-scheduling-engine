//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Talent.TalentContracts.InterviewService
{
    using System.Runtime.Serialization;
    using TalentEntities.Enum;

    /// <summary>
    /// The applicants metadata request contract.
    /// </summary>
    [DataContract]
    public class ApplicantsMetadataRequest
    {
        /// <summary>
        /// Gets or sets skip
        /// </summary>
        [DataMember(Name = "Skip", IsRequired = false, EmitDefaultValue = false)]
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or sets Take
        /// </summary>
        [DataMember(Name = "Take", IsRequired = false, EmitDefaultValue = false)]
        public int? Take { get; set; }

        /// <summary>
        /// Gets or sets search text
        /// </summary>
        [DataMember(Name = "SearchText", IsRequired = false, EmitDefaultValue = false)]
        public string SearchText { get; set; }

        /// <summary>
        /// Gets or sets filter by stage
        /// </summary>
        [DataMember(Name = "Stage", IsRequired = false, EmitDefaultValue = false)]
        public JobApplicationActivityType? Stage { get; set; }
    }
}
