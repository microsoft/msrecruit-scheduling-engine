//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// The hiring team member data contract.
    /// </summary>
    [DataContract]
    public class HiringTeamMember : Person
    {
        /// <summary>
        /// Gets or sets member role.
        /// </summary>
        [DataMember(Name = "role")]
        public JobParticipantRole Role { get; set; }

        /// <summary>
        /// Gets or sets User action.
        /// </summary>
        [DataMember(Name = "userAction", IsRequired = false, EmitDefaultValue = false)]
        public UserAction UserAction { get; set; }

        /// <summary>Gets or sets the title.</summary>
        [DataMember(Name = "title", IsRequired = false, EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <summary>Gets or sets the ordinal.</summary>
        [DataMember(Name = "ordinal", IsRequired = false, EmitDefaultValue = false)]
        public long? Ordinal { get; set; }

        /// <summary>
        /// Gets or sets member activities.
        /// </summary>
        [DataMember(Name = "activities", IsRequired = false, EmitDefaultValue = false)]
        public List<Activity> Activities { get; set; }

        /// <summary>
        /// Gets or sets member feedbacks.
        /// </summary>
        [DataMember(Name = "feedbacks", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Feedback> Feedbacks { get; set; }

        /// <summary>
        /// Gets or sets the list of delegates.
        /// </summary>
        [DataMember(Name = "delegates", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Delegate> Delegates { get; set; }

        /// <summary>
        /// Gets or sets member metadata.
        /// </summary>
        [DataMember(Name = "metadata", IsRequired = false, EmitDefaultValue = false)]
        public JobApplicationParticipantMetadata Metadata { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether Hiring Team Member can be deleted or not.
        /// </summary>
        [DataMember(Name = "isDeleteAllowed", IsRequired = false, EmitDefaultValue = false)]
        public bool? IsDeleteAllowed { get; set; }

        /// <summary>Gets or sets the AddedOnDate </summary>
        [DataMember(Name = "AddedOnDate", EmitDefaultValue = false, IsRequired = false)]
        public DateTime? AddedOnDate { get; set; }
    }
}
