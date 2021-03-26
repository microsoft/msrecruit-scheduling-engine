//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using HR.TA.ServicePlatform.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using HR.TA.ServicePlatform.Privacy;

namespace HR.TA.Common.Routing.Exceptions
{
    /// <summary>
    /// Failed call to the global service
    /// </summary>
    [Serializable]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
       Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "HR.TA.Common.Routing.Exceptions", "GlobalService_Exception", MonitoredExceptionKind.Service)]
    public class GlobalServiceException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalServiceException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public GlobalServiceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalServiceException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public GlobalServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Invalid operations e.g. empty tenantId key or trying to create a route in B2C mode
    /// </summary>
    [Serializable]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
       Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, "HR.TA.Common.Routing.Exceptions", "GlobalServiceInvalidOperation_Exception", MonitoredExceptionKind.Benign)]
    public class GlobalServiceInvalidOperationException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalServiceInvalidOperationException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public GlobalServiceInvalidOperationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalServiceInvalidOperationException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public GlobalServiceInvalidOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Gets error type</summary>
        [ExceptionCustomData(Name = Base.Constants.ExceptionErrorTypeName, PrivacyLevel = PrivacyLevel.PublicData, Serialize = true)]
        public int ErrorType => (int)Base.ErrorType.Benign;
    }

    /// <summary>
    /// Failed to retrieve route map information
    /// </summary>
    [Serializable]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
       Justification = "Small closely related classes may be combined.")]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "HR.TA.Common.Routing.Exceptions", "GlobalServiceMappingsRetrieval_Exception", MonitoredExceptionKind.Service)]
    public class GlobalServiceMappingsRetrievalException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalServiceMappingsRetrievalException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public GlobalServiceMappingsRetrievalException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalServiceMappingsRetrievalException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public GlobalServiceMappingsRetrievalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
