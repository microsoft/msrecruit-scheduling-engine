//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using MS.GTA.Common.TalentEntities.Common;

namespace MS.GTA.ScheduleService.Data.Models
{
    /// <summary>
    /// The <see cref="PendingFeedback"/> class holds necessary info to send email reminder for a pending feedback.
    /// </summary>
    public class PendingFeedback
    {
        /// <summary>
        /// Gets or sets the job application id.
        /// </summary>
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the job position title.
        /// </summary>
        public string PositionTitle { get; set; }

        /// <summary>
        /// Gets or sets the candidate name.
        /// </summary>
        public string CandidateName { get; set; }

        /// <summary>
        /// Gets or sets the external job opening id.
        /// </summary>
        public string ExternalJobOpeningId { get; set; }

        /// <summary>
        /// Gets or sets the interviewer.
        /// </summary>
        /// <value>
        /// An instance of <see cref="Worker"/>
        /// </value>
        public Worker Interviewer { get; set; }

        /// <summary>
        /// Gets or sets the hiring manager.
        /// </summary>
        /// <value>
        /// An instance of <see cref="Worker"/>
        /// </value>
        public Worker HiringManager { get; set; }

        /// <summary>
        /// Gets or sets the recruiter.
        /// </summary>
        /// <value>
        /// An instance of <see cref="Worker"/>
        /// </value>
        public Worker Recruiter { get; set; }

        /// <summary>
        /// Gets or sets the schedule requester.
        /// </summary>
        /// <value>
        /// An instance of <see cref="Worker"/>
        /// </value>
        public Worker ScheduleRequester { get; set; }
    }
}
