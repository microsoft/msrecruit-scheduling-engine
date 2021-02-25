//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net;

namespace MS.GTA.ServicePlatform.Exceptions
{
    /// <summary>
    /// A monitored exception that will result in a conflict (409) http status.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.Conflict, ErrorNamespaces.ServicePlatform, ErrorCodes.ConflictStatusError, MonitoredExceptionKind.Benign)]
    public class ConflictStatusException : MonitoredException
    {
        public ConflictStatusException()
        {
        }

        public ConflictStatusException(string message)
            : base(message)
        {
        }

        public ConflictStatusException(string message, Exception innerException)
            : base(message)
        {
        }
    }
}
