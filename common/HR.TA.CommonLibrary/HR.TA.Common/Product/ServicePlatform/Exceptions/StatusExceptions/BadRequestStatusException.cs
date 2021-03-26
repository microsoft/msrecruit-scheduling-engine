//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net;

namespace HR.TA.ServicePlatform.Exceptions
{
    /// <summary>
    /// A monitored exception that will result in a bad request (400) http status.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.BadRequest, ErrorNamespaces.ServicePlatform, ErrorCodes.BadRequestStatusError, MonitoredExceptionKind.Benign)]
    public class BadRequestStatusException : MonitoredException
    {
        public BadRequestStatusException()
        {
        }

        public BadRequestStatusException(string message)
            : base(message)
        {
        }

        public BadRequestStatusException(string message, Exception innerException)
            : base(message)
        {
        }
    }
}
