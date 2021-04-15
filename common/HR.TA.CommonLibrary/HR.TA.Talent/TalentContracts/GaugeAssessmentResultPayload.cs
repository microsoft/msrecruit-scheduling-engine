//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The assessment result as received from the Gauge external assessment provider.
    /// </summary>
    [DataContract]
    public class GaugeAssessmentResultPayload
    {
        /// <summary>
        /// Gets or sets the raw assessment score percentage.
        /// </summary>
        [DataMember(Name = "percentage", IsRequired = true, EmitDefaultValue = false)]
        public int Percentage { get; set; }

        /// <summary>
        /// Gets or sets the assessment status
        /// </summary>
        [DataMember(Name = "status", IsRequired = false, EmitDefaultValue = true)]
        public GaugeAssessmentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the assessment subject (ex: skill, personality, etc.)
        /// </summary>
        [DataMember(Name = "subject", IsRequired = false, EmitDefaultValue = true)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the assessment grade url
        /// </summary>
        [DataMember(Name = "gradeUrl", IsRequired = false, EmitDefaultValue = true)]
        public string GradeUrl { get; set; }

        /// <summary>
        /// Gets or sets the assessment subject (ex: skill, personality, etc.)
        /// </summary>
        [DataMember(Name = "detailUrl", IsRequired = false, EmitDefaultValue = true)]
        public string DetailUrl { get; set; }

        /// <summary>
        /// Gets or sets the assessment additional information
        /// </summary>
        [DataMember(Name = "additionalInformation", IsRequired = false, EmitDefaultValue = true)]
        public string AdditionalInformation { get; set; }
    }
}
