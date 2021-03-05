//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using TalentEntities.Enum;

    /// <summary>
    /// The feedback data contract.
    /// </summary>
    [DataContract]
    public class Feedback
    {       
        /// <summary>
        /// Gets or sets job stage.
        /// </summary>
        [DataMember(Name = "stage", IsRequired = false, EmitDefaultValue = false)]
        public JobStage Stage { get; set; }

        /// <summary>
        /// Gets or sets job stage.
        /// </summary>
        [DataMember(Name = "stageOrder", IsRequired = false, EmitDefaultValue = false)]
        public int StageOrder { get; set; }

        /// <summary>
        /// Gets or sets strength comment.
        /// </summary>
        [DataMember(Name = "strengthComment", IsRequired = false, EmitDefaultValue = false)]
        public string StrengthComment { get; set; }

        /// <summary>
        /// Gets or sets weakness comment.
        /// </summary>
        [DataMember(Name = "weaknessComment", IsRequired = false, EmitDefaultValue = false)]
        public string WeaknessComment { get; set; }

        /// <summary>
        /// Gets or sets overall comment.
        /// </summary>
        [DataMember(Name = "overallComment", IsRequired = false, EmitDefaultValue = false)]
        public string OverallComment { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        [DataMember(Name = "Status", IsRequired = false)]
        public JobApplicationAssessmentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets status reason.
        /// </summary>
        [DataMember(Name = "statusReason", IsRequired = false)]
        public JobApplicationAssessmentStatusReason StatusReason { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is recommended to continue.
        /// </summary>
        [DataMember(Name = "isRecommendedToContinue")]
        public bool IsRecommendedToContinue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the OID for the user who provided the feedback
        /// </summary>
        [DataMember(Name = "feedbackProvider", IsRequired = false)]
        public Delegate FeedbackProvider { get; set; }

        /// <summary>
        /// Gets or sets a value for the Job Application ID
        /// </summary>
        [DataMember(Name = "jobApplicationID", IsRequired = true, EmitDefaultValue = false)]
        public string jobApplicationId { get; set; }
    }
}
