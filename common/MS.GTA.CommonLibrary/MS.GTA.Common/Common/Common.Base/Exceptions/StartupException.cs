// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="StartupException.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Base.Exceptions
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