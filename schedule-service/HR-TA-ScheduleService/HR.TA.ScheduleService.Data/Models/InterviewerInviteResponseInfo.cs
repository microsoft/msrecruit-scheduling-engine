//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.ScheduleService.Data.Models
{
    using HR.TA.ScheduleService.Contracts.V1;

    /// <summary>
    /// The <see cref="InterviewerInviteResponseInfo"/> class holds the interviewer invite response with message and job application with particiapnts info.
    /// </summary>
    public class InterviewerInviteResponseInfo
    {
        /// <summary>
        /// Gets or sets the interviewer response notification.
        /// </summary>
        /// <value>
        /// The instance of <see cref="InterviewerResponseNotification"/>.
        /// </value>
        public InterviewerResponseNotification ResponseNotification { get; set; }

        /// <summary>
        /// Gets or sets the interviewer message.
        /// </summary>
        /// <value>
        /// The interviewer message.
        /// </value>
        public string InterviewerMessage { get; set; }

        /// <summary>
        /// Gets or sets the job application and it's participants information.
        /// </summary>
        /// <value>
        /// The instance of <see cref="JobApplicationParticipants"/>.
        /// </value>
        public JobApplicationParticipants ApplicationParticipants { get; set; }
    }
}
