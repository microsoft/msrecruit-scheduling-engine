//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ScheduleAvailability.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The applicant schedule availability contract.
    /// </summary>
    [DataContract]
    public class ScheduleAvailability
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets schedule start date and time.
        /// </summary>
        [DataMember(Name = "startDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets schedule end date and time.
        /// </summary>
        [DataMember(Name = "endDate", IsRequired = false, EmitDefaultValue = false)]
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether purposed schedule  is selected by candidate.
        /// </summary>
        [DataMember(Name = "isHiringTeamAvailable", IsRequired = false, EmitDefaultValue = false)]
        public bool IsHiringTeamAvailable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether purposed schedule  is selected by candidate.
        /// </summary>
        [DataMember(Name = "isCandidateAvailable", IsRequired = false, EmitDefaultValue = false)]
        public bool IsCandidateAvailable { get; set; }

        /// <summary>
        /// Gets or sets a Time zone
        /// </summary>
        [DataMember(Name = "timeZone", IsRequired = false, EmitDefaultValue = false)]
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets a user action
        /// </summary>
        [DataMember(Name = "userAction", IsRequired = false, EmitDefaultValue = false)]
        public UserAction UserAction { get; set; }
    }
}
