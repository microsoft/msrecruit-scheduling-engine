//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AssessmentResult.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    public class AssessmentResult
    {
        public string AdditionalInformation { get; set; }

        public AssessmentReport AssessmentReport { get; set; }

        public string AssessmentResultID { get; set; }

        public string AssessmentReportID { get; set; }

        public string ResultSubject { get; set; }

        public string ScoreType { get; set; }

        public string ScoreValue { get; set; }

        public string AdditionalResultData { get; set; }
    }
}
