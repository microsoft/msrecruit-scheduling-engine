//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using TalentEntities.Enum;

    /// <summary>
    /// The job (opening) assessment data contract.
    /// </summary>
    [DataContract]
    public class JobAssessment
    {
        /// <summary>Gets or sets jobAssessmentID.</summary>
        [DataMember(Name = "jobAssessmentID", IsRequired = false)]
        public string JobAssessmentID { get; set; }

        /// <summary>Gets or sets packageID.</summary>
        [DataMember(Name = "packageID", IsRequired = false)]
        public string PackageID { get; set; }

        /// <summary>Gets or sets jobOpeningId.</summary>
        [DataMember(Name = "jobOpeningID", IsRequired = false)]
        public string JobOpeningID { get; set; }

        /// <summary>Gets or sets provider.</summary>
        [DataMember(Name = "provider", IsRequired = false)]
        public AssessmentProvider Provider { get; set; }

        /// <summary>Gets or sets provider key.</summary>
        [DataMember(Name = "providerKey", IsRequired = false)]
        public string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [DataMember(Name = "title", IsRequired = false)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the number of question.
        /// </summary>
        [DataMember(Name = "numberOfQuestions", IsRequired = false)]
        public int NumberOfQuestions { get; set; }

        /// <summary>
        /// Gets or sets the previewUrl.
        /// </summary>
        [DataMember(Name = "previewURL", IsRequired = false)]
        public string PreviewURL { get; set; }

        /// <summary>
        /// Gets or sets the assessment.
        /// </summary>
        [DataMember(Name = "assessment", IsRequired = false)]
        public ExternalAssessment Assessment { get; set; }

        /// <summary>
        /// Gets or sets IsRequired
        /// </summary>
        [DataMember(Name = "isRequired", IsRequired = false)]
        public JobOpeningAssessmentRequirementStatus IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the job opening assessment's stage
        /// </summary>
        [DataMember(Name = "stage", IsRequired = false)]
        public JobStage Stage { get; set; }
    }
}
