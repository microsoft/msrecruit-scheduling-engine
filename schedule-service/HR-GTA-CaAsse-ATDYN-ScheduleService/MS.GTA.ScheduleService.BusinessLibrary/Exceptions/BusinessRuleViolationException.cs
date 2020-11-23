// <copyright file="BusinessRuleViolationException.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.Exceptions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using MS.GTA.Common.Base.Exceptions;
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>
    /// The <see cref="BusinessRuleViolationException"/> class represents the exception scenario preventing the further operation.
    /// </summary>
    /// <seealso cref="MonitoredException" />
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "MS.GTA.SchedulerService.Exceptions", "BusinessRuleViolated", MonitoredExceptionKind.Benign)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class", Justification = "Small closely related classes may be combined.")]
    public class BusinessRuleViolationException : BenignException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRuleViolationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BusinessRuleViolationException(string message)
            : base(message)
        {
        }
    }
}
