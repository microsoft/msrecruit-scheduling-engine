//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The contract for assessments provided by external assessment providers.
    /// </summary>
    [DataContract]
    public class AssessmentProject
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string ID { get; set; }        

        /// <summary>
        /// Gets or sets the created Date.
        /// </summary>
        [DataMember(Name = "created", IsRequired = false, EmitDefaultValue = false)]
        public string Created { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [DataMember(Name = "title", IsRequired = false, EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the previewURL.
        /// </summary>
        [DataMember(Name = "previewURL", IsRequired = false, EmitDefaultValue = false)]
        public string PreviewURL { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember(Name = "projectStatus", IsRequired = false, EmitDefaultValue = true)]
        public string ProjectStatus { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember(Name = "instructions", IsRequired = false, EmitDefaultValue = true)]
        public AssessmentInstructions Instructions { get; set; }

        /// <summary>
        /// Gets or sets the Template.
        /// </summary>
        [DataMember(Name = "templates", IsRequired = false, EmitDefaultValue = true)]
        public IList<ExternalAssessment> Templates { get; set; }

        /// <summary>
        /// Gets or sets the Users.
        /// </summary>
        [DataMember(Name = "users", IsRequired = false, EmitDefaultValue = true)]
        public IList<AssessmentUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the Company name.
        /// </summary>
        [DataMember(Name = "companyName", IsRequired = false, EmitDefaultValue = true)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the Job title.
        /// </summary>
        [DataMember(Name = "jobTitle", IsRequired = false, EmitDefaultValue = true)]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the Job description.
        /// </summary>
        [DataMember(Name = "jobDescription", IsRequired = false, EmitDefaultValue = true)]
        public string JobDescription { get; set; }
    }
}
