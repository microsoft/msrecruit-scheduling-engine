//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Email
{
    /// <summary>
    /// Enum defining different types of templates in the system
    /// </summary>
    public enum MessageTemplateType
    {
        /// <summary>
        /// The interviewer used in hiring app
        /// </summary>
        Interviewer = 1,

        /// <summary>
        /// The candidate used in hiring app
        /// </summary>
        Candidate = 2,

        /// <summary>
        /// The interview used in hiring app
        /// </summary>
        Interview = 3,

        /// <summary>
        /// Performance Feedback
        /// </summary>
        PerformanceFeedback = 4,

        /// <summary>
        /// Default Performance Feedback used in performance app
        /// </summary>
        DefaultPerformanceFeedback = 5,

        /// <summary>
        /// The new hire used in onboarding app
        /// </summary>
        NewHire = 6,

        /// <summary>
        /// The hiring or recruiting manager used in hiring app
        /// </summary>
        ManagerInvite = 7,

        /// <summary>
        /// Invite candidate to take online assessment
        /// </summary>
        CandidateInviteToTakeAssessment = 8,
    }
}
