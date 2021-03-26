//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Talent.TalentContracts.TeamsIntegration
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Candiadcy Stage
    /// </summary>
    [DataContract]
    public enum CandidacyStage
    {
        /// <summary>
        /// Incomplete Stage
        /// </summary>
        [EnumMember(Value = "Incomplete")]
        Incomplete = 0,

        /// <summary>
        /// Apply Stage
        /// </summary>
        [EnumMember(Value = "Apply")]
        Apply = 1,

        /// <summary>
        /// Screen Stage
        /// </summary>
        [EnumMember(Value = "Screen")]
        Screen = 2,

        /// <summary>
        /// Interview Stage
        /// </summary>
        [EnumMember(Value = "Interview")]
        Interview = 3,

        /// <summary>
        /// Offer Stage
        /// </summary>
        [EnumMember(Value = "Offer")]
        Offer = 4,

        /// <summary>
        /// PreOnboard Stage
        /// </summary>
        [EnumMember(Value = "PreOnboard")]
        PreOnboard = 5,

        /// <summary>
        /// Dispositioned Stage
        /// </summary>
        [EnumMember(Value = "Dispositioned")]
        Dispositioned = 6
    }
}
