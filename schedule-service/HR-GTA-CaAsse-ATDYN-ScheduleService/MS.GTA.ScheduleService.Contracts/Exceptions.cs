//----------------------------------------------------------------------------
// <copyright file="Exceptions.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using MS.GTA.Common.Base.Exceptions;
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>
    /// Job application activity not found exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1649:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.NotFound, "MS.HR.GTA.ScheduleService.Data", "JobApplicationActivityNotFoundException", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class JobApplicationActivityNotFoundException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobApplicationActivityNotFoundException" /> class.
        /// </summary>
        /// <param name="jobApplicationActivityId">The job application activity id.</param>
        public JobApplicationActivityNotFoundException(string jobApplicationActivityId)
            : base($"Job Application Activity: {jobApplicationActivityId} does not exist in the system.")
        {
        }
    }

    /// <summary>
    /// Data validation exception- Not Allowed Operation Exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, "MS.HR.GTA.ScheduleService.Data", "UnauthorizedOperation", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class NotAllowedOperationException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotAllowedOperationException" /> class.
        /// </summary>
        /// <param name="operation">The operation user is trying to perform.</param>
        public NotAllowedOperationException(string operation)
            : base($"Not allowed to: {operation}.")
        {
            this.Operation = operation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotAllowedOperationException" /> class.
        /// </summary>
        /// <param name="operation">The operation user is trying to perform.</param>
        /// <param name="innerException">The inner exception</param>
        public NotAllowedOperationException(string operation, Exception innerException)
            : base($"Not allowed to: {operation}.", innerException)
        {
            this.Operation = operation;
        }

        /// <summary>
        /// Gets the not allowed message.
        /// </summary>
        [ExceptionCustomData(
            Name = "Operation",
            Serialize = true)]
        public string Operation { get; }
    }

    /// <summary>
    /// Job opening not found exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.NotFound, "MS.HR.GTA.ScheduleService.Data", "JobOpeningNotFoundException", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class JobOpeningNotFoundException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobOpeningNotFoundException" /> class.
        /// </summary>
        /// <param name="jobOpeningId">The job opening id.</param>
        public JobOpeningNotFoundException(string jobOpeningId)
            : base($"Job Opening: {jobOpeningId} does not exist in the system.")
        {
        }
    }

    /// <summary>
    /// Invalid job application activity update exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.HR.GTA.ScheduleService.Data", "InvalidActivityUpdateException", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class InvalidActivityUpdateException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidActivityUpdateException" /> class.
        /// </summary>
        /// <param name="activityId">The job application activity Id.</param>
        public InvalidActivityUpdateException(string activityId)
            : base($"Activity {activityId} cannot be updated")
        {
        }
    }

    /// <summary>
    /// Scheduler processing exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Data", "SchedulerProcessingException", MonitoredExceptionKind.Service)]
    public sealed class SchedulerProcessingException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerProcessingException" /> class.
        /// </summary>
        /// <param name="exceptionMessage">Exception during scheduling service scheduler processing.</param>
        public SchedulerProcessingException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }
    }

    /// <summary>
    /// Job application not found exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.NotFound, "MS.HR.GTA.ScheduleService.Data", "JobApplicationNotFoundException", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class JobApplicationNotFoundException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobApplicationNotFoundException" /> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public JobApplicationNotFoundException(string message)
            : base($"{message}")
        {
        }
    }

    /// <summary>
    /// Talent email service token exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService", "ScheduleEmailServiceTokenException", MonitoredExceptionKind.Service)]
    public sealed class ScheduleEmailServiceTokenException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleEmailServiceTokenException" /> class.
        /// </summary>
        /// <param name="errorMessage">Exception during talent email service token acquisition.</param>
        public ScheduleEmailServiceTokenException(string errorMessage)
            : base($"{errorMessage}")
        {
        }
    }

    /// <summary>
    /// Scheduler service token exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Data", "SchedulerSecretException", MonitoredExceptionKind.Service)]
    public sealed class SchedulerSecretException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerSecretException" /> class.
        /// </summary>
        /// <param name="exception">Exception during scheduling service token acquisition.</param>
        public SchedulerSecretException(Exception exception)
            : base($"Issue while read secret, Error: {exception.Message} stacktrace:{exception.StackTrace}")
        {
        }
    }

    /// <summary>
    /// Graph meeting time exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.HR.GTA.ScheduleService.Data", "GraphException", MonitoredExceptionKind.Service)]
    public sealed class GraphException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphException" /> class.
        /// </summary>
        /// <param name="method"> Type of call to graph (GET/POST/PUT)</param>
        /// <param name="url"> URL to graph service </param>
        /// <param name="status">Response status code from graph service.</param>
        /// <param name="message">Response error message from graph service.</param>
        public GraphException(string method, string url, HttpStatusCode status, string message)
            : base($"Exception during {method}:{url} call to graph. Response {status.ToString()} with error message {message}")
        {
        }
    }

    /// <summary>
    /// Scheduler update calendar exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.HR.GTA.ScheduleService.Data", "SchedulerUpdateCalendarException", MonitoredExceptionKind.Service)]
    public sealed class SchedulerUpdateCalendarException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerUpdateCalendarException" /> class.
        /// </summary>
        /// <param name="exception">Exception during scheduling update calendar processing.</param>
        public SchedulerUpdateCalendarException(Exception exception)
            : base($"Issue while updating calendar, stacktrace:{exception.StackTrace}, innerException:{exception.InnerException}")
        {
        }
    }

    /// <summary>
    /// Exception for Delete Calendar event failure.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Data", "DeleteCalendarFailure", MonitoredExceptionKind.Service)]
    [Serializable]
    public sealed class DeleteCalendarEventException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCalendarEventException"/> class.
        /// </summary>
        public DeleteCalendarEventException()
            : base("Delete Calendar event failed")
        {
        }
    }

    /// <summary>
    /// Data validation exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.HR.GTA.ScheduleService", "RequestBodyValidationException", MonitoredExceptionKind.Benign)]
    public sealed class InvalidRequestDataValidationException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestDataValidationException" /> class.
        /// </summary>
        /// <param name="validationError">The validation error.</param>
        public InvalidRequestDataValidationException(string validationError)
            : base($"Data validation failed: {validationError}.")
        {
        }
    }

    /// <summary>
    /// Create Invitation exception
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.HR.GTA.ScheduleService", "CreateInvitationException", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class CreateInvitationException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateInvitationException" /> class.
        /// </summary>
        /// <param name="applicationID">The application ID</param>
        public CreateInvitationException(string applicationID)
            : base($"Could not create invitation token for jobApplication ID: {applicationID}.")
        {
        }
    }

    /// <summary>
    /// Aggregate Graph exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Data", "AggregateGraphException", MonitoredExceptionKind.Service)]
    public sealed class AggregateGraphException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateGraphException" /> class.
        /// </summary>
        /// <param name="exception">Exception during microsoft graph processing.</param>
        public AggregateGraphException(Exception exception)
            : base($"Issue while processing multiple graph calls, stacktrace:{exception.StackTrace}, innerException:{exception.InnerException}")
        {
        }
    }

    /////// <summary>
    /////// Graph Exception thrown when a particular mailbox fails to schedule
    /////// </summary>
    ////public sealed class MailBoxFailedException : BenignException
    ////{
    ////    /// <summary>
    ////    /// Initializes a new instance of the <see cref="MailBoxFailedException" /> class.
    ////    /// </summary>
    ////    /// <param name="exceptions">Exceptions during microsoft graph processing.</param>
    ////    /// <param name="mailbox">Mailbox used</param>
    ////    public MailBoxFailedException(List<Exception> exceptions, string mailbox)
    ////        : base($"Issue while processing multiple graph calls with mailbox: {mailbox}, stacktraces:{string.Join(" ", exceptions?.Select(ex => ex.StackTrace) ?? new List<string>())}, innerExceptions:{string.Join(" ", exceptions?.Select(ex => ex.InnerException) ?? new List<Exception>())}")
    ////    {
    ////    }
    ////}

    /// <summary>
    /// Notification processing exception. Returning 202 as status code so that service retrying will not retry on failure.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.Accepted, "MS.HR.GTA.ScheduleService.data", "NotificationProcessingException", MonitoredExceptionKind.Service)]
    public sealed class NotificationProcessingException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationProcessingException" /> class.
        /// </summary>
        /// <param name="exception">Exception during scheduling service notification processing.</param>
        public NotificationProcessingException(Exception exception)
            : base($"Issue while processing notification, stacktrace:{exception.StackTrace}, innerException:{exception}")
        {
        }
    }

    /// <summary>
    /// Scheduler get rooms exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Data", "SchedulerGetRoomsException", MonitoredExceptionKind.Service)]
    public sealed class SchedulerGetRoomsException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerGetRoomsException" /> class.
        /// </summary>
        /// <param name="exceptionMessage">Exception when scheduler gets rooms.</param>
        public SchedulerGetRoomsException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }
    }

    /// <summary>
    /// Scheduler get rooms exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, "MS.HR.GTA.ScheduleService.Data", "GetRoomsUnAuthorizedOrForbiddenException", MonitoredExceptionKind.Service)]
    public sealed class GetRoomsUnAuthorizedOrForbiddenException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoomsUnAuthorizedOrForbiddenException" /> class.
        /// </summary>
        /// <param name="exceptionMessage">Exception when scheduler gets rooms.</param>
        public GetRoomsUnAuthorizedOrForbiddenException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }
    }

    /// <summary>
    /// Document database processing exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Data", "DocumentDbProcessingException", MonitoredExceptionKind.Service)]
    public sealed class DocumentDbProcessingException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbProcessingException" /> class.
        /// </summary>
        /// <param name="exception">Exception during document database processing.</param>
        public DocumentDbProcessingException(Exception exception)
            : base($"Issue while documentdb processing, stacktrace:{exception.StackTrace}, innerException:{exception.InnerException}")
        {
        }
    }

    /// <summary>
    /// Reminder processing exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Data", "StatefulServiceReliableCollectionException", MonitoredExceptionKind.Service)]
    public sealed class ReminderProcessingException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderProcessingException" /> class.
        /// </summary>
        /// <param name="exception">Exception during adding to the reminder collection.</param>
        public ReminderProcessingException(Exception exception)
            : base($"Issue while adding calendar events for tracking, stacktrace:{exception.StackTrace}, innerException:{exception.InnerException}")
        {
        }
    }

    /// <summary>
    /// Scheduler service token exception.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Data", "SchedulerServiceTokenException", MonitoredExceptionKind.Service)]
    public sealed class SchedulerServiceTokenException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerServiceTokenException" /> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        public SchedulerServiceTokenException(string message)
            : base($"{message}")
        {
        }
    }

    /// <summary>
    /// Exception for getting a meeting info failure.
    /// Throw 500 until the race condition between create/update meetinginfo is solved. Should be updated to 400 later.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Data.Exceptions", "GetMeetingInfoFailure", MonitoredExceptionKind.Service)]
    [Serializable]
    public sealed class GetMeetingInfoException
        : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMeetingInfoException"/> class.
        /// </summary>
        /// <param name="meetingInfoId">the id of the meeting info not found</param>
        public GetMeetingInfoException(string meetingInfoId)
            : base($"Get meeting info failed for meetingInfo {meetingInfoId}")
        {
        }
    }

    /// <summary>
    /// Exception for Get cortana schedule failure.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Data.Exceptions", "GetCortanaScheduleFailure", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class GetCortanaScheduleException
        : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCortanaScheduleException"/> class.
        /// </summary>
        /// <param name="url">Url - get cortana schedule</param>
        public GetCortanaScheduleException(string url)
            : base($"Get cortana schedule call with url : {url} failed")
        {
        }
    }

    /// <summary>
    /// Forbidden for document database operation.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, "MS.HR.GTA.ScheduleService.Data.Exceptions", "DocumentDbUnauthorized", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class DocumentDbForbidden : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbForbidden"/> class.
        /// </summary>
        public DocumentDbForbidden()
            : base("User forbidden for operation")
        {
        }
    }

    /// <summary>
    /// Outlook response status not received exception
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.HR.GTA.ScheduleService.Exceptions", "ResponseStatusNotUpdatedException", MonitoredExceptionKind.Fatal)]
    [Serializable]
    public sealed class ResponseStatusNotUpdatedException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseStatusNotUpdatedException" /> class.
        /// </summary>
        public ResponseStatusNotUpdatedException()
            : base($"Outlook response status is not being updated")
        {
        }
    }

    /// <summary>
    /// Graph subscription not found exception
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.NotFound, "MS.HR.GTA.ScheduleService.Exceptions", "GraphSubscriptionNotFoundException", MonitoredExceptionKind.Benign)]
    [Serializable]
    public sealed class GraphSubscriptionNotFoundException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphSubscriptionNotFoundException" /> class.
        /// </summary>
        /// <param name="subscriptionId">The subscriptionId.</param>
        public GraphSubscriptionNotFoundException(string subscriptionId)
            : base($"Subscription {subscriptionId} is not found")
        {
        }
    }

    /// <summary>
    /// The <see cref="OperationFailedException"/> class represents the exceptional case of failed operation.
    /// </summary>
    /// <seealso cref="MS.GTA.ServicePlatform.Exceptions.MonitoredException" />
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.InterviewService.Exceptions", "OperationFailure", MonitoredExceptionKind.Service)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class", Justification = "Small closely related classes may be combined.")]
    public class OperationFailedException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public OperationFailedException(string message)
            : base(message)
        {
        }
    }
}
