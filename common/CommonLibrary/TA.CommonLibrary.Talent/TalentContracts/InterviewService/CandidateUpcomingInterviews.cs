//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Talent.TalentContracts.InterviewService
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TA.CommonLibrary.Talent.TalentContracts.InterviewService;
    using TA.CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// The upcoming interviews for candidate data contract.
    /// </summary>
    [DataContract]
    public class CandidateUpcomingInterviews
    {
        /// <summary>Gets or sets the position title.</summary>
        [DataMember(Name = "PositionTitle", EmitDefaultValue = false, IsRequired = false)]
        public string PositionTitle { get; set; }

        /// <summary>Gets or sets the external job opening id.</summary>
        [DataMember(Name = "ExternalJobOpeningId", EmitDefaultValue = false, IsRequired = true)]
        public string ExternalJobOpeningId { get; set; }

        /// <summary>Gets or sets the external job opening id.</summary>
        [DataMember(Name = "ExternalCandidateId", EmitDefaultValue = false, IsRequired = true)]
        public string ExternalCandidateId { get; set; }

        /// <summary>Gets or sets the location of the job opening.</summary>
        [DataMember(Name = "PositionLocation", EmitDefaultValue = false, IsRequired = false)]
        public string PositionLocation { get; set; }

        /// <summary>Gets or sets current stage.</summary>
        [DataMember(Name = "CurrentStage", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationActivityType? CurrentStage { get; set; }

        /// <summary>Gets or sets the schedule.</summary>
        [DataMember(Name = "Schedules", EmitDefaultValue = false, IsRequired = false)]
        public IList<InterviewSchedule> Schedules { get; set; }
    }
}
