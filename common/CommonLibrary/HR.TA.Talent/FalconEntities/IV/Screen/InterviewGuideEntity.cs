//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Talent.FalconEntities.IV.Screen
{
    using HR.TA.Common.DocumentDB.Contracts;
    using HR.TA.Talent.EnumSetModel;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class for Interview Guide Entity.
    /// </summary>
	[DataContract]
    public class InterviewGuideEntity : DocDbEntity
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
		public IList<SkillAttributeEntity> SkillAttributes { get; set; }
	}
}
