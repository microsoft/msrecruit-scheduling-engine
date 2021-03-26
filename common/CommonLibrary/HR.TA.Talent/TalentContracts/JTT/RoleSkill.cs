//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Talent.TalentContracts.JTT
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class for the RoleSkill Entity in JTT.
    /// </summary>
    [DataContract]
    public class RoleSkill
    {
        /// <summary>
        /// Unique Identifier for the Profession associated to the Role Skill.
        /// </summary>
        [DataMember(Name = "professionId", IsRequired = true)]
        public string ProfessionId { get; set; }

        /// <summary>
        /// Unique Identifier for the Discipline associated to the Role Skill.
        /// </summary>
        [DataMember(Name = "disciplineId", IsRequired = true)]
        public string DisciplineId { get; set; }

        /// <summary>
        /// Unique Identifier for the Role associated to the Role Skill.
        /// </summary>
        [DataMember(Name = "roleId", IsRequired = true)]
        public string RoleId { get; set; }

        /// <summary>
        /// Unique Identifier for the Career Stage associated to the Role Skill.
        /// </summary>
        [DataMember(Name = "careerStageId", IsRequired = true)]
        public string CareerStageId { get; set; }

        /// <summary>
        /// Unique Identifier of the Role Skill.
        /// </summary>
        [DataMember(Name = "roleSkillId", IsRequired = true)]
        public string RoleSkillId { get; set; }

        /// <summary>
        /// Name of the Role Skill.
        /// </summary>
        [DataMember(Name = "roleSkillName", IsRequired = true)]
        public string RoleSkillName { get; set; }

        /// <summary>
        /// Description of the Role Skill.
        /// </summary>
        [DataMember(Name = "roleSkillDescription")]
        public string RoleSkillDescription { get; set; }

        /// <summary>
        /// Date from which the Role Skill is effective.
        /// </summary>
        [DataMember(Name = "startDate", IsRequired = true)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date until which the Role Skill is effective.
        /// </summary>
        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Status of the Role Skill - Active/Inactive.
        /// </summary>
        [DataMember(Name = "status", IsRequired = true)]
        public string Status { get; set; }

        /// <summary>
        /// Boolean to indicate whether the Role Skill is being maintained for existing and filled positions only (Yes) or can be used for new and open positions (No).
        /// </summary>
        [DataMember(Name = "exception")]
        public bool Exception { get; set; }

        /// <summary>
        /// Boolean to indicate whether the Role Skill is only available for usage with documented permission (Yes) or available for usage without restrictions (No).
        /// </summary>
        [DataMember(Name = "confidential")]
        public bool Confidential { get; set; }
    }
}
