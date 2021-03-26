//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Talent.EnumSetModel.SchedulingService
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Template Type enums
    /// </summary>
    [DataContract]
    public enum TemplateType
    {
        /// <summary>
        /// Candidate Interview Request Templates => Final Schedule
        /// </summary>
        CandidateInterview = 0,

        /// <summary>
        /// Candidate Invite Templates => First email when we add candidate
        /// </summary>
        CandidateInvite = 1,

        /// <summary>
        /// Candidate Request Availability => to choose dates
        /// </summary>
        CandidateAvailability = 2,

        /// <summary>
        /// Interviewer Meeting Request Templates => We send to accept meeting to Interviewer
        /// </summary>
        InterviewerMeeting = 3,

        /// <summary>
        /// Interviewer Summary Templates => Before 24 hours or Recruiter/Scheduler will send summery email to all Interviewers.
        /// </summary>
        InterviewerSummary = 4,

        /// <summary>
        /// Interviewer Meeting Reminder Templates  => This is background email that we sent to Interviewer to accept meeting.
        /// </summary>
        InterviewerMeetingReminder = 5,

        /// <summary>
        /// Interviewer Feedback Reminder Templates => This will be background reminder for Interviewer to submit feedback 
        /// </summary>
        InterviewerFeedbackReminder = 6,

        /// <summary>
        /// Review Activities Template => If any activity is pending send in general message to candidate
        /// </summary>
        ReviewActivitiesTemplate = 7,

        /// <summary>
        /// Offer Notification to Candidate
        /// </summary>
        OfferNotificationTemplate = 8,

        /// <summary>
        /// new candidate to schedule – to scheduler
        /// </summary>
        NewCandidateScheduler = 9,

        /// <summary>
        /// Notify Scheduler after interviewer accept decline meeting
        /// </summary>
        NotifySchedulerAcceptDecline = 10,

        /// <summary>
        /// Request For Feedback
        /// </summary>
        RequestForFeedback = 11,

        /// <summary>
        /// Room Decline Reminder
        /// </summary>
        RoomDeclineReminder = 12,

        /// <summary>
        /// Pending Declined Response Reminder
        /// </summary>
        PendingDeclinedResponseReminder = 13,

        /// <summary>
        /// Prospect candidate apply invite
        /// </summary>
        ProspectCandidateApplyInvite = 14,

        /// <summary>
        /// Interview Invitation Failed email template
        /// </summary>
        InterviewInvitationFailed = 15,

        /// <summary>
        /// Send request for job approval
        /// </summary>
        RequestForJobApproval = 16,

        /// <summary>
        /// Send response for job approval request
        /// </summary>
        NotifyJobApprovedToRequester = 17,

        /// <summary>
        /// Notification to inform of job moving to approved status
        /// </summary>
        NotifyJobDeniedToRequester = 18,

        /// <summary>
        /// Request for offer approval
        /// </summary>
        RequestOfferApproval = 19,

        /// <summary>
        /// Notification to inform about the approved offer
        /// </summary>
        NotifyOfferApproved = 20,

        /// <summary>
        /// Notification to inform about the rejected offer
        /// </summary>
        NotifyOfferRejected = 21,

        /// <summary>
        /// Notification to inform about the offer Publish To Candidate
        /// </summary>
        NotifyOfferPublishedToCandidate = 22,

        /// <summary>
        /// Notification to inform about the withdrawal of offer
        /// </summary>
        NotifyOfferWithdraw = 23,

        /// <summary>
        /// Reminder to inform about the offer expiry
        /// </summary>
        OfferExpiryReminder = 24,

        /// <summary>
        /// Notification to inform about the decline of offer to recruiter
        /// </summary>
        NotifyOfferDeclinedToRecruiter = 25,

        /// <summary>
        /// Notification to inform about the decline of offer to hiring manager
        /// </summary>
        NotifyOfferDeclinedToHiringManager = 26,

        /// <summary>
        /// Notification to inform about the acceptance of offer To Recruiter
        /// </summary>
        NotifyOfferAcceptedToRecruiter = 27,

        /// <summary>
        /// Notification to inform about the acceptance of offer to hiring manager
        /// </summary>
        NotifyOfferAcceptedToHiringManager = 28,

        /// <summary>
        /// Notification to inform of expired offer
        /// </summary>
        NotifyOfferExpired = 29,

        /// <summary>
        /// Notification to inform about offer approval to Offer Creator
        /// </summary>
        NotifyOfferApprovalToOfferCreator = 30,

        /// <summary>
        /// Notification to inform about offer rejected to Offer Creator
        /// </summary>
        NotifyOfferRejectionToOfferCreator = 31,

        /// <summary>
        /// Notification to inform about closed offer to Recruiter
        /// </summary>
        NotifyOfferCloseToRecruiter = 32,

        /// <summary>
        /// Notification to inform about closed offer to Hiring Manager
        /// </summary>
        NotifyOfferCloseToHiringManager = 33,

        /// <summary>
        /// Candidate login failed email template
        /// </summary>
        CandidateLoginFailed = 34,

        /// <summary>
        /// Candidate login information email template
        /// </summary>
        CandidateLoginInformation = 35,

        /// <summary>
        /// Send request for job approval reminder
        /// </summary>
        RequestForJobApprovalReminder = 36,

        /// <summary>
        /// Notification to inform candidate that their job application has been rejected
        /// </summary>
        NotifyApplicationRejectedToCandidate = 37,
    }
}
