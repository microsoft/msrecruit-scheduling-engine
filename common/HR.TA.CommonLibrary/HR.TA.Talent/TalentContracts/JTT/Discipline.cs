//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.TalentContracts.JTT
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class for the Discipline Entity in JTT.
    /// </summary>
    [DataContract]
    public class Discipline
    {
        /// <summary>
        /// Unique Identifier of the Profession associated to the Discipline.
        /// </summary>
        [DataMember(Name = "professionId", IsRequired = true)]
        public string ProfessionId { get; set; }

        /// <summary>
        /// Unique Identifier for the Discipline.
        /// </summary>
        [DataMember(Name = "disciplineId", IsRequired = true)]
        public string DisciplineId { get; set; }

        /// <summary>
        /// Name of the Discipline.
        /// </summary>
        [DataMember(Name = "disciplineName", IsRequired = true)]
        public string DisciplineName { get; set; }

        /// <summary>
        /// Description of the Discipline.
        /// </summary>
        [DataMember(Name = "disciplineDescription")]
        public string DisciplineDescription { get; set; }

        /// <summary>
        /// Date from which the Discipline is effective.
        /// </summary>
        [DataMember(Name = "startDate", IsRequired = true)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date until which the Discipline is effective.
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Status of the Discipline - Active/Inactive.
        /// </summary>
        [DataMember(Name = "status", IsRequired = true)]
        public string Status { get; set; }

        /// <summary>
        /// Boolean to indicate whether the Discipline is being maintained for existing and filled positions only (Yes) or can be used for new and open positions (No).
        /// </summary>
        [DataMember(Name = "exception")]
        public bool Exception { get; set; }

        /// <summary>
        /// Boolean to indicate whether the Discipline is only available for usage with documented permission (Yes) or available for usage without restrictions (No).
        /// </summary>
        [DataMember(Name = "confidential")]
        public bool Confidential { get; set; }
    }
}
