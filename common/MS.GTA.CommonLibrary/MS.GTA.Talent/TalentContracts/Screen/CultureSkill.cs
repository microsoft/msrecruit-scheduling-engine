//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Talent.TalentContracts.Screen
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class for the CultureSkill Entity of GTA.
    /// </summary>
    [DataContract]
    public class CultureSkill
    {
        /// <summary>
        /// Unique Identifier for the Career Stage associated to the Culture Skill.
        /// </summary>
        [DataMember(Name = "careerStageId", IsRequired = true)]
        public string CareerStageId { get; set; }

        /// <summary>
        /// Unique Identifier of the Culture Skill.
        /// </summary>
        [DataMember(Name = "cultureSkillId", IsRequired = true)]
        public string CultureSkillId { get; set; }

        /// <summary>
        /// Name of the Culture Skill.
        /// </summary>
        [DataMember(Name = "cultureSkillName", IsRequired = true)]
        public string CultureSkillName { get; set; }

        /// <summary>
        /// Description of the Culture Skill.
        /// </summary>
        [DataMember(Name = "cultureSkillDescription")]
        public string CultureSkillDescription { get; set; }

        /// <summary>
        /// Date from which the Culture Skill is effective.
        /// </summary>
        [DataMember(Name = "startDate", IsRequired = true)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date until which the Culture Skill is effective.
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Status of the Culture Skill - Active/Inactive.
        /// </summary>
        [DataMember(Name = "status", IsRequired = true)]
        public string Status { get; set; }
    }
}
