//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.TalentContracts.QueryStringParameters
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The interview guide query parameters.
    /// </summary>
    [DataContract]
    public class InterviewGuideQueryParameters : PaginationAndSortQueryParameters
    {
        /// <summary>
        /// Gets or sets the Profession Id for search.
        /// </summary>
        [DataMember(Name = "professionId", IsRequired = false)]
        public string ProfessionId { get; set; }

        /// <summary>
        /// Gets or sets the Discipline Id for search.
        /// </summary>
        [DataMember(Name = "disciplineId", IsRequired = false)]
        public string DisciplineId { get; set; }

        /// <summary>
        /// Gets or sets the Role Id for search.
        /// </summary>
        [DataMember(Name = "roleId", IsRequired = false)]
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the Career Stage Id for search.
        /// </summary>
        [DataMember(Name = "careerStageId", IsRequired = false)]
        public string CareerStageId { get; set; }

        /// <summary>
        /// Gets or sets the Skill Id for search.
        /// </summary>
        [DataMember(Name = "skillId", IsRequired = false)]
        public string SkillId { get; set; }
    }
}
