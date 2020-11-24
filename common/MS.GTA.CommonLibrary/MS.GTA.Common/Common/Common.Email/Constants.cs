//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

using System.Collections.Generic;

namespace MS.GTA.Common.Email
{
    /// <summary>
    /// Class to encapsulate all the constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>Prefix for EmailSubjectTemplate Resource</summary>
        public const string EmailSubjectTemplateResourcePrefix = "EmailSubjectTemplate_";

        /// <summary>Prefix for EmailMessageTemplate Resource</summary>
        public const string EmailMessageTemplateResourcePrefix = "EmailMessageTemplate_";

        /// <summary>SendGrid Url</summary>
        public const string SendGridUrl = "https://api.sendgrid.com/";

        /// <summary>SendGrid Key Token Secret Name</summary>
        public const string SendGridApiKeyTokenSecretName = "SendGridApiKeyToken";

        /// <summary>Email Send Rest Endpoint</summary>
        public const string SendGridEmailSendEndpoint = "v3/mail/send";

        /// <summary>Email Footer Field Name.</summary>
        public const string EmailFooterFieldName = "EmailFooterPrivateField";

        /// <summary>Limit of Characters in an email message.</summary>
        public const int EmailMessageLengthLimitInCharacters = 20000;

        /// <summary>Limit of Characters in an email Subject.</summary>
        public const int EmailSubjectLengthLimitInCharacters = 1000;

        /// <summary>Limit on the email addresses one can send emails to.</summary>
        public const int EmailLimitOnToAddresses = 25;

        /// <summary>Limit on the number of attachments in an email.</summary>
        public const int EmailLimitOnNumberOfAttachments = 15;

        /// <summary>Limit on the number of headers in an email.</summary>
        public const int EmailLimitOnNumberOfHeaders = 20;

        /// <summary>Limit on the size of each attachment.</summary>
        public const int EmailLimitOnSizeOfAttachmentsInKB = 1024;

        /// <summary>Number of bytes in one KiloByte.</summary>
        public const int KiloBytes = 1024;

        /// <summary>Email At Char.</summary>
        public const string EmailAtCharacter = "@";

        public const string GraphSendMailUrl = "https://graph.microsoft.com/v1.0/me/sendMail";

        public const string EmailContentType = "html";
        /// <summary>
        /// Default Email alias from where the email should be sent from.
        /// Set to Internal because we don't want the other services to access it.
        /// </summary>
        internal const string DefaultEmailAliasToSendFrom = "No-reply";

        /// <summary>
        /// Default Name of Email from where the email should be sent from.
        /// Set to Internal because we don't want the other services to access it.
        /// </summary>
        internal const string DefaultEmailNameToSendFrom = "Dynamics 365 for Talent";

        /// <summary>Too many requests status code</summary>
        public const int TooManyRequestCode = 429;

        /// <summary>Max Retry Count for request</summary>
        public const int MaxRetryCount = 3;
        public static readonly string Candidate = "Candidate";
        public static readonly string Interviewer = "Interviewer";
        public static readonly string Scheduler = "Scheduler";
        public static readonly string Approver = "Approver";
        public static readonly string ApprovalRequester = "ApprovalRequester";
        public static readonly string OfferApprover = "OfferApprover";
        public static readonly string HiringManager = "HiringManager";
        public static readonly string Recruiter = "Recruiter";
        public static readonly string OfferCreator = "OfferCreator";
        public static readonly string Contributor = "Contributor";
        public static readonly string Unknown = "Unknown";

        public static readonly Dictionary<string, string> TemplateTypeRecipientMap = new Dictionary<string, string>()
        {
            { "CandidateInterview", Candidate },
            { "CandidateInvite", Candidate },
            { "CandidateAvailability", Candidate },
            { "InterviewerMeeting", Interviewer },
            { "InterviewerSummary", Interviewer },
            { "InterviewerMeetingReminder", Interviewer },
            { "InterviewerFeedbackReminder", Interviewer },
            { "ReviewActivitiesTemplate", Candidate },
            { "OfferNotificationTemplate", Unknown }, //// Not in Attract Client
            { "NewCandidateScheduler", Contributor },
            { "NotifySchedulerAcceptDecline", Scheduler },
            { "RequestForFeedback", Interviewer },
            { "RoomDeclineReminder", Scheduler },
            { "PendingDeclinedResponseReminder", Scheduler },
            { "ProspectCandidateApplyInvite", Candidate }, //// Not in Attract Client
            { "InterviewInvitationFailed", Scheduler },
            { "RequestForJobApproval", Approver },
            { "NotifyJobApprovedToRequester", ApprovalRequester },
            { "NotifyJobDeniedToRequester", ApprovalRequester },
            { "RequestOfferApproval", OfferApprover },
            { "NotifyOfferApproved", ApprovalRequester }, //// Not in Attract Client
            { "NotifyOfferRejected", ApprovalRequester }, //// Not in Attract Client
            { "NotifyOfferPublishedToCandidate", Candidate },
            { "NotifyOfferWithdraw", Candidate },
            { "OfferExpiryReminder", Candidate },
            { "NotifyOfferDeclinedToRecruiter", Recruiter },
            { "NotifyOfferDeclinedToHiringManager", HiringManager },
            { "NotifyOfferAcceptedToRecruiter", Recruiter },
            { "NotifyOfferAcceptedToHiringManager", HiringManager },
            { "NotifyOfferExpired", Candidate },
            { "NotifyOfferApprovalToOfferCreator", OfferCreator },
            { "NotifyOfferRejectionToOfferCreator", OfferCreator },
            { "NotifyOfferCloseToRecruiter", Recruiter },
            { "NotifyOfferCloseToHiringManager", HiringManager },
            { "CandidateLoginFailed", Candidate },
            { "CandidateLoginInformation", Candidate }, //// Not in Attract Client
            { "RequestForJobApprovalReminder", Approver },
            { "NotifyApplicationRejectedToCandidate", Candidate },
            { "InviteProspectToApply", Candidate },
        };

        /// <summary>The company name</summary>
        public const string CompanyName = "Microsoft";
    }
}
