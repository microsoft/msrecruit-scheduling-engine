//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.Contracts;

    /// <summary>
    /// Work experience data contract.
    /// </summary>
    [DataContract]
    public class WorkExperience : TalentBaseContract
    {
        /// <summary>
        /// Gets or sets the WorkExperienceId.
        /// </summary>
        [DataMember(Name = "WorkExperienceId", EmitDefaultValue = false, IsRequired = false)]
        public Guid? WorkExperienceId { get; set; }

        /// <summary>
        /// Gets or sets the work experience title.
        /// </summary>
        [DataMember(Name = "Title", EmitDefaultValue = false, IsRequired = false)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the organization name.
        /// </summary>
        [DataMember(Name = "Organization", EmitDefaultValue = false, IsRequired = false)]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the work location.
        /// </summary>
        [DataMember(Name = "Location", EmitDefaultValue = false, IsRequired = false)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the work description.
        /// </summary>
        [DataMember(Name = "Description", EmitDefaultValue = false, IsRequired = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets whether the current work experience is the candidate's current position.
        /// </summary>
        [DataMember(Name = "IsCurrentPosition", EmitDefaultValue = false, IsRequired = false)]
        public bool? IsCurrentPosition { get; set; }

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
