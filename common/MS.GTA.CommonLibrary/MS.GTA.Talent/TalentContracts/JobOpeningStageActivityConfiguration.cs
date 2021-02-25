//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Configuration for Job Opening Stage Activity.
    /// </summary>
    [DataContract]
    public class JobOpeningStageActivityConfiguration
    {
        /// <summary>
        /// Gets or sets forCandidate value
        /// </summary>
        [DataMember(Name = "forCandidate", IsRequired = false, EmitDefaultValue = false)]
        public bool ForCandidate { get; set; }

        /// <summary>
        /// Gets or sets allow adding participants value
        /// </summary>
        [DataMember(Name = "allowAddingParticipants", IsRequired = false, EmitDefaultValue = false)]
        public bool AllowAddingParticipants { get; set; }
    }
}