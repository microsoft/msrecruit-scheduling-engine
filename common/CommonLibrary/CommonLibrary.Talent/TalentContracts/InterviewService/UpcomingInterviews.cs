//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Talent.TalentContracts.InterviewService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The upcoming interviews data contract.
    /// </summary>
    [DataContract]
    public class UpcomingInterviews
    {
        /// <summary>Gets or sets the schedule for the day.</summary>
        [DataMember(Name = "ScheduleForDay", EmitDefaultValue = false, IsRequired = false)]
        public IList<InterviewDetails> ScheduleForDay { get; set; }

        /// <summary>Gets or sets upcoming scheduled dates for month.</summary>
        [DataMember(Name = "ScheduledDatesForMonth", EmitDefaultValue = false, IsRequired = false)]
        public IList<DateTime> ScheduledDatesForMonth { get; set; }

        /// <summary>Gets or sets upcoming schedules for month.</summary>
        [DataMember(Name = "SchedulesForMonth", EmitDefaultValue = false, IsRequired = false)]
        public IList<DateTime> SchedulesForMonth { get; set; }
    }
}
