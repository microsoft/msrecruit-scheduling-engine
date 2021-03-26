//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net;

namespace TA.CommonLibrary.ServicePlatform.Exceptions
{
    /// <summary>
    /// A monitored exception that will result in a unauthorized (401) http status.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.Unauthorized, ErrorNamespaces.ServicePlatform, ErrorCodes.UnauthorizedStatusError, MonitoredExceptionKind.Benign)]
    public class UnauthorizedStatusException : MonitoredException
    {
        public UnauthorizedStatusException()
        {
        }

        public UnauthorizedStatusException(string message)
            : base(message)
        {
        }

        public UnauthorizedStatusException(string message, Exception innerException)
            : base(message)
        {
        }
    }
}
