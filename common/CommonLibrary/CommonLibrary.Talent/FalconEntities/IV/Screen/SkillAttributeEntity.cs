//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Talent.FalconEntities.IV.Screen
{
    using CommonLibrary.Talent.EnumSetModel;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class for Skill Attribute Entity.
    /// </summary>
	[DataContract]
    public class SkillAttributeEntity
    {
		/// <summary>
		/// Name of the Attribute associated to the Skill.
		/// </summary>
		[DataMember(Name = "skillAttributeName", IsRequired = true)]
		public string SkillAttributeName { get; set; }

		/// <summary>
		/// Boolean to indicate if the Skill Attribute is associated to a managerial role.
		/// </summary>
		[DataMember(Name = "isManagerialAttribute", IsRequired = true)]
		public bool IsManagerialAttribute { get; set; }

		/// <summary>
		/// Expected behavior associated to the Skill Attribute.
		/// </summary>
		[DataMember(Name = "skillAttributeExpectedBehavior", IsRequired = true)]
		public string SkillAttributeExpectedBehavior { get; set; }

		/// <summary>
		/// Collection of Questions associated to the Skill Attribute.
		/// </summary>
		[DataMember(Name = "skillAttributeInterviewQuestions", IsRequired = true)]
		public IList<string> SkillAttributeInterviewQuestions { get; set; }

		/// <summary>
		/// Collection of Probing Questions associated to the Skill Attribute.
		/// </summary>
		[DataMember(Name = "skillAttributeProbingQuestions", IsRequired = true)]
		public IList<string> SkillAttributeProbingQuestions { get; set; }

		/// <summary>
		/// Mapping between the Evaluation Standard and the Expectations associated to the Skill Attribute.
		/// </summary>
		[DataMember(Name = "skillAttributeEvaluationStandards", IsRequired = true)]
		public IDictionary<SkillEvaluationStandard, IList<string>> SkillAttributeEvaluationStandards { get; set; }
	}
}
