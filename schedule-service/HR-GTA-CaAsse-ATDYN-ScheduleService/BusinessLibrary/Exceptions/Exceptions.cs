//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Exceptions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using HR.TA.ServicePlatform.Exceptions;

    /// <summary>
    /// The <see cref="OperationNotAllowedException"/> class represents the exception scenario preventing the further operation.
    /// </summary>
    /// <seealso cref="MonitoredException" />
    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, "HR.TA.SchedulerService.Exceptions", "BusinessConditionsNotMet", MonitoredExceptionKind.Benign)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class", Justification = "Small closely related classes may be combined.")]
    public class OperationNotAllowedException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationNotAllowedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public OperationNotAllowedException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// The <see cref="OperationFailedException"/> class represents the exceptional case of failed operation.
    /// </summary>
    /// <seealso cref="HR.TA.ServicePlatform.Exceptions.MonitoredException" />
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "HR.TA.SchedulerService.Exceptions", "OperationFailure", MonitoredExceptionKind.Service)]
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
