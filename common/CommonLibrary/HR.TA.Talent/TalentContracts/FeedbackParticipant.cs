//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA.TalentEntities.Enum;

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
