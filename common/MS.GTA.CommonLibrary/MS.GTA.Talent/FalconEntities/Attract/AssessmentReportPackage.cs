//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    [DataContract]
    public class AssessmentReportPackage
    {
        [DataMember(Name = "AssessmentReportPackageID", EmitDefaultValue = false, IsRequired = false)]
        public string AssessmentReportPackageID { get; set; }

        [DataMember(Name = "PackageID", EmitDefaultValue = false, IsRequired = false)]
        public string PackageID { get; set; }

        [DataMember(Name = "OrderID", EmitDefaultValue = false, IsRequired = false)]
        public string OrderID { get; set; }

        [DataMember(Name = "ReceiptID", EmitDefaultValue = false, IsRequired = false)]
        public string ReceiptID { get; set; }

        [DataMember(Name = "ClientID", EmitDefaultValue = false, IsRequired = false)]
        public string ClientID { get; set; }

        [DataMember(Name = "Requester", EmitDefaultValue = false, IsRequired = false)]
        public Worker Requester { get; set; }

        [DataMember(Name = "AssessmentSubject", EmitDefaultValue = false, IsRequired = false)]
        public Candidate AssessmentSubject { get; set; }

        [DataMember(Name = "Provider", EmitDefaultValue = false, IsRequired = false)]
        public AssessmentProvider? Provider { get; set; }

        [DataMember(Name = "ProviderKey", EmitDefaultValue = false, IsRequired = false)]
        public string ProviderKey { get; set; }

        [DataMember(Name = "PackageStatus", EmitDefaultValue = false, IsRequired = false)]
        public string PackageStatus { get; set; }

        [DataMember(Name = "PackageTitle", EmitDefaultValue = false, IsRequired = false)]
        public string PackageTitle { get; set; }

        [DataMember(Name = "Comment", EmitDefaultValue = false, IsRequired = false)]
        public string Comment { get; set; }

        [DataMember(Name = "AdditionalInformation", EmitDefaultValue = false, IsRequired = false)]
        public string AdditionalInformation { get; set; }

        [DataMember(Name = "DateOrdered", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? DateOrdered { get; set; }

        [DataMember(Name = "DateCompleted", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? DateCompleted { get; set; }

        [DataMember(Name = "AssessmentReports", EmitDefaultValue = false, IsRequired = false)]
        public IList<AssessmentReport> AssessmentReports { get; set; }

        [DataMember(Name = "JobApplicationAssessments", EmitDefaultValue = false, IsRequired = false)]
        public IList<JobApplicationAssessment> JobApplicationAssessments { get; set; }
    }
}
