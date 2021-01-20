//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AssessmentQuestion.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
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
