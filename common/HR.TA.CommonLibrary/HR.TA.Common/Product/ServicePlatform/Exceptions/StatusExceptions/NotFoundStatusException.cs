//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net;

namespace HR.TA.ServicePlatform.Exceptions
{
    /// <summary>
    /// A monitored exception that will result in a not found (404) http status.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.NotFound, ErrorNamespaces.ServicePlatform, ErrorCodes.NotFoundStatusError, MonitoredExceptionKind.Benign)]
    public class NotFoundStatusException : MonitoredException
    {
        public NotFoundStatusException()
        {
        }

        public NotFoundStatusException(string message)
            : base(message)
        {
        }

        public NotFoundStatusException(string message, Exception innerException)
            : base(message)
        {
        }
    }
}
