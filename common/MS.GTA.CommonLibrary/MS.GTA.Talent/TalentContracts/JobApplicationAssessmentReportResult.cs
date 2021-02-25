//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    
    /// <summary>The job application assessment report result.</summary>
    [DataContract]
    public class JobApplicationAssessmentReportResult
    {
        /// <summary>Gets or sets the score type.</summary>
        [DataMember(Name = nameof(ScoreType))]
        public string ScoreType { get; set; }

        /// <summary>Gets or sets the score value.</summary>
        [DataMember(Name = nameof(ScoreValue))]
        public string ScoreValue { get; set; }

        /// <summary>Gets or sets the result subject.</summary>
        [DataMember(Name = nameof(ResultSubject))]
        public string ResultSubject { get; set; }

        /// <summary>Gets or sets the additional information.</summary>
        [DataMember(Name = nameof(AdditionalInformation))]
        public string AdditionalInformation { get; set; }

        /// <summary>Gets or sets the additional result data.</summary>
        [DataMember(Name = nameof(AdditionalResultData))]
        public string AdditionalResultData { get; set; }
    }
}