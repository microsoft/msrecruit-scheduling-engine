//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TA.CommonLibrary.Talent.TalentContracts.Screen
{
    using TA.CommonLibrary.Common.Contracts;
    using TA.CommonLibrary.Talent.EnumSetModel;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class for Interview Guide.
    /// </summary>
	[DataContract]
    public class InterviewGuide : TalentBaseContract
    {
		/// <summary>
		/// Unique Identifier of the Interview Guide.
		/// </summary>
		[DataMember(Name = "interviewGuideId", IsRequired = true)]
		public string InterviewGuideId { get; set; }

		/// <summary>
		/// Unique Identifier for the Profession associated to the Interview Guide.
		/// </summary>
		[DataMember(Name = "professionId")]
		public string ProfessionId { get; set; }

		/// <summary>
		/// Unique Identifier for the Discipline associated to the Interview Guide.
		/// </summary>
		[DataMember(Name = "disciplineId")]
		public string DisciplineId { get; set; }

		/// <summary>
		/// Unique Indentifier for the Role associated to the Interview Guide.
		/// </summary>
		[DataMember(Name = "roleId")]
		public string RoleId { get; set; }

		/// <summary>
		/// Unique Indentifier for the Career Stage associated to the Interview Guide.
		/// </summary>
		[DataMember(Name = "careerStageId")]
		public string CareerStageId { get; set; }

		/// <summary>
		/// Unique Indentifier for the Skill associated to the Interview Guide.
		/// </summary>
		[DataMember(Name = "skillId", IsRequired = true)]
		public string SkillId { get; set; }

		/// <summary>
		/// Type of the Skill associated to the <see cref="SkillId"/>.
		/// </summary>
		[DataMember(Name = "type", IsRequired = true)]
		public SkillType Type { get; set; }

		/// <summary>
		/// List of skill attributes associated to the Skill.
		/// </summary>
		[DataMember(Name = "skillAttributes", IsRequired = true)]
		public IList<SkillAttribute> SkillAttributes { get; set; }
	}
}
