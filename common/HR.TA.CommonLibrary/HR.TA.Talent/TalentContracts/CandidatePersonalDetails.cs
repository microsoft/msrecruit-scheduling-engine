﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;
    using HR.TA.Common.Contracts;
    using HR.TA.TalentEntities.Enum;

    /// <summary>
    /// Candidate personal details data contract.
    /// </summary>
    [DataContract]
    public class CandidatePersonalDetails : TalentBaseContract
    {
        [DataMember(Name = "Gender", EmitDefaultValue = false, IsRequired = false)]
        public CandidateGender? Gender { get; set; }

        [DataMember(Name = "Ethnicity", EmitDefaultValue = false, IsRequired = false)]
        public CandidateEthnicOrigin? Ethnicity { get; set; }

        [DataMember(Name = "DisabilityStatus", EmitDefaultValue = false, IsRequired = false)]
        public CandidateDisabilityStatus? DisabilityStatus { get; set; }

        [DataMember(Name = "VeteranStatus", EmitDefaultValue = false, IsRequired = false)]
        public CandidateVeteranStatus? VeteranStatus { get; set; }

        [DataMember(Name = "MilitaryStatus", EmitDefaultValue = false, IsRequired = false)]
        public CandidateMilitaryStatus? MilitaryStatus { get; set; }

        [DataMember(Name = "submittedOn", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? SubmittedOn { get; set; }

        [DataMember(Name = "jobApplicationActivityId", EmitDefaultValue = false, IsRequired = false)]
        public string JobApplicationActivityId { get; set; }
    }
}
