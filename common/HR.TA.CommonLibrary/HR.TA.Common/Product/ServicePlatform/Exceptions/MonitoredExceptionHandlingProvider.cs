//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using HR.TA.ServicePlatform.Communication.Http;
using Microsoft.Extensions.Logging;

namespace HR.TA.ServicePlatform.Exceptions
{
    /// <summary>
    /// An specialization of <see cref="DefaultServiceExceptionHandlingProvider"/> to provide additional support for <see cref="MonitoredException"/>.
    /// </summary>
    /// <remarks>This is required as the functionality overriden in this implementation, required by monitored exception, is not portable.</remarks>
    internal class MonitoredExceptionHandlingProvider : DefaultServiceExceptionHandlingProvider
    {
        public MonitoredExceptionHandlingProvider(ILoggerFactory loggerFactory = null)
            : base(loggerFactory)
        {
        }

        /// <summary>
        /// Determines whether the provided <paramref name="exception"/> should be considered fatal.
        /// </summary>
        public override bool IsExceptionFatal(Exception exception)
        {
            // base will check for null and default fatal decision based on MonitoredExceptionMetadataAttribute
            bool isFatal = base.IsExceptionFatal(exception);

            if (!isFatal)
            {
                // check for nested fatal exception
                isFatal = exception.IsFatal();                
            }

            return isFatal;
        }
        
        public override int GetHttpStatusCode(Exception exception)
        {
            if (exception is NonSuccessHttpResponseException nonSuccessHttpResponseException
                && nonSuccessHttpResponseException?.RemoteServiceError?.PropagateError == true)
            {
                return (int) nonSuccessHttpResponseException.RemoteStatusCode;
            }

            return base.GetHttpStatusCode(exception);
        }

        /// <summary>
        /// Creates an instance of <see cref="ServiceError"/> out of the <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The exception to be converted to service error.</param>
        /// <returns>The service error representing the exception.</returns>
        /// <remarks>Required by tests that explicit want to manage service errors.</remarks>
        internal ServiceError ToServiceError(MonitoredException exception)
        {
            return this.CreateServiceError(exception);
        }

        /// <summary>
        /// Creates an instance of <see cref="ServiceError"/> out of the <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The exception to be converted to service error.</param>
        /// <returns>The service error representing the exception.</returns>
        protected override ServiceError CreateServiceError(Exception exception)
        {
            ServiceError error;

            if (exception != null
                && exception is MonitoredException monitoredException 
                && monitoredException?.RemoteServiceError?.PropagateError == true)
            {
                error = monitoredException.RemoteServiceError;
            }
            else
            {
                error = base.CreateServiceError(exception);
            }
            
            // TODO - 770203: [anbencic] Implement error message when needed. It cannot be the exception message by default
            //                https://msazure.visualstudio.com/OneAgile/_workitems/edit/770203
            //
            return new ServiceError(
                error.ErrorNamespace,
                error.ErrorCode,
                message: null,
                customData: error.CustomData.Values,
                stackTrace: null,
                propagateError: error.PropagateError);
        }
    }
}