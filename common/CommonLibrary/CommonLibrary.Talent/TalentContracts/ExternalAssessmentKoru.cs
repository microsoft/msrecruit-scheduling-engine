//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The contract for Koru assessment.
    /// </summary>
    [DataContract]
    public class ExternalAssessmentKoru
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [DataMember(Name = "assessmentId", IsRequired = false, EmitDefaultValue = false)]
        public string AssessmentId { get; set; }

        /// <summary>
        /// Gets or sets assessment type.
        /// </summary>
        [DataMember(Name = "assessmentType", IsRequired = false, EmitDefaultValue = false)]
        public string AssessmentType { get; set; }

        /// <summary>
        /// Gets or sets assessment name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Preview url.
        /// </summary>
        [DataMember(Name = "previewUrl", IsRequired = false, EmitDefaultValue = false)]
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Gets or sets Number of questions
        /// </summary>
        [DataMember(Name = "numberOfQuestions", IsRequired = false, EmitDefaultValue = false)]
        public int NumberOfQuestions { get; set; }
    }

    /// <summary>
    /// The contract for Koru assessments.
    /// </summary>
    [DataContract]
    public class KoruAssessments
    {
        /// <summary>
        /// Gets or sets Koru assessments.
        /// </summary>
        [DataMember(Name = "assessments", IsRequired = false, EmitDefaultValue = false)]
        public IList<ExternalAssessmentKoru> Assessments { get; set; }
    }
}
