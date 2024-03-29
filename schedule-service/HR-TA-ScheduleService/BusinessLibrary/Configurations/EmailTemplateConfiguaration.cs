//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Configurations
{
    using HR.TA.ServicePlatform.Configuration;

    /// <summary>
    /// Email Template Configuration
    /// </summary>
    [SettingsSection("EmailTemplateConfiguaration")]
    public class EmailTemplateConfiguaration
    {
        /// <summary>
        /// Gets or sets InterviewerDeclineEmailTemplate
        /// </summary>
        public string InterviewerDeclineEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets InterviewerDeclineEmailTemplate with message email template
        /// </summary>
        public string InterviewerDeclineWithMessageEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets the interviewer decline with proposed time email template.
        /// </summary>
        /// <value>
        /// The interviewer decline with proposed time email template.
        /// </value>
        public string InterviewerDeclineProposedTimeEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets the interviewer decline with proposed time and message email template.
        /// </summary>
        /// <value>
        /// The interviewer decline with proposed time and message email template.
        /// </value>
        public string InterviewerDeclineProposedTimeMessageEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets FeedbackReminderEmailTemplate
        /// </summary>
        public string FeedbackReminderEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets AutomatedFeedbackReminderEmailTemplate
        /// </summary>
        public string AutomatedFeedbackReminderEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets InterviewerFeedbackReminderForUniversityReqs
        /// </summary>
        public string InterviewerFeedbackReminderForUniversityReqs { get; set; }

        /// <summary>
        /// Gets or sets InterviewerFeedbackReminderForUniversityReqs
        /// </summary>
        public string AutomatedInterviewerFeedbackReminderForUniversityReqs { get; set; }

        /// <summary>
        /// Gets or sets Scheduler Reminder Email Template
        /// </summary>
        public string SchedulerReminderEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets Scheduler Reminder Email Template with notes
        /// </summary>
        public string SchedulerReminderEmailTemplateWithNotes { get; set; }

        /// <summary>
        /// Gets or sets InterviewInvitationFailed
        /// </summary>
        public string InterviewInvitationFailedEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets ScheduleAssignmentEmailTemplate
        /// </summary>
        public string ScheduleAssignmentEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets ScheduleAssignmentEmailTemplate with notes
        /// </summary>
        public string ScheduleAssignmentEmailTemplateWithNotes { get; set; }

        /// <summary>
        /// Gets or sets the feedback only email template.
        /// </summary>
        /// <value>
        /// The feedback only email template.
        /// </value>
        public string FeedbackOnlyEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets PilotEmailTemplate
        /// </summary>
        /// <value>
        /// The pilot only email template.
        /// </value>
        public string PilotEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets ShareFeedbackEmailTemplate
        /// </summary>
        /// <value>
        /// The share feedback email template.
        /// </value>
        public string ShareFeedbackEmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets Delegation assignment template
        /// </summary>
        public string DelegationAssignmentTemplate { get; set; }
    }
}
