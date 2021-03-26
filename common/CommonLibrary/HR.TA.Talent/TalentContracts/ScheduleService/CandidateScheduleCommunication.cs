//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Talent.TalentContracts.ScheduleService
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <see cref="CandidateScheduleCommunication"/> class stores candidate communication status flags.
    /// </summary>
    [DataContract]
    public class CandidateScheduleCommunication
    {
        /// <summary>
        /// Gets or sets the schedule identifier.
        /// </summary>
        /// <value>
        /// The schedule identifier.
        /// </value>
        [DataMember(Name = "scheduleId", EmitDefaultValue = false, IsRequired = true)]
        [Required(ErrorMessage = "The schedule Id is mandatory.", AllowEmptyStrings = false)]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interview schedule is shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the interview schedule is shared; otherwise <c>false</c>.
        /// </value>
        [DataMember(Name = "isInterviewScheduleShared", EmitDefaultValue = false, IsRequired = true)]
        [Required(ErrorMessage = "The schedule sharing not specified.")]
        public bool IsInterviewScheduleShared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the interviewer panel names are shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the interviewer panel names are shared; otherwise <c>false</c>.
        /// </value>
        [DataMember(Name = "isInterviewerNameShared", EmitDefaultValue = false, IsRequired = true)]
        public bool IsInterviewerNameShared { get; set; }
    }
}
