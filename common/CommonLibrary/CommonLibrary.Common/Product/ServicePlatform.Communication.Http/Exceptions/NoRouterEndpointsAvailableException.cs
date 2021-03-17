//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Net;
using CommonLibrary.ServicePlatform.Exceptions;

namespace CommonLibrary.ServicePlatform.Communication.Http
{
    /// <summary>
    /// Thrown when there are no (more) router endpoints available.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class NoRouterEndpointsAvailableException : MonitoredException
    {
        internal NoRouterEndpointsAvailableException(HttpCommunicationException lastException)
            : base("No (more) router endpoints available", lastException)
        {
        }
    }
}
