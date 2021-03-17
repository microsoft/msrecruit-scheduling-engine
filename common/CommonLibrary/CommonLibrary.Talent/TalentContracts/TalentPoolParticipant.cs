//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;
    using CommonLibrary.TalentEntities.Enum;

    /// <summary>
    /// The talent pool participant
    /// </summary>
    [DataContract]
    public class TalentPoolParticipant : Person
    {
        /// <summary>
        /// Gets or sets Talent pool participant role
        /// </summary>
        [DataMember(Name = "role", IsRequired = false, EmitDefaultValue = false)]
        public TalentPoolParticipantRole Role { get; set; }

        /// <summary>
        /// Gets or sets User action.
        /// </summary>
        [DataMember(Name = "userAction", IsRequired = false, EmitDefaultValue = false)]
        public UserAction UserAction { get; set; }
    }
}
