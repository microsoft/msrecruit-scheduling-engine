//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.TalentContracts.ScheduleService
{
    using HR.TA.ScheduleService.Contracts.V1;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="InterviewersNotificationRequest"/> contains the interviewers information to whom notification is sent.
    /// </summary>
    [DataContract]
    public class InterviewersNotificationRequest
    {
        /// <summary>
        /// Gets or sets the job application identifier.
        /// </summary>
        /// <value>
        /// The job application identifier.
        /// </value>
        [DataMember(Name = "jobApplicationId", IsRequired = true, EmitDefaultValue = false)]
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the interviewers.
        /// </summary>
        /// <value>
        /// The interviewers list.
        /// </value>
        [DataMember(Name = "interviewers", IsRequired = true, EmitDefaultValue = false)]
        public List<GraphPerson> Interviewers { get; set; }
    }
}
