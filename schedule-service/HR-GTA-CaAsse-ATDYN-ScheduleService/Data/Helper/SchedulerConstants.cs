//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="SchedulerConstants.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Data.Helper
{
    /// <summary>
    /// Creates constants whose scope spans many components of Scheduling service.
    /// </summary>
    public static class SchedulerConstants
    {
        /// <summary>
        /// Enabled attribute string
        /// </summary>
        public const string Enabled = "Enabled";

        /// <summary>
        /// Disabled attribute string
        /// </summary>
        public const string Disabled = "Disabled";

        /// <summary>
        /// Give permission to everyone
        /// </summary>
        public const string Everyone = "Everyone";

        /// <summary>
        /// Coordinated Universal Time time zone
        /// </summary>
        public const string UTCTimezone = "UTC";

        /// <summary>
        /// Coordinated Universal Time time zone identifier
        /// </summary>
        public const string UTCTimezoneId = "Z";

        /// <summary>
        /// Round-trip date/time pattern constant
        /// o: 2008-06-15T21:15:07.0000000
        /// </summary>
        public const string RoundTripDateTimePattern = "o";

        /// <summary>
        /// Resource for event subscription
        /// </summary>
        public const string EventResource = "me/events";

        /// <summary>
        /// Resource for message subscription
        /// </summary>
        public const string MessageResource = "me/mailfolders('inbox')/messages";

        /// <summary>
        /// Filter to get extensions from graph
        /// </summary>
        public const string ExpandExtensionFilter = "?$expand=extensions($filter=id%20eq%20'Microsoft.OutlookServices.OpenTypeExtension.Com.GTA.Scheduler')";

        /// <summary>The max length of response comment</summary>
        public const int MaxCommentLength = 2000;

        /// <summary>
        /// Odata query to expand event from message
        /// </summary>
        public const string ExpandMessageEvent = "?$expand=microsoft.graph.eventMessage/event($expand=extensions($filter=id%20eq%20'Microsoft.OutlookServices.OpenTypeExtension.Com.GTA.Scheduler'))";

        /// <summary>
        /// Graph User search filter
        /// </summary>
        public const string GraphUserSearchFilter = "?$filter=startswith(userPrincipalName,%27{0}%27)%20or%20startswith(mail,%27{0}%27)&$top=1";

        /// <summary>The Namespace used in Metrics.</summary>
        public const string ActivityMetricNamespace = "MS.GTA.Health";

        /// <summary>The status for Service failure.</summary>
        public const string ServiceFailed = "Failed";

        /// <summary>The HTML content type</summary>
        public const string HTMLContentType = "html";

        /// <summary>
        /// PNG extension.
        /// </summary>
        public const string PNG = "png";

        /// <summary>
        /// Candidate email.
        /// </summary>
        public const string Candidate = "Candidate";

        /// <summary>
        /// Candidate name.
        /// </summary>
        public const string CandidateName = "CandidateName";

        /// <summary>
        /// Hiring Manager name.
        /// </summary>
        public const string HiringManagerName = "HiringManagerName";

        /// <summary>
        /// Thirty minute meeting duration.
        /// </summary>
        public const string ThirtyMinuteMeetingDuration = "PT30M";

        /// <summary>
        /// Thirty minute meeting duration (freeBusy).
        /// </summary>
        public const string ThirtyMinuteFreeBusy = "30";

        /// <summary>
        /// None meeting response.
        /// </summary>
        public const string ResponseNone = "none";

        /// <summary>
        /// Not responded status.
        /// </summary>
        public const string NotResponded = "notresponded";

        /// <summary>
        /// Accepted meeting response.
        /// </summary>
        public const string ResponseAccepted = "accepted";

        /// <summary>
        /// Tentatively accepted meeting response.
        /// </summary>
        public const string ResponseTentativelyAccepted = "tentativelyAccepted";

        /// <summary>
        /// Declined meeting response.
        /// </summary>
        public const string ResponseDeclined = "declined";

        /// <summary>
        /// Meeting Attendee type required
        /// </summary>
        public const string AttendeeTypeRequired = "required";

        /// <summary>
        /// Meeting Attendee type resource
        /// </summary>
        public const string AttendeeTypeResource = "resource";

        /// <summary>
        /// Meeting Attendee type optional
        /// </summary>
        public const string AttendeeTypeOptional = "optional";

        /// <summary>
        /// Meeting time constraint unrestricted
        /// </summary>
        public const string ActivityDomainUnrestricted = "Unrestricted";

        /// <summary>
        /// Meeting time constraint for work hours
        /// </summary>
        public const string ActivityDomainWork = "work";

        /// <summary> Scheduling Database Id </summary>
        public const string SchedulingDatabaseId = "SchedulingDatabase";

        /// <summary> Hcm Database Id </summary>
        public const string HcmDatabaseId = "HCMDatabase";

        /// <summary>
        /// Notification updated event
        /// </summary>
        public const string NotificationUpdatedEvent = "updated";

        /// <summary> Number Of Documents Returned</summary>
        public const int NumberOfDocumentsReturned = 100;

        /// <summary>
        /// Header Bearer string
        /// </summary>
        public static readonly string Bearer = "Bearer";

        /// <summary>
        /// Unknown free busy time string
        /// </summary>
        public static readonly string UnknownFreeBusy = "Unknown";

        /// <summary>
        /// Tentative time string
        /// </summary>
        public static readonly string Tentative = "tentative";

        /// <summary>Interviewer Reminder Frequency In Hours </summary>
        public static readonly int InterviewerReminderFrequencyInHours = 24;

        /// <summary> Send Response Summary Before In Hours </summary>
        public static readonly int SendResponseSummaryBeforeInHours = 48;

        /// <summary> Max attempts to create a calendar event </summary>
        public static readonly int PostCalendarEventMaxRetry = 5;

        /// <summary>Private meeting </summary>
        public static readonly string PrivateMeetingText = "private";

        /// <summary>Normal meeting </summary>
        public static readonly string NormalMeetingText = "normal";

        /// <summary> The default start minutes offset for Inbox message scan </summary>
        public static readonly int DefaultInboxMessageReceivedStartDateTimeOffsetInMinutes = 25;

        /// <summary> The default end minutes offset for Inbox message scan </summary>
        public static readonly int DefaultInboxMessageReceivedEndDateTimeOffsetInMinutes = 5;

        /// <summary> The max scheduled task run time limit </summary>
        public static readonly int MaxScheduledTaskExecutionTimeLimitInSeconds = 10 * 60;

        /// <summary> The number of time buffer before a token becomes invalid </summary>
        public static readonly int TokenValidToBufferTimeInMinutes = 5;

        /// <summary> The max scheduled task run time limit </summary>
        public static readonly int MaxResponseStatusRestoreExecutionTimeLimitInSeconds = 60 * 60;

        /// <summary> start end time range odata filter </summary>
        public static readonly string InboxMessageReceivedDateTimeRangeFilter = "&$filter=ReceivedDateTime ge {0} and receivedDateTime lt {1}";

        /// <summary> message response count </summary>
        public static readonly string InboxMessageResponseCount = "&top=20";

        /// <summary> graph UTC date time format </summary>
        public static readonly string GraphUTCDateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

        /// <summary> the scheduled task name for update meeting response scheduled task name </summary>
        public static readonly string UpdateMeetingResponseScheduledTaskName = "SchedulingServiceUpdateMeetingResponseScheduledTask";

        /// <summary> Prefer header value to use transaction id behaviour </summary>
        public static readonly string PreferHeaderValueTransactionId = "exchange.behavior=EventTransactionId";
    }
}
