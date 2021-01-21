//----------------------------------------------------------------------------
// <copyright file="IEmailManager.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.ScheduleService.Contracts.V1;
    using MS.GTA.Talent.TalentContracts.InterviewService;
    using MS.GTA.Talent.TalentContracts.ScheduleService;

    /// <summary>
    /// IEmailManager interface
    /// </summary>
    public interface IEmailManager
    {
        /// <summary>
        /// Sends a reminder to interviewer to complete the feedback.
        /// </summary>
        /// <param name="pendingFeedbackRequest">Pending Feedback information.</param>
        /// <returns>Reminder Success response.</returns>
        Task<bool> SendFeedbackReminder(PendingFeedback pendingFeedbackRequest);

        /// <summary>
        /// Notify scheduler when interviewer decline or tentatively accepted the invitation
        /// </summary>
        /// <param name="interviewerResponseNotification">interviewer response for notification</param>
        /// <param name="messageResponse">The message response if any.</param>
        /// <returns>task</returns>
        Task SendDeclineEmailToScheduler(InterviewerResponseNotification interviewerResponseNotification, string messageResponse = null);

        /// <summary>
        /// Sends a reminder to scheduler to remind about the interviwer's response.
        /// </summary>
        /// <returns>Reminder Success response.</returns>
        Task<bool> SendSchedulerReminder();

        /// <summary>
        /// sends a reminder to scheduler on invites failure
        /// </summary>
        /// <param name="scheduleID">scheduleid</param>
        /// <returns>Reminder Success response</returns>
        Task<bool> SendInviteFailReminder(string scheduleID);

        /// <summary>
        /// send new assignment email to scheduler
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="schedulerOid">scheduler oid</param>
        /// <param name="candidateName">candidate name</param>
        /// <returns>True/false</returns>
        Task<bool> SendAssignmentEmailToScheduler(string jobApplicationId, string schedulerOid, string candidateName);

        /// <summary>
        /// Sends the feedback email.
        /// </summary>
        /// <param name="notificationRequest">The instance of <see cref="EmailNotificationRequest"/>.</param>
        /// <param name="userOid">Requester Office Graph Identifier</param>
        /// <param name="isWobUser">Checks if the user is a wob user</param>
        /// <returns>The instance of <see cref="Task{Boolean}"/> representing the asynchronous operation.</returns>
        Task<bool> SendFeedbackEmailAsync(EmailNotificationRequest notificationRequest, string userOid, bool isWobUser = false);

        /// <summary>
        /// Send email notification about tool switching to IV.
        /// </summary>
        /// <param name="requisitionId">requisition id</param>
        /// <returns>email Success response</returns>
        Task<bool> SendPilotNotification(string requisitionId);

        /// <summary>
        /// Sends a mail to inform AA on sharing feedback.
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="aaOid">AA oid</param>
        /// <returns><c>true</c> if the email is sent; otherwise <c>false</c>".</returns>
        Task<bool> SendShareFeedbackEmailToAA(string jobApplicationId, string[] aaOid);

        /// <summary>
        /// Send email notification.
        /// </summary>
        /// <param name="jobApplicationId">job application id</param>
        /// <param name="emailDetails">email details</param>
        /// <param name="userMailAddress">user mail address</param>
        /// <param name="requesterOid"> The Requester Office Graph Identifier</param>
        /// <param name="isWobUser"> Flag for checking the user wob Authentication</param>
        /// <returns>email Success response</returns>
        Task<bool> SendEmailNotification(string jobApplicationId, ScheduleInvitationDetails emailDetails, string userMailAddress, string requesterOid = null, bool isWobUser = false);

        /// <summary>
        /// Sends email reminders to interviewers to complete the pending feedbacks if due for more than 24 hours.
        /// </summary>
        /// <returns>An instance of <see cref="Task{Boolean}"/>.</returns>
        Task<bool> SendFeedbackReminderToAllAsync();

        /// <summary>
        /// Sends email to schedulers for delegation assignment.
        /// </summary>
        /// <param name="delegationRequestCollection">Delegation request collection.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        Task SendDelegationAssignmentToSchedulerAsync(IEnumerable<DelegationRequest> delegationRequestCollection);
    }
}
