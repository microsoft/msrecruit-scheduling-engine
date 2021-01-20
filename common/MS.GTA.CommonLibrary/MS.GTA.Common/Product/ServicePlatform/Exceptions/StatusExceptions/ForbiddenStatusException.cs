//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Net;

namespace MS.GTA.ServicePlatform.Exceptions
{
    /// <summary>
    /// A monitored exception that will result in a forbidden (403) http status.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.Forbidden, ErrorNamespaces.ServicePlatform, ErrorCodes.ForbiddenStatusError, MonitoredExceptionKind.Benign)]
    public class ForbiddenStatusException : MonitoredException
    {
        public ForbiddenStatusException()
        {
        }

        public ForbiddenStatusException(string message)
            : base(message)
        {
        }

        public ForbiddenStatusException(string message, Exception innerException)
            : base(message)
        {
        }
    }
}
