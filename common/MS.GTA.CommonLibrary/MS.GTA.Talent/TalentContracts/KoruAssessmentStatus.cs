//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="KoruAssessmentStatus.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum about the Assessment status
    /// </summary>
    [DataContract]
    public enum KoruAssessmentStatus
    {
        /// <summary>
        /// Candidate added to the assessment
        /// </summary>
        Added = 1,

        /// <summary>
        /// Assessment sent
        /// </summary>
        Sent,

        /// <summary>
        /// Candidate started the assessment
        /// </summary>
        Started,

        /// <summary>
        /// Candidate submit the assessment
        /// </summary>
        Submitted,

        /// <summary>
        /// The assessment is completed.
        /// </summary>
        Done
    }
}
