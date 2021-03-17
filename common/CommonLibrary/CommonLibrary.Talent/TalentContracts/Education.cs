//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Education data contract.
    /// </summary>
    [DataContract]
    public class Education
    {
        /// <summary>
        /// Gets or sets the EducationId
        /// </summary>
        [DataMember(Name = "EducationId", EmitDefaultValue = false, IsRequired = false)]
        public Guid? EducationId { get; set; }

        /// <summary> 
        /// Gets or sets the school title.
        /// </summary>
        [DataMember(Name = "school", EmitDefaultValue = false, IsRequired = false)]
        public string School { get; set; }

        /// <summary>
        /// Gets or sets the degree name.
        /// </summary>
        [DataMember(Name = "degree", EmitDefaultValue = false, IsRequired = false)]
        public string Degree { get; set; }

        /// <summary>
        /// Gets or sets the field of study.
        /// </summary>
        [DataMember(Name = "fieldOfStudy", EmitDefaultValue = false, IsRequired = false)]
        public string FieldOfStudy { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        [DataMember(Name = "grade", EmitDefaultValue = false, IsRequired = false)]
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the education description.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start date; optional.
        /// </summary>
        [DataMember(Name = "FromMonthYear", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? FromMonthYear { get; set; }

        /// <summary>
        /// Gets or sets the end date; optional.
        /// </summary>
        [DataMember(Name = "ToMonthYear", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? ToMonthYear { get; set; }
    }
}
