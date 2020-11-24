﻿//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Net;

namespace MS.GTA.ServicePlatform.Exceptions
{
    /// <summary>
    /// If request is for another service but happened to be received by this service return 410 response immediately to force re-resolve from gateway.
    /// </summary>
    [MonitoredExceptionMetadata(HttpStatusCode.Gone, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Benign)]
    public sealed class RequestIntendedForOtherServiceException : MonitoredException
    {
        public RequestIntendedForOtherServiceException(string indendedServiceName, string currentServiceName)
            : base($"Request was intended for service: {indendedServiceName} but was recieved by service: {currentServiceName}")
        {
        }
    }
}
