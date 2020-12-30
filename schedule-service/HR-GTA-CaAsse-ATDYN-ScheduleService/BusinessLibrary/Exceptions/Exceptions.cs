// <copyright file="Exceptions.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "File contains all list of monitored exception.")]

namespace MS.GTA.ScheduleService.BusinessLibrary.Exceptions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>
    /// The <see cref="OperationNotAllowedException"/> class represents the exception scenario preventing the further operation.
    /// </summary>
    /// <seealso cref="MonitoredException" />
    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, "MS.GTA.SchedulerService.Exceptions", "BusinessConditionsNotMet", MonitoredExceptionKind.Benign)]
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
    /// <seealso cref="MS.GTA.ServicePlatform.Exceptions.MonitoredException" />
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.SchedulerService.Exceptions", "OperationFailure", MonitoredExceptionKind.Service)]
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
