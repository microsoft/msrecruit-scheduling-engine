//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="InterviewDetails.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Talent.TalentContracts.InterviewService
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The interview details data contract.
    /// </summary>
    [DataContract]
    public class InterviewDetails
    {
        /// <summary>Gets or sets job application id.</summary>
        [DataMember(Name = "JobApplicationID", EmitDefaultValue = false, IsRequired = true)]
        public string JobApplicationID { get; set; }

        /// <summary>Gets or sets the name of the candidate.</summary>
        [DataMember(Name = "CandidateName", EmitDefaultValue = false, IsRequired = false)]
        public string CandidateName { get; set; }

        /// <summary>Gets or sets the job title.</summary>
        [DataMember(Name = "PositionTitle", EmitDefaultValue = false, IsRequired = false)]
        public string PositionTitle { get; set; }

        /// <summary>Gets or sets the interview schedules for day.</summary>
        [DataMember(Name = "SchedulesForDay", EmitDefaultValue = false, IsRequired = false)]
        public IList<InterviewSchedule> SchedulesForDay { get; set; }
    }
}