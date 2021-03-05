//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TalentEntities.Enum;

    /// <summary>
    /// The feedback participant data contract.
    /// </summary>
    [DataContract]
    public class FeedbackParticipant : Person
    {
        /// <summary>
        /// Gets or sets member role.
        /// </summary>
        [DataMember(Name = "role")]
        public JobParticipantRole Role { get; set; }

        /// <summary>
        /// Gets or sets member feedbacks.
        /// </summary>
        [DataMember(Name = "feedbacks", IsRequired = false, EmitDefaultValue = false)]
        public IEnumerable<Feedback> Feedbacks { get; set; }
    }
}
