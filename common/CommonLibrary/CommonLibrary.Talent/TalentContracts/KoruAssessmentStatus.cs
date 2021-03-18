//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
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
