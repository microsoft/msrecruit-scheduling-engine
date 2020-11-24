//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ExternalAssessment.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The contract for assessments provided by external assessment providers.
    /// </summary>
    [DataContract]
    public class ExternalAssessment
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the created Date.
        /// </summary>
        [DataMember(Name = "created", IsRequired = false, EmitDefaultValue = false)]
        public string Created { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [DataMember(Name = "title", IsRequired = false, EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the preview url.
        /// </summary>
        [DataMember(Name = "previewUrl", IsRequired = false, EmitDefaultValue = false)]
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Gets or sets the count of questions.
        /// </summary>
        [DataMember(Name = "numberOfQuestions", IsRequired = false, EmitDefaultValue = true)]
        public int NumberOfQuestions { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember(Name = "instructions", IsRequired = false, EmitDefaultValue = true)]
        public AssessmentInstructions Instructions { get; set; }

        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        [DataMember(Name = "questions", IsRequired = false, EmitDefaultValue = true)]
        public IList<AssessmentQuestion> Questions { get; set; }
    }
}
