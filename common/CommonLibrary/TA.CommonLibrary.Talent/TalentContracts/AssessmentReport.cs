//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using TA.CommonLibrary.TalentEntities.Enum;

    public class AssessmentReport
    {
        public string AdditionalInformation { get; set; }

        public string AssessmentReportID { get; set; }

        public string ExternalAssessmentReportID { get; set; }

        public string AssessmentStatus { get; set; }

        public string AssessmentType { get; set; }

        public string AssessmentURL { get; set; }

        public AssessmentStatus ApplicantAssessmentStatus { get; set; }

        public string Comment { get; set; }

        public DateTime? DateCompleted { get; set; }

        public AssessmentReportType ReportType { get; set; }

        public string ReportURL { get; set; }

        public IList<AssessmentResult> AssessmentResults { get; set; }
    }
}
