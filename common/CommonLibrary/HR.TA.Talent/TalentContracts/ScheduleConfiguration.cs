//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Configuration for Schedule activity.
    /// </summary>
    [DataContract]
    public class ScheduleConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether to request the candidate is available or not.
        /// </summary>
        [DataMember(Name = "requestCandidateAvailability", IsRequired = false, EmitDefaultValue = false)]
        public bool RequestCandidateAvailability { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable skype meeting.
        /// </summary>
        [DataMember(Name = "enableSkypeMeeting", IsRequired = false, EmitDefaultValue = false)]
        public bool EnableSkypeMeeting { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to send mail to candidate.
        /// </summary>
        [DataMember(Name = "sendMailToCandidate", IsRequired = false, EmitDefaultValue = false)]
        public bool SendMailToCandidate { get; set; }
    }
}
