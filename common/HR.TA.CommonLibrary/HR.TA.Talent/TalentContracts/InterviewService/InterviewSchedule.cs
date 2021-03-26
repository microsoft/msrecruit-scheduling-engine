//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.TalentContracts.InterviewService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.Common.TalentAttract.Contract;
    using HR.TA.Talent.EnumSetModel;

    /// <summary>
    /// The interview schedule data contract.
    /// </summary>
    [DataContract]
    public class InterviewSchedule
    {
        /// <summary>Gets or sets interview start date and time.</summary>
        [DataMember(Name = "InterviewStartDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? InterviewStartDateTime { get; set; }

        /// <summary>Gets or sets interview end date and time.</summary>
        [DataMember(Name = "InterviewEndDateTime", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? InterviewEndDateTime { get; set; }

        /// <summary>Gets or sets the list of interviewers.</summary>
        [DataMember(Name = "Interviewers", EmitDefaultValue = false, IsRequired = false)]
        public IList<Interviewer> Interviewers { get; set; }

        /// <summary>Gets or sets the mode of interview.</summary>
        [DataMember(Name = "InterviewMode", EmitDefaultValue = false, IsRequired = false)]
        public InterviewMode? InterviewMode { get; set; }

        /// <summary>Gets or sets the schedule status of interview.</summary>
        [DataMember(Name = "InterviewScheduleStatus", EmitDefaultValue = false, IsRequired = false)]
        public ScheduleStatus? InterviewScheduleStatus { get; set; }

        /// <summary>Gets or sets the location of the interview or teams meeting link if online.</summary>
        [DataMember(Name = "InterviewLocationOrTeamsMeetingLink", EmitDefaultValue = false, IsRequired = false)]
        public string InterviewLocationOrTeamsMeetingLink { get; set; }
    }
}
