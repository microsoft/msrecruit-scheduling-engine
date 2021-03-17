//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The contract for assessments provided by external assessment providers.
    /// </summary>
    [DataContract]
    public class AssessmentQuestion
    {
        /// <summary>
        /// Gets or sets the questionType.
        /// </summary>
        [DataMember(Name = "questionType", IsRequired = false, EmitDefaultValue = false)]
        public int QuestionType { get; set; }
    }
}
