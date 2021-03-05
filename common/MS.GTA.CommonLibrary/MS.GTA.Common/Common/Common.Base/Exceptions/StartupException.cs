//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.Base.Exceptions
{
    using System.Net;
    using ServicePlatform.Exceptions;
    
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "Startup", "Startup", MonitoredExceptionKind.Fatal)]
    public class StartupException : MonitoredException
    {
        public StartupException(string message) : base($"The service failed to start: {message}")
        {
        }
    }
}
